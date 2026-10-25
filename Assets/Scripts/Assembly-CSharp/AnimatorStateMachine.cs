using System;
using System.Collections.Generic;
using Assets.Scripts.Animation;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

public class AnimatorStateMachine
{
	public class StateChangeEventArgs : EventArgs
	{
		public AnimatorStateMachineStatic.State PreviousState { get; set; }

		public AnimatorStateMachineStatic.State CurrentState { get; set; }
	}

	public class StateMachineChangeEventArgs : EventArgs
	{
		public AnimatorStateMachineStatic.StateMachine PreviousStateMachine { get; set; }

		public AnimatorStateMachineStatic.StateMachine CurrentStateMachine { get; set; }
	}

	public class TransitionEventArgs : EventArgs
	{
		public AnimatorStateMachineStatic.Transition Transition { get; set; }
	}

	public class StateEventArgs : EventArgs
	{
		public AnimatorStateMachineStatic.State State { get; set; }

		public int Layer { get; set; }

		public int LoopCount { get; set; }
	}

	public class StateMachineEventArgs : EventArgs
	{
		public AnimatorStateMachineStatic.StateMachine StateMachine { get; set; }
	}

	private AnimatorStateInfo[] m_LastCurrentStateInfo;

	private AnimatorTransitionInfo[] m_LastTransitionInfo;

	private Animator m_Animator;

	private AnimatorStateMachineStatic m_AnimatorStateMachineStatic;

	private StateMachineEventArgs m_StateMachineEventArgs = new StateMachineEventArgs();

	private StateMachineChangeEventArgs m_StateMachineChangeEventArgs = new StateMachineChangeEventArgs();

	private StateChangeEventArgs m_StateChangeEventArgs = new StateChangeEventArgs();

	private StateEventArgs m_StateEventArgs = new StateEventArgs();

	private TransitionEventArgs m_TransitionEventArgs = new TransitionEventArgs();

	private static AnimatorTransitionInfo s_EmptyTransitionInfo = default(AnimatorTransitionInfo);

	private static AnimatorStateMachineStatic.StateMachineRoute s_route = new AnimatorStateMachineStatic.StateMachineRoute();

	public Dictionary<int, AnimatorStateMachineStatic.Parameter> DefaultParameters
	{
		get
		{
			return m_AnimatorStateMachineStatic.DefaultParameters;
		}
	}

	public event EventHandler<StateChangeEventArgs> StateChanged;

	public event EventHandler<StateEventArgs> StateEntered;

	public event EventHandler<StateEventArgs> StateExited;

	public event EventHandler<StateMachineChangeEventArgs> StateMachineChanged;

	public event EventHandler<StateMachineEventArgs> StateMachineEntered;

	public event EventHandler<StateMachineEventArgs> StateMachineExited;

	public event EventHandler<TransitionEventArgs> TransitionStarted;

	public AnimatorStateMachine(Animator _animator)
	{
		m_Animator = _animator;
		if (m_Animator.layerCount == 0)
		{
			m_Animator.Rebind();
		}
		m_LastCurrentStateInfo = new AnimatorStateInfo[m_Animator.layerCount];
		m_LastTransitionInfo = new AnimatorTransitionInfo[m_Animator.layerCount];
		if (UbiCore.Utils.Singleton<AnimationMain>.IsAvailable)
		{
			m_AnimatorStateMachineStatic = UbiCore.Utils.Singleton<AnimationMain>.Instance.GetAnimatorStateMachineStatic(m_Animator);
		}
	}

	public void Update()
	{
		Profiling.Start("AnimatorStateMachine::Update", float.MaxValue);
		for (int i = 0; i < m_Animator.layerCount; i++)
		{
			AnimatorStateInfo currentAnimatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(i);
			if (m_LastCurrentStateInfo[i].fullPathHash != currentAnimatorStateInfo.fullPathHash || m_LastCurrentStateInfo[i].normalizedTime >= currentAnimatorStateInfo.normalizedTime || Mathf.Floor(currentAnimatorStateInfo.normalizedTime) > Mathf.Floor(m_LastCurrentStateInfo[i].normalizedTime))
			{
				if (currentAnimatorStateInfo.fullPathHash != 0)
				{
					OnStateChange(i, m_LastCurrentStateInfo[i], currentAnimatorStateInfo);
				}
				m_LastCurrentStateInfo[i] = currentAnimatorStateInfo;
			}
			if (m_Animator.IsInTransition(i))
			{
				AnimatorTransitionInfo animatorTransitionInfo = m_Animator.GetAnimatorTransitionInfo(i);
				if (m_LastTransitionInfo[i].fullPathHash != animatorTransitionInfo.fullPathHash)
				{
					OnTransitionChange(i, m_LastTransitionInfo[i], animatorTransitionInfo);
					m_LastTransitionInfo[i] = animatorTransitionInfo;
				}
			}
			else
			{
				m_LastTransitionInfo[i] = s_EmptyTransitionInfo;
			}
		}
		Profiling.Stop();
	}

