using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskPlayerMoveTo : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public NavMeshPath Path { get; set; }

		public float Speed { get; set; }

		public Vector3 FacingDirection { get; set; }

		public bool EnableFacing { get; set; }

		public bool ResetAgentOnCancel { get; set; }

		public float StoppingDistanceSq { get; set; }

		public EStartIdleAnimAfterTaskPolicy StartIdleWhenFinishedPolicy { get; set; }

		public int LayerMask { get; set; }

		public Parameters()
			: base(AnimationTaskType.PlayerMoveTo)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.PlayerMoveTo;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private bool m_ResetAgentOnCancel;

	private EStartIdleAnimAfterTaskPolicy m_startIdleWhenFinishedPolicy;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.PlayerMoveTo;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public override EStartIdleAnimAfterTaskPolicy StartIdleWhenFinishedPolicy
	{
		get
		{
			return m_startIdleWhenFinishedPolicy;
		}
	}

	public AnimationTaskPlayerMoveTo()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskPlayerMoveTo(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStateFollowPath(_parameters.AnimationController, true, _parameters.ResetAgentOnCancel, _parameters.StoppingDistanceSq));
		OnUpdateTarget(_parameters.Path, _parameters.Speed, _parameters.EnableFacing, _parameters.FacingDirection, _parameters.ResetAgentOnCancel, _parameters.StartIdleWhenFinishedPolicy, _parameters.StoppingDistanceSq, _parameters.LayerMask);
		m_ResetAgentOnCancel = _parameters.ResetAgentOnCancel;
		m_startIdleWhenFinishedPolicy = _parameters.StartIdleWhenFinishedPolicy;
	}

	public void OverrideResetAgentOnCancel()
	{
		m_ResetAgentOnCancel = true;
		AnimationStateFollowPath state = GetState<AnimationStateFollowPath>();
		state.ResetMovementState = true;
	}

	public void OnUpdateTarget(NavMeshPath _path, float _speed, bool _enableFacing, Vector3 _facingDirection, bool _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, float _stoppingDistanceSq = float.MaxValue, int _layerMask = 0)
	{
		m_ResetAgentOnCancel = _resetAgentOnCancel;
		m_startIdleWhenFinishedPolicy = _startIdleWhenFinishedPolicy;
		Character entity = AnimationController.EntityId.GetEntity<Character>();
		entity.UnityComponentCache.NavMeshAgent.areaMask = ((_layerMask != 0) ? _layerMask : entity.GetAutoPassableNavMeshLayerMask());
		AnimationController.Agent.speed = _speed;
		if (_path.corners.Length <= 0)
		{
			AnimationController.Agent.ResetPath();
			AnimationController.Agent.Stop();
		}
		else
		{
			if (!AnimationController.Agent.SetPath(_path))
			{
				NavMeshHit _hit;
				NavigationHelper.CascadingSamplePosition(AnimationController.NextPosition, out _hit, AnimationController.Agent.areaMask);
				AnimationController.Agent.Warp(_hit.position);
				AnimationController.Agent.SetPath(_path);
			}
			AnimationController.Agent.Resume();
		}
		AnimationStateFollowPath state = GetState<AnimationStateFollowPath>();
		state.EnableFacing = _enableFacing;
		state.FacingDirection = _facingDirection;
		state.StoppingDistanceSq = _stoppingDistanceSq;
		state.ResetMovementState = _resetAgentOnCancel;
		AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
		AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		if (_isCanceled && m_ResetAgentOnCancel && AnimationController.Agent.enabled)
		{
			AnimationController.Agent.Stop();
			AnimationController.Agent.ResetPath();
		}
		AnimationController.Agent.speed = AnimationController.CalculateSpeed(AnimationController.ReferenceMovementSpeed);
	}

	public override string ToString()
	{
		return string.Format("{0} ({1})", GetType().ToString(), base.ID);
	}
}
