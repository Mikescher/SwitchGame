using MonoSAMFramework.Portable.DeviceBridge;

namespace SwitchGame.Shared.DeviceBridge
{
	public interface ISGOperatingSystemBridge : ISAMOperatingSystemBridge
	{
		string AppType { get; }

		IBillingAdapter IAB { get; }

		void OpenURL(string url);
		void ShareAppLink();
	}
}
