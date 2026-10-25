using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Utilities.AutoUpdating;

public abstract class AnimationStateBase : AutoUpdateablesCarrier, IAnimation
{
	private AnimationStateType m_StateType = AnimationStateType.Unknown;

	private AnimationController m_AnimationController;

	private bool m_IsFinished;

	private bool m_IsEnabled = true;

	public AnimationController AnimationController
	{
		get
		{
			return m_AnimationController;
		}
		private set
		{
			m_AnimationController = value;
		}
	}

	public string ProfilerTag { get; private set; }

	public bool IsEnabled
	{
		get
		{
			return m_IsEnabled;
		}
		set
		{
			m_IsEnabled = value;
		}
	}

	public bool IsFinished
	{
		get
		{
			return m_IsFinished;
		}
		protected set
		{
			m_IsFinished = value;
			if (m_IsFinished)
			{
				StopAutoUpdateables();
			}
		}
	}

	public AnimationStateType StateType
	{
		get
		{
			return m_StateType;
		}
		private set
		{
			m_StateType = value;
		}
	}

	public AnimationTaskBase Task { get; set; }

	protected AnimationStateBase(AnimationStateType _stateType, AnimationController _animationController)
	{
		ProfilerTag = string.Format("{0}::OnUpdate", _stateType.ToString());
		StateType = _stateType;
		AnimationController = _animationController;
	}

	public virtual void OnDrawGizmos()
	{
	}

	public virtual void CheckPreConditions()
	{
	}

	public virtual void OnEnter()
	{
		StartAutoUpdateables();
	}

	public virtual void CheckPostConditions()
	{
	}

	public virtual void OnExit(bool _isCanceled)
	{
		StopAutoUpdateables();
	}

	public virtual void OnUpdate()
	{
	}

	public virtual bool CanCancel()
	{
		return true;
	}

	public override string ToString()
	{
		return GetType().ToString();
	}

	protected bool GetMatchTargetData(out float _matchStart, out float _matchEnd, int _layer)
	{
		return AnimationHelper.GetMatchTargetData(AnimationController, out _matchStart, out _matchEnd, _layer);
	}

	public virtual void OnDispose()
	{
	}
}
