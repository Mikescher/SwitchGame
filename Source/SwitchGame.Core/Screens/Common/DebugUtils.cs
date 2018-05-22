using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MonoSAMFramework.Portable;
using MonoSAMFramework.Portable.DebugTools;
using MonoSAMFramework.Portable.Extensions;
using MonoSAMFramework.Portable.GameMath;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Language;
using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.Network.Multiplayer;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.Entities.Particles;
using SwitchGame.Shared.Resources;

namespace SwitchGame.Shared.Screens.Common
{
	public static class DebugUtils
	{
#if DEBUG
		public static TimingCounter TIMING_DRAW_BACKGROUND     => GameScreen.TIMING_DRAW_BACKGROUND;
		public static TimingCounter TIMING_DRAW_ENTITIES       => GameScreen.TIMING_DRAW_ENTITIES;
		public static TimingCounter TIMING_DRAW_SCREEN         => GameScreen.TIMING_DRAW_SCREEN;
		public static TimingCounter TIMING_DRAW_BACKGROUNDPOST => GameScreen.TIMING_DRAW_BACKGROUNDPOST;
		public static TimingCounter TIMING_DRAW_ENTITIESPOST   => GameScreen.TIMING_DRAW_ENTITIESPOST;
		public static TimingCounter TIMING_DRAW_HUD            => GameScreen.TIMING_DRAW_HUD;
		public static TimingCounter TIMING_DRAW_DEBUGSCREEN    => GameScreen.TIMING_DRAW_DEBUGSCREEN;
		public static TimingCounter TIMING_DRAW_PROXIES        => GameScreen.TIMING_DRAW_PROXIES;
		public static TimingCounter TIMING_DRAW_BATCH_GAME     => GameScreen.TIMING_DRAW_BATCH_GAME;
		public static TimingCounter TIMING_DRAW_BATCH_HUD      => GameScreen.TIMING_DRAW_BATCH_HUD;

