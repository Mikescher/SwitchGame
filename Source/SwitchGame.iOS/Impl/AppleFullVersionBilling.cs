using MonoSAMFramework.Portable.DeviceBridge;
using MonoSAMFramework.Portable.LogProtocol;

namespace SwitchGame.iOS.Impl
{
	class AppleFullVersionBilling : IBillingAdapter
	{
		public bool IsConnected => true;

		public bool Connect(string[] productIDs)
		{
			return true;
		}
		
		public void Disconnect()
		{
			//
		}

		public PurchaseResult StartPurchase(string id)
		{
			SAMLog.Error("IAB_IOS::StartPurchase", $"Calling StartPurchase({id}) in full version");
			return PurchaseResult.PurchaseStarted;
		}

		public PurchaseQueryResult IsPurchased(string id)
		{
			SAMLog.Error("IAB_IOS::IsPurchased", $"Calling IsPurchased({id}) in full version");
			return PurchaseQueryResult.Purchased;
		}
	}
}