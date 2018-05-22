using System;

namespace SwitchGame.Shared.Resources
{
	public static class __Secrets
	{
		public const string SERVER_SECRET = "?";

		public static readonly string BILLING_PUBLIC_KEY = "?";

		public static string UnlockCode() => UnlockCode(DateTime.UtcNow);
		
		public static string UnlockCode(DateTime dn) => "?";

		public static bool TestUnlockCode(string number)
		{
			if (number == UnlockCode(DateTime.UtcNow)) return true;
			if (number == UnlockCode(DateTime.UtcNow.AddMinutes(-10))) return true;

			return false;
		}
	}
}