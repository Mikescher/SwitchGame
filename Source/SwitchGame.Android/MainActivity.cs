﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using SwitchGame.Shared;
using Microsoft.Xna.Framework;
using Android.Content;
using SwitchGame.Android.Impl;
using MonoSAMFramework.Portable.LogProtocol;
using MonoSAMFramework.Portable;

namespace SwitchGame.Android
{
	[Activity(Label = "Switch Game",
		MainLauncher = true,
		Icon = "@drawable/icon",
		Theme = "@style/Theme.Splash",
		LaunchMode = LaunchMode.SingleInstance,
		ScreenOrientation = ScreenOrientation.SensorLandscape,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard | ConfigChanges.ScreenSize)]

	// ReSharper disable once ClassNeverInstantiated.Global
	public class MainActivity : AndroidGameActivity
	{
		private AndroidBridge_Full _impl;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_impl = new AndroidBridge(this);
			MonoSAMGame.StaticBridge = _impl;
			var g = new MainGame();
			SetContentView(g.Services.GetService<View>());
			g.Run();
		}

		protected override void OnDestroy()
		{
			try
			{
				_impl.OnDestroy();

				base.OnDestroy();
			}
			catch (Exception e)
			{
				SAMLog.Error("AMA::OnDestroy", e);
			}
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			try
			{
				SAMLog.Debug("AMA::OnActivityResult(" + data?.Action + ")");

				_impl.HandleActivityResult(requestCode, resultCode, data);
			}
			catch (Exception e)
			{
				SAMLog.Error("AMA::OnActivityResult", e);
			}
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{
			for (int i = 0; i < Math.Min(permissions.Length, grantResults.Length); i++)
			{
				SAMLog.Debug($"PermissionRequestResult {permissions[i]} = {grantResults[i]}");
			}
		}
	}
}


