using System;
using System.IO;
using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable;
using MonoSAMFramework.Portable.DeviceBridge;
using MonoSAMFramework.Portable.Extensions;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Sound;
using System.Text;
using SwitchGame.Shared.DeviceBridge;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.Localization;
using SwitchGame.Shared.SaveData;
using SwitchGame.Shared.Network.Backend;
using SwitchGame.Shared.Resources;
using SwitchGame.Shared.Screens.MainMenuScreen;

namespace SwitchGame.Shared
{
	public class MainGame : MonoSAMGame
	{
		public const float MAX_LOG_SEND_DELTA = 25f; // Max send 5 logs in 25sec
		public const int   MAX_LOG_SEND_COUNT = 5;

		public UserSettings Settings;
		public ISGServerAPI Backend;

		public static MainGame Inst;

		public readonly SGSounds SGSound = new SGSounds();
		public override SAMSoundPlayer Sound => SGSound;

		public readonly float[] LastSendLogTimes = new float[MAX_LOG_SEND_COUNT];

		public ISGOperatingSystemBridge SGBridge => (ISGOperatingSystemBridge)Bridge;
		
		public static bool IsShaderless() => StaticBridge.SystemType == SAMSystemType.MONOGAME_IOS;

		public MainGame() : base()
		{
			Backend = new SGServerAPI(SGBridge);
			//Backend = new DummySGServerAPI();

			SGBridge.IAB.Connect(SGConstants.IABList);

			Settings = new UserSettings();

			var sdata = FileHelper.Inst.ReadDataOrNull(SGConstants.PROFILE_FILENAME); // %LOCALAPPDATA%\IsolatedStorage\...
			if (sdata != null)
			{
				try
				{
#if DEBUG
					var starttime = Environment.TickCount;
#endif
					Settings.DeserializeFromString(sdata);
#if DEBUG
					SAMLog.Debug($"Deserialized settings in {Environment.TickCount - starttime}ms");
#endif
				}
				catch (Exception e)
				{
					SAMLog.Error("Deserialization", e);

					Settings = new UserSettings();
					SaveUserConfig();
				}
			}
			else
			{
				SaveUserConfig();
			}

			SAMLog.LogEvent += SAMLogOnLogEvent;
			SAMLog.AdditionalLogInfo.Add(GetLogInfo);

			for (int i = 0; i < MAX_LOG_SEND_COUNT; i++) LastSendLogTimes[i] = float.MinValue;

			L10NImpl.Init(Settings.Language);
			
			Inst = this;
		}

		private void SAMLogOnLogEvent(object sender, SAMLog.LogEventArgs args)
		{
			if (args.Level == SAMLogLevel.ERROR || args.Level == SAMLogLevel.FATAL_ERROR)
			{
				//Prevent sending logs too fast
				if (CurrentTime.TotalElapsedSeconds - LastSendLogTimes[0] < MAX_LOG_SEND_DELTA)
				{
					SAMLog.Info("Backend::LogSpam", $"Do not send log '{args.Entry.MessageShort}', cause too many online logs in last time");
					return;
				}


				Backend.LogClient(Settings, args.Entry).EnsureNoError();


				for (int i = 0; i < MAX_LOG_SEND_COUNT-1; i++)
				{
					LastSendLogTimes[i] = LastSendLogTimes[i + 1];
				}
				LastSendLogTimes[MAX_LOG_SEND_COUNT - 1] = CurrentTime.TotalElapsedSeconds;
			}
		}

		protected override void OnInitialize()
		{
			if (IsDesktop())
			{
#if DEBUG
				const double ZOOM = 0.525;

				IsMouseVisible = true;
				Graphics.IsFullScreen = false;

				Graphics.PreferredBackBufferWidth = (int) (1920 * ZOOM);
				Graphics.PreferredBackBufferHeight = (int) (1080 * ZOOM);
				Window.AllowUserResizing = true;

				Graphics.SynchronizeWithVerticalRetrace = false;
				IsFixedTimeStep = false;
				TargetElapsedTime = TimeSpan.FromMilliseconds(1);

				Graphics.ApplyChanges();

				//Window.Position = new Point((1920 - Graphics.PreferredBackBufferWidth) / 2, (1080 - Graphics.PreferredBackBufferHeight) / 2);


#endif
			}
			else
			{
				Graphics.IsFullScreen = true;
				Graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft;
				Graphics.ApplyChanges();
			}
		}

		protected override void OnAfterInitialize()
		{
			SetCurrentScreen(new MainMenuScreen(this, Graphics));
		}

		protected override void OnUpdate(SAMTime gameTime)
		{
			SGSound.IsEffectsMuted = !Settings.SoundsEnabled;
			SGSound.IsMusicMuted = !(Settings.MusicEnabled && Settings.SoundsEnabled);
		}

		protected override void LoadContent()
		{
			Textures.Initialize(Content, GraphicsDevice);
			try
			{
				Sound.Initialize(Content);
			}
			catch (Exception e)
			{
				SAMLog.Error("MG::LC", "Initializing sound failed", e.ToString());
				Sound.InitErrorState = true;
			}
		}

		protected override void UnloadContent()
		{
			// NOP
		}

		public void SaveUserConfig()
		{
			var sdata = Settings.SerializeToString();

			try
			{
				FileHelper.Inst.WriteData(SGConstants.PROFILE_FILENAME, sdata);
			}
			catch (IOException e)
			{
				if (e.Message.Contains("Disk full"))
				{
					DispatchBeginInvoke(() =>
					{
						ShowToast("MG::OOM", L10N.T(L10NImpl.STR_ERR_OUTOFMEMORY), 32, FlatColors.Flamingo, FlatColors.Foreground, 3f);
					});
				}
				else
				{
					SAMLog.Error("MG::WRITE", e);
				}
			}

#if DEBUG
			SAMLog.Debug($"Profile saved ({sdata.Length})");

			try
			{
				var p = new UserSettings();
				p.DeserializeFromString(sdata);
				var sdata2 = p.SerializeToString();

				if (sdata2 != sdata)
				{
					SAMLog.Error("Serialization_mismatch", "Serialization test mismatch", $"Data_1:\n{sdata}\n\n----------------\n\nData_2:\n{sdata2}");
				}
			}
			catch (Exception e)
			{
				SAMLog.Error("Serialization-Ex", "Serialization test mismatch", e.ToString());
			}
#endif
		}

		public string GetLogInfo()
		{
			StringBuilder b = new StringBuilder();

			b.AppendLine("GameCycleCounter: " + GameCycleCounter);
			b.AppendLine("IsInitializationLag: " + IsInitializationLag);
			b.AppendLine("MainGame.Alive: " + Alive);

			var scrn = screens?.CurrentScreen;

			if (scrn != null)
			{
				b.AppendLine("MainGame.CurrentScreen:    " + scrn.GetType());
				b.AppendLine("GameScreen.Entities.Count: " + scrn.Entities?.Count());
				b.AppendLine("GameScreen.HUD.DeepCount:  " + scrn.HUD?.DeepCount());
				b.AppendLine("GameScreen.IsRemoved:      " + scrn.IsRemoved);
				b.AppendLine("GameScreen.IsShown:        " + scrn.IsShown);
				b.AppendLine("GameScreen.GameSpeed:      " + scrn.GameSpeed);
			}

			return b.ToString();
		}
	}
}

