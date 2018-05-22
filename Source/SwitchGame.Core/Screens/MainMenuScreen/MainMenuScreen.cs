using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoSAMFramework.Portable;
using MonoSAMFramework.Portable.BatchRenderer;
using MonoSAMFramework.Portable.ColorHelper;
using MonoSAMFramework.Portable.DebugTools;
using MonoSAMFramework.Portable.Extensions;
using MonoSAMFramework.Portable.GameMath;
using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable.RenderHelper;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.Background;
using MonoSAMFramework.Portable.Screens.Entities;
using MonoSAMFramework.Portable.Screens.HUD;
using MonoSAMFramework.Portable.Screens.HUD.Elements.Other;
using MonoSAMFramework.Portable.Screens.ViewportAdapters;
using SwitchGame.Shared.Resources;
using SwitchGame.Shared.Screens.Common;
using SwitchGame.Shared.Screens.MainMenuScreen.HUD;

namespace SwitchGame.Shared.Screens.MainMenuScreen
{
	public class MainMenuScreen : GameScreen
	{
		public MainMenuScreen(MonoSAMGame game, GraphicsDeviceManager gdm) : base(game, gdm)
		{
			Initialize();
		}

		protected override EntityManager CreateEntityManager() => new MainMenuEntityManager(this);
		protected override GameHUD CreateHUD() => new MainMenuHUD(this);
		protected override GameBackground CreateBackground() => new SolidColorBackground(this, Color.DarkMagenta);
		protected override SAMViewportAdapter CreateViewport() => new TolerantBoxingViewportAdapter(Game.Window, Graphics, SGConstants.VIEW_WIDTH, SGConstants.VIEW_HEIGHT);
		protected override DebugMinimap CreateDebugMinimap() => new StandardDebugMinimapImplementation(this, 192, 32);
		protected override FRectangle CreateMapFullBounds() => new FRectangle(0, 0, 1, 1);
		protected override float GetBaseTextureScale() => Textures.DEFAULT_TEXTURE_SCALE_F;

		private void Initialize()
		{
#if DEBUG
			DebugUtils.CreateShortcuts(this);
			DebugDisp = DebugUtils.CreateDisplay(this);
#endif
		}

		protected override void OnShow()
		{

		}

		protected override void OnUpdate(SAMTime gameTime, InputState istate)
		{
#if DEBUG
			DebugDisp.IsEnabled = DebugSettings.Get("DebugTextDisplay");
			DebugDisp.Scale = 0.75f;
#endif

			//
			
		}

		protected override void OnDrawGame(IBatchRenderer sbatch)
		{
			//
		}

		protected override void OnDrawHUD(IBatchRenderer sbatch)
		{
			//
		}
	}
}
