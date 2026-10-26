using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskAssassinateLeapInstigator : AnimationTaskBase, IAnimatorEventListener
{
	public class Parameters : AnimationTaskParameters
	{
		public GameObject Instigator { get; set; }

		public GameObject Target { get; set; }

		public bool WithKillCam { get; set; }

		public Parameters()
			: base(AnimationTaskType.AssassinateLeapInstigator)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.AssassinateLeapInstigator;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private AnimationController.Direction m_targetDir;

	private bool m_initiated;

	private EntityId m_targetId;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.AssassinateLeapInstigator;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public GameObject Target { get; private set; }

	public GameObject Instigator { get; private set; }

	public AnimationTaskAssassinateLeapInstigator()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskAssassinateLeapInstigator(Parameters _parameters)
		: base(_parameters)
	{
		Instigator = _parameters.Instigator;
		Target = _parameters.Target;
		m_targetId = Target.GetComponent<AnimationController>().EntityId;
		NavMeshPath path = new NavMeshPath();
		AnimationController.Agent.CalculatePath(Target.transform.position, path);
		AnimationController.Agent.SetPath(path);
		AnimationController.Agent.updatePosition = true;
		AnimationController.Agent.updateRotation = true;
		AnimationController.Agent.Resume();
		AnimationController.ApplyRootMotion = false;
		AnimationController.Animator.SetCurrentMovementState(MovementState.Movement);
		AnimationController.StateMachine.StateMachineEntered += OnStateMachineEntered;
		AnimationController.StateMachine.StateMachineExited += OnStateMachineExited;
		AnimationController.DataController.RegisterEventListener(this);
		m_targetDir = AnimationHelper.DetermineDirection(Instigator, Target);
	}

	private void OnStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationLeap))
		{
			Vector3 vector = Target.transform.position - Instigator.transform.position;
			vector.Normalize();
			vector *= 0.9f;
			Vector3 vector2 = new Vector3(vector.z, vector.y, 0f - vector.x);
			vector2 *= 0.35f;
			vector += vector2;
			AnimationController.MatchTarget(Target.transform.position + vector, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(new Vector3(1f, 0f, 1f), 0f), 0.05f, 0.23f);
			AnimationController.Animator.SetCurrentActionType(ActionType.Default);
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineEntered;
		}
	}

	private void OnStateMachineExited(object _sender, AnimatorStateMachine.StateMachineEventArgs _event)
	{
		if (_event.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_AssassinationLeap))
		{
			AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
			base.IsFinished = true;
			AnimationController.StateMachine.StateMachineEntered -= OnStateMachineExited;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		if (!m_initiated)
		{
			Vector3 vector = Instigator.transform.position - Target.transform.position;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			float assassinationLeapRangeMax = AnimationController.GetAssassinationLeapRangeMax(m_targetDir);
			if (!AnimationController.Agent.hasPath || magnitude <= assassinationLeapRangeMax)
			{
				m_initiated = true;
				AnimationController.ApplyRootMotion = true;
				AnimationController.Agent.updatePosition = false;
				AnimationController.Agent.updateRotation = false;
				AnimationController.Agent.Stop();
				AnimationController.Agent.ResetPath();
				AnimationController.Animator.SetCurrentActionType(ActionType.AssassinateLeap);
				AnimationController.NextRotation = AnimationHelper.AlignInstigatorToTarget(Instigator, Target);
			}
		}
		if (AnimationController.StateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_AssassinationLeap.NameHash)
		{
			AnimatorStateInfo currentAnimatorStateInfo = AnimationController.Animator.GetCurrentAnimatorStateInfo(0);
			float num = 0.75f;
			float num2 = 0f;
			if (currentAnimatorStateInfo.normalizedTime < num)
			{
				num2 = 1f * (currentAnimatorStateInfo.normalizedTime / num);
				AnimationHelper.AlignEntityToNavMeshPlaneWithTargetOrientation(AnimationController.EntityId, AnimationController.Agent.areaMask, num2, m_targetId);
			}
			else
			{
				num2 = 1f - (currentAnimatorStateInfo.normalizedTime - num) / (1f - num);
				AnimationHelper.AlignEntityToUpVector(AnimationController.EntityId, Vector3.up, 1f - num2);
			}
		}
	}

	public override void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawCube(Instigator.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
		Gizmos.DrawCube(Target.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
		Gizmos.color = Color.red;
		if (AnimationController.Agent.hasPath)
		{
			NavMeshPath path = AnimationController.Agent.path;
			Vector3[] corners = path.corners;
			Gizmos.DrawCube(corners[path.corners.Length - 1], new Vector3(0.25f, 0.25f, 0.25f));
		}
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
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.CameraEnd)
		{
			CameraStateHelper.OnCameraEnd();
		}
		return false;
	}

	public override bool CanCancel()
	{
		return false;
	}
}
