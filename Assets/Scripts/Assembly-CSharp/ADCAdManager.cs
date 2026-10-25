using System;
using System.Collections.Generic;
using UnityEngine;

public class ADCAdManager : MonoBehaviour
{
	private static ADCAdManager _instance;

	public int onVideoFinishedCounter;

	public int onVideoFinishedWithInfoCounter;

	public int onV4VCResultCounter;

	public int onAdAvailabilityChangeCounter;

	public int regularCurrency;

	public string version = "1.1";

	public string appId = string.Empty;

	public Dictionary<string, ADCVideoZone> videoZones = new Dictionary<string, ADCVideoZone>();

	public static ADCAdManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType(typeof(ADCAdManager)) as ADCAdManager;
				if (_instance == null)
				{
					_instance = new GameObject("ADCAdManager").AddComponent<ADCAdManager>();
				}
			}
			return _instance;
		}
	}

	public int GetCounter(string counterName)
	{
		switch (counterName)
		{
		case "VideoFinished":
			return onVideoFinishedCounter;
		case "VideoFinishedWithInfo":
			return onVideoFinishedWithInfoCounter;
		case "V4VC":
			return onV4VCResultCounter;
		case "AdAvailable":
			return onAdAvailabilityChangeCounter;
		default:
			return -1;
		}
	}

	private void Awake()
	{
		ConfigureADCPlugin();
		AddOnVideoStartedMethod(OnVideoStarted);
		AddOnVideoFinishedMethod(OnVideoFinished);
		AddOnVideoFinishedWithInfoMethod(OnVideoFinishedWithInfo);
		AddOnV4VCResultMethod(OnV4VCResult);
		AddOnAdAvailabilityChangeMethod(OnAdAvailabilityChange);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Pause()
	{
		GameObject gameObject = GameObject.Find("MusicGameObject");
		if (gameObject != null)
		{
			gameObject.GetComponent<AudioSource>().Pause();
		}
	}

	public void Resume()
	{
		GameObject gameObject = GameObject.Find("MusicGameObject");
		if (gameObject != null)
		{
			gameObject.GetComponent<AudioSource>().Play();
		}
	}

	public void ConfigureADCPlugin()
	{
		ConfigureZones();
		AdColony.Configure(version, appId, GetVideoZoneIdsAsStringArray());
	}

	public void ConfigureZones()
	{
		appId = "app185a7e71e1714831a49ec7";
		AddZoneToManager("VideoZone1", "vz06e8c32a037749699e7050", ADCVideoZoneType.Interstitial);
		AddZoneToManager("VideoZone2", "vz06e8c32a037749699e7050", ADCVideoZoneType.Interstitial);
		AddZoneToManager("V4VCZone1", "vz1fd5a8b2bf6841a0a4b826", ADCVideoZoneType.V4VC);
	}

	private void OnVideoStarted()
	{
		Pause();
	}

	private void OnVideoFinished(bool adWasShown)
	{
		onVideoFinishedCounter++;
		Debug.Log("On Video Finished Counter " + onVideoFinishedCounter);
		Debug.Log("On Video Finished, and Ad was shown: " + adWasShown);
		Resume();
	}

	private void OnVideoFinishedWithInfo(AdColonyAd ad)
	{
		onVideoFinishedWithInfoCounter++;
		Debug.Log("On Video Finished With Info, ad Played: " + ad.toString());
		if (ad.iapEnabled)
		{
			AdColony.NotifyIAPComplete("ProductID", "TransactionID", null, 0.0, 1);
		}
		Resume();
	}

	private void OnV4VCResult(bool success, string name, int amount)
	{
		onV4VCResultCounter++;
		if (success)
		{
			Debug.Log("V4VC SUCCESS: name = " + name + ", amount = " + amount);
			AddToCurrency(amount);
		}
		else
		{
			Debug.LogWarning("V4VC FAILED!");
		}
	}

	private void OnAdAvailabilityChange(bool avail, string zoneId)
	{
		onAdAvailabilityChangeCounter++;
		Debug.Log("Ad Availability Changed to available=" + avail + " In zone: " + zoneId);
	}

	public static void AddOnVideoStartedMethod(AdColony.VideoStartedDelegate onVideoStarted)
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Combine(AdColony.OnVideoStarted, onVideoStarted);
	}

	public static void RemoveOnVideoStartedMethod(AdColony.VideoStartedDelegate onVideoStarted)
	{
		AdColony.OnVideoStarted = (AdColony.VideoStartedDelegate)Delegate.Remove(AdColony.OnVideoStarted, onVideoStarted);
	}

	public static void AddOnVideoFinishedMethod(AdColony.VideoFinishedDelegate onVideoFinished)
	{
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Combine(AdColony.OnVideoFinished, onVideoFinished);
	}

	public static void RemoveOnVideoFinishedMethod(AdColony.VideoFinishedDelegate onVideoFinished)
	{
		AdColony.OnVideoFinished = (AdColony.VideoFinishedDelegate)Delegate.Remove(AdColony.OnVideoFinished, onVideoFinished);
	}

	public static void AddOnVideoFinishedWithInfoMethod(AdColony.VideoFinishedWithInfoDelegate onVideoFinishedWithInfo)
	{
		AdColony.OnVideoFinishedWithInfo = (AdColony.VideoFinishedWithInfoDelegate)Delegate.Combine(AdColony.OnVideoFinishedWithInfo, onVideoFinishedWithInfo);
	}

	public static void RemoveOnVideoFinishedWithInfoMethod(AdColony.VideoFinishedWithInfoDelegate onVideoFinishedWithInfo)
	{
		AdColony.OnVideoFinishedWithInfo = (AdColony.VideoFinishedWithInfoDelegate)Delegate.Remove(AdColony.OnVideoFinishedWithInfo, onVideoFinishedWithInfo);
	}

	public static void AddOnV4VCResultMethod(AdColony.V4VCResultDelegate onV4VCResult)
	{
		AdColony.OnV4VCResult = (AdColony.V4VCResultDelegate)Delegate.Combine(AdColony.OnV4VCResult, onV4VCResult);
	}

	public static void RemoveOnV4VCResultMethod(AdColony.V4VCResultDelegate onV4VCResult)
	{
		AdColony.OnV4VCResult = (AdColony.V4VCResultDelegate)Delegate.Remove(AdColony.OnV4VCResult, onV4VCResult);
	}

	public static void AddOnAdAvailabilityChangeMethod(AdColony.AdAvailabilityChangeDelegate onAdAvailabilityChange)
	{
		AdColony.OnAdAvailabilityChange = (AdColony.AdAvailabilityChangeDelegate)Delegate.Combine(AdColony.OnAdAvailabilityChange, onAdAvailabilityChange);
	}

	public static void RemoveOnAdAvailabilityChangeMethod(AdColony.AdAvailabilityChangeDelegate onAdAvailabilityChange)
	{
		AdColony.OnAdAvailabilityChange = (AdColony.AdAvailabilityChangeDelegate)Delegate.Remove(AdColony.OnAdAvailabilityChange, onAdAvailabilityChange);
	}

	public static void AddToCurrency(int amountToAddToCurrency)
	{
		Instance.regularCurrency += amountToAddToCurrency;
	}

	public static int GetRegularCurrencyAmount()
	{
		return Instance.regularCurrency;
	}

	public static void ResetADCAdManagerZones()
	{
		GetVideoZonesDictionary().Clear();
	}

	public static void AddZoneToManager(string zoneKey, string zoneId, ADCVideoZoneType videoZoneType)
	{
		zoneKey = zoneKey.ToLower();
		if (ContainsZoneKey(zoneKey))
		{
			Debug.LogWarning("The ad manager overwrote the previous video zoneId: " + GetZoneIdByKey(zoneKey) + " for the video zone named " + zoneKey + " with the new video zoneId of: " + zoneId);
		}
		else
		{
			Debug.LogWarning("The ad manager has added the video zone named " + zoneKey + " with the video zoneId of: " + zoneId);
			GetVideoZonesDictionary().Add(zoneKey, new ADCVideoZone(zoneId, videoZoneType));
		}
	}

	public static ADCVideoZone GetVideoZoneObjectByKey(string key)
	{
		key = key.ToLower();
		if (ContainsZoneKey(key))
		{
			return GetVideoZonesDictionary()[key];
		}
		return null;
	}

	public static string GetZoneIdByKey(string key)
	{
		key = key.ToLower();
		if (ContainsZoneKey(key))
		{
			return GetVideoZonesDictionary()[key].zoneId;
		}
		return string.Empty;
	}

	public static bool ContainsZoneKey(string key)
	{
		key = key.ToLower();
		if (GetVideoZonesDictionary().ContainsKey(key))
		{
			return true;
		}
		return false;
	}

	public static void RemoveZoneFromManager(string zoneKey)
	{
		zoneKey = zoneKey.ToLower();
		GetVideoZonesDictionary().Remove(zoneKey);
	}

	public static string[] GetVideoZoneIdsAsStringArray()
	{
		Dictionary<string, ADCVideoZone> videoZonesDictionary = GetVideoZonesDictionary();
		string[] array = new string[GetVideoZonesDictionary().Count];
		int num = 0;
		foreach (KeyValuePair<string, ADCVideoZone> item in videoZonesDictionary)
		{
			array[num] = item.Value.zoneId;
			num++;
		}
		return array;
	}

	public static Dictionary<string, ADCVideoZone> GetVideoZonesDictionary()
	{
		return Instance.videoZones;
	}

	public static void ShowVideoAdByZoneKey(string zoneIdKey, bool offerV4VCBeforePlay = false, bool showPopUpAfter = false)
	{
		ADCVideoZone videoZoneObjectByKey = GetVideoZoneObjectByKey(zoneIdKey);
		string zoneIdByKey = GetZoneIdByKey(zoneIdKey);
		if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.Interstitial && AdColony.IsVideoAvailable(zoneIdByKey))
		{
			AdColony.ShowVideoAd(zoneIdByKey);
		}
		else if (videoZoneObjectByKey.zoneType == ADCVideoZoneType.V4VC && AdColony.IsV4VCAvailable(zoneIdByKey))
		{
			if (offerV4VCBeforePlay)
			{
				AdColony.OfferV4VC(showPopUpAfter, zoneIdByKey);
			}
			else
			{
				AdColony.ShowV4VC(showPopUpAfter, zoneIdByKey);
			}
		}
		else
		{
			Debug.Log("AdColony ---- The zone '" + zoneIdByKey + "' was requested to play, but it is NOT ready to play yet.");
		}
	}
}
