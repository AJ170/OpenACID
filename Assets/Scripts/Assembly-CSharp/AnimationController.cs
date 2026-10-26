using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Utilities.AutoUpdating;
using SharedBaseLib.System;
using UbiCore;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class AnimationController : MonoBehaviour, IAnimatorEventListener
{
	public enum Direction
	{
		Unknown = 0,
		Front = 1,
		Right = 3,
		Back = 4,
		Left = 2,
		FrontLeft = 5,
		FrontRight = 6,
		BackLeft = 7,
		BackRight = 8
	}

	[Serializable]
	public class MovementInformation
	{
		public const float InvalidVelocityOverwrite = -1f;

		[SerializeField]
		private EMovementSpeed m_Movement;

		[SerializeField]
		private float m_Velocity;

		private float m_VelocityOverwrite = -1f;

		public EMovementSpeed SpeedType
		{
			get
			{
				return m_Movement;
			}
		}

		public float SpeedFactor
		{
			get
			{
				return (!(m_VelocityOverwrite > MathHelper.Epsilon)) ? m_Velocity : m_VelocityOverwrite;
			}
		}

		public MovementInformation(EMovementSpeed _type, float _factor)
		{
			m_Movement = _type;
			m_Velocity = Mathf.Clamp01(_factor);
		}

		public void OverwriteSpeedFactor(float _speedFactor)
		{
			m_VelocityOverwrite = _speedFactor;
		}

		public void ResetSpeedFactor()
		{
			m_VelocityOverwrite = -1f;
		}
	}

	public const int MinAnimatorNpcId = 1;

	public const int MaxAnimatorNpcId = 4;

	public const int DeltaAnimatorNpcId = 3;

	public static readonly int Invalid = -1;

	public static readonly int TagWalkRun = Animator.StringToHash("WalkRun");

	public static float HaystackTargetDistanceMin = 0.5f;

	public static float HaystackTargetDistanceMax = 1f;

	public static float BenchTargetDistance = 1.7f;

	public static float BenchTargetDistanceRelative = 0.35f;

	public static float BenchTargetDistanceMax = 2f;

	public static float RooftopGardenTargetDistanceMax = 1f;

	public static float GroundHeightOffset = 0f;

	public static bool Debugging = false;

	public static bool WalkOnCollider = false;

	[SerializeField]
	private float stopAnimDistance = 0.7f;

	[SerializeField]
	private float turnThreshold180 = 135f;

	[SerializeField]
	private float m_RotationMargin = 0.5f;

	[SerializeField]
	private float m_TranslateMargin = 0.1f;

	[SerializeField]
	public FloatRange m_GroundJump = new FloatRange(1f, 2.5f);

	[SerializeField]
	private FloatRange m_WallJump = new FloatRange(0.2f, 1.8f);

	[SerializeField]
	private float m_BankRange = 90f;

	[SerializeField]
	public List<MovementInformation> m_MovementSpeeds = new List<MovementInformation>
	{
		new MovementInformation(EMovementSpeed.WalkSlow, 0.1f),
		new MovementInformation(EMovementSpeed.Walk, 0.3f),
		new MovementInformation(EMovementSpeed.WalkFast, 0.44f),
		new MovementInformation(EMovementSpeed.Run, 0.7f),
		new MovementInformation(EMovementSpeed.Sprint, 1f)
	};

	[SerializeField]
	private Vector3 m_HandOffset = new Vector3(0f, -0f, 0.05f);

	[SerializeField]
	public float m_AssassinationLeapRangeMin_Left = 2f;

	[SerializeField]
	public float m_AssassinationLeapRangeMax_Left = 3.5f;

	[SerializeField]
	public float m_AssassinationLeapRangeMin_Right = 2f;

	[SerializeField]
	public float m_AssassinationLeapRangeMax_Right = 3.7f;

	[SerializeField]
	public float m_AssassinationLeapRangeMin_Front = 2f;

	[SerializeField]
	public float m_AssassinationLeapRangeMax_Front = 3.5f;

	[SerializeField]
	public float m_AssassinationLeapRangeMin_Back = 2f;

	[SerializeField]
	public float m_AssassinationLeapRangeMax_Back = 3.5f;

	private EntityId m_EntityId = EntityId.InvalidId;

	private Vector3 m_Velocity = Vector3.zero;

	private float m_AgentMaxSpeed;

	private NavMeshAgent m_Agent;

	private AnimationTaskManager m_TaskManager;

	private AnimatorStateMachineStatic m_AnimatorStateMachineStatic;

	private CharacterData.ICharacterDefinition m_characterDefinition;

	private float m_LastRandomUpdateSec;

	private AnimatorStateMachine m_AnimatorStateMachine;

	private AnimatorDataController m_DataController;

	private bool m_RightFoot;

	private Vector3 m_ikLookAtPosition = Vector3.zero;

	private float m_ikWeight;

	private float m_ikBodyWeight;

	private float m_ikHeadWeight = 1f;

	private float m_ikEyesWeight;

	private float m_ikClampWeight = 0.5f;

	private float m_ikWeightTarget;

	private float m_ikBodyWeightTarget;

	private float m_ikHeadWeightTarget = 1f;

	private float m_ikEyesWeightTarget;

	private float m_ikClampWeightTarget = 0.5f;

	private EMovementSpeed m_referenceMovementSpeed = EMovementSpeed.Run;

	private IdleType m_referenceIdleType = IdleType.Invalid;

	public bool ApplyRootMotion { get; set; }

	public AnimatorDataController DataController
	{
		get
		{
			return m_DataController;
		}
		private set
		{
			m_DataController = value;
		}
	}

	public Character Character
	{
		get
		{
			return m_EntityId.TryGetEntity<Character>();
		}
	}

	public CharacterData.ICharacterDefinition CharacterDefinition
	{
		get
		{
			return Character.DoIfSet((Character c) => c.CharacterDefinition.Definition, null);
		}
	}

	public FloatRange GroundJump
	{
		get
		{
			return m_GroundJump;
		}
	}

	public FloatRange WallJump
	{
		get
		{
			return m_WallJump;
		}
	}

	public Vector3 HandOffset
	{
		get
		{
			return m_HandOffset;
		}
		private set
		{
			m_HandOffset = value;
		}
	}

	public float TranslateMargin
	{
		get
		{
			return m_TranslateMargin;
		}
		private set
		{
			m_TranslateMargin = value;
		}
	}

	public float RotationMargin
	{
		get
		{
			return m_RotationMargin;
		}
		private set
		{
			m_RotationMargin = value;
		}
	}

	public float StopAnimDistance
	{
		get
		{
			return stopAnimDistance;
		}
		private set
		{
			stopAnimDistance = value;
		}
	}

	public float TurnThreshold180
	{
		get
		{
			return turnThreshold180;
		}
		private set
		{
			turnThreshold180 = value;
		}
	}

	public Vector3 Velocity
	{
		get
		{
			return m_Velocity;
		}
		set
		{
			m_Velocity = value;
		}
	}

	public AnimationTaskManager TaskManager
	{
		get
		{
			return m_TaskManager;
		}
		private set
		{
			m_TaskManager = value;
		}
	}

	public Animator Animator { get; private set; }

	public NavMeshAgent Agent
	{
		get
		{
			return m_Agent;
		}
		private set
		{
			m_Agent = value;
		}
	}

	public AnimatorStateMachine StateMachine
	{
		get
		{
			return m_AnimatorStateMachine;
		}
		private set
		{
			m_AnimatorStateMachine = value;
		}
	}

	public AnimatorStateMachineStatic StateMachineStatic
	{
		get
		{
			return m_AnimatorStateMachineStatic;
		}
	}

	public float AgentMaxSpeed
	{
		get
		{
			return m_AgentMaxSpeed;
		}
		private set
		{
			m_AgentMaxSpeed = value;
		}
	}

	public AutoScalarInterpolation CrouchValue { get; set; }

	public Vector3 NextPosition { get; set; }

	public Quaternion NextRotation { get; set; }

	public Vector3 PreviousPosition { get; private set; }

	public Quaternion PreviousRotation { get; private set; }

	// Target matching in script. Unity 2017's Animator.MatchTarget throws the character tens of metres on these clips
	// (jump downs, climbing; once to the world origin), so the game's MatchTarget calls come here instead: between
	// matchStart and matchEnd (normalized time of the current base layer state) the body part is steered onto the
	// target, closing the remaining gap a frame at a time so it arrives at matchEnd.
	private bool m_matching;

	private Vector3 m_matchPosition;

	private Quaternion m_matchRotation;

	private AvatarTarget m_matchPart;

	private MatchTargetWeightMask m_matchMask;

	private float m_matchStart;

	private float m_matchEnd;

	private int m_matchState;

	public bool IsMatchingTarget
	{
		get
		{
			return m_matching;
		}
	}

	public void MatchTarget(Vector3 _position, Quaternion _rotation, AvatarTarget _part, MatchTargetWeightMask _mask, float _start, float _end)
	{
		m_matching = true;
		m_matchPosition = _position;
		m_matchRotation = _rotation;
		m_matchPart = _part;
		m_matchMask = _mask;
		m_matchStart = _start;
		m_matchEnd = _end;
		m_matchState = Animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
	}

	public void InterruptMatchTarget()
	{
		m_matching = false;
	}

	private Vector3 MatchPartPosition()
	{
		switch (m_matchPart)
		{
		case AvatarTarget.Body:
			return Animator.bodyPosition;
		case AvatarTarget.LeftFoot:
			return Animator.GetBoneTransform(HumanBodyBones.LeftFoot).position;
		case AvatarTarget.RightFoot:
			return Animator.GetBoneTransform(HumanBodyBones.RightFoot).position;
		case AvatarTarget.LeftHand:
			return Animator.GetBoneTransform(HumanBodyBones.LeftHand).position;
		case AvatarTarget.RightHand:
			return Animator.GetBoneTransform(HumanBodyBones.RightHand).position;
		default:
			return base.transform.position;
		}
	}

	private void ApplyMatchTarget()
	{
		AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);
		if (state.fullPathHash != m_matchState)
		{
			m_matching = false;
			return;
		}
		float time = state.normalizedTime;
		if (m_matchEnd <= 1f && time > 1f && state.loop)
		{
			time %= 1f;
		}
		if (time < m_matchStart)
		{
			return;
		}
		float remaining = (m_matchEnd - time) * state.length;
		float step = Time.deltaTime * Mathf.Abs(Animator.speed);
		float weight = (remaining <= step) ? 1f : (step / remaining);
		// where the body part ends up this frame: its last pose moved by this frame's root motion
		Vector3 error = m_matchPosition - (MatchPartPosition() + (NextPosition - base.transform.position));
		NextPosition += Vector3.Scale(error, m_matchMask.positionXYZWeight) * weight;
		if (m_matchMask.rotationWeight > 0f)
		{
			NextRotation = Quaternion.Slerp(NextRotation, m_matchRotation, weight * m_matchMask.rotationWeight);
		}
		if (weight >= 1f)
		{
			m_matching = false;
		}
	}

	public EntityId EntityId
	{
		get
		{
			return m_EntityId;
		}
		set
		{
			m_EntityId = value;
		}
	}

	public EMovementSpeed ReferenceMovementSpeed
	{
		get
		{
			return m_referenceMovementSpeed;
		}
		set
		{
			m_referenceMovementSpeed = value;
		}
	}

	public IdleType ReferenceIdleType
	{
		get
		{
			return m_referenceIdleType;
		}
		set
		{
			m_referenceIdleType = value;
		}
	}

	public static void Clear()
	{
		AnimatorDataIndex.Shutdown();
	}

	public static void Copy(AnimationController _source, GameObject _dest)
	{
		Profiling.Start("AnimationController::Copy()", float.MaxValue);
		Profiling.Start("AddComponent", float.MaxValue);
		AnimationController animationController = _dest.AddComponent<AnimationController>();
		Profiling.Stop();
		animationController.turnThreshold180 = _source.turnThreshold180;
		animationController.m_BankRange = _source.m_BankRange;
		animationController.stopAnimDistance = _source.stopAnimDistance;
		animationController.m_RotationMargin = _source.m_RotationMargin;
		animationController.m_TranslateMargin = _source.m_TranslateMargin;
		animationController.m_HandOffset = _source.m_HandOffset;
		animationController.m_AssassinationLeapRangeMin_Back = _source.m_AssassinationLeapRangeMin_Back;
		animationController.m_AssassinationLeapRangeMin_Front = _source.m_AssassinationLeapRangeMin_Front;
		animationController.m_AssassinationLeapRangeMin_Left = _source.m_AssassinationLeapRangeMin_Left;
		animationController.m_AssassinationLeapRangeMin_Right = _source.m_AssassinationLeapRangeMin_Right;
		animationController.m_AssassinationLeapRangeMax_Back = _source.m_AssassinationLeapRangeMax_Back;
		animationController.m_AssassinationLeapRangeMax_Front = _source.m_AssassinationLeapRangeMax_Front;
		animationController.m_AssassinationLeapRangeMax_Left = _source.m_AssassinationLeapRangeMax_Left;
		animationController.m_AssassinationLeapRangeMax_Right = _source.m_AssassinationLeapRangeMax_Right;
		animationController.m_GroundJump = _source.m_GroundJump;
		animationController.m_WallJump = _source.m_WallJump;
		animationController.m_MovementSpeeds.Clear();
		animationController.m_MovementSpeeds.AddRange(_source.m_MovementSpeeds);
		Profiling.Stop();
	}

	public float GetBankAmount(NavMeshAgent _agent)
	{
		return Mathf.Clamp(_agent.GetTurningAngle(m_Agent.steeringTarget), 0f - m_BankRange, m_BankRange) / m_BankRange;
	}

	public float GetSpeed(Vector3 _velocity)
	{
		return _velocity.magnitude / m_AgentMaxSpeed;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.SwitchToLeftHand:
		case AnimatorEventType.SwitchToTwoHandsFromLeft:
			OnAnimatorEventSwitchToLeftHand(_event);
			break;
		case AnimatorEventType.SwitchToRightHand:
		case AnimatorEventType.SwitchToTwoHandsFromRight:
			OnAnimatorEventSwitchToRightHand(_event);
			break;
		case AnimatorEventType.SwitchToLeftFoot:
			OnAnimatorEventSwitchToLeftFoot(_event);
			break;
		case AnimatorEventType.SwitchToRightFoot:
			OnAnimatorEventSwitchToRightFoot(_event);
			break;
		}
		return false;
	}

	public void SetRightFoot(bool _rightFoot)
	{
		m_RightFoot = _rightFoot;
	}

	public void SetLookAtWeight(float _weight, float _bodyWeight = 0f, float _headWeight = 1f, float _eyesWeight = 0f, float _clampWeight = 0.5f)
	{
		m_ikWeightTarget = _weight;
		m_ikBodyWeightTarget = _bodyWeight;
		m_ikHeadWeightTarget = _headWeight;
		m_ikEyesWeightTarget = _eyesWeight;
		m_ikClampWeightTarget = _clampWeight;
	}

	public void SetLookAtPosition(Vector3 _pos)
	{
		m_ikLookAtPosition = _pos;
	}

	public bool GetRightFoot()
	{
		return m_RightFoot;
	}

	public float CalculateSpeed(EMovementSpeed _speed)
	{
		return GetSpeedFactor(_speed) * AgentMaxSpeed;
	}

	public Quaternion FaceDirection(Vector3 _direction, float _damping = 0.1f)
	{
		_direction.y = 0f;
		_direction.Normalize();
		Quaternion quaternion = NextRotation;
		if (_direction.sqrMagnitude > 0.01f)
		{
			quaternion = Quaternion.LookRotation(_direction);
		}
		float t = ((!(LogicMain.FrameTime <= 0f)) ? (LogicMain.FrameTime / (LogicMain.FrameTime + _damping)) : 0f);
		NextRotation = Quaternion.Lerp(NextRotation, quaternion, t);
		return quaternion;
	}

	public float Damping(float _start, float _end, float _damping = 0.1f)
	{
		float t = ((!(LogicMain.FrameTime <= 0f)) ? (LogicMain.FrameTime / (LogicMain.FrameTime + _damping)) : 0f);
		return Mathf.Lerp(_start, _end, t);
	}

	public bool CorrectPosition(float _damping = 0.1f)
	{
		if (LogicMain.FrameTime < float.Epsilon)
		{
			return true;
		}
		Vector3 vector = NextPosition;
		Vector3 previousPosition = PreviousPosition;
		Vector3 vector2 = vector - previousPosition;
		Vector3 normalized = vector2.normalized;
		float magnitude = vector2.magnitude;
		if (magnitude / LogicMain.FrameTime > 10f)
		{
			return true;
		}
		Profiling.Start("CorrectPosition.FindGround", float.MaxValue);
		NavMeshHit _hit;
		if (NavigationHelper.CascadingSamplePosition(vector, out _hit, m_Agent.areaMask))
		{
			Vector3 vector3 = _hit.position - vector;
			vector3.y = 0f;
			vector = _hit.position;
			if (vector3.sqrMagnitude > 0.0001f)
			{
				Profiling.Start("CorrectPosition.FindSlope", float.MaxValue);
				float num = 0.3f;
				NavMeshHit _hit2;
				NavigationHelper.SafeRaycast(previousPosition, previousPosition + normalized * num, out _hit2, m_Agent.areaMask);
				if (_hit2.distance > 0.01f && _hit2.distance < num * 2f + 0.1f)
				{
					NavMeshHit _hit3;
					NavigationHelper.SafeRaycast(_hit2.position, previousPosition, out _hit3, m_Agent.areaMask);
					if (_hit3.distance > 0.01f && _hit3.distance < num * 2f + 0.1f)
					{
						Vector3 vector4 = _hit2.position - _hit3.position;
						vector = _hit3.position + vector4.normalized * magnitude;
					}
				}
				Profiling.Stop();
			}
		}
		Profiling.Stop();
		RaycastHit hitInfo;
		if (WalkOnCollider && Physics.Raycast(vector + Vector3.up * 0.5f, Vector3.down, out hitInfo, 1f, Layers.StaticCollidersLayersMaskPreset))
		{
			vector = hitInfo.point;
		}
		if (Debugging)
		{
		}
		return CorrectPosition(vector + Vector3.up * GroundHeightOffset, _damping);
	}

	public bool CorrectPosition(Vector3 _targetPosition, float _damping = 0.1f)
	{
		if ((NextPosition - _targetPosition).sqrMagnitude > 0.0001f)
		{
			float t = ((!(LogicMain.FrameTime <= 0f)) ? (LogicMain.FrameTime / (LogicMain.FrameTime + _damping)) : 0f);
			NextPosition = Vector3.Lerp(NextPosition, _targetPosition, t);
			return true;
		}
		return false;
	}

	public void KeepDistance(Vector3 _point, Vector3 _normal, float _minDistance, bool _moveCloser = false, float _damping = 0.1f)
	{
		Vector3 nextPosition = NextPosition;
		float num = Vector3.Dot(nextPosition - _point, _normal);
		if (num < _minDistance || _moveCloser)
		{
			Vector3 b = nextPosition - _normal * (num - _minDistance);
			if (Debugging)
			{
			}
			float t = ((!(LogicMain.FrameTime <= 0f)) ? (LogicMain.FrameTime / (LogicMain.FrameTime + _damping)) : 0f);
			Vector3 nextPosition2 = Vector3.Lerp(nextPosition, b, t);
			NextPosition = nextPosition2;
		}
	}

	private void Start()
	{
		CrouchValue = new AutoScalarInterpolation();
		DataController = base.gameObject.GetComponent<AnimatorDataController>();
		DataController.RegisterEventListener(this);
		Animator.SetClimbRightHand(false);
		m_Agent.updatePosition = false;
		m_Agent.updateRotation = false;
		Animator.applyRootMotion = true;
		ApplyRootMotion = Animator.applyRootMotion;
		if (UbiCore.Utils.Singleton<AnimationMain>.IsAvailable)
		{
			m_AnimatorStateMachineStatic = UbiCore.Utils.Singleton<AnimationMain>.Instance.GetAnimatorStateMachineStatic(Animator);
		}
		m_TaskManager = new AnimationTaskManager(this);
		m_AgentMaxSpeed = m_Agent.speed;
		PreviousPosition = base.transform.position;
		PreviousRotation = base.transform.rotation;
		NextPosition = PreviousPosition;
		NextRotation = PreviousRotation;
		LogicConstants.InitializeDefaultSpeeds(this);
	}

	public void OnAnimatorIK()
	{
		Animator.SetLookAtPosition(m_ikLookAtPosition);
		m_ikWeight = Damping(m_ikWeight, m_ikWeightTarget);
		m_ikBodyWeight = Damping(m_ikBodyWeight, m_ikBodyWeightTarget);
		m_ikHeadWeight = Damping(m_ikHeadWeight, m_ikHeadWeightTarget);
		m_ikEyesWeight = Damping(m_ikEyesWeight, m_ikEyesWeightTarget);
		m_ikClampWeight = Damping(m_ikClampWeight, m_ikClampWeightTarget);
		Animator.SetLookAtWeight(m_ikWeight, m_ikBodyWeight, m_ikHeadWeight, m_ikEyesWeight, m_ikClampWeight);
	}

	public void OnAnimatorMove()
	{
		EntityWrapperComponent component = base.gameObject.GetComponent<EntityWrapperComponent>();
		if ((!(component != null) || component.Entity != null) && !(DataController == null))
		{
			NextPosition = base.transform.position;
			NextRotation = base.transform.rotation;
			if (ApplyRootMotion && !UbiCore.Utils.Singleton<Timer>.Instance.IsPaused())
			{
				NextPosition += Animator.deltaPosition;
				NextRotation *= Animator.deltaRotation;
			}
			if (CrouchValue.IsRunning || CrouchValue.Scalar != Animator.GetCurrentCrouchWalk())
			{
				Animator.SetCurrentCrouchWalk(CrouchValue.Scalar);
			}
			DataController.UpdateStates();
			OnUpdateRandom();
			m_AnimatorStateMachine.Update();
			m_TaskManager.OnUpdate();
			if (m_matching)
			{
				ApplyMatchTarget();
			}
			Profiling.Start("Apply transforms", float.MaxValue);
			float num = 0.01f * LogicMain.FrameTime;
			num *= num;
			if (Assets.Scripts.Offline.PortDebug.Enabled && (NextPosition - base.transform.position).sqrMagnitude > 4f && base.name.StartsWith("PlayerCharacter"))
			{
				Assets.Scripts.Offline.PortDebug.Log(string.Format("teleport {0}: {1} -> {2} (root motion {3}, state {4}, agent {5})", base.name, base.transform.position, NextPosition, Animator.deltaPosition, m_AnimatorStateMachine.GetStateNameByNameHash(m_AnimatorStateMachine.GetCurrentState()), Agent.nextPosition));
			}
			if ((NextPosition - base.transform.position).sqrMagnitude > num)
			{
				base.transform.position = NextPosition;
			}
			if (Quaternion.Angle(base.transform.rotation, NextRotation) > 1f * LogicMain.FrameTime)
			{
				base.transform.rotation = NextRotation;
			}
			if (!Agent.updatePosition && (NextPosition - Agent.nextPosition).sqrMagnitude > num)
			{
				Agent.nextPosition = NextPosition;
			}
			PreviousPosition = NextPosition;
			PreviousRotation = NextRotation;
			Profiling.Stop();
		}
	}

	public void Attach()
	{
		if (m_DataController != null)
		{
			m_DataController.Attach();
		}
		m_LastRandomUpdateSec = 0f;
		OnUpdateRandom();
	}

	public void Detach()
	{
		if (m_TaskManager != null)
		{
			m_TaskManager.Detach();
		}
		if (m_DataController != null)
		{
			m_DataController.Detach();
		}
		m_AnimatorStateMachine.Reset();
	}

	private void OnEnable()
	{
		Agent = GetComponent<NavMeshAgent>();
		Animator = GetComponent<Animator>();
		m_AnimatorStateMachine = new AnimatorStateMachine(Animator);
		m_Agent.SnapToMesh(m_Agent.height * 3f, m_Agent.areaMask);
	}

	private void OnDestroy()
	{
	}

	private void OnDrawGizmos()
	{
		if (m_TaskManager != null)
		{
			m_TaskManager.OnDrawGizmos();
		}
	}

	public float GetSpeedFactor(EMovementSpeed _speed)
	{
		for (int i = 0; i < m_MovementSpeeds.Count; i++)
		{
			MovementInformation movementInformation = m_MovementSpeeds[i];
			if (movementInformation.SpeedType == _speed)
			{
				return movementInformation.SpeedFactor;
			}
		}
		return 1f;
	}

	public float GetSpeedFactor()
	{
		return m_Agent.speed / m_AgentMaxSpeed;
	}

	public void OverrideSpeedFactor(EMovementSpeed _movementSpeed, float _speedFactor)
	{
		for (int i = 0; i < m_MovementSpeeds.Count; i++)
		{
			MovementInformation movementInformation = m_MovementSpeeds[i];
			if (movementInformation.SpeedType == _movementSpeed)
			{
				if (_speedFactor > MathHelper.Epsilon)
				{
					movementInformation.OverwriteSpeedFactor(_speedFactor);
				}
				else
				{
					movementInformation.ResetSpeedFactor();
				}
			}
		}
	}

	public float GetAssassinationLeapRangeMin(Direction _direction)
	{
		switch (_direction)
		{
		case Direction.Left:
			return m_AssassinationLeapRangeMin_Left;
		case Direction.Right:
			return m_AssassinationLeapRangeMin_Right;
		case Direction.Front:
			return m_AssassinationLeapRangeMin_Front;
		case Direction.Back:
			return m_AssassinationLeapRangeMin_Back;
		default:
			return 1f;
		}
	}

	public float GetAssassinationLeapRangeMax(Direction _direction)
	{
		switch (_direction)
		{
		case Direction.Left:
			return m_AssassinationLeapRangeMax_Left;
		case Direction.Right:
			return m_AssassinationLeapRangeMax_Right;
		case Direction.Front:
			return m_AssassinationLeapRangeMax_Front;
		case Direction.Back:
			return m_AssassinationLeapRangeMax_Back;
		default:
			return 1f;
		}
	}

	public void StartCrouch(float _targetCrouch, float _durationSec)
	{
		if (CrouchValue.IsRunning)
		{
			CrouchValue.Stop();
		}
		float currentCrouchWalk = Animator.GetCurrentCrouchWalk();
		float num = Mathf.Clamp01(Mathf.Abs(_targetCrouch - currentCrouchWalk));
		if (num > 0f)
		{
			CrouchValue.From = currentCrouchWalk;
			CrouchValue.To = _targetCrouch;
			CrouchValue.Speed = _durationSec / num;
			CrouchValue.Start();
		}
	}

	public bool IsEnteringCrouch()
	{
		return CrouchValue.Scalar > 0f && CrouchValue.To > 0f;
	}

	public bool IsLeavingCrouch()
	{
		return CrouchValue.Scalar < 1f && CrouchValue.To <= 0f;
	}

	public bool WarpCloseToDestination(Vector3 _destination)
	{
		NavMeshHit _hit;
		if (NavigationHelper.CascadingSamplePosition(_destination, out _hit, Agent.areaMask))
		{
			_destination = _hit.position;
		}
		NavMesh.Raycast(_destination, NextPosition, out _hit, Agent.areaMask);
		return Agent.Warp(_hit.position);
	}

	private void OnUpdateRandom()
	{
		if (m_LastRandomUpdateSec == 0f || m_LastRandomUpdateSec >= 1f)
		{
			float value = UnityEngine.Random.value;
			Animator.SetFloat(AnimatorParameters.RandomHash, value);
			m_LastRandomUpdateSec = 0f;
		}
		if (UbiCore.Utils.Singleton<Timer>.IsAvailable)
		{
			m_LastRandomUpdateSec += UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		}
		else
		{
			m_LastRandomUpdateSec += Time.deltaTime;
		}
	}

	private void OnAnimatorEventSwitchToLeftHand(AnimatorEvent _event)
	{
	}

	private void OnAnimatorEventSwitchToRightHand(AnimatorEvent _event)
	{
	}

	private void OnAnimatorEventSwitchToLeftFoot(AnimatorEvent _event)
	{
		SetRightFoot(false);
	}

	private void OnAnimatorEventSwitchToRightFoot(AnimatorEvent _event)
	{
		SetRightFoot(true);
	}
}
