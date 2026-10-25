using UnityEngine.AI;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;

public class AnimationTaskDead : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Parameters()
			: base(AnimationTaskType.Dead)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Dead;

	public static float AlignmentDamping = 0.1f;

	private bool m_rootMotion;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Dead;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskDead()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskDead(Parameters _parameters)
		: base(_parameters)
	{
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		AnimationHelper.AlignEntityToNavMeshPlane(AnimationController.EntityId, AnimationController.Agent.areaMask, LogicMain.FrameTime / (LogicMain.FrameTime + AlignmentDamping));
	}

	public override bool CanCancel()
	{
		Character character = AnimationController.EntityId.TryGetEntity<Character>();
		if (character != null && !character.GetHasZeroHealth())
		{
			return true;
		}
		return false;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
		AnimationController.Animator.SetCurrentMovementState(MovementState.Idle);
		m_rootMotion = AnimationController.ApplyRootMotion;
		AnimationController.ApplyRootMotion = true;
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.ApplyRootMotion = m_rootMotion;
		GameEntity gameEntity = AnimationController.EntityId.TryGetEntity<GameEntity>();
		if (gameEntity != null)
		{
			gameEntity.UnityComponentCache.NavMeshAgent.enabled = true;
			gameEntity.UnityComponentCache.NavMeshAgent.ResetPath();
		}
	}
}
