using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class EnvironmentBridgeMain : BehaviourSingleton<EnvironmentBridgeMain>, IManager
	{
		[SerializeField]
		private Camera m_uiMainCamera;

		public Transform Background;

		private Transform m_terrainMesh;

		private Transform m_floorMesh;

		private GameObject m_dimmingBackground;

		private GameObject m_floorStaticObject;

		private GameObject m_floorMatteShadow;

		private GameObject m_lensflare;

		private Camera m_gameCamera;

		private EarthGlobeData m_earthGlobeData;

		public Camera MainCamera
		{
			get
			{
				return m_gameCamera;
			}
		}

		public CameraMenuManager CameraMenuManager { get; private set; }

		public EarthTransitionManager EarthTransitionManager { get; private set; }

		public EarthSpinManager EarthSpinManager { get; private set; }

		public EarthGlobeData EarthGlobeData
		{
			get
			{
				if (m_earthGlobeData == null)
				{
					m_earthGlobeData = new EarthGlobeData();
				}
				return m_earthGlobeData;
			}
		}

		public Transform FloorMesh
		{
			get
			{
				return m_floorMesh;
			}
		}

		public GameObject FloorStaticObject
		{
			get
			{
				return m_floorStaticObject;
			}
		}

		public GameObject FloorMatteShadow
		{
			get
			{
				return m_floorMatteShadow;
			}
		}

		public GameObject DimmingBackground
		{
			get
			{
				return m_dimmingBackground;
			}
		}

		public bool Initialized { get; private set; }

		public void Initialize()
		{
		}

		public void PurgeManager()
		{
			Purge();
		}

		public static void ResetManager()
		{
			BehaviourSingleton<EnvironmentBridgeMain>.Reset();
		}

		public void CancelEarthTransitionAndZoomOut()
		{
			EarthTransitionManager.CancelTransition(CameraMenuManager, EarthSpinManager);
		}

		protected override void OnPurge()
		{
			if (m_earthGlobeData != null)
			{
				m_earthGlobeData.Clear();
			}
			m_earthGlobeData = null;
			EarthSpinManager = null;
			CameraMenuManager = null;
		}

		private void Awake()
		{
			Initialized = InitTerrain() & HookUpEnvironmentObjects() & EarthGlobeData.InitEarthGlobeObjects() & FindAndHideDummyObjects() & InitCamera();
			if (!Initialized)
			{
				Purge();
				return;
			}
			EarthGlobeData.Init(base.gameObject, m_gameCamera, m_uiMainCamera);
			CameraMenuManager = new CameraMenuManager(m_gameCamera, m_lensflare, CameraMenuManager.EType.Missions);
			EarthSpinManager = base.gameObject.AddComponentIfNotExist<EarthSpinManager>();
			EarthSpinManager.Init(EarthGlobeData);
			EarthSpinManager.LoadAndEnableAudio();
			EarthSpinManager.Current = EraExtensions.EEarthPin.Italy;
			EarthTransitionManager = new EarthTransitionManager();
		}

		private void Update()
		{
			if (Initialized)
			{
				CameraMenuManager.UpdateCamera();
				EarthSpinManager.UpdateGlobe();
				EarthTransitionManager.UpdateTransition(CameraMenuManager, EarthSpinManager);
			}
		}

		private bool HookUpEnvironmentObjects()
		{
			GameObject _target;
			TryFind("DebugMenu", out _target);
			if (!TryFind("EnvScenario/AnimusLensflare", out m_lensflare))
			{
				return false;
			}
			return true;
		}

		private bool InitTerrain()
		{
			GameObject _target;
			if (TryFind("EnvScenario/Terrain Billboard Mesh", out _target))
			{
				GameObject gameObject = Object.Instantiate(_target);
				GameObject gameObject2 = Object.Instantiate(_target);
				gameObject.transform.Translate(-60f, 0f, 0f);
				gameObject.transform.parent = _target.transform;
				gameObject2.transform.Translate(60f, 0f, 0f);
				gameObject2.transform.parent = _target.transform;
				m_terrainMesh = _target.transform;
			}
			GameObject _target2 = null;
			if (TryFind("EnvScenario/AnimusFloorIslands", out _target2))
			{
				m_floorMesh = _target2.transform;
			}
			GameObject _target3 = null;
			if (TryFind("EnvScenario/AnimusFloorIslands/IslandFloorStatic", out _target3))
			{
				m_floorStaticObject = _target3;
			}
			m_floorMatteShadow = Object.Instantiate(Resources.Load<GameObject>("UI/Animus/Widgets/Assassin/AssassinPreviewFloor/AssassinPreviewFloorPlane"));
			m_floorMatteShadow.transform.parent = m_floorStaticObject.transform.parent;
			m_floorMatteShadow.transform.position = new Vector3(-3.86f, 11.652f, -0.55f);
			m_floorMatteShadow.name = "AssassinPreviewFloorPlane";
			m_floorMatteShadow.SetActive(false);
			GameObject _target4 = null;
			if (TryFind("EnvScenario/AnimusDarkenBg_Assassins", out _target4))
			{
				m_dimmingBackground = _target4;
			}
			GameObject _target5;
			if (TryFind("EnvScenario/Background", out _target5))
			{
				Background = _target5.transform;
				if (m_terrainMesh != null)
				{
					m_terrainMesh.transform.parent = Background.transform;
				}
			}
			Bounds bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(100000f, 100000f, 100000f));
			OverrideBoundingBox[] array = Object.FindObjectsOfType<OverrideBoundingBox>();
			for (int i = 0; i < array.Length; i++)
			{
				MeshFilter[] componentsInChildren = array[i].gameObject.GetComponentsInChildren<MeshFilter>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].sharedMesh.bounds = bounds;
				}
			}
			return true;
		}

		private bool FindAndHideDummyObjects()
		{
			GameObject _target;
			if (!TryFind("EnvScenario/Menu Objects/Object Shop (Dummy)", out _target))
			{
				return false;
			}
			GameObject _target2;
			if (!TryFind("EnvScenario/Menu Objects/Object Social (Dummy)", out _target2))
			{
				return false;
			}
			GameObject _target3;
			if (!TryFind("EnvScenario/Menu Objects/Object Assassins (Dummy)", out _target3))
			{
				return false;
			}
			_target.SetActive(false);
			_target2.SetActive(false);
			_target2.SetActive(false);
			_target3.SetActive(false);
			return true;
		}

		private bool InitCamera()
		{
			m_gameCamera = Camera.main;
			if (m_gameCamera != null)
			{
				m_gameCamera.fieldOfView = 40f;
				m_gameCamera.gameObject.AddComponent<FlareLayer>();
				return true;
			}
			return false;
		}

		public static bool TryFind(string _path, out GameObject _target)
		{
			_target = GameObject.Find(_path);
			if (_target == null)
			{
				return false;
			}
			return true;
		}
	}
}