		public static DebugTextDisplay CreateDisplay(GameScreen scrn)
		{
			var debugDisp = new DebugTextDisplay(scrn.Graphics.GraphicsDevice, Textures.DebugFont);

			debugDisp.AddLine("ShowInfos",     () => $"Device = {MainGame.Inst.SGBridge.DeviceName} | Version = {MainGame.Inst.SGBridge.DeviceVersion} | App = {SGConstants.Version} | Debugger = {Debugger.IsAttached}");
			debugDisp.AddLine("ShowInfos",     () => $"FPS={scrn.FPSCounter.AverageAPS:0000.0} (curr={scrn.FPSCounter.CurrentAPS:0000.0} delta={scrn.FPSCounter.AverageDelta * 1000:000.00} min={scrn.FPSCounter.MinimumAPS:0000.0} (d={scrn.FPSCounter.MaximumDelta * 1000:0000.0}) cycletime={scrn.FPSCounter.AverageCycleTime * 1000:000.00} (max={scrn.FPSCounter.MaximumCycleTime * 1000:000.00} curr={scrn.FPSCounter.CurrentCycleTime * 1000:000.00}) total={scrn.FPSCounter.TotalActions:000000})");
			debugDisp.AddLine("ShowInfos",     () => $"UPS={scrn.UPSCounter.AverageAPS:0000.0} (curr={scrn.UPSCounter.CurrentAPS:0000.0} delta={scrn.UPSCounter.AverageDelta * 1000:000.00} min={scrn.UPSCounter.MinimumAPS:0000.0} (d={scrn.UPSCounter.MaximumDelta * 1000:0000.0}) cycletime={scrn.UPSCounter.AverageCycleTime * 1000:000.00} (max={scrn.UPSCounter.MaximumCycleTime * 1000:000.00} curr={scrn.UPSCounter.CurrentCycleTime * 1000:000.00}) total={scrn.UPSCounter.TotalActions:000000})");
			debugDisp.AddLine("ShowInfos",     () => $"GC = Time since GC:{scrn.GCMonitor.TimeSinceLastGC:00.00}s ({scrn.GCMonitor.TimeSinceLastGC0:000.00}s | {scrn.GCMonitor.TimeSinceLastGC1:000.00}s | {scrn.GCMonitor.TimeSinceLastGC2:000.00}s) Memory = {scrn.GCMonitor.TotalMemory:000.0}MB Frequency = {scrn.GCMonitor.GCFrequency:0.000}");
			debugDisp.AddLine("ShowInfos",     () => $"Quality = {Textures.TEXTURE_QUALITY} | Texture.Scale={1f / Textures.DEFAULT_TEXTURE_SCALE.X:#.00} | Pixel.Scale={Textures.GetDeviceTextureScaling(scrn.Game.GraphicsDevice):#.00}");
			debugDisp.AddLine("ShowInfos",     () => $"Entities = {scrn.Entities.Count(),3} | EntityOps = {scrn.Entities.Enumerate().Sum(p => p.ActiveEntityOperations.Count()):00} | Particles = {scrn.Entities.Enumerate().OfType<IParticleOwner>().Sum(p => p.ParticleCount),3} (Visible: {scrn.Entities.Enumerate().Where(p => p.IsInViewport).OfType<IParticleOwner>().Sum(p => p.ParticleCount),3})");
			debugDisp.AddLine("ShowInfos",     () => $"GamePointer = ({scrn.InputStateMan.GetCurrentState().GamePointerPosition.X:000.0}|{scrn.InputStateMan.GetCurrentState().GamePointerPosition.Y:000.0}) | HUDPointer = ({scrn.InputStateMan.GetCurrentState().HUDPointerPosition.X:000.0}|{scrn.InputStateMan.GetCurrentState().HUDPointerPosition.Y:000.0}) | PointerOnMap = ({scrn.InputStateMan.GetCurrentState().GamePointerPositionOnMap.X:000.0}|{scrn.InputStateMan.GetCurrentState().GamePointerPositionOnMap.Y:000.0})");
			debugDisp.AddLine("DebugGestures", () => $"Pinching = {scrn.InputStateMan.GetCurrentState().IsGesturePinching} & PinchComplete = {scrn.InputStateMan.GetCurrentState().IsGesturePinchComplete} & PinchPower = {scrn.InputStateMan.GetCurrentState().LastPinchPower}");
			debugDisp.AddLine("ShowInfos",     () => $"OGL Sprites = {scrn.LastReleaseRenderSpriteCount:0000} (+ {scrn.LastDebugRenderSpriteCount:0000}); OGL Text = {scrn.LastReleaseRenderTextCount:0000} (+ {scrn.LastDebugRenderTextCount:0000})");
			debugDisp.AddLine("ShowInfos",     () => $"Map Offset = {scrn.MapOffset} (Map Center = {scrn.MapViewportCenter})");
			debugDisp.AddLine("ShowInfos",     () => $"SamSound = [Effects]: {MainGame.Inst.SGSound.GetEffectsStringState()} | [Music]: {MainGame.Inst.SGSound.GetMusicStringState()}");
			debugDisp.AddLine("ShowInfos",     () => $"Mediaplayer[{Microsoft.Xna.Framework.Media.MediaPlayer.State}] = (Volume: {Microsoft.Xna.Framework.Media.MediaPlayer.Volume:0.00}) ({Microsoft.Xna.Framework.Media.MediaPlayer.PlayPosition.TotalSeconds:0}s) {{{string.Join(",", new[] { Microsoft.Xna.Framework.Media.MediaPlayer.IsMuted ? "IsMuted" : "", Microsoft.Xna.Framework.Media.MediaPlayer.GameHasControl ? "GameHasControl" : "", Microsoft.Xna.Framework.Media.MediaPlayer.IsShuffled ? "IsShuffled" : "", Microsoft.Xna.Framework.Media.MediaPlayer.IsVisualizationEnabled ? "IsVisualizationEnabled" : "", Microsoft.Xna.Framework.Media.MediaPlayer.IsRepeating ? "IsRepeating" : "" }.Where(p => !string.IsNullOrWhiteSpace(p)))}}}");

			debugDisp.AddLine("DebugTimings", () => $"Drawing: Background        [{TIMING_DRAW_BACKGROUND.Format()}]\n" +
													$"         Entities:         [{TIMING_DRAW_ENTITIES.Format()}]\n" +
													$"         Screen:           [{TIMING_DRAW_SCREEN.Format()}]\n" +
													$"         Background(post): [{TIMING_DRAW_BACKGROUNDPOST.Format()}]\n" +
													$"         Entities(post):   [{TIMING_DRAW_ENTITIESPOST.Format()}]\n" +
													$"         HUD:              [{TIMING_DRAW_HUD.Format()}]\n" +
													$"         DebugScreen:      [{TIMING_DRAW_DEBUGSCREEN.Format()}]\n" +
													$"         Proxies:          [{TIMING_DRAW_PROXIES.Format()}]\n");
			debugDisp.AddLine("DebugTimings", () => $"Drawing[BATCH_GAME]: [{TIMING_DRAW_BATCH_GAME.Format()}]\nDrawing[BATCH_HUD]:  [{TIMING_DRAW_BATCH_HUD.Format()}]");

			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GraphicsDevice.Viewport=[{scrn.Game.GraphicsDevice.Viewport.Width}|{scrn.Game.GraphicsDevice.Viewport.Height}]");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.VirtualGuaranteedSize={scrn.VAdapterGame.VirtualGuaranteedSize} || GameAdapter.VirtualGuaranteedSize={scrn.VAdapterHUD.VirtualGuaranteedSize}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.RealGuaranteedSize={scrn.VAdapterGame.RealGuaranteedSize} || GameAdapter.RealGuaranteedSize={scrn.VAdapterHUD.RealGuaranteedSize}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.VirtualTotalSize={scrn.VAdapterGame.VirtualTotalSize} || GameAdapter.VirtualTotalSize={scrn.VAdapterHUD.VirtualTotalSize}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.RealTotalSize={scrn.VAdapterGame.RealTotalSize} || GameAdapter.RealTotalSize={scrn.VAdapterHUD.RealTotalSize}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.VirtualOffset={scrn.VAdapterGame.VirtualGuaranteedBoundingsOffset} || GameAdapter.VirtualOffset={scrn.VAdapterHUD.VirtualGuaranteedBoundingsOffset}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.RealOffset={scrn.VAdapterGame.RealGuaranteedBoundingsOffset} || GameAdapter.RealOffset={scrn.VAdapterHUD.RealGuaranteedBoundingsOffset}");
			debugDisp.AddLine("ShowMatrixTextInfos", () => $"GameAdapter.Scale={scrn.VAdapterGame.Scale} || GameAdapter.Scale={scrn.VAdapterHUD.Scale}");

