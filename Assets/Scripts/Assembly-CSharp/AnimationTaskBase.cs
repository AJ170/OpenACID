using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Utilities.AutoUpdating;
using UbiCore.Debug;

public abstract class AnimationTaskBase : AutoUpdateablesCarrier, IAnimation, IAnimationTask
{
	private static int s_idCounter;

	private readonly int m_ID = ++s_idCounter;

	private AnimationController m_AnimationController;

	private List<AnimationStateBase> m_States = new List<AnimationStateBase>();

	private int m_CurrentStateIndex = -1;

	private bool m_TaskStarted;

	private bool m_IsFinished;

	private string m_ProfilerTag;

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

	public int StateCount
	{
		get
		{
			return m_States.Count;
		}
	}

	public EntityId Owner
	{
		get
		{
			return AnimationController.EntityId;
		}
	}

	protected List<AnimationStateBase> States
	{
		get
		{
			return m_States;
		}
		private set
		{
			m_States = value;
		}
	}

	public AnimationTaskManager.ELayer Layer { get; private set; }

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

	public int CurrentStateIndex
	{
		get
		{
			return m_CurrentStateIndex;
		}
	}

	public AnimationStateBase CurrentState
	{
		get
		{
			if (m_CurrentStateIndex >= 0 && m_CurrentStateIndex < m_States.Count)
			{
				return m_States[m_CurrentStateIndex];
			}
			return null;
		}
	}

	public int ID
	{
		get
		{
			return m_ID;
		}
	}

	public abstract AnimationTaskType TaskType { get; }

	public virtual EStartIdleAnimAfterTaskPolicy StartIdleWhenFinishedPolicy
	{
		get
		{
			return EStartIdleAnimAfterTaskPolicy.DontCare;
		}
	}

	protected AnimationTaskBase(AnimationTaskParameters _parameters)
	{
		m_ProfilerTag = string.Format("{0}::OnUpdate", _parameters.AnimationTaskType.ToString());
		AnimationController = _parameters.AnimationController;
		Layer = _parameters.Layer;
	}

	public override string ToString()
	{
		return string.Format("{0} ({1})", GetType().ToString(), ID);
	}

	public virtual bool CanCancel()
	{
		bool result = true;
		AnimationStateBase currentState = CurrentState;
		if (currentState != null)
		{
			result = currentState.CanCancel();
		}
		return result;
	}

	protected AnimationStateBase GetState(int _stateIndex)
	{
		AnimationStateBase result = null;
		if (_stateIndex >= 0 && _stateIndex < m_States.Count)
		{
			result = m_States[_stateIndex];
		}
		return result;
	}

	public string GetStateName(int _stateIndex)
	{
		string result = string.Empty;
		AnimationStateBase state = GetState(_stateIndex);
		if (state != null)
		{
			result = state.ToString();
		}
		return result;
	}

	public bool GetIsStateEnabled(int _stateIndex)
	{
		bool result = false;
		AnimationStateBase state = GetState(_stateIndex);
		if (state != null)
		{
			result = state.IsEnabled;
		}
		return result;
	}

	protected T GetState<T>() where T : AnimationStateBase
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			AnimationStateBase animationStateBase = m_States[i];
			if (animationStateBase.GetType() == typeof(T))
			{
				return animationStateBase as T;
			}
		}
		return (T)null;
	}

	protected T OnAddState<T>(T _state) where T : AnimationStateBase
	{
		_state.Task = this;
		m_States.Add(_state);
		return _state;
	}

	protected virtual void OnStateExited(AnimationStateBase _state, bool _canceled)
	{
	}

	public virtual void OnDrawGizmos()
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			m_States[i].OnDrawGizmos();
		}
	}

	public virtual void OnEnter()
	{
		StartAutoUpdateables();
		AnimationController.TaskManager.OnAnimationTaskEnter(this);
	}

	public virtual void OnExit(bool _isCanceled)
	{
		StopAutoUpdateables();
		if (_isCanceled && CurrentState != null)
		{
			HandleAnimationStateExit(CurrentState, true);
		}
		AnimationController.TaskManager.OnAnimationTaskExit(this, _isCanceled);
	}

	public virtual void OnUpdate()
	{
		Profiling.Start(m_ProfilerTag, float.MaxValue);
		if (!m_TaskStarted)
		{
			OnEnter();
			m_TaskStarted = true;
		}
		bool flag = true;
		if (m_States.Count > 0)
		{
			if (m_CurrentStateIndex < 0)
			{
				m_CurrentStateIndex = 0;
				if (CurrentState.IsEnabled)
				{
					HandleAnimationStateEnter(CurrentState);
				}
				flag = false;
			}
			if (CurrentState != null)
			{
				bool flag2 = true;
				do
				{
					if (CurrentState.IsEnabled)
					{
						AnimationStateBase currentState = CurrentState;
						Profiling.Start(currentState.ProfilerTag, float.MaxValue);
						currentState.OnUpdate();
						Profiling.Stop();
					}
					if (CurrentState.IsFinished || !CurrentState.IsEnabled)
					{
						if (CurrentState.IsEnabled)
						{
							HandleAnimationStateExit(CurrentState, false);
						}
						m_CurrentStateIndex++;
						if (m_CurrentStateIndex < m_States.Count)
						{
							if (CurrentState.IsEnabled)
							{
								HandleAnimationStateEnter(CurrentState);
							}
							flag2 = false;
							flag = false;
						}
					}
					else
					{
						flag2 = true;
						flag = false;
					}
				}
				while (!flag2 && CurrentState != null);
			}
		}
		else
		{
			flag = IsFinished;
		}
		if (flag)
		{
			OnExit(false);
		}
		Profiling.Stop();
	}

	public void Restart()
	{
		m_CurrentStateIndex = -1;
	}

	public void SetCurrentStateIndex(int _index)
	{
		m_CurrentStateIndex = _index;
	}

	private void HandleAnimationStateEnter(AnimationStateBase _animationState)
	{
		AnimationController.TaskManager.OnAnimationStateEnter(_animationState, this);
		_animationState.CheckPreConditions();
		_animationState.OnEnter();
	}

	private void HandleAnimationStateExit(AnimationStateBase _animationState, bool _canceled)
	{
		_animationState.OnExit(_canceled);
		AnimationController.TaskManager.OnAnimationStateExit(_animationState, this, _canceled);
		OnStateExited(_animationState, _canceled);
		_animationState.CheckPostConditions();
	}

	public virtual void OnDispose()
	{
		for (int i = 0; i < m_States.Count; i++)
		{
			m_States[i].OnDispose();
		}
	}
}
