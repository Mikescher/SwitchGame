using MonoSAMFramework.Portable.GameMath.Geometry;
using MonoSAMFramework.Portable.Input;
using MonoSAMFramework.Portable.Screens;
using MonoSAMFramework.Portable.Screens.Entities;

namespace SwitchGame.Shared.Screens.MainMenuScreen
{
	class MainMenuEntityManager : EntityManager
	{
		public MainMenuEntityManager(MainMenuScreen screen) : base(screen)
		{
		}

		public override void DrawOuterDebug()
		{
			// NOP
		}

		protected override FRectangle RecalculateBoundingBox()
		{
			return Owner.VAdapterGame.VirtualTotalBoundingBox;
		}

		protected override void OnBeforeUpdate(SAMTime gameTime, InputState state)
		{
			// NOP
		}

		protected override void OnAfterUpdate(SAMTime gameTime, InputState state)
		{
			// NOP
		}
	}
}