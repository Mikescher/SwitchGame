﻿using MonoSAMFramework.Portable.DeviceBridge;
using System.Collections.Generic;

namespace SwitchGame.Windows
{
	class WindowsEmulatingBillingAdapter : IBillingAdapter
	{
		public bool IsConnected => true;

		private readonly List<string> _purchased = new List<string>();

		public bool Connect(string[] productIDs)
		{
			return true;
		}

		public void Disconnect()
		{
			//
		}

		public PurchaseQueryResult IsPurchased(string id)
		{
			return _purchased.Contains(id) ? PurchaseQueryResult.Purchased : PurchaseQueryResult.NotPurchased;
		}

		public PurchaseResult StartPurchase(string id)
		{
			_purchased.Add(id);
			return PurchaseResult.PurchaseStarted;
		}
	}
}
