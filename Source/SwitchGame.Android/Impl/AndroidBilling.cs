﻿using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using MonoSAMFramework.Portable.DeviceBridge;
using MonoSAMFramework.Portable.LogProtocol;
using Xamarin.InAppBilling;

namespace SwitchGame.Android.Impl
{
	// https://components.xamarin.com/gettingstarted/xamarin.inappbilling
	class AndroidBilling : IBillingAdapter
	{
		private readonly string PUBLIC_KEY = __Secrets.BILLING_PUBLIC_KEY;
		
		private readonly MainActivity _activity;
		private InAppBillingServiceConnection _serviceConnection;

		public bool IsConnected => (_serviceConnection != null && _serviceConnection.Connected);

		private List<Product> _products;
		private List<Purchase> _purchases;
		private string[] _productIDs;
		private bool _isInitializing = false;

		public AndroidBilling(MainActivity a)
		{
			SAMLog.Debug("AndroidBilling.ctr");
			_activity = a;
		}

		public void HandleActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if (_serviceConnection == null) return;
			if (_serviceConnection.BillingHandler == null) return;

			_serviceConnection.BillingHandler.HandleActivityResult(requestCode, resultCode, data);
		}

		public bool Connect(string[] productIDs)
		{
			try
			{
				SAMLog.Debug("AndroidBilling.Connect");

				_purchases = null;
				_productIDs = productIDs;

				if (IsConnected) Disconnect();

				_serviceConnection = new InAppBillingServiceConnection(_activity, PUBLIC_KEY);

				_serviceConnection.OnConnected += OnConnected;

				_serviceConnection.Connect(); // throws Exception
				return true;
			}
			catch (Exception e)
			{
				SAMLog.Info("IAB::Connect", e);
				return false;
			}
		}

		public PurchaseQueryResult IsPurchased(string id)
		{
			try
			{
				if (_isInitializing) return PurchaseQueryResult.CurrentlyInitializing;

				if (!IsConnected || _purchases == null) return PurchaseQueryResult.NotConnected;

				int result = -1;
				foreach (var purch in _purchases.Where(p => p.ProductId == id).OrderBy(p => p.PurchaseTime))
				{
					result = purch.PurchaseState;
					if (purch.PurchaseState == AndroidBillingHelper.STATE_PURCHASED) return PurchaseQueryResult.Purchased;
				}

				if (result == -1) return PurchaseQueryResult.NotPurchased;
				if (result == AndroidBillingHelper.STATE_CANCELLED) return PurchaseQueryResult.Cancelled;
				if (result == AndroidBillingHelper.STATE_REFUNDED) return PurchaseQueryResult.Refunded;

				SAMLog.Error("IAB::IsPurchased-Inv", "result has invalid value: " + result);
				return PurchaseQueryResult.Error;
			}
			catch (Exception e)
			{
				SAMLog.Error("IAB::IsPurchased-Ex", e);
				return PurchaseQueryResult.Error;
			}
		}

		public PurchaseResult StartPurchase(string id)
		{
			SAMLog.Debug($"AndroidBilling.StartPurchase({id})");

			//  You must make the call to BuyProduct from the main thread of your Activity or it will fail.

			if (_isInitializing) return PurchaseResult.CurrentlyInitializing;
			if (!IsConnected) return PurchaseResult.NotConnected;

			var prod = _products.FirstOrDefault(p => p.ProductId == id);
			if (prod == null) return PurchaseResult.ProductNotFound;

			_serviceConnection.BillingHandler.BuyProduct(prod);
			return PurchaseResult.PurchaseStarted;
		}

		public void Disconnect()
		{
			try
			{
				SAMLog.Debug($"AndroidBilling.Disconnect");

				if (IsConnected) _serviceConnection.Disconnect();
			}
			catch (Exception e)
			{
				SAMLog.Error("IAB::Disconnect", e);
			}
		}

		private void OnConnected()
		{
			try
			{
				_isInitializing = true;
				SAMLog.Debug($"AndroidBilling.OnConnected[1]");

				_serviceConnection.BillingHandler.QueryInventoryError += QueryInventoryError;
				_serviceConnection.BillingHandler.BuyProductError += BuyProductError;
				_serviceConnection.BillingHandler.OnProductPurchased += OnProductPurchased;
				_serviceConnection.BillingHandler.OnProductPurchasedError += OnProductPurchasedError;
				_serviceConnection.BillingHandler.OnPurchaseFailedValidation += OnPurchaseFailedValidation;

				_purchases = _serviceConnection.BillingHandler.GetPurchases(ItemType.Product).ToList();
				SAMLog.Debug($"AndroidBilling.OnConnected[2]");


				_products = _serviceConnection.BillingHandler.QueryInventoryAsync(_productIDs.ToList(), ItemType.Product).Result.ToList();
				SAMLog.Debug($"AndroidBilling.OnConnected[3]");
			}
			catch (Exception e)
			{
				SAMLog.Info("IAB::OnConnected", e);
				_serviceConnection = null;
			}
			finally
			{
				_isInitializing = false;
			}
		}

		private void QueryInventoryError(int responseCode, Bundle skuDetails)
		{
			SAMLog.Info("IAB::QueryInventoryError", $"QueryInventoryError error code={responseCode}");
			Disconnect();
		}

		private void BuyProductError(int responseCode, string sku)
		{
			SAMLog.Info("IAB::BuyProductError", $"BuyProductError error code={responseCode}");
		}

		private void OnProductPurchasedError(int responseCode, string sku)
		{
			SAMLog.Info("IAB::OnProductPurchasedError", $"OnProductPurchasedError error code={responseCode}");
		}

		private void OnPurchaseFailedValidation(Purchase purchase, string purchaseData, string purchaseSignature)
		{
			SAMLog.Info("IAB::OnPurchaseFailedValidation", $"OnPurchaseFailedValidation error id={purchase.ProductId}");
		}

		private void OnProductPurchased(int response, Purchase purchase, string purchaseData, string purchaseSignature)
		{
			SAMLog.Debug($"OnProductPurchased({response}, {purchaseData}, {purchaseSignature})");

			_purchases.Add(purchase);
		}
	}
}