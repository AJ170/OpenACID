using Assets.Scripts.Animation;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils;
using UnityEngine;

public class AnimationTaskLeapOfFaith : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public Haystack Haystack { get; set; }

		public Parameters()
			: base(AnimationTaskType.LeapOfFaith)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.LeapOfFaith;

	public static float TurnMinAngle = 90f;

	public static float HaystackReactionTriggerDistance = 3f;

	public static float MaxMatchHeight = 30f;

	public static float MinMatchRatio = 0.7f;

	public static float MatchRatioMult = 0.5f;

	public static float FallVelocityDamping = 0.15f;

	public static float FallAcceleration = 0f;

	public static float MinDampingHeight = 2f;

	public static float MaxDampingHeight = 4f;

	public static float PerceivedVelocityDamping = 0.05f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private Haystack m_haystack;

	private Vector3 m_startPosition;

	private Vector3 m_endPosition;

	private Vector3 m_fallOffset;

	private Vector3 m_fallFacing;

	private float m_fallHeight;

	private float m_fallLength;

	private bool m_haystackReactionPlayed;

	private bool m_fallToHaystack;

	private float m_fallSpeed = 1f;

	private Vector3 m_perceivedVelocity = Vector3.zero;

	private Vector3 m_lastHipPosition = Vector3.zero;

	private Transform m_hipTransform;

	private bool m_askMatch;

	private Vector3 m_matchPoint = Vector3.zero;

	private AvatarTarget m_matchBodyPart = AvatarTarget.Body;

	private static MatchTargetWeightMask s_weightMask = new MatchTargetWeightMask(new Vector3(1f, 1f, 1f), 0f);

	private bool m_waitForTurn;

	private NpcTurnDirection m_turnDirection;

	private float m_turnCorrectionAngle;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.LeapOfFaith;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskLeapOfFaith()
		: this(ParametersTemplate)
	{
		AnimationController.StateMachine.StateEntered += OnStateEntered;
		AnimationController.GetComponent<AnimatorDataController>().RegisterEventListener(this);
	}

	public AnimationTaskLeapOfFaith(Parameters _parameters)
		: base(_parameters)
	{
		m_haystack = _parameters.Haystack;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.ApplyRootMotion = true;
		m_startPosition = AnimationController.NextPosition;
		m_endPosition = m_haystack.InteractionLocators.Center.transform.position;
		m_fallOffset = m_endPosition - m_startPosition;
		m_fallHeight = 0f - m_fallOffset.y;
		m_fallFacing = m_fallOffset;
		m_fallFacing.y = 0f;
		m_fallLength = m_fallFacing.magnitude;
		m_fallFacing.Normalize();
		float num = (m_fallHeight - MaxMatchHeight) / m_fallHeight;
		float num2 = Mathf.Lerp(1f, MinMatchRatio, Mathf.Clamp01(num * MatchRatioMult));
		AnimationController.Animator.SetCurrentJumpDistanceY(Mathf.Min(m_fallHeight, MaxMatchHeight));
		AnimationController.Animator.SetCurrentJumpDistanceZ(m_fallLength);
		m_matchPoint = m_startPosition + m_fallFacing * m_fallLength * num2 + Vector3.down * Mathf.Min(m_fallHeight, MaxMatchHeight);
		float num3 = Vector3.Angle(m_fallFacing, AnimationController.transform.forward) * Mathf.Sign(Vector3.Dot(m_fallFacing, AnimationController.transform.right));
		if (Mathf.Abs(num3) > TurnMinAngle)
		{
			float currentSpeed = AnimationController.Animator.GetCurrentSpeed();
			float speed = Mathf.Min(currentSpeed, 0.4f);
			AnimationController.Animator.SetCurrentSpeed(speed);
			m_turnDirection = AnimationHelper.DetermineNpcTurnDirection(0f, num3);
			m_turnCorrectionAngle = num3 - AnimationHelper.NpcTurnDirectionToAngle(m_turnDirection);
			AnimationController.Animator.SetCurrentNpcTurnDirection(m_turnDirection);
			AnimationController.Animator.SetCurrentMovementState(MovementState.Turn);
			m_waitForTurn = true;
		}
		else
		{
			StartLeap();
		}
	}

	private void StartLeap()
	{
		AnimationController.Animator.SetCurrentLinkType(eLinkType.ToHayStack);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.OnLink);
		AnimationController.Animator.SetCurrentStopAction(false);
		m_hipTransform = AnimationController.Animator.GetBoneTransform(HumanBodyBones.Hips);
		m_lastHipPosition = m_hipTransform.position;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentStopAction(false);
		AnimationController.NextPosition = m_endPosition;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.GetComponent<AnimatorDataController>().UnregisterEventListener(this);
	}

	private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _event)
	{
		if (_event.State.CompareNameHash(AnimationStateHashes.State_HayStack_Idle))
		{
			base.IsFinished = true;
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
			AnimationController.Agent.Warp(m_endPosition);
		}
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		switch (_event.Type)
		{
		case AnimatorEventType.MatchStart:
			m_askMatch = true;
			TryStartMatching();
			break;
		case AnimatorEventType.MatchEnd:
			m_fallToHaystack = true;
			m_fallSpeed = m_perceivedVelocity.magnitude;
			AnimationController.ApplyRootMotion = false;
			break;
		case AnimatorEventType.TurnEnd:
			if (m_waitForTurn)
			{
				StartLeap();
			}
			m_waitForTurn = false;
			break;
		}
		return false;
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		bool matchTargetData = AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0);
		AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
		if (matchTargetData)
		{
			AnimationController.Animator.MatchTarget(m_matchPoint, Quaternion.identity, m_matchBodyPart, s_weightMask, currentAnimatorStateInfo.normalizedTime, _matchEnd);
			m_askMatch = false;
		}
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_waitForTurn)
		{
			float num = Damping.Damp(m_turnCorrectionAngle, 0f, 0.2f, 1f);
			AnimationController.NextRotation = Quaternion.Euler(0f, m_turnCorrectionAngle - num, 0f) * AnimationController.NextRotation;
			m_turnCorrectionAngle = num;
			return;
		}
		AnimationController.FaceDirection(m_fallFacing);
		if (m_askMatch)
		{
			TryStartMatching();
		}
		if (AnimationController.Animator.isMatchingTarget)
		{
			Vector3 position = m_hipTransform.position;
			Vector3 vector = m_matchPoint - position;
		}
		Vector3 vector2 = m_endPosition - AnimationController.NextPosition;
		Vector3 normalized = vector2.normalized;
		float num2 = 0f - vector2.y;
		float magnitude = vector2.magnitude;
		if (magnitude <= HaystackReactionTriggerDistance && !m_haystackReactionPlayed)
		{
			m_haystackReactionPlayed = true;
			m_haystack.TriggerJumpInAnimation();
		}
		if (m_fallToHaystack)
		{
			m_fallSpeed += FallAcceleration * LogicMain.FrameTime;
			Vector3 b = normalized * m_fallSpeed;
			Vector3 vector3 = (m_perceivedVelocity = Vector3.Lerp(m_perceivedVelocity, b, Damping.CalculateFactor(FallVelocityDamping * Mathf.Clamp01((num2 - MinDampingHeight) / (MaxDampingHeight - MinDampingHeight)), 1f)));
			if (AnimationController.NextPosition.y + vector3.y * LogicMain.FrameTime <= m_endPosition.y)
			{
				AnimationController.Animator.SetCurrentStopAction(true);
				m_fallToHaystack = false;
				AnimationController.NextPosition = m_endPosition;
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_body_enter_hay);
				if (!m_haystackReactionPlayed)
				{
					m_haystackReactionPlayed = true;
					m_haystack.TriggerJumpInAnimation();
				}
			}
			else
			{
				AnimationController.NextPosition += vector3 * LogicMain.FrameTime;
			}
		}
		else
		{
			m_perceivedVelocity = Vector3.Lerp(m_perceivedVelocity, (AnimationController.NextPosition - m_lastHipPosition) / LogicMain.FrameTime, LogicMain.FrameTime / (LogicMain.FrameTime + PerceivedVelocityDamping));
			m_lastHipPosition = AnimationController.NextPosition;
		}
	}
}
