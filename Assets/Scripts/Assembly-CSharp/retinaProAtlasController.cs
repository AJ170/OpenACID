using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Database;
using Database;
using UnityEngine;

public class retinaProAtlasController : MonoBehaviour
{
	public static retinaProAtlasController sharedInstance;

	public bool autoRefreshOnStartup = true;

	private retinaProDataRuntime rt;

	private int activeDeviceIdx;

	private int activeScreenIdx;

	public retinaProRuntimeDevice CurrentSelectedDevice
	{
		get
		{
			return (rt == null) ? null : rt.deviceList[activeDeviceIdx];
		}
	}

	private void Awake()
	{
		sharedInstance = this;
		rt = new retinaProDataRuntime();
		if (rt.loaded)
		{
			refreshActiveDevice();
			fixRoot();
		}
	}

	private void Start()
	{
		if (autoRefreshOnStartup)
		{
			fixAll();
		}
	}

	public void refreshAll()
	{
		refreshActiveDevice();
		fixAll();
	}

	private void fixAll()
	{
		if (activeDeviceIdx != -1 && activeScreenIdx != -1)
		{
			fixRoot();
			fixAtlases();
			fixFonts();
			Resources.UnloadUnusedAssets();
		}
	}

	public bool isRuntimeDeviceLoaded()
	{
		return rt.loaded;
	}

	private void checkAspect(ref retinaProRuntimeScreen rs, float aspect, float deviceAspect, bool _useLowAtlas, ref int i, ref int s, ref float bestAspectDiff, ref int bestIdx, ref int bestScreenIdx, ref int highestWidth)
	{
		float num = Mathf.Abs(aspect - deviceAspect);
		if (bestIdx == -1)
		{
			bestAspectDiff = num;
			bestIdx = i;
			bestScreenIdx = s;
			highestWidth = rs.width;
		}
		else if (num <= bestAspectDiff && ((_useLowAtlas && rs.width <= highestWidth) || rs.width >= highestWidth))
		{
			bestAspectDiff = num;
			bestIdx = i;
			bestScreenIdx = s;
			highestWidth = rs.width;
		}
	}

	public void refreshActiveDevice()
	{
		activeDeviceIdx = -1;
		activeScreenIdx = -1;
		float aspect = (float)Screen.width / (float)Screen.height;
		bool lowMemoryDevice = Main.Instance.LowMemoryDevice;
		if (!lowMemoryDevice)
		{
			for (int i = 0; i < rt.deviceList.Count; i++)
			{
				retinaProRuntimeDevice retinaProRuntimeDevice2 = rt.deviceList[i];
				for (int j = 0; j < retinaProRuntimeDevice2.screens.Count; j++)
				{
					retinaProRuntimeScreen retinaProRuntimeScreen2 = retinaProRuntimeDevice2.screens[j];
					if (retinaProRuntimeScreen2.useForBothLandscapePortrait)
					{
						if ((retinaProRuntimeScreen2.width == Screen.width && retinaProRuntimeScreen2.height == Screen.height) || (retinaProRuntimeScreen2.width == Screen.height && retinaProRuntimeScreen2.height == Screen.width))
						{
							activeDeviceIdx = i;
							activeScreenIdx = j;
							break;
						}
					}
					else if (retinaProRuntimeScreen2.width == Screen.width && retinaProRuntimeScreen2.height == Screen.height)
					{
						activeDeviceIdx = i;
						activeScreenIdx = j;
						break;
					}
				}
			}
		}
		if (activeDeviceIdx == -1)
		{
			int bestIdx = -1;
			int bestScreenIdx = -1;
			float bestAspectDiff = 0f;
			int highestWidth = 0;
			for (int k = 0; k < rt.deviceList.Count; k++)
			{
				retinaProRuntimeDevice retinaProRuntimeDevice3 = rt.deviceList[k];
				for (int l = 0; l < retinaProRuntimeDevice3.screens.Count; l++)
				{
					retinaProRuntimeScreen rs = retinaProRuntimeDevice3.screens[l];
					float deviceAspect = (float)rs.width / (float)rs.height;
					checkAspect(ref rs, aspect, deviceAspect, lowMemoryDevice, ref k, ref l, ref bestAspectDiff, ref bestIdx, ref bestScreenIdx, ref highestWidth);
					if (rs.useForBothLandscapePortrait)
					{
						deviceAspect = (float)rs.height / (float)rs.width;
						checkAspect(ref rs, aspect, deviceAspect, lowMemoryDevice, ref k, ref l, ref bestAspectDiff, ref bestIdx, ref bestScreenIdx, ref highestWidth);
					}
				}
			}
			activeDeviceIdx = bestIdx;
			activeScreenIdx = bestScreenIdx;
		}
		if (activeDeviceIdx != -1 && activeScreenIdx == -1)
		{
		}
	}

