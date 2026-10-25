using System;
using System.IO;
using BB.Mobile.Net;
using SharedBaseLib.Storage;
using SharedBaseLib.System.Cache;
using UbiCore.Utils;
using UnityEngine;

public class SharedMain : MonoBehaviour
{
	public const string ClearColorPropertyKey = "__ClearColor";

	private Camera m_inactiveCamera;

	private static SharedMain m_instance;

	private static bool m_hasInstance;

	public static bool IsShuttingDown { get; protected set; }

	public static SharedMain Instance
	{
		get
		{
			return m_instance;
		}
	}

	public IAndroidExternalStorage AndroidExternalStorage { get; protected set; }

	protected virtual void FixFilePermissionsImpl(string _fn)
	{
		File.SetAttributes(_fn, FileAttributes.Normal);
	}

	public static void SafeUnityInvoke(Delegate _method, params object[] _args)
	{
		if (UnityInvoke.Application.InvokeRequired)
		{
			UnityInvoke.Application.BeginInvoke(_method, _args, delegate(IAsyncResult ar)
			{
				try
				{
					UnityInvoke.Application.EndInvoke(ar);
				}
				catch (Exception)
				{
					throw;
				}
			}, null, ThreadPoolScheduler.DefaultScheduler);
		}
		else
		{
			_method.DynamicInvoke(_args);
		}
	}

	public static void FixFilePermissions(string _fn)
	{
		if (m_hasInstance)
		{
			SafeUnityInvoke((Action<string>)delegate(string s)
			{
				if (File.Exists(s))
				{
					m_instance.FixFilePermissionsImpl(s);
				}
			}, _fn);
		}
		else
		{
			File.SetAttributes(_fn, FileAttributes.Normal);
		}
	}

	protected virtual ulong GetFreeDiskspaceImpl()
	{
		return ulong.MaxValue;
	}

	public static ulong GetFreeDiskSpace()
	{
		if (m_hasInstance)
		{
			return m_instance.GetFreeDiskspaceImpl();
		}
		return ulong.MaxValue;
	}

	protected virtual void Awake()
	{
		if (!m_hasInstance)
		{
			m_instance = this;
			m_hasInstance = true;
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
			base.transform.localScale = Vector3.one;
			UnityEngine.Object.DontDestroyOnLoad(this);
			base.gameObject.AddComponent<UnityInvoke>();
			Singleton<Manager>.Initialize();
		}
	}

	private void OnDestroy()
	{
		IsShuttingDown = true;
		m_instance = null;
		m_hasInstance = false;
	}

	protected virtual void OnGUI()
	{
	}

	public static void ApplyDefaultClearColor(Camera _c)
	{
		switch (PlayerPrefs.GetInt("__ClearColor", 0))
		{
		case 0:
			_c.backgroundColor = Color.black;
			break;
		case 1:
			Camera.main.backgroundColor = Color.black;
			break;
		case 2:
			Camera.main.backgroundColor = Color.cyan;
			break;
		case 3:
			Camera.main.backgroundColor = Color.magenta;
			break;
		}
	}

	public GameObject CreateSharedCamera()
	{
		GameObject gameObject = new GameObject("Camera");
		gameObject.tag = "MainCamera";
		Camera camera = gameObject.AddComponent<Camera>();
		camera.clearFlags = CameraClearFlags.Color;
		camera.nearClipPlane = 0.1f;
		camera.farClipPlane = 1200f;
		camera.fieldOfView = 70f;
		camera.cullingMask = ~((1 << LayerMask.NameToLayer("ReflectionSkybox")) | (1 << LayerMask.NameToLayer("Invisible")));
		camera.depth = 0f;
		Camera[] componentsInChildren = Camera.main.GetComponentsInChildren<Camera>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			ApplyDefaultClearColor(componentsInChildren[i]);
		}
		gameObject.AddComponent<GUILayer>();
		return gameObject;
	}

	protected GameObject CreateFreeCamera()
	{
		GameObject obj = CreateSharedCamera();
		obj.AddComponent<FreeCameraController>();
		return obj;
	}

	private void OnToggleFreeCamera()
	{
		Camera main = Camera.main;
		if (main == null || main.GetComponent<FreeCameraController>() == null)
		{
			GameObject gameObject = CreateFreeCamera();
			if (main != null)
			{
				gameObject.transform.position = main.transform.position;
				gameObject.transform.rotation = main.transform.rotation;
			}
			gameObject.GetComponent<FreeCameraController>().enabled = false;
			Rendering.RefreshPostEffects(gameObject.GetComponent<Camera>());
		}
		if (main != null && main.GetComponent<FreeCameraController>() != null)
		{
			UnityEngine.Object.Destroy(main.gameObject);
			InputHandler.ToggleAllHandlers(false);
			if (m_inactiveCamera != null)
			{
				m_inactiveCamera.gameObject.SetActive(true);
				m_inactiveCamera = null;
			}
		}
		if (main != null && main.GetComponent<FreeCameraController>() == null)
		{
			m_inactiveCamera = main;
			m_inactiveCamera.gameObject.SetActive(false);
		}
	}
}
