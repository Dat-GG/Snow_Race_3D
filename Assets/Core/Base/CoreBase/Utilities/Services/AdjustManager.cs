using com.adjust.sdk;
using com.adjust.sdk.purchase;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustManager : MonoBehaviour
{
	static string currency, TransactionId, productID;
	static double price;

	/// <summary>
	/// validateAndSendInAppPurchase_Android
	/// </summary>
	/// <param name="ItemSKU">Unique order ID (SKU)</param>
	/// <param name="ItemToken">A token that uniquely identifies a purchase</param>
	/// <param name="DeveloperPayload"> A developer-specified string that contains supplemental information about an order</param>
	public static void validateAndSendInAppPurchase_Android(string ItemSKU, string ItemToken, string DeveloperPayload, double price, string currency, string TransactionID, string ProductId)
	{
#if !UNITY_ANDROID
     return;
#endif

		AdjustManager.price = price;
		AdjustManager.currency = currency;
		AdjustManager.TransactionId = TransactionID;
		AdjustManager.productID = productID;
#if DEVELOPMENT
		Debug.Log("Adjust validate " + ItemSKU + " " + ItemToken + " " + DeveloperPayload);
		LogPurchaseTest();
#endif

		AdjustPurchase.VerifyPurchaseAndroid(ItemSKU, ItemToken, DeveloperPayload, VerificationInfoDelegate);
	}

	/// <summary>
	/// validateAndSendInAppPurchase_IOS
	/// </summary>
	/// <param name="Receipt">App receipt</param>
	/// <param name="TransactionID">Finished transaction identifier</param>
	/// <param name="ProductId">Your purchased product identifier</param>
	public static void validateAndSendInAppPurchase_IOS(string Receipt, string TransactionID, string ProductId, double price, string currency)
	{
#if !UNITY_IOS
		Debug.LogError("Adjust: not ios,return ");
		return;
#endif

		AdjustManager.price = price;
		AdjustManager.currency = currency;
		AdjustManager.TransactionId = TransactionID;
		AdjustManager.productID = ProductId;
#if DEVELOPMENT
		Debug.LogError("Adjust validate " + Receipt + " " + TransactionID + " " + ProductId);
		LogPurchaseTest();
#endif
		AdjustPurchase.VerifyPurchaseiOS(Receipt, TransactionID, ProductId, VerificationInfoDelegate);
	}

	public static void LogPurchaseTest()
	{

		Debug.LogError("Adjust Log test " + AdjustManager.price + " " + price);
		if (AdjustManager.price != 0)
		{
			AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.purchase_test);
			adjustEvent.setRevenue(price, AdjustManager.currency);
			adjustEvent.setTransactionId(AdjustManager.TransactionId);
			Adjust.trackEvent(adjustEvent);
			Debug.LogError("Adjust Log test done " + AdjustManager.currency + " " + AdjustManager.TransactionId);
		}
	}

	private static void VerificationInfoDelegate(ADJPVerificationInfo verificationInfo)
	{
		if (verificationInfo.VerificationState == ADJPVerificationState.ADJPVerificationStatePassed)
		{

			if (AdjustManager.price != 0)
			{
				AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.purchase);
				adjustEvent.setRevenue(price, AdjustManager.currency);
				adjustEvent.setTransactionId(AdjustManager.TransactionId);
				Adjust.trackEvent(adjustEvent);

				try
				{
				//	int level = GameData.Instance.MissionsGroup.CurrentMissionId;
				//	Config.LogEvent(TrackingConstants.IAP_EVENT, "pack", AdjustManager.productID, "level", level);
				}
				catch (Exception)
				{

					//throw;
				}

			}
		}
		else if (verificationInfo.VerificationState == ADJPVerificationState.ADJPVerificationStateFailed)
		{
			AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.purchase_failed);
			Adjust.trackEvent(adjustEvent);
		}
		else if (verificationInfo.VerificationState == ADJPVerificationState.ADJPVerificationStateUnknown)
		{
			AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.purchase_unknown);
			Adjust.trackEvent(adjustEvent);
		}
		else
		{
			AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.purchase_notverified);
			Adjust.trackEvent(adjustEvent);
		}
	}
	public static void LogLevelAchieved(int level)
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.cyber_level_achieved);
		adjustEvent.addPartnerParameter("level", level.ToString());
		//	adjustEvent.addPartnerParameter("foo", "bar");
		Adjust.trackEvent(adjustEvent);
	}
	public static void LogLevelClear(int level)
	{
		var purchaseEvent = new Dictionary<string, string>();
		if (level == 1001 || level == 1003 || level == 1005 || level == 1008 || level == 1010 || level == 2001 || level == 3001 || level == 4001)
		{
			string tokenEvent = AdjustEventConstants.cyber_level_1_1;
			switch (level)
			{
				case 1001: tokenEvent = AdjustEventConstants.cyber_level_1_1; break;
				case 1003: tokenEvent = AdjustEventConstants.cyber_level_1_3; break;
				case 1005: tokenEvent = AdjustEventConstants.cyber_level_1_5; break;
				case 1008: tokenEvent = AdjustEventConstants.cyber_level_1_8; break;
				case 1010: tokenEvent = AdjustEventConstants.cyber_level_1_10; break;
				case 2001: tokenEvent = AdjustEventConstants.cyber_level_2_1; break;
				case 3001: tokenEvent = AdjustEventConstants.cyber_level_3_1; break;
				case 4001: tokenEvent = AdjustEventConstants.cyber_level_4_1; break;
				default:
					tokenEvent = AdjustEventConstants.cyber_level_1_1;
					break;
			}
			AdjustEvent adjustEventLV = new AdjustEvent(tokenEvent);
			Adjust.trackEvent(adjustEventLV);
		}
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.cyber_level_clear);
		adjustEvent.addPartnerParameter("level", level.ToString());
		//	adjustEvent.addPartnerParameter("foo", "bar");
		Adjust.trackEvent(adjustEvent);
	}
	public static void LogOpenApp()
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.open_app);
		Adjust.trackEvent(adjustEvent);
	}
	public static void LogFirstOpenApp()
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.first_open_app);
		Adjust.trackEvent(adjustEvent);
	}
	public static void LogVisitShop()
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.cyber_visit_shop);
		Adjust.trackEvent(adjustEvent);
	}
	public static void LogAdView()
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.ad_view);
		Adjust.trackEvent(adjustEvent);
		try
		{
		//	int currentLevel = GameData.Instance.MissionsGroup.CurrentMissionId;
		//	Config.LogEvent(TrackingConstants.ADS_VIEW_USER, "level", currentLevel);
		}
		catch (Exception ex)
		{

		}

	}
	public static void LogAdClick()
	{
		AdjustEvent adjustEvent = new AdjustEvent(AdjustEventConstants.ad_click);
		Adjust.trackEvent(adjustEvent);
	}
}

public class AdjustEventConstants
{
	public const string purchase = "m96foh";
	public const string purchase_failed = "lbmdzl";
	public const string purchase_notverified = "1r60yr";
	public const string purchase_unknown = "dhyz79";

	public const string purchase_test = "uffp2t";

	public const string ad_view = "71cus6";
	public const string ad_click = "rz62jl";

	public const string first_open_app = "m8gceu";
	public const string open_app = "m7n44l";
	public const string cyber_visit_shop = "q20j4r";
	public const string cyber_level_clear = "b2x2zp";
	public const string cyber_level_achieved = "4v0kt4";

	public const string cyber_level_1_1 = "qp4dar";
	public const string cyber_level_1_10 = "dqa77t";
	public const string cyber_level_1_3 = "dqa77t";
	public const string cyber_level_1_5 = "ig6eyi";
	public const string cyber_level_1_8 = "tcsp7q";
	public const string cyber_level_2_1 = "kki01o";
	public const string cyber_level_3_1 = "en9gi0";
	public const string cyber_level_4_1 = "osyd1c";
}
