using System;
using System.Collections.Generic;

public class AnimationPlotSequence
{
	private Action<bool> m_actionOnFinish;

	private List<AnimationPlotStateBase> m_states;

	public bool IsFinished { get; private set; }

	public AnimationPlotSequence()
	{
		m_states = new List<AnimationPlotStateBase>();
	}

	public void AddState(AnimationPlotStateBase _state)
	{
		_state.Owner = this;
		m_states.Add(_state);
	}

	public void Start(Action<bool> _actionOnFinish = null)
	{
		m_actionOnFinish = _actionOnFinish;
		if (m_states.Count > 0)
		{
			m_states[0].OnStart();
		}
	}

	public void OnStateFinished(AnimationPlotStateBase _state)
	{
		int num = m_states.IndexOf(_state);
		if (num < 0)
		{
			return;
		}
		m_states[num].OnStop();
		if (num < m_states.Count - 1)
		{
			m_states[++num].OnStart();
			return;
		}
		IsFinished = true;
		if (m_actionOnFinish != null)
		{
			m_actionOnFinish(false);
		}
	}
}
