using Foundation;
using SwitchGame.iOS.Full.Impl;
using SwitchGame.Shared;
using MonoSAMFramework.Portable;
using UIKit;

namespace SwitchGame.iOS.Full
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
