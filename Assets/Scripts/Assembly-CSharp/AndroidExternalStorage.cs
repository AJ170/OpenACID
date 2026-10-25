using SharedBaseLib.Storage;
using UnityEngine;

public class AndroidExternalStorage : IAndroidExternalStorage
{
	private static AndroidJavaObject ms_nativeToolsObject;

	private static AndroidJavaObject NativeObject
	{
		get
		{
			if (ms_nativeToolsObject == null)
			{
				ms_nativeToolsObject = new AndroidJavaObject("com.acid.plugins.nativetools.NativeTools");
			}
			return ms_nativeToolsObject;
		}
	}

	public string SecondaryExternalStorage
	{
		get
		{
			return NativeObject.CallStatic<string>("_GetExternalStorageInfo", new object[0]);
		}
	}

	public bool IsDirectoryWritable(string _dir)
	{
		return NativeObject.CallStatic<bool>("_IsDirectoryWritable", new object[1] { _dir });
	}
}