			debugDisp.AddLine("ShowOperations", () => string.Join(Environment.NewLine, scrn.Entities.Enumerate().SelectMany(e => e.ActiveEntityOperations).Select(o => o.Name).GroupBy(p=>p).Select(p=>p.Count()==1 ? p.Key : $"{p.Key} (x{p.Count()})")));
			debugDisp.AddLine("ShowOperations", () => string.Join(Environment.NewLine, scrn.HUD.Enumerate().SelectMany(e => e.ActiveHUDOperations).Select(o => o.Name).GroupBy(p => p).Select(p => p.Count() == 1 ? p.Key : $"{p.Key} (x{p.Count()})")));
			debugDisp.AddLine("ShowOperations", () => string.Join(Environment.NewLine, scrn.GetAllAgents().Select(o => o.Name).GroupBy(p => p).Select(p => p.Count() == 1 ? p.Key : $"{p.Key} (x{p.Count()})")));
			debugDisp.AddLine("ShowOperations", () => string.Join(Environment.NewLine, MainGame.Inst.GetAllAgents().Select(o => o.Name).GroupBy(p => p).Select(p => p.Count() == 1 ? p.Key : $"{p.Key} (x{p.Count()})")));

			debugDisp.AddTabularLine("ShowDebugShortcuts", DebugSettings.GetCategorizedSummaries);

			debugDisp.AddLogLines();

			debugDisp.AddLine("ShowSerializedProfile", () => MainGame.Inst.Settings.SerializeToString(128));

