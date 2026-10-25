using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Camera;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.CameraController;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class AcierCameraController : CameraController
{
	public class CameraStateEventArgs : EventArgs
	{
		public ICameraState CameraState { get; set; }
	}

	public enum ECameraState
	{
		None = 0,
		Any = 1,
		Roaming = 2,
		Combat = 3,
		Kill = 4,
		Haystack = 5,
		RooftopGarden = 6,
		Bench = 7,
		Climb = 8,
		JumpDown = 9,
		Roof = 10,
		Codex = 11,
		Chest = 12,
		CinematicAssassination = 13,
		StealthAssassination = 14,
		BenchAssassination = 15,
		HaystackAssassination = 16,
		RooftopGardenAssassination = 17,
		AirAssassination = 18,
		LeapAssassination = 19,
		NPC = 20,
		Introduction = 21,
		Outro = 22,
		LeapOfFaith = 23,
		RoofEdge = 24,
		Dead = 25,
		PigeonCoop = 26,
		VR = 27,
		CustomAnimated = 28
	}

	[Serializable]
	public class CameraTransitionEntry
	{
		[SerializeField]
		private ECameraState m_source;

		[SerializeField]
		private ECameraState m_destination;

		[SerializeField]
		private CameraTransition m_transition;

		public ECameraState Source
		{
			get
			{
				return m_source;
			}
		}

		public ECameraState Destination
		{
			get
			{
				return m_destination;
			}
		}

		public CameraTransition Transition
		{
			get
			{
				return m_transition;
			}
		}

		public CameraTransitionEntry(ECameraState _src, ECameraState _dest, CameraTransition _transition)
		{
			m_source = _src;
			m_destination = _dest;
			m_transition = _transition;
		}
	}

	public class StackedState
	{
		private float m_wantedWeight;

		private float m_grantedWeight = 1f;

		private int m_instanceId;

		private float m_time;

		private CameraTransition m_transition;

		private static Color[] m_colors = new Color[7]
		{
			Color.cyan,
			Color.magenta,
			Color.red,
			Color.green,
			Color.blue,
			Color.yellow,
			Color.red * 0.5f + Color.yellow * 0.5f
		};

		private static int instanceCnt = 0;

		public ICameraState CameraState { get; private set; }

		public float WantedWeight
		{
			get
			{
				return m_wantedWeight;
			}
		}

		public float GrantedWeight
		{
			get
			{
				return m_grantedWeight;
			}
		}

		public Color DebuggingColor
		{
			get
			{
				return m_colors[m_instanceId % m_colors.Length];
			}
		}

		public StackedState(ICameraState _state, CameraTransition _transition)
		{
			CameraState = _state;
			m_transition = _transition;
			m_instanceId = ++instanceCnt;
		}

		public void UpdateWeight(ref float remainingWeight)
		{
			m_wantedWeight = CalculateWantedWeight();
			m_grantedWeight = Mathf.Min(remainingWeight, m_wantedWeight);
			remainingWeight = Mathf.Max(0f, remainingWeight - m_wantedWeight);
		}

		private float CalculateWantedWeight()
		{
			if (m_transition == null || m_transition.Type == CameraTransition.EType.None)
			{
				return 1f;
			}
			m_time = Mathf.Min(m_time + UbiCore.Utils.Singleton<Timer>.Instance.deltaTime / m_transition.EaseDuration, 1f);
			if (m_time < 1f)
			{
				return Interpolate.Ease01(m_transition.EaseType, m_time);
			}
			return 1f;
		}
	}

	public class TransformFilter
	{
		public float PositionJitterRadius = 0.1f;

		public float PositionDamping = 0.1f;

		public float RotationJitterAngle = 5f;

		public float RotationDamping = 0.1f;

		public float VelocityDamping = 0.1f;

		public Vector3 Position { get; private set; }

		public Vector3 Velocity { get; private set; }

		public Vector3 FlatVelocity { get; private set; }

		public Vector3 Direction { get; private set; }

		public Vector3 FlatDirection { get; private set; }

		public Quaternion Rotation { get; private set; }

		public Quaternion FlatRotation { get; private set; }

		public TransformFilter(Transform _transform, float _positionJitterRadius, float _positionDamping, float _rotationJitterAngle, float _rotationDamping, float _velocityDamping)
		{
			Position = _transform.position;
			Direction = _transform.forward;
			Vector3 forward = _transform.forward;
			forward.y = 0f;
			forward.Normalize();
			FlatDirection = forward;
			Rotation = _transform.rotation;
			FlatRotation = SafeLookRotation(forward, FlatRotation);
			PositionJitterRadius = _positionJitterRadius;
			PositionDamping = _positionDamping;
			RotationJitterAngle = _rotationJitterAngle;
			RotationDamping = _rotationDamping;
			VelocityDamping = _velocityDamping;
		}

		public void Update(Transform _reference, float _dt)
		{
			Update(_reference.position, _reference.rotation, _dt);
		}

		public void Update(Vector3 _referencePosition, Quaternion _referenceRotation, float _dt)
		{
			float t = ((!(_dt <= 0f)) ? (_dt / (_dt + PositionDamping)) : 0f);
			float t2 = ((!(_dt <= 0f)) ? (_dt / (_dt + RotationDamping)) : 0f);
			float t3 = ((!(_dt <= 0f)) ? (_dt / (_dt + VelocityDamping)) : 0f);
			Vector3 vector = _referencePosition - Position;
			Vector3 normalized = vector.normalized;
			float magnitude = vector.magnitude;
			Vector3 b = Position + normalized * Mathf.Max(magnitude - PositionJitterRadius, 0f);
			Vector3 vector2 = Vector3.Lerp(Position, b, t);
			Vector3 vector3 = Vector3.Lerp(Velocity, (!(_dt <= 0f)) ? ((vector2 - Position) / _dt) : Velocity, t3);
			float num = Quaternion.Angle(Rotation, _referenceRotation);
			Quaternion b2 = ((!(num <= 0f)) ? Quaternion.Slerp(Rotation, _referenceRotation, Mathf.Max(num - RotationJitterAngle, 0f) / num) : _referenceRotation);
			Quaternion quaternion = Quaternion.Slerp(Rotation, b2, t2);
			Position = vector2;
			Rotation = quaternion;
			Velocity = vector3;
			Direction = quaternion * Vector3.forward;
			vector3.y = 0f;
			FlatVelocity = vector3;
			Vector3 direction = Direction;
			direction.y = 0f;
			direction.Normalize();
			if (direction.sqrMagnitude > 0.5f)
			{
				FlatDirection = direction;
				FlatRotation = Quaternion.LookRotation(direction);
			}
		}
	}

	public const bool EnableVerboseLog = false;

	public static AcierCameraController Instance;

	public static bool Prototyping;

	public static bool CanGoBehindObjects = true;

	public static bool CullCharacter;

	public static bool ShowCharacterOutline = true;

	public static float RealignSpeedMultiplier = 1f;

	private CameraStateEventArgs m_cameraStateEventArgs = new CameraStateEventArgs();

	[SerializeField]
	private CameraTransition m_defaultTransition = new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseInOutCubic, 1f);

	[SerializeField]
	private CameraTransitionEntry[] m_transitions;

	private List<Type> m_states = new List<Type>
	{
		typeof(CameraStateRoaming),
		typeof(CameraStateCombat),
		typeof(CameraStateKill),
		typeof(CameraStateHaystack),
		typeof(CameraStateRooftopGarden),
		typeof(CameraStateBench),
		typeof(CameraStateClimb),
		typeof(CameraStateJumpDown),
		typeof(CameraStateRoof),
		typeof(CameraStateRoofEdge),
		typeof(CameraStateCodex),
		typeof(CameraStateScriptedChest),
		typeof(CameraStateScriptedBenchAssassination),
		typeof(CameraStateScriptedStealthAssassination),
		typeof(CameraStateScriptedLeapAssassination),
		typeof(CameraStateScriptedAirAssassination),
		typeof(CameraStateScriptedHaystackAssassination),
		typeof(CameraStateScriptedRooftopGardenAssassination),
		typeof(CameraStateScriptedPigeonCoop),
		typeof(CameraStateNPC),
		typeof(CameraStateIntroduction),
		typeof(CameraStateOutro),
		typeof(CameraStateLeapOfFaith),
		typeof(CameraStateDead),
		typeof(CameraStateVR),
		typeof(CameraStateCustomAnimation),
		typeof(CameraStateRoaming_Florent),
		typeof(CameraStateClimb_Florent),
		typeof(CameraStateCombat_Florent),
		typeof(CameraStateJumpDown_Florent),
		typeof(CameraStateRoof_Florent)
	};

	private BBDictionary<int, ICameraState> m_templates;

	private BBDictionary<int, ICameraState> m_prototypeTemplates;

	private CameraValues m_globalValues;

	private ICameraState m_current;

	public List<StackedState> m_stack = new List<StackedState>();

	private RaycastValues m_raycastValues;

	private bool m_active = true;

	private bool m_outlines;

	public float WallAvoidanceCorrection;

	public float WallAvoidanceDistance = float.MaxValue;

	public float WallAvoidanceDamping = 0.2f;

	private Vector3 m_raycastPosition;

	private Vector3 m_targetPosition;

	public float MinDistanceToWall = 0.25f;

	public float MaxDistanceToWall = 0.8f;

	public float MaxSeeThroughDistance = 0.5f;

	public CameraCollisionProbe CollisionProbe;

	private Vector2 m_animationRange;

	private float m_animationTime;

	private Vector3 m_shakeDistance;

	private float m_animationStrength;

	private bool m_animate;

	private float m_animationStart;

	private int m_activeLayerMask;

	private bool[] m_visible = new bool[2];

	public static CameraEvents Events { get; private set; }

	public int LayerMask
	{
		get
		{
			return m_activeLayerMask;
		}
	}

	public int LayerMaskBlockers
	{
		get
		{
			return Layers.StaticColliderCameraBlockersLayersMaskPreset;
		}
	}

	public int LayerMaskSeeThrough
	{
		get
		{
			return (int)Layers.StaticColliderCameraBlockersLayersMaskPreset | (int)Layers.StaticColliderCameraSeeThroughLayersMaskPreset;
		}
	}

	public Vector2 Scale
	{
		get
		{
			RenderingUpscaler component = Camera.GetComponent<RenderingUpscaler>();
			if (component != null)
			{
				return component.Scale;
			}
			return new Vector2(1f, 1f);
		}
	}

	public Vector2 ViewportSize
	{
		get
		{
			return new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
		}
	}

	public static NavMeshAgent NavMeshAgent
	{
		get
		{
			return LogicMain.PlayerEntity.UnityComponentCache.NavMeshAgent;
		}
	}

	public static Animator Animator
	{
		get
		{
			return LogicMain.PlayerEntity.UnityComponentCache.Animator;
		}
	}

	public static EMovementSpeed Speed
	{
		get
		{
			return LogicMain.PlayerEntity.UnityComponentCache.AnimationController.ReferenceMovementSpeed;
		}
	}

	public override bool suppressesInteraction
	{
		get
		{
			return false;
		}
	}

	public CameraValues GlobalValues
	{
		get
		{
			return m_globalValues;
		}
	}

	public ICameraState CurrentState
	{
		get
		{
			return m_current;
		}
	}

	public ECameraState CurrentStateType
	{
		get
		{
			if (m_current != null)
			{
				return m_current.StateType;
			}
			return ECameraState.None;
		}
	}

	public List<StackedState> Stack
	{
		get
		{
			return m_stack;
		}
	}

	public bool DontRemoveNotContributing { get; set; }

	public static TransformFilter AgentDataFilter { get; private set; }

	public Vector3 LastCameraPosition { get; private set; }

	public Vector3 LastCameraForward { get; private set; }

	public Plane[] LastCameraFrustum { get; private set; }

	public Camera Camera { get; private set; }

	public Vector3 RaycastPosition
	{
		get
		{
			return m_raycastPosition;
		}
	}

	public Vector3 TargetPosition
	{
		get
		{
			return m_targetPosition;
		}
	}

	public EPlayerState CurrentPlayerState { get; private set; }

	public event EventHandler<CameraStateEventArgs> CameraStateStarted;

	public static Quaternion SafeLookRotation(Vector3 _direction, Quaternion _fallback)
	{
		if (_direction.sqrMagnitude < 1E-05f)
		{
			return _fallback;
		}
		return Quaternion.LookRotation(_direction);
	}

	public void OnDrawGizmos()
	{
	}

	private void ShowPlayerTrajectory()
	{
		float num = 5f;
		Vector3 vector = AgentDataFilter.Velocity * 0.9f * LogicMain.FrameTime;
		Vector3 position = NavMeshAgent.transform.position;
		Color red = Color.red;
		red = Color.green;
		Vector3 position2 = NavMeshAgent.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head).position;
		red = Color.blue;
		Vector3 position3 = NavMeshAgent.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips).position;
	}

	public static AcierCameraController Create()
	{
		AcierCameraController acierCameraController = (Instance = Camera.main.transform.parent.gameObject.AddComponentIfNotExist<AcierCameraController>());
		acierCameraController.Initialize();
		CameraStateHelper.Initialize();
		return acierCameraController;
	}

	public static void Shutdown()
	{
		if (Instance != null)
		{
			UnityEngine.Object.DestroyImmediate(Instance.gameObject);
			if ((bool)Instance.CollisionProbe)
			{
				UnityEngine.Object.DestroyImmediate(Instance.CollisionProbe);
			}
			Instance = null;
		}
	}

	private void OnPlayerStateChange(PlayerStateChangeEvent _event)
	{
		CurrentPlayerState = _event.PlayerState;
	}

	private void InitializeTemplates(GameObject parent)
	{
		m_templates = new BBDictionary<int, ICameraState>();
		m_templates[2] = parent.GetComponent<CameraStateRoaming>();
		m_templates[3] = parent.GetComponent<CameraStateCombat>();
		m_templates[4] = parent.GetComponent<CameraStateKill>();
		m_templates[5] = parent.GetComponent<CameraStateHaystack>();
		m_templates[6] = parent.GetComponent<CameraStateRooftopGarden>();
		m_templates[7] = parent.GetComponent<CameraStateBench>();
		m_templates[8] = parent.GetComponent<CameraStateClimb>();
		m_templates[9] = parent.GetComponent<CameraStateJumpDown>();
		m_templates[23] = parent.GetComponent<CameraStateLeapOfFaith>();
		m_templates[10] = parent.GetComponent<CameraStateRoof>();
		m_templates[24] = parent.GetComponent<CameraStateRoofEdge>();
		m_templates[11] = parent.GetComponent<CameraStateCodex>();
		m_templates[12] = parent.GetComponent<CameraStateScriptedChest>();
		m_templates[15] = parent.GetComponent<CameraStateScriptedBenchAssassination>();
		m_templates[14] = parent.GetComponent<CameraStateScriptedStealthAssassination>();
		m_templates[19] = parent.GetComponent<CameraStateScriptedLeapAssassination>();
		m_templates[18] = parent.GetComponent<CameraStateScriptedAirAssassination>();
		m_templates[16] = parent.GetComponent<CameraStateScriptedHaystackAssassination>();
		m_templates[17] = parent.GetComponent<CameraStateScriptedRooftopGardenAssassination>();
		m_templates[26] = parent.GetComponent<CameraStateScriptedPigeonCoop>();
		m_templates[20] = parent.GetComponent<CameraStateNPC>();
		m_templates[21] = parent.GetComponent<CameraStateIntroduction>();
		m_templates[22] = parent.GetComponent<CameraStateOutro>();
		m_templates[25] = parent.GetComponent<CameraStateDead>();
		m_templates[27] = parent.GetComponent<CameraStateVR>();
		m_templates[28] = parent.GetComponent<CameraStateCustomAnimation>();
		m_prototypeTemplates = new BBDictionary<int, ICameraState>();
		m_prototypeTemplates[2] = parent.GetComponent<CameraStateRoaming_Florent>();
		m_prototypeTemplates[3] = parent.GetComponent<CameraStateCombat_Florent>();
		m_prototypeTemplates[4] = parent.GetComponent<CameraStateKill>();
		m_prototypeTemplates[5] = parent.GetComponent<CameraStateHaystack>();
		m_prototypeTemplates[6] = parent.GetComponent<CameraStateRooftopGarden>();
		m_prototypeTemplates[7] = parent.GetComponent<CameraStateBench>();
		m_prototypeTemplates[8] = parent.GetComponent<CameraStateClimb_Florent>();
		m_prototypeTemplates[9] = parent.GetComponent<CameraStateJumpDown_Florent>();
		m_prototypeTemplates[23] = parent.GetComponent<CameraStateLeapOfFaith>();
		m_prototypeTemplates[10] = parent.GetComponent<CameraStateRoof_Florent>();
		m_prototypeTemplates[24] = parent.GetComponent<CameraStateRoofEdge>();
		m_prototypeTemplates[11] = parent.GetComponent<CameraStateCodex>();
		m_prototypeTemplates[12] = parent.GetComponent<CameraStateScriptedChest>();
		m_prototypeTemplates[15] = parent.GetComponent<CameraStateScriptedBenchAssassination>();
		m_prototypeTemplates[14] = parent.GetComponent<CameraStateScriptedStealthAssassination>();
		m_prototypeTemplates[19] = parent.GetComponent<CameraStateScriptedLeapAssassination>();
		m_prototypeTemplates[18] = parent.GetComponent<CameraStateScriptedAirAssassination>();
		m_prototypeTemplates[16] = parent.GetComponent<CameraStateScriptedHaystackAssassination>();
		m_prototypeTemplates[17] = parent.GetComponent<CameraStateScriptedRooftopGardenAssassination>();
		m_prototypeTemplates[26] = parent.GetComponent<CameraStateScriptedPigeonCoop>();
		m_prototypeTemplates[20] = parent.GetComponent<CameraStateNPC>();
		m_prototypeTemplates[21] = parent.GetComponent<CameraStateIntroduction>();
		m_prototypeTemplates[22] = parent.GetComponent<CameraStateOutro>();
		m_prototypeTemplates[25] = parent.GetComponent<CameraStateDead>();
		m_prototypeTemplates[27] = parent.GetComponent<CameraStateVR>();
		m_prototypeTemplates[28] = parent.GetComponent<CameraStateCustomAnimation>();
	}

	private ICameraState CreateState(ECameraState _state)
	{
		ICameraState cameraState2;
		if (Prototyping)
		{
			ICameraState cameraState = m_prototypeTemplates[(int)_state];
			cameraState2 = cameraState;
		}
		else
		{
			cameraState2 = m_templates[(int)_state];
		}
		ICameraState cameraState3 = cameraState2;
		ICameraState cameraState4 = (ICameraState)base.gameObject.AddComponent(cameraState3.GetType());
		cameraState4.CopyFrom(cameraState3);
		return cameraState4;
	}

	public void Initialize()
	{
		Camera = GetComponentInChildren<Camera>();
		m_activeLayerMask = LayerMaskBlockers;
		AgentDataFilter = new TransformFilter(NavMeshAgent.transform, 0.1f, 0.05f, 2.5f, 0.05f, 0.1f);
		Events = base.gameObject.AddComponentIfNotExist<CameraEvents>();
		GameObject gameObject = UnityEngine.Resources.Load("Camera/CameraSetup") as GameObject;
		m_globalValues = gameObject.GetComponent<CameraValues>();
		m_templates = new BBDictionary<int, ICameraState>();
		GameObject gameObject2 = new GameObject("Templates");
		gameObject2.transform.parent = base.transform;
		gameObject2.SetActive(false);
		if (CollisionProbe == null)
		{
			GameObject gameObject3 = UnityEngine.Resources.Load("Camera/CameraCollisionProbe") as GameObject;
			if (gameObject3 != null)
			{
				UnityEngine.Object.Instantiate(gameObject3);
			}
		}
		foreach (Type state in m_states)
		{
			ICameraState cameraState = (ICameraState)gameObject2.AddComponent(state);
			if (cameraState != null && gameObject != null)
			{
				ICameraState cameraState2 = (ICameraState)gameObject.GetComponent(state);
				if (cameraState2 != null)
				{
					cameraState.CopyFrom(cameraState2);
				}
			}
		}
		InitializeTemplates(gameObject2);
		m_transitions = new CameraTransitionEntry[21]
		{
			new CameraTransitionEntry(ECameraState.Any, ECameraState.Codex, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Codex, ECameraState.Any, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Any, ECameraState.CustomAnimated, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.JumpDown, ECameraState.Roaming, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.Linear, 1f)),
			new CameraTransitionEntry(ECameraState.Introduction, ECameraState.Any, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Any, ECameraState.Outro, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Any, ECameraState.RoofEdge, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseInOutCubic, 1f)),
			new CameraTransitionEntry(ECameraState.RoofEdge, ECameraState.Any, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseInOutCubic, 1.5f)),
			new CameraTransitionEntry(ECameraState.Combat, ECameraState.Kill, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Roaming, ECameraState.Chest, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Roaming, ECameraState.CinematicAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Roaming, ECameraState.StealthAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Bench, ECameraState.BenchAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Roaming, ECameraState.LeapAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Any, ECameraState.AirAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Haystack, ECameraState.HaystackAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.RooftopGarden, ECameraState.RooftopGardenAssassination, new CameraTransition(CameraTransition.EType.None, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Roaming, ECameraState.PigeonCoop, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseOutCirc, 3.5f)),
			new CameraTransitionEntry(ECameraState.Kill, ECameraState.Combat, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseOutCirc, 0.5f)),
			new CameraTransitionEntry(ECameraState.Any, ECameraState.Dead, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.Linear, 0f)),
			new CameraTransitionEntry(ECameraState.Dead, ECameraState.Any, new CameraTransition(CameraTransition.EType.EaseFunction, Interpolate.EaseType.EaseOutCubic, 5f))
		};
		Reset();
		m_raycastValues = UnityEngine.Resources.Load<GameObject>("Camera/Values/Raycast").GetComponent<RaycastValues>();
	}

	private void OnDestroy()
	{
		if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<PlayerStateChangeEvent>(Instance.OnPlayerStateChange);
		}
		Events = null;
	}

	public void Reset()
	{
		m_current = null;
		UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<PlayerStateChangeEvent>(Instance.OnPlayerStateChange);
		Instance.CurrentPlayerState = EPlayerState.Undefined;
	}

	public void SetActive(bool _active)
	{
		m_active = _active;
	}

	public void UpdateFrame()
	{
		if (NavMeshAgent == null)
		{
			return;
		}
		AgentDataFilter.Update(NavMeshAgent.transform, LogicMain.FrameTime);
		CameraStateHelper.Refresh();
		if (!m_active)
		{
			return;
		}
		LastCameraPosition = Camera.transform.position;
		LastCameraForward = Camera.transform.forward;
		LastCameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera);
		ECameraState wantedState = CameraStateHelper.WantedState;
		Vector3 vector;
		Vector3 vector2;
		Quaternion quaternion;
		float num;
		if (m_current == null)
		{
			StartState(wantedState);
			m_current.UpdateState();
			StartOutlines();
			vector = m_current.CurrentPosition;
			vector2 = m_current.CurrentRaycastPosition;
			quaternion = m_current.CurrentRotation;
			num = m_current.CurrentFieldOfView;
		}
		else
		{
			Profiling.Start("AcierCameraController::UpdateState()", float.MaxValue);
			if (wantedState != m_current.StateType)
			{
				StartState(wantedState);
			}
			float remainingWeight = 1f;
			if (m_stack.Count > 1)
			{
				float[] array = new float[m_stack.Count];
				int num2 = m_stack.Count - 1;
				for (int num3 = num2; num3 >= 0; num3--)
				{
					StackedState stackedState = m_stack[num3];
					ICameraState cameraState = stackedState.CameraState;
					stackedState.UpdateWeight(ref remainingWeight);
					float grantedWeight = stackedState.GrantedWeight;
					if (grantedWeight > 0f)
					{
						cameraState.UpdateState();
					}
					array[num3] = grantedWeight;
				}
				Vector3 zero = Vector3.zero;
				Vector3 zero2 = Vector3.zero;
				vector = Vector3.zero;
				vector2 = Vector3.zero;
				num = 0f;
				for (int i = 0; i < m_stack.Count; i++)
				{
					StackedState stackedState2 = m_stack[i];
					ICameraState cameraState2 = stackedState2.CameraState;
					float num4 = array[i];
					vector += cameraState2.CurrentPosition * num4;
					vector2 += cameraState2.CurrentRaycastPosition * num4;
					num += cameraState2.CurrentFieldOfView * num4;
					zero += cameraState2.CurrentRotation * Vector3.forward * num4;
					zero2 += cameraState2.CurrentRotation * Vector3.up * num4;
				}
				float magnitude = zero.magnitude;
				float magnitude2 = zero2.magnitude;
				quaternion = ((!(magnitude > 1E-05f) || !(magnitude2 > 1E-05f) || !((zero / magnitude).sqrMagnitude > 1E-05f) || !((zero2 / magnitude2).sqrMagnitude > 1E-05f)) ? m_current.CurrentRotation : Quaternion.LookRotation(zero / magnitude, zero2 / magnitude2));
				if (!DontRemoveNotContributing)
				{
					RemoveNonContributingStates();
				}
			}
			else
			{
				m_current.UpdateState();
				vector = m_current.CurrentPosition;
				vector2 = m_current.CurrentRaycastPosition;
				quaternion = m_current.CurrentRotation;
				num = m_current.CurrentFieldOfView;
			}
			Profiling.Stop();
		}
		if (Quaternion.Angle(base.transform.localRotation, quaternion) > 0.005f)
		{
			base.transform.localRotation = quaternion;
		}
		if (m_animate && !UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
		{
			m_shakeDistance = new Vector3(UnityEngine.Random.Range(m_animationRange.x, m_animationRange.y), UnityEngine.Random.Range(m_animationRange.x, m_animationRange.y), UnityEngine.Random.Range(m_animationRange.x, m_animationRange.y));
			base.transform.Rotate(m_shakeDistance * m_animationStrength);
			m_animationStrength = 1f - (Time.time - m_animationStart) / m_animationTime;
		}
		if (m_animationStrength <= 0f)
		{
			m_animate = false;
		}
		Camera.fieldOfView = num;
		UpdateOutlines();
		CameraStateRaycast cameraStateRaycast = m_current as CameraStateRaycast;
		if (cameraStateRaycast != null)
		{
			m_raycastValues = cameraStateRaycast.Values;
		}
		float num5 = Vector3.SqrMagnitude(m_raycastPosition - vector2);
		m_targetPosition = vector;
		m_raycastPosition = vector2;
		bool useOverride = num5 > 2500f;
		base.transform.position = CorrectPosition(vector, vector2, vector2, useOverride);
		UpdateVisibility();
		if (Events != null)
		{
			Events.ClearEvents();
		}
	}

	public void StartAnimating(Vector2 range, float time)
	{
		m_animate = true;
		m_animationStart = Time.time;
		m_animationRange = range;
		m_animationTime = time;
	}

	protected Vector3 CorrectPosition(Vector3 position, Vector3 raycastPosition, Vector3 _collProbeOverridePos, bool _useOverride)
	{
		Vector3 result = position;
		if (m_current.StateType == ECameraState.VR)
		{
			return result;
		}
		Vector3 vector = position - m_raycastPosition;
		Vector3 normalized = vector.normalized;
		float maxDistance = Mathf.Max(1E-05f, vector.magnitude);
		RaycastHit hitInfo;
		if (CollisionProbe == null)
		{
			if (Physics.Raycast(raycastPosition, normalized, out hitInfo, maxDistance, LayerMask))
			{
				Vector3 vector2 = raycastPosition + normalized * hitInfo.distance;
				result = vector2 + hitInfo.normal * MinDistanceToWall;
			}
			return result;
		}
		Vector3 vector3 = ((!_useOverride) ? CollisionProbe.transform.position : _collProbeOverridePos);
		Vector3 vector4 = position - vector3;
		Vector3 normalized2 = vector4.normalized;
		float magnitude = vector4.magnitude;
		WallAvoidanceCorrection = Damping.Damp(WallAvoidanceCorrection, 0f, WallAvoidanceDamping, 1f);
		Collider[] array = Physics.OverlapSphere(position, MaxSeeThroughDistance, Layers.StaticColliderCameraSeeThroughLayersMaskPreset);
		if (array.Length > 0)
		{
			m_activeLayerMask = LayerMaskSeeThrough;
		}
		else
		{
			m_activeLayerMask = LayerMaskBlockers;
		}
		if (Physics.SphereCast(vector3, MaxDistanceToWall, normalized2, out hitInfo, magnitude, m_activeLayerMask))
		{
			result = vector3 + normalized2 * hitInfo.distance;
			WallAvoidanceCorrection = Damping.Damp(WallAvoidanceCorrection, 1f - hitInfo.distance / magnitude, WallAvoidanceDamping, 1f);
		}
		if (Physics.SphereCast(vector3, MinDistanceToWall, normalized2, out hitInfo, magnitude, m_activeLayerMask))
		{
			result = vector3 + normalized2 * hitInfo.distance;
			WallAvoidanceCorrection = Mathf.Max(WallAvoidanceCorrection, 1f - hitInfo.distance / magnitude);
		}
		WallAvoidanceDistance = (1f - WallAvoidanceCorrection) * magnitude;
		return vector3 + normalized2 * WallAvoidanceDistance;
	}

	private void StartOutlines()
	{
		m_outlines = m_current.Outlines && ShowCharacterOutline;
		if (!m_outlines)
		{
			AssassinRendering.DisableOutline();
		}
	}

	private void UpdateOutlines()
	{
		bool flag = m_current.Outlines && ShowCharacterOutline;
		if (m_outlines != flag)
		{
			if (flag)
			{
				AssassinRendering.EnableOutline();
			}
			else
			{
				AssassinRendering.DisableOutline();
			}
			m_outlines = flag;
		}
	}

	private void UpdateVisibility()
	{
		Transform boneTransform = Animator.GetBoneTransform(HumanBodyBones.Head);
		Vector3 b = boneTransform.position - boneTransform.right * m_raycastValues.hideReferenceHeightOffset;
		float num = Vector3.Distance(base.transform.position, b);
		float num2 = m_raycastValues.hideCharacterDistance;
		if (!m_visible[0])
		{
			num2 += m_raycastValues.hideCharacterFilterDistance;
		}
		m_visible[0] = num > num2;
		float num3 = m_raycastValues.hideCharacterRaycastDistance;
		if (!m_visible[1])
		{
			num3 += m_raycastValues.hideCharacterRaycastFilterDistance;
		}
		m_visible[1] = WallAvoidanceDistance > num3;
		AssassinRendering.ToggleVisible(((m_visible[0] && m_visible[1]) || !CullCharacter) && m_current.DrawAvatar);
	}

	public void RemoveNonContributingStates()
	{
		List<StackedState> list = new List<StackedState>();
		for (int i = 0; i < m_stack.Count; i++)
		{
			StackedState stackedState = m_stack[i];
			if (stackedState.GrantedWeight <= 0f)
			{
				list.Add(stackedState);
			}
		}
		foreach (StackedState item in list)
		{
			if (m_current != item.CameraState)
			{
				m_stack.Remove(item);
				item.CameraState.EndState();
				UnityEngine.Object.Destroy((UnityEngine.Object)item.CameraState);
			}
		}
	}

	public void StartState(ECameraState _state)
	{
		ICameraState current = m_current;
		m_current = CreateState(_state);
		m_current.StartState(current);
		m_cameraStateEventArgs.CameraState = m_current;
		if (this.CameraStateStarted != null)
		{
			this.CameraStateStarted(this, m_cameraStateEventArgs);
		}
		CameraTransition transition = ((current == null) ? null : FindTransition((CameraStateBase)current, (CameraStateBase)m_current));
		m_stack.Add(new StackedState(m_current, transition));
	}

	public CameraTransition FindTransition(CameraStateBase _src, CameraStateBase _dest)
	{
		ECameraState src = ((_src != null) ? _src.TransitionStateType : ECameraState.None);
		ECameraState dest = _dest.TransitionStateType;
		CameraTransitionEntry[] array = Array.FindAll(m_transitions, (CameraTransitionEntry o) => o.Source == src && o.Destination == dest);
		if (array.Length > 0)
		{
			return array[0].Transition;
		}
		array = Array.FindAll(m_transitions, (CameraTransitionEntry o) => o.Source == src && o.Destination == ECameraState.Any);
		if (array.Length > 0)
		{
			return array[0].Transition;
		}
		array = Array.FindAll(m_transitions, (CameraTransitionEntry o) => o.Source == ECameraState.Any && o.Destination == dest);
		if (array.Length > 0)
		{
			return array[0].Transition;
		}
		return m_defaultTransition;
	}
}
