using MonoSAMFramework.Portable.DebugTools;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.HUD;
using SwitchGame.Shared.Resources;

namespace SwitchGame.Shared.Screens.MainMenuScreen.HUD
{
	public class MainMenuHUD : GameHUD
	{
		public MainMenuHUD(MainMenuScreen scrn) : base(scrn, Textures.HUDFontRegular)
		{
			//
		}

#if DEBUG
		protected override void OnUpdate(SAMTime gameTime, InputState istate)
		{
			root.IsVisible = !DebugSettings.Get("HideHUD");
		}
#endif
	}
}