			debugDisp.AddLine("FALSE", () => scrn.InputStateMan.GetCurrentState().GetFullDebugSummary());
			debugDisp.AddLine("FALSE", () => MainGame.Inst.SGBridge.FullDeviceInfoString);

			return debugDisp;
		}

		public static void CreateShortcuts(GameScreen scrn)
		{
			if (MonoSAMGame.IsDesktop())
				DebugSettings.AddSwitch(null, "DBG", scrn, KCL.C(SKeys.D, SKeys.AndroidMenu), true);
			else
				DebugSettings.AddSwitch(null, "DBG", scrn, KCL.C(SKeys.D, SKeys.AndroidMenu), true);

			DebugSettings.AddFunctionless("DBG", "DebugTextDisplay", scrn);

			DebugSettings.AddTrigger("DBG",      "SetQuality_1",         scrn, SKeys.D1, KeyModifier.Control,  x => Textures.ChangeQuality(scrn.Game.Content, TextureQuality.FD));
			DebugSettings.AddTrigger("DBG",      "SetQuality_2",         scrn, SKeys.D2, KeyModifier.Control,  x => Textures.ChangeQuality(scrn.Game.Content, TextureQuality.BD));
			DebugSettings.AddTrigger("DBG",      "SetQuality_3",         scrn, SKeys.D3, KeyModifier.Control,  x => Textures.ChangeQuality(scrn.Game.Content, TextureQuality.LD));
			DebugSettings.AddTrigger("DBG",      "SetQuality_4",         scrn, SKeys.D4, KeyModifier.Control,  x => Textures.ChangeQuality(scrn.Game.Content, TextureQuality.MD));
			DebugSettings.AddTrigger("DBG",      "SetQuality_5",         scrn, SKeys.D5, KeyModifier.Control,  x => Textures.ChangeQuality(scrn.Game.Content, TextureQuality.HD));
			DebugSettings.AddTrigger("DBG",      "ClearMessages",        scrn, SKeys.C, KeyModifier.None,      x => scrn.DebugDisp.Clear());

			DebugSettings.AddSwitch("DBG",       "ShowInfos",             scrn, SKeys.F2,  KeyModifier.None,    true);
			DebugSettings.AddSwitch("DBG",       "DebugBackground",       scrn, SKeys.F3,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("DBG",       "DebugHUDBorders",       scrn, SKeys.F4,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("ShowInfos", "DebugNetwork",          scrn, SKeys.F5,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("ShowInfos", "ShowMatrixTextInfos",   scrn, SKeys.F6,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("DBG",       "ShowDebugMiniMap",      scrn, SKeys.F7,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("DBG",       "DebugEntityBoundaries", scrn, SKeys.F8,  KeyModifier.None,    false);
			DebugSettings.AddSwitch("DBG",       "DebugEntityMouseAreas", scrn, SKeys.F9,  KeyModifier.None,    false); 
			DebugSettings.AddSwitch("ShowInfos", "ShowOperations",        scrn, SKeys.F10, KeyModifier.None,    true);
			DebugSettings.AddSwitch("ShowInfos", "DebugGestures",         scrn, SKeys.F11, KeyModifier.None,    false);
			DebugSettings.AddSwitch("DBG",       "DebugTimings",          scrn, SKeys.F12, KeyModifier.None,    false); 

			DebugSettings.AddPush("DBG",         "ShowDebugShortcuts",    scrn, SKeys.Tab, KeyModifier.None);
			DebugSettings.AddPush("DBG",         "ShowSerializedProfile", scrn, SKeys.O,   KeyModifier.None);
			DebugSettings.AddPush("TRUE",        "HideHUD",               scrn, SKeys.H,   KeyModifier.None);
			DebugSettings.AddTrigger("DBG",      "RotateLang",            scrn, SKeys.L,   KeyModifier.Control,      RotateLang);
		}

		private static void RotateLang(DebugListener obj)
		{
			MainGame.Inst.Settings.Language = (MainGame.Inst.Settings.Language + 1) % L10N.LANG_COUNT;
			MainGame.Inst.SaveUserConfig();
			L10N.ChangeLanguage(MainGame.Inst.Settings.Language);
		}

#endif
	}
}
