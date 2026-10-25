using System;
using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class CameraMenuManager
	{
		private class LensFlareLocation
		{
			public Vector3 Position { get; set; }

			public LensFlareLocation(Vector3 _position)
			{
				Position = _position;
			}
		}

		private class CameraLocation
		{
			private Vector3 m_zoomPosition;

			private Vector3 m_zoomLookAt;

			private Vector3 m_position;

			private Vector3 m_lookAt;

			public long PositionSpeed { get; set; }

			public long LookatSpeed { get; set; }

			public bool InstantTransition { get; set; }

			public CameraLocation(Vector3 _position, Vector3 _lookAt, Vector3 _zoomPosition, Vector3 _zoomLookAt, long _positionSpeed, long _lookatSpeed, bool _instantTransition = false)
			{
				m_position = _position;
				m_lookAt = _lookAt;
				m_zoomPosition = _zoomPosition;
				m_zoomLookAt = _zoomLookAt;
				PositionSpeed = _positionSpeed;
				LookatSpeed = _lookatSpeed;
				InstantTransition = _instantTransition;
			}

			public CameraLocation(Vector3 _position, Vector3 _lookAt, long _positionSpeed, long _lookatSpeed, bool _instantTransition = false)
			{
				m_position = _position;
				m_lookAt = _lookAt;
				m_zoomPosition = _position;
				m_zoomLookAt = _lookAt;
				PositionSpeed = _positionSpeed;
				LookatSpeed = _lookatSpeed;
				InstantTransition = _instantTransition;
			}

			public Vector3 GetPosition(bool _isZoomed)
			{
				Vector3 result = ((!_isZoomed) ? m_position : m_zoomPosition);
				if (_isZoomed)
				{
					result.z += BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthSpinManager.CurrentZoomOffset;
				}
				return result;
			}

			public Vector3 GetLookAt(bool _isZoomed)
			{
				return (!_isZoomed) ? m_lookAt : m_zoomLookAt;
			}
		}

		public enum EType
		{
			None = -1,
			Missions = 0,
			MissionsEra = 1,
			MissionsEraLevel = 2,
			Assassins = 3,
			AssassinsNaming = 4,
			AssassinsInventory = 5,
			AssassinsStartTweenRight = 6,
			AssassinsStartTweenLeft = 7,
			AssassinsQuickSkills = 8,
			AssassinsSkillTree = 9,
			AssassinsStatistics = 16,
			Social = 10,
			Shop = 11,
			Codex = 12,
			AccountCreation = 14,
			WelcomeScreen = 15,
			AccountCreationCreateOrLogin = 17,
			AccountCreationTypeName = 18,
			AccountCreationConfirm = 19,
			Friends = 20,
			NewsFeed = 21,
			Credits = 13,
			MysteryBox = 23,
			Events = 24
		}

		public delegate void ChangeStartedDelegate(EType _eCurrent, EType _eOld);

		public delegate void ChangeEndedDelegate(EType _eCurrent, EType _eOld);

		private const float c_nextAssassinOffset = 12f;

		private EType m_current = EType.None;

		private readonly Dictionary<int, CameraLocation> m_cameraLocations = new Dictionary<int, CameraLocation>();

		private readonly Dictionary<int, LensFlareLocation> m_lensFlareLocations = new Dictionary<int, LensFlareLocation>();

		private readonly List<SmoothPositionTransition> m_transitions = new List<SmoothPositionTransition>();

		private Camera m_camera;

		private GameObject m_lensFlare;

		private Camera m_assassinCamera;

		public bool m_isZoomed;

		private PositionTransitioner m_positionTransitioner;

		private LookAtTransitioner m_lookAtTransitioner;

		private bool m_hasChangeEndedCallbackBeenCalled;

		public Camera Camera
		{
			get
			{
				return m_camera;
			}
			set
			{
				m_camera = value;
				m_lookAtTransitioner = new LookAtTransitioner(m_camera.transform);
				m_positionTransitioner = new PositionTransitioner(m_camera.transform);
			}
		}

		public Camera AssassinCamera
		{
			get
			{
				return m_assassinCamera;
			}
		}

		public bool HasChangeEndedCallbackBeenCalled
		{
			get
			{
				return m_hasChangeEndedCallbackBeenCalled;
			}
		}

		public bool IsTweening { get; private set; }

		public bool IsZoomed
		{
			get
			{
				return m_isZoomed;
			}
			set
			{
				if (m_isZoomed != value)
				{
					m_isZoomed = value;
					UpdateCameraLocation();
				}
			}
		}

		public EType Old { get; private set; }

		public EType Current
		{
			get
			{
				return m_current;
			}
			set
			{
				CameraLocation value2 = null;
				if (m_cameraLocations.TryGetValue((int)value, out value2) && value2.InstantTransition)
				{
					SetCurrentInstant(value);
					return;
				}
				Old = m_current;
				m_current = value;
				UpdateCameraLocation();
			}
		}

		public Vector3 CurrentCameraPosition
		{
			get
			{
				return m_cameraLocations[(int)Current].GetPosition(IsZoomed);
			}
		}

		public event ChangeStartedDelegate OnChangeStarted;

		public event ChangeEndedDelegate OnChangeEnded;

		public CameraMenuManager(Camera _camera, GameObject _lensFlare, EType _currentType)
		{
			m_camera = _camera;
			m_lensFlare = _lensFlare;
			m_assassinCamera = (UnityEngine.Object.Instantiate(new GameObject(), Vector3.zero, Quaternion.identity) as GameObject).AddComponent<Camera>();
			m_assassinCamera.cullingMask = 1 << LayerMask.NameToLayer("Character");
			m_assassinCamera.fieldOfView = 40f;
			m_assassinCamera.depth = 50f;
			m_assassinCamera.clearFlags = CameraClearFlags.Depth;
			m_assassinCamera.name = "OverlayUICamera";
			m_assassinCamera.gameObject.SetActive(false);
			m_lookAtTransitioner = new LookAtTransitioner(m_camera.transform);
			m_positionTransitioner = new PositionTransitioner(m_camera.transform);
			AddCameraLocations();
			AddLensFlareLocations();
			CameraLocation value = null;
			if (m_cameraLocations.TryGetValue(5, out value))
			{
				m_assassinCamera.transform.position = value.GetPosition(false);
				m_assassinCamera.transform.LookAt(value.GetLookAt(false));
			}
			m_camera.transform.position = m_cameraLocations[(int)_currentType].GetPosition(IsZoomed);
			m_camera.transform.LookAt(m_cameraLocations[(int)_currentType].GetLookAt(IsZoomed));
			m_lensFlare.transform.position = m_lensFlareLocations[(int)_currentType].Position;
			this.OnChangeStarted = (ChangeStartedDelegate)Delegate.Combine(this.OnChangeStarted, new ChangeStartedDelegate(OnChangeStartedCallback));
			this.OnChangeEnded = (ChangeEndedDelegate)Delegate.Combine(this.OnChangeEnded, new ChangeEndedDelegate(OnChangeEndedCallback));
			Current = _currentType;
		}

		public void InstantChangeZoom(bool _isZoomed)
		{
			m_isZoomed = _isZoomed;
			SetCurrentInstant(m_current);
		}

		public void UpdateCamera()
		{
			for (int num = m_transitions.Count - 1; num >= 0; num--)
			{
				if (m_transitions[num].IsDone())
				{
					if (!m_hasChangeEndedCallbackBeenCalled && m_positionTransitioner.Progress >= 1f)
					{
						m_hasChangeEndedCallbackBeenCalled = true;
						if (this.OnChangeEnded != null)
						{
							this.OnChangeEnded(Current, Old);
						}
					}
					m_transitions.RemoveAt(num);
				}
				else
				{
					m_transitions[num].Step();
				}
			}
		}

		public void SetCurrentInstant(EType _eType)
		{
			if (!(m_camera == null))
			{
				Old = m_current;
				m_current = _eType;
				m_transitions.Clear();
				if (this.OnChangeStarted != null)
				{
					this.OnChangeStarted(Current, Old);
				}
				CameraLocation value = null;
				if (m_cameraLocations.TryGetValue((int)_eType, out value))
				{
					m_camera.transform.position = value.GetPosition(IsZoomed);
					m_camera.transform.LookAt(value.GetLookAt(IsZoomed));
					m_positionTransitioner.Position = m_camera.transform.position;
					m_lookAtTransitioner.Position = m_camera.transform.position;
				}
				LensFlareLocation value2 = null;
				if (m_lensFlareLocations.TryGetValue((int)_eType, out value2))
				{
					m_lensFlare.transform.position = value2.Position;
				}
				if (this.OnChangeEnded != null)
				{
					this.OnChangeEnded(Current, Old);
				}
			}
		}

		public void UpdateCameraLocation()
		{
			m_hasChangeEndedCallbackBeenCalled = false;
			if (this.OnChangeStarted != null)
			{
				this.OnChangeStarted(Current, Old);
			}
			m_transitions.Clear();
			SetCameraLocation(m_current);
			SetLensFlareLocation(m_current);
		}

		private void AddCameraLocations()
		{
			m_cameraLocations.Add(0, new CameraLocation(new Vector3(-16f, 15f, -13f), new Vector3(-16f, 17f, 8f), new Vector3(-18.7f, 15.5f, -5.1f), new Vector3(-18.7f, 15.5f, 10f), 100L, 100L));
			m_cameraLocations.Add(1, new CameraLocation(new Vector3(-18f, 15f, 4f), new Vector3(-18f, 16f, 25f), 150L, 150L));
			m_cameraLocations.Add(2, new CameraLocation(new Vector3(-4.2f, 12.5f, -7f), new Vector3(-3.25f, 13.25f, 5f), 150L, 150L));
			m_cameraLocations.Add(3, new CameraLocation(new Vector3(-4.2f, 12.5f, -7f), new Vector3(-3.25f, 13.25f, 5f), 150L, 150L));
			m_cameraLocations.Add(4, new CameraLocation(new Vector3(-4.4f, 13.09f, -5f), new Vector3(-4.2f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(5, new CameraLocation(new Vector3(-4.2f, 12.5f, -7f), new Vector3(-3.25f, 13.25f, 5f), 150L, 150L));
			m_cameraLocations.Add(6, new CameraLocation(new Vector3(7.8f, 12.5f, -7f), new Vector3(8.75f, 13.25f, 5f), 150L, 150L));
			m_cameraLocations.Add(7, new CameraLocation(new Vector3(-16.2f, 12.5f, -7f), new Vector3(-15.25f, 13.25f, 5f), 150L, 150L));
			m_cameraLocations.Add(8, new CameraLocation(new Vector3(-3.5f, 13f, -6f), new Vector3(-0.5f, 12f, 5f), 150L, 150L));
			m_cameraLocations.Add(9, new CameraLocation(new Vector3(-4.2f, 13.5f, 7f), new Vector3(-3.25f, 14.25f, 18f), 150L, 150L));
			m_cameraLocations.Add(16, new CameraLocation(new Vector3(-4.2f, 12.5f, -6f), new Vector3(-4.2f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(11, new CameraLocation(new Vector3(16f, 12.5f, -6f), new Vector3(12f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(10, new CameraLocation(new Vector3(6f, 12.5f, -6f), new Vector3(4f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(20, new CameraLocation(new Vector3(6f, 13f, 8f), new Vector3(4f, 14f, 19f), 150L, 150L));
			m_cameraLocations.Add(21, new CameraLocation(new Vector3(-26f, 12.5f, -6f), new Vector3(-24f, 13f, 5f), 100L, 100L));
			m_cameraLocations.Add(12, new CameraLocation(new Vector3(26f, 12.5f, -6f), new Vector3(20f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(24, new CameraLocation(new Vector3(36f, 12.5f, -6f), new Vector3(28f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(14, new CameraLocation(new Vector3(6f, 12.5f, -6f), new Vector3(4f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(15, new CameraLocation(new Vector3(6f, 12.5f, -6f), new Vector3(4f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(17, new CameraLocation(new Vector3(9f, 12.5f, -6f), new Vector3(6f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(18, new CameraLocation(new Vector3(12f, 12.5f, -6f), new Vector3(8f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(19, new CameraLocation(new Vector3(16f, 12.5f, -6f), new Vector3(11f, 13f, 5f), 150L, 150L));
			m_cameraLocations.Add(13, new CameraLocation(new Vector3(6f, 13f, 8f), new Vector3(4f, 14f, 19f), 150L, 150L, true));
			m_cameraLocations.Add(23, new CameraLocation(new Vector3(-3.26f, 12.9f, -3.45f), new Vector3(-4.3f, 11.8f, 0.7f), 150L, 150L));
		}

		private void AddLensFlareLocations()
		{
			m_lensFlareLocations.Add(0, new LensFlareLocation(new Vector3(-14f, 22f, 9f)));
			m_lensFlareLocations.Add(3, new LensFlareLocation(new Vector3(-11f, 18f, 11f)));
		}

		private void SetCameraLocation(EType _eType)
		{
			CameraLocation value = null;
			if (m_cameraLocations.TryGetValue((int)_eType, out value))
			{
				m_transitions.Add(new SmoothPositionTransition(m_lookAtTransitioner, value.GetLookAt(IsZoomed), value.LookatSpeed));
				m_transitions.Add(new SmoothPositionTransition(m_positionTransitioner, value.GetPosition(IsZoomed), value.PositionSpeed));
			}
		}

		private void SetLensFlareLocation(EType _eType)
		{
			LensFlareLocation value = null;
			if (m_lensFlareLocations.TryGetValue((int)_eType, out value))
			{
				m_transitions.Add(new SmoothPositionTransition(new PositionTransitioner(m_lensFlare.transform), value.Position, 150f));
			}
		}

		private void OnChangeStartedCallback(EType _eCurrent, EType _eOld)
		{
			m_camera.nearClipPlane = 0.25f;
			m_camera.farClipPlane = 50f;
			QualitySettings.shadowProjection = ShadowProjection.CloseFit;
			EarthGlobeData earthGlobeData = BehaviourSingleton<EnvironmentBridgeMain>.Instance.EarthGlobeData;
			IsTweening = true;
			if (_eCurrent == EType.Missions || _eCurrent == EType.MissionsEra)
			{
				earthGlobeData.IsGlobeActive = true;
				earthGlobeData.IsLightActive = true;
			}
			else
			{
				earthGlobeData.IsLightActive = false;
				earthGlobeData.IsGlobeActive = false;
			}
		}

		private void OnChangeEndedCallback(EType _eCurrent, EType _eOld)
		{
			IsTweening = false;
		}
	}
}