	public AnimatorStateMachineStatic.StateMachine GetCurrentStateMachine(AnimationTaskManager.ELayer _layer = AnimationTaskManager.ELayer.Base)
	{
		int currentStateMachineHash = GetCurrentStateMachineHash(_layer);
		if (currentStateMachineHash != 0)
		{
			return m_AnimatorStateMachineStatic.StateMachines[currentStateMachineHash];
		}
		return null;
	}

	public string GetStateNameByNameHash(int _nameHash)
	{
		return m_AnimatorStateMachineStatic.GetStateNameByNameHash(_nameHash);
	}

	public string GetTransitionNameByNameHash(int _nameHash)
	{
		return m_AnimatorStateMachineStatic.GetTransitionNameByNameHash(_nameHash);
	}

	public AnimatorStateMachineStatic.Transition GetTransition(int _nameHash)
	{
		return m_AnimatorStateMachineStatic.GetTransition(_nameHash);
	}

	public int GetCurrentState(AnimationTaskManager.ELayer _layer = AnimationTaskManager.ELayer.Base)
	{
		return m_LastCurrentStateInfo[(int)_layer].fullPathHash;
	}

	public int GetCurrentStateMachineHash(AnimationTaskManager.ELayer _layer = AnimationTaskManager.ELayer.Base)
	{
		AnimatorStateMachineStatic.State value;
		if (m_AnimatorStateMachineStatic.States.TryGetValue(m_LastCurrentStateInfo[(int)_layer].fullPathHash, out value))
		{
			return value.ParentStateMachine.NameHash;
		}
		return 0;
	}

