using System;
using MonoSAMFramework.Portable.Extensions;

namespace SwitchGame.Shared.Resources
{
	public static class SGConstants
	{
		public static readonly Version Version = new Version(0,0,0,1);
		public static ulong IntVersion { get; } = Version.ToNum();
		
		public const int VIEW_WIDTH  = 1024;
		public const int VIEW_HEIGHT = 640;

		public static readonly string[] IABList =
		{
#if DEBUG
			MonoSAMFramework.Portable.DeviceBridge.AndroidBillingHelper.PID_CANCELED,
			MonoSAMFramework.Portable.DeviceBridge.AndroidBillingHelper.PID_PURCHASED,
			MonoSAMFramework.Portable.DeviceBridge.AndroidBillingHelper.PID_REFUNDED,
			MonoSAMFramework.Portable.DeviceBridge.AndroidBillingHelper.PID_UNAVAILABLE,
#endif
		};

		public const string LOGO_STRING = "?"; //TODO 
		public const string BFB_URL     = @"http://blackforestbytes.de/";
		public const string PROFILE_FILENAME = "USERPROFILE";

#if DEBUG
		public const string SERVER_URL = "http://localhost:666";
#else
		public const string SERVER_URL = "?"; //TODO
#endif
		public const string SERVER_SECRET = __Secrets.SERVER_SECRET;
	}
}
