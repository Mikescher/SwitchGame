using SwitchGame.Shared;
using SwitchGame.Windows;
using MonoSAMFramework.Portable;
using System;

namespace SwitchGame.DirectX
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			MonoSAMGame.StaticBridge = new WindowsBridge();

			using (var game = new MainGame()) game.Run();
		}
	}
}

