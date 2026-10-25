using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.System;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskBlackjackInstigator : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public EntityId Instigator { get; set; }

		public EntityId Target { get; set; }

		public AnimationController.Direction TargetDir { get; set; }

		public Parameters()
			: base(AnimationTaskType.BlackjackInstigator)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BlackjackInstigator;

	private const float SlerpDurationSec = 0.2f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private readonly Character m_instigatorChar;

	private EntityId m_target = EntityId.InvalidId;

	private readonly AnimationController.Direction m_targetDir;

	private float m_slerpCurrent;

	private Quaternion m_initialRotation = Quaternion.identity;

	private Quaternion m_targetRotation = Quaternion.identity;

	private bool m_initiated;

	private bool m_targetReactionStarted;

	private float m_startTime;

	private readonly bool m_rootMotion;

	private bool m_askMatch;

	private static readonly MatchTargetWeightMask WeightMask = new MatchTargetWeightMask(new Vector3(1f, 1f, 1f), 1f);

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BlackjackInstigator;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public GameObject Target
	{
		get
		{
			return m_target.TryGetEntity<Character>().GameObject;
		}
	}

	public AnimationTaskBlackjackInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBlackjackInstigator(Parameters _parameters)
		: base(_parameters)
	{
		m_instigatorChar = _parameters.Instigator.GetEntity<Character>();
		m_target = _parameters.Target;
		m_targetDir = _parameters.TargetDir;
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.DataController.RegisterEventListener(this);
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Blackjack))
		{
			AnimationController.Animator.SetCurrentActionType(ActionType.Default);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Blackjack))
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineExited;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		m_startTime = UbiCore.Utils.Singleton<Timer>.Instance.time;
		AnimationController.Agent.updatePosition = false;
		AnimationController.Agent.updateRotation = false;
		AnimationController.Agent.Stop();
		AnimationController.Agent.ResetPath();
		m_initialRotation = AnimationController.NextRotation;
		Character character = m_target.TryGetEntity<Character>();
		if (character != null && character.IsAttached)
		{
			m_targetRotation = AnimationHelper.AlignInstigatorToTarget(m_instigatorChar.GameObject, character.GameObject);
		}
		else
		{
			m_targetRotation = m_initialRotation;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (m_askMatch)
		{
			TryStartMatching();
		}
		Vector3 vector = Vector3.zero;
		Character character = m_target.TryGetEntity<Character>();
		if (character != null)
		{
			vector = m_instigatorChar.Position - character.Position;
		}
		if (AnimationController.Animator.GetCurrentMovementState() != 0 && vector.sqrMagnitude < 1f)
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		}
		float num = Quaternion.RotateTowards(m_initialRotation, m_targetRotation, 360f).eulerAngles.y / 180f;
		float num2 = 0.8f * num;
		float num3 = ((!(num2 > float.Epsilon)) ? 0f : Mathf.Clamp(UbiCore.Utils.Singleton<Timer>.Instance.time - m_startTime, 0f, num2));
		bool flag = false;
		if (num3 > 0f)
		{
			num3 /= num2;
			num3 = Mathf.Clamp01(num3);
			flag = num3 >= 1f;
		}
		else
		{
			flag = true;
		}
		if (!m_initiated && flag)
		{
			m_initiated = true;
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			AnimationController.Animator.SetCurrentActionType(ActionType.Blackjack);
			AnimationController.Animator.SetTargetDirection(m_targetDir);
		}
		if (!m_targetReactionStarted && !flag)
		{
			num3 = Mathf.Clamp01(m_slerpCurrent / 0.2f);
			AnimationController.NextRotation = Quaternion.Slerp(m_initialRotation, m_targetRotation, num3);
			m_slerpCurrent += LogicMain.FrameTime;
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = m_rootMotion;
	}

	public override bool CanCancel()
	{
		return false;
	}

	public override void OnDispose()
	{
		base.OnDispose();
		AnimationController.DataController.UnregisterEventListener(this);
		AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited -= OnStateMachineExited;
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		if (!AnimationStateHashes.IsStateContainedInArray(_event.StateNameHash, AnimationStateHashes.States_Blackjack))
		{
			return false;
		}
		switch (_event.Type)
		{
		case AnimatorEventType.Hit:
			m_targetReactionStarted = true;
			break;
		case AnimatorEventType.MatchStart:
			m_askMatch = true;
			TryStartMatching();
			break;
		}
		return false;
	}

	private float GetBlackjackRange(AnimationController.Direction _direction)
	{
		return 1.1f;
	}

	private void TryStartMatching()
	{
		float _matchStart;
		float _matchEnd;
		if (!AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, 0))
		{
			return;
		}
		m_askMatch = false;
		Character character = m_target.TryGetEntity<Character>();
		if (character != null)
		{
			Vector3 vector = m_instigatorChar.Position - character.Position;
			vector.Normalize();
			vector *= GetBlackjackRange(m_targetDir);
			Vector3 vector2 = character.Position + vector;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(vector2, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				vector2 = _hit.position;
			}
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			AnimationController.Animator.MatchTarget(vector2, m_targetRotation, AvatarTarget.Root, WeightMask, currentAnimatorStateInfo.normalizedTime, _matchEnd);
		}
	}
}
