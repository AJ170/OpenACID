using Assets.Scripts.Animation;
using UbiCore;
using UnityEngine;

public class AnimationTaskStrafe : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public float DirectionX { get; set; }

		public float DirectionZ { get; set; }

		public Parameters()
			: base(AnimationTaskType.Strafe)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Strafe;

	public const float TurningSpeed = 2f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	private float m_directionX;

	private float m_directionZ;

	private bool m_dirty;

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Strafe;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskStrafe()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskStrafe(Parameters _parameters)
		: base(_parameters)
	{
		OnUpdateDirectionX(_parameters.DirectionX);
		OnUpdateDirectionZ(_parameters.DirectionZ);
	}

	public void OnUpdateDirectionX(float _directionX)
	{
		OnUpdateDirection(_directionX, AnimatorParameters.StrafeDirectionX, ref m_directionX);
	}

	public void OnUpdateDirectionZ(float _directionZ)
	{
		OnUpdateDirection(_directionZ, AnimatorParameters.StrafeDirectionZ, ref m_directionZ);
	}

	public override void OnEnter()
	{
		base.OnEnter();
		AnimationController.Animator.SetCurrentMovementState(MovementState.Strafe);
	}

	public override void OnUpdate()
	{
		base.OnUpdate();
		bool flag = !MathHelper.Approximately(m_directionX, 0f) || !MathHelper.Approximately(m_directionZ, 0f);
		if (m_dirty)
		{
			m_dirty = false;
			Animator animator = AnimationController.Animator;
			if (flag)
			{
				animator.SetCurrentStrafeDirectionX(m_directionX);
				animator.SetCurrentStrafeDirectionZ(m_directionZ);
			}
			else
			{
				animator.SetCurrentStrafeDirectionX(0f);
				animator.SetCurrentStrafeDirectionZ(0f);
				AnimationController.Agent.Stop();
			}
		}
		if (flag)
		{
			AnimationController.CorrectPosition();
		}
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		Animator animator = AnimationController.Animator;
		animator.SetCurrentStrafeDirectionX(0f);
		animator.SetCurrentStrafeDirectionZ(0f);
	}

	public override string ToString()
	{
		return string.Format("{0} ({1} - {2}/{3})", GetType().ToString(), base.ID, m_directionX, m_directionZ);
	}

	private void OnUpdateDirection(float _direction, int _directionParamId, ref float _newDirection)
	{
		if (!MathHelper.Approximately(_direction, AnimationController.Animator.GetFloat(_directionParamId)))
		{
			_newDirection = _direction;
			m_dirty = true;
		}
	}
}
