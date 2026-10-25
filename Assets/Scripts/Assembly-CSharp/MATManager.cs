using Assets.Scripts.Native;
using Assets.Scripts.UI.State;
using MATSDK;
using UnityEngine;

public class MATManager : MonoBehaviour
{
	private const string mkADVERTISER_ID = "6918";

	private const string mkCONVERSATION_KEY = "ab1642daf2afb6d3f8cf83b8c7df4b07";

	private const string mkMATInstallKey = "MATInstall";

	public static bool IsMATAvailable
	{
		get
		{
			return false;
		}
	}

	private void Awake()
	{
		if (IsMATAvailable)
		{
			Object.DontDestroyOnLoad(this);
			MATBinding.Init("6918", "ab1642daf2afb6d3f8cf83b8c7df4b07");
			MATBinding.SetDeviceId(NativeTools.DeviceUniqueId);
			MATBinding.SetUserId(Singleton<StateManager>.Instance.ProfileManager.Profile.id);
			MATBinding.SetGoogleAdvertisingId(NativeTools.AdvertisingID, NativeTools.IsLimitAdTrackingEnabled);
			MATBinding.MeasureSession();
		}
	}

	private void OnApplicationPause(bool isPaused)
	{
		if (IsMATAvailable && !isPaused)
		{
			MATBinding.MeasureSession();
		}
	}

	public static void LogPurchase(string productID, string ubiTransactionId, float rawPrice, string currency = "USD")
	{
		if (!IsMATAvailable)
		{
			return;
		}
		MATItem mATItem = new MATItem(productID);
		mATItem.unitPrice = rawPrice;
		mATItem.quantity = 1;
		double? unitPrice = mATItem.unitPrice;
		double? revenue;
		if (unitPrice.HasValue)
		{
			int? quantity = mATItem.quantity;
			if (quantity.HasValue)
			{
				revenue = unitPrice.Value * (double)quantity.Value;
				goto IL_007a;
			}
		}
		revenue = null;
		goto IL_007a;
		IL_007a:
		mATItem.revenue = revenue;
		mATItem.attribute1 = NativeTools.AdvertisingID;
		mATItem.attribute2 = ubiTransactionId;
		mATItem.attribute3 = currency;
		MATItem[] eventItems = new MATItem[1] { mATItem };
		MATEvent tuneEvent = new MATEvent("Purchase");
		tuneEvent.eventItems = eventItems;
		tuneEvent.revenue = rawPrice;
		tuneEvent.currencyCode = currency;
		MATBinding.MeasureEvent(tuneEvent);
	}

	public static void LogCompletedTutorial()
	{
		if (IsMATAvailable)
		{
			MATEvent tuneEvent = new MATEvent("Completed Tutorial");
			MATBinding.MeasureEvent(tuneEvent);
		}
	}
}