	public void fixRoot()
	{
		int num;
		int num2;
		bool flag;
		if (rt.deviceList[activeDeviceIdx].rootAuto)
		{
			num = rt.deviceList[activeDeviceIdx].screens[activeScreenIdx].width;
			num2 = rt.deviceList[activeDeviceIdx].screens[activeScreenIdx].height;
			flag = rt.deviceList[activeDeviceIdx].screens[activeScreenIdx].useForBothLandscapePortrait;
		}
		else
		{
			num = rt.deviceList[activeDeviceIdx].rootWidth;
			num2 = rt.deviceList[activeDeviceIdx].rootHeight;
			flag = rt.deviceList[activeDeviceIdx].rootUseBothPortLand;
		}
		int num3 = (int)((float)num2 * rt.deviceList[activeDeviceIdx].pixelSize);
		if (flag)
		{
			float num4 = (float)Screen.width / (float)Screen.height;
			float num5 = (float)rt.deviceList[activeDeviceIdx].screens[activeScreenIdx].width / (float)rt.deviceList[activeDeviceIdx].screens[activeScreenIdx].height;
			float num6 = Mathf.Abs(num4 - num5);
			if (num6 > 0.01f)
			{
				num3 = (int)((float)num * rt.deviceList[activeDeviceIdx].pixelSize);
			}
		}
		UIRoot[] array = Resources.FindObjectsOfTypeAll(typeof(UIRoot)) as UIRoot[];
		if (array == null || array.Length == 0)
		{
			return;
		}
		UIRoot[] array2 = array;
		foreach (UIRoot uIRoot in array2)
		{
			retinaProIgnoreUIRoot component = uIRoot.gameObject.GetComponent<retinaProIgnoreUIRoot>();
			if (!(component != null) || !component.ignoreUIRoot)
			{
				uIRoot.scalingStyle = UIRoot.Scaling.Constrained;
				uIRoot.manualHeight = num3;
				uIRoot.minimumHeight = num3;
				uIRoot.maximumHeight = num3;
			}
		}
	}

	public void fixAtlases()
	{
		UIAtlas[] array = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
		for (int i = 0; i < array.Length; i++)
		{
			UIAtlas atlasRef = array[i];
			fixAtlas(ref atlasRef);
		}
	}

	public void fixFonts()
	{
		UIFont[] array = Resources.FindObjectsOfTypeAll(typeof(UIFont)) as UIFont[];
		for (int i = 0; i < array.Length; i++)
		{
			UIFont fontRef = array[i];
			fixFont(ref fontRef);
		}
	}

	public void fixAtlas(ref UIAtlas atlasRef)
	{
		retinaProParent component = atlasRef.gameObject.GetComponent<retinaProParent>();
		if (component == null)
		{
			return;
		}
		retinaProRuntimeDevice retinaProRuntimeDevice2 = rt.deviceList[activeDeviceIdx];
		string text = atlasRef.name + "~" + retinaProRuntimeDevice2.name;
		bool flag = false;
		if (atlasRef.replacement == null)
		{
			flag = true;
		}
		else if (atlasRef.replacement.name.CompareTo(text) != 0)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		string path = retinaProDataRuntime.atlasFolder + retinaProRuntimeDevice2.name + "/" + text;
		UIAtlas uIAtlas = null;
		if (Main.IsInitialized && Main.Instance.GameDB != null)
		{
			List<DBEntry> list = new List<DBEntry>(Main.Instance.GameDB.GetSubDB(typeof(UIAtlasDefinition)).Data.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Name == Path.GetFileNameWithoutExtension(path))
				{
					GameObject gameObject = ((UIAtlasDefinition)list[i]).Atlas.AccessObject<GameObject>();
					uIAtlas = gameObject.GetComponent<UIAtlas>();
					break;
				}
			}
		}
		if (uIAtlas == null)
		{
			uIAtlas = Resources.Load(path, typeof(UIAtlas)) as UIAtlas;
		}
		if (uIAtlas != null)
		{
			atlasRef.replacement = uIAtlas;
		}
	}

	public void fixFont(ref UIFont fontRef)
	{
		retinaProParent component = fontRef.gameObject.GetComponent<retinaProParent>();
		if (component == null)
		{
			return;
		}
		retinaProRuntimeDevice retinaProRuntimeDevice2 = rt.deviceList[activeDeviceIdx];
		string text = fontRef.name + "~" + retinaProRuntimeDevice2.name;
		bool flag = false;
		if (fontRef.replacement == null)
		{
			flag = true;
		}
		else if (fontRef.replacement.name.CompareTo(text) != 0)
		{
			flag = true;
		}
		if (flag)
		{
			string path = retinaProDataRuntime.atlasFolder + retinaProRuntimeDevice2.name + "/" + text;
			UIFont uIFont = Resources.Load(path, typeof(UIFont)) as UIFont;
			if (uIFont != null)
			{
				fontRef.replacement = uIFont;
			}
		}
	}

	public void fixAtlasOrFont(GameObject go)
	{
		UIAtlas atlasRef = go.GetComponent<UIAtlas>();
		if ((bool)atlasRef)
		{
			fixAtlas(ref atlasRef);
		}
		UIFont fontRef = go.GetComponent<UIFont>();
		if ((bool)fontRef)
		{
			fixFont(ref fontRef);
		}
	}

	private IEnumerator fixPixelPerfect()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		NGUITools.Broadcast("MakePixelPerfect");
		yield return null;
	}
}