	public void Reset()
	{
		BBDictionary<int, AnimatorStateMachineStatic.Parameter> defaultParameters = m_AnimatorStateMachineStatic.DefaultParameters;
		Dictionary<int, AnimatorStateMachineStatic.Parameter>.Enumerator enumerator = defaultParameters.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<int, AnimatorStateMachineStatic.Parameter> current = enumerator.Current;
			if (!m_Animator.IsParameterControlledByCurve(current.Value.NameHash))
			{
				switch (current.Value.Type)
				{
				case AnimatorStateMachineStatic.Parameter.EType.Bool:
					m_Animator.SetBool(current.Value.NameHash, current.Value.ValueBool);
					break;
				case AnimatorStateMachineStatic.Parameter.EType.Float:
					m_Animator.SetFloat(current.Value.NameHash, current.Value.ValueFloat);
					break;
				case AnimatorStateMachineStatic.Parameter.EType.Integer:
					m_Animator.SetInteger(current.Value.NameHash, current.Value.ValueInteger);
					break;
				case AnimatorStateMachineStatic.Parameter.EType.Trigger:
					m_Animator.ResetTrigger(current.Value.NameHash);
					break;
				}
			}
		}
		AnimatorStateMachineStatic.StateMachine stateMachine = m_AnimatorStateMachineStatic.StateMachines[AnimationStateMachineHashes.StateMachine_Base.NameHash];
		if (stateMachine != null)
		{
			m_Animator.Play(stateMachine.DefaultStateHash, 0);
		}
		stateMachine = m_AnimatorStateMachineStatic.StateMachines[AnimationStateMachineHashes.StateMachine_UpperBodyLayer.NameHash];
		if (stateMachine != null)
		{
			m_Animator.Play(stateMachine.DefaultStateHash, 1);
		}
	}

	private void OnStateChange(int _layer, AnimatorStateInfo _previous, AnimatorStateInfo _current)
	{
		if (m_AnimatorStateMachineStatic != null)
		{
			AnimatorStateMachineStatic.State state = null;
			AnimatorStateMachineStatic.State state2 = m_AnimatorStateMachineStatic.States[_current.fullPathHash];
			if (_previous.fullPathHash != 0)
			{
				state = m_AnimatorStateMachineStatic.States[_previous.fullPathHash];
			}
			if (this.StateChanged != null)
			{
				m_StateChangeEventArgs.PreviousState = state;
				m_StateChangeEventArgs.CurrentState = state2;
				this.StateChanged(this, m_StateChangeEventArgs);
			}
			if (this.StateExited != null && state != null)
			{
				m_StateEventArgs.State = state;
				m_StateEventArgs.Layer = _layer;
				this.StateExited(this, m_StateEventArgs);
			}
			if (this.StateEntered != null)
			{
				m_StateEventArgs.State = state2;
				m_StateEventArgs.Layer = _layer;
				m_StateEventArgs.LoopCount = Mathf.FloorToInt(_current.normalizedTime);
				this.StateEntered(this, m_StateEventArgs);
			}
			if (state == null || state.ParentStateMachine != state2.ParentStateMachine)
			{
				OnStateMachineChange(_layer, (state == null) ? null : state, state2);
			}
		}
	}

	private void OnStateMachineChange(int _layer, AnimatorStateMachineStatic.State _previousState, AnimatorStateMachineStatic.State _currentState)
	{
		List<AnimatorStateMachineStatic.StateMachine> sortedStateMachinePath = GetSortedStateMachinePath(_previousState, _currentState);
		for (int i = 0; i < sortedStateMachinePath.Count - 1; i++)
		{
			if (this.StateMachineChanged != null)
			{
				m_StateMachineChangeEventArgs.PreviousStateMachine = sortedStateMachinePath[i];
				m_StateMachineChangeEventArgs.CurrentStateMachine = sortedStateMachinePath[i + 1];
				this.StateMachineChanged(this, m_StateMachineChangeEventArgs);
			}
			if (sortedStateMachinePath[i + 1].HasChild(sortedStateMachinePath[i]) && this.StateMachineExited != null)
			{
				m_StateMachineEventArgs.StateMachine = sortedStateMachinePath[i];
				this.StateMachineExited(this, m_StateMachineEventArgs);
			}
			if (sortedStateMachinePath[i].HasChild(sortedStateMachinePath[i + 1]) && this.StateMachineEntered != null)
			{
				m_StateMachineEventArgs.StateMachine = sortedStateMachinePath[i + 1];
				this.StateMachineEntered(this, m_StateMachineEventArgs);
			}
		}
	}

	private void OnTransitionChange(int _layer, AnimatorTransitionInfo _previous, AnimatorTransitionInfo _current)
	{
		if (this.TransitionStarted != null && _current.fullPathHash != 0)
		{
			AnimatorStateMachineStatic.Transition value;
			m_AnimatorStateMachineStatic.Transitions.TryGetValue(_current.fullPathHash, out value);
			if (value != null)
			{
				m_TransitionEventArgs.Transition = value;
				this.TransitionStarted(this, m_TransitionEventArgs);
			}
		}
	}

	private List<AnimatorStateMachineStatic.StateMachine> GetSortedStateMachinePath(AnimatorStateMachineStatic.State _previousState, AnimatorStateMachineStatic.State _currentState)
	{
		s_route.Start = _previousState;
		s_route.End = _currentState;
		BBList<AnimatorStateMachineStatic.StateMachine> value = null;
		if (!m_AnimatorStateMachineStatic.StateMachineRoutes.TryGetValue(s_route, out value))
		{
			value = new BBList<AnimatorStateMachineStatic.StateMachine>();
			if (_previousState != null && _previousState.ParentStateMachine != _currentState.ParentStateMachine)
			{
				BBList<AnimatorStateMachineStatic.StateMachine> bBList = new BBList<AnimatorStateMachineStatic.StateMachine>();
				BBList<AnimatorStateMachineStatic.StateMachine> bBList2 = new BBList<AnimatorStateMachineStatic.StateMachine>();
				AnimatorStateMachineStatic.StateMachine parentStateMachine = _previousState.ParentStateMachine;
				AnimatorStateMachineStatic.StateMachine parentStateMachine2 = _currentState.ParentStateMachine;
				while (!bBList.Contains(parentStateMachine2) && !bBList2.Contains(parentStateMachine))
				{
					bBList.Add(parentStateMachine);
					if (parentStateMachine != parentStateMachine2)
					{
						bBList2.Add(parentStateMachine2);
						if (parentStateMachine.ParentStateMachine != null)
						{
							parentStateMachine = parentStateMachine.ParentStateMachine;
						}
						if (parentStateMachine2.ParentStateMachine != null)
						{
							parentStateMachine2 = parentStateMachine2.ParentStateMachine;
						}
						continue;
					}
					break;
				}
				value.AddRange(bBList);
				bBList2.Reverse();
				value.AddRange(bBList2);
			}
			m_AnimatorStateMachineStatic.StateMachineRoutes.Add(new AnimatorStateMachineStatic.StateMachineRoute(_previousState, _currentState), value);
		}
		return value;
	}
}
