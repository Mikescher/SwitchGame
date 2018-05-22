using Foundation;
using SwitchGame.Shared;
using MonoSAMFramework.Portable;
using UIKit;
using SwitchGame.iOS.Impl;

namespace SwitchGame.iOS
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		public static MainGame game;
		public static AppleBridge _impl;

		internal static void RunGame()
		{
			MonoSAMGame.StaticBridge = _impl = new AppleBridge();
			game = new MainGame();
			game.Run();
		}

		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}

		public override void FinishedLaunching(UIApplication app)
		{
			RunGame();
		}
	}
}
