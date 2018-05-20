using SwitchGame.Shared;
using System;
using SwitchGame.Windows;
using MonoSAMFramework.Portable;

namespace SwitchGame.OpenGL
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			MonoSAMGame.StaticBridge = new WindowsBridge();

			using (var game = new MainGame()) game.Run();
		}
	}
}
