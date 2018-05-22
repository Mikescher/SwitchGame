using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.Localization;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.Sound;
using SwitchGame.Shared;
using SwitchGame.Shared.Resources;

namespace SwitchGame.Shared.Resources
{
	public class SGSounds : SAMSoundPlayer
	{
		private const float MUSIC_LEVEL_FADEIN = 1.0f;
		private const float MUSIC_FADEOUT      = 0.5f;
		private const float MUSIC_FADENEXT     = 0.0f;
		
		private const float MUSIC_BACKGROUND_FADEIN = 2.5f;

		private SoundEffect effectButton;
		private SoundEffect effectKeyboardClick;

		public override void Initialize(ContentManager content)
		{
			effectButton        = content.Load<SoundEffect>("sounds/button");
			effectKeyboardClick = content.Load<SoundEffect>("sounds/click");

			ButtonClickEffect         = effectButton;
			ButtonKeyboardClickEffect = effectKeyboardClick;
		}

		protected override void OnEffectError()
		{
			IsEffectsMuted = true;
			MainGame.Inst.Settings.SoundsEnabled = false;
			SAMLog.Warning("SSP::DISABLE_1", "Disable Music due to error");

			MainGame.Inst.DispatchBeginInvoke(() =>
			{
				MainGame.Inst.ShowToast("SSP::ERR_EFFECTS", L10N.T(L10NImpl.STR_ERR_SOUNDPLAYBACK), 32, FlatColors.Flamingo, FlatColors.Foreground, 8f);
			});
		}

		protected override void OnSongError()
		{
			IsMusicMuted = true;
			MainGame.Inst.Settings.MusicEnabled = false;
			SAMLog.Warning("SSP::DISABLE_2", "Disable Sounds due to error");

			MainGame.Inst.DispatchBeginInvoke(() =>
			{
				MainGame.Inst.ShowToast("SSP::ERR_SONG", L10N.T(L10NImpl.STR_ERR_MUSICPLAYBACK), 32, FlatColors.Flamingo, FlatColors.Foreground, 8f);
			});
		}
	}
}
