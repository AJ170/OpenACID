using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskMoveTo : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public NavMeshPath Path { get; set; }

		public bool Accelerate { get; set; }

		public float Speed { get; set; }

		public bool ResetAgentOnCancel { get; set; }

		public float StoppingDistanceSq { get; set; }

		public EStartIdleAnimAfterTaskPolicy StartIdleWhenFinishedPolicy { get; set; }

		public int LayerMask { get; set; }

		public Parameters()
			: base(AnimationTaskType.MoveTo)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.MoveTo;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private NavMeshPath m_Path;

	private bool m_Accelerate;

	private float m_Speed;

	private bool m_ResetAgentOnCancel;

	private float m_StoppingDistanceSq;

	private EStartIdleAnimAfterTaskPolicy m_StartIdleWhenFinishedPolicy;

	private int m_LayerMask;

	private Vector3 m_Target = Vector3.zero;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.MoveTo;
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
			return m_StartIdleWhenFinishedPolicy;
		}
	}

	public AnimationTaskMoveTo()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskMoveTo(Parameters _parameters)
		: base(_parameters)
	{
		OnAddState(new AnimationStateFollowPath(_parameters.AnimationController, _parameters.Accelerate, _parameters.ResetAgentOnCancel, _parameters.StoppingDistanceSq));
		m_Path = _parameters.Path;
		m_Accelerate = _parameters.Accelerate;
		m_ResetAgentOnCancel = _parameters.ResetAgentOnCancel;
		m_StoppingDistanceSq = _parameters.StoppingDistanceSq;
		m_Speed = _parameters.Speed;
		m_StartIdleWhenFinishedPolicy = _parameters.StartIdleWhenFinishedPolicy;
		m_LayerMask = _parameters.LayerMask;
	}

	public void OnUpdateTarget(NavMeshPath _path, float _speed, bool _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, bool _accelerate, float _stoppingDistanceSq = float.MaxValue, int _layerMask = 0)
	{
		m_ResetAgentOnCancel = _resetAgentOnCancel;
		m_StartIdleWhenFinishedPolicy = _startIdleWhenFinishedPolicy;
		m_LayerMask = _layerMask;
		Character entity = AnimationController.EntityId.GetEntity<Character>();
		entity.UnityComponentCache.NavMeshAgent.areaMask = ((m_LayerMask != 0) ? m_LayerMask : entity.GetAutoPassableNavMeshLayerMask());
		if (_path.corners.Length > 0 && _path.corners[_path.corners.Length - 1] != m_Target)
		{
			AnimationController.Agent.speed = _speed;
			Vector3[] corners = _path.corners;
			m_Target = ((corners.Length <= 1) ? AnimationController.Agent.steeringTarget : corners[1]);
			AnimationController.Animator.SetCurrentLinkType(eLinkType.Invalid);
			AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
			if (!AnimationController.Agent.SetPath(_path))
			{
				AnimationController.Agent.Warp(_path.corners[0]);
				AnimationController.Agent.SetPath(_path);
			}
			AnimationController.Agent.Resume();
			if (base.CurrentState != null)
			{
				Restart();
			}
			if (!_accelerate)
			{
				Vector3 velocity = m_Target - AnimationController.NextPosition;
				velocity.Normalize();
				velocity *= _speed;
				AnimationController.Agent.velocity = velocity;
			}
			AnimationStateFollowPath state = GetState<AnimationStateFollowPath>();
			state.StoppingDistanceSq = _stoppingDistanceSq;
			state.ResetMovementState = _resetAgentOnCancel;
		}
	}

	public override void OnEnter()
	{
		base.OnEnter();
		OnUpdateTarget(m_Path, m_Speed, m_ResetAgentOnCancel, m_StartIdleWhenFinishedPolicy, m_Accelerate, m_StoppingDistanceSq, m_LayerMask);
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
		return string.Format("{0} ({1} - {2})", GetType().ToString(), base.ID, m_Target);
	}
}
