using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using UbiCore.Debug;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorDataController : MonoBehaviour
{
	private struct StateData
	{
		private int m_layer;

		private int m_nameHash;

		private float m_curTime;

		private int m_curLoop;

		private int m_lastLoop;

		private float m_length;

		private readonly List<BlendedEvent> m_currentEventIndex;

		private readonly List<BlendedEvent> m_currentEvents;

		private readonly List<BlendedEvent> m_collectedEvents;

		public static readonly StateData Invalid = new StateData
		{
			m_nameHash = 0
		};

		public int NameHash
		{
			get
			{
				return m_nameHash;
			}
		}

		public int Layer
		{
			get
			{
				return m_layer;
			}
		}

		private StateData(int _layer, AnimatorStateInfo _state, AnimatorClipInfo[] _animations, AnimatorDataController _controller)
		{
			m_layer = _layer;
			m_curTime = -1f;
			m_curLoop = 0;
			m_currentEventIndex = new List<BlendedEvent>();
			m_currentEvents = new List<BlendedEvent>();
			m_collectedEvents = new List<BlendedEvent>();
			m_nameHash = _state.fullPathHash;
			m_lastLoop = m_curLoop;
			m_length = _state.length;
			m_curLoop = (int)_state.normalizedTime;
			m_curTime = _state.normalizedTime - (float)m_curLoop;
		}

		public static StateData Construct(int _layer, AnimatorStateInfo _state, AnimatorClipInfo[] _animations, AnimatorDataController _controller)
		{
			StateData result = new StateData(_layer, _state, _animations, _controller);
			result.UpdateData(_state, _animations, _controller);
			return result;
		}

		public override bool Equals(object _obj)
		{
			return _obj is StateData && this == (StateData)_obj;
		}

		public override int GetHashCode()
		{
			return m_nameHash;
		}

		private BlendedEvent FindEvent(int _id)
		{
			for (int i = 0; i < m_currentEventIndex.Count; i++)
			{
				BlendedEvent blendedEvent = m_currentEventIndex[i];
				if (blendedEvent.m_blendID == _id)
				{
					return blendedEvent;
				}
			}
			return null;
		}

		public void UpdateData(AnimatorStateInfo _state, AnimatorClipInfo[] _animations, AnimatorDataController _controller)
		{
			m_nameHash = _state.fullPathHash;
			m_lastLoop = m_curLoop;
			m_length = _state.length;
			m_curLoop = (int)_state.normalizedTime;
			m_curTime = _state.normalizedTime - (float)m_curLoop;
			m_currentEventIndex.ClearIfNotEmpty();
			AnimatorDataIndex dataIndex = _controller.m_dataIndex;
			for (int i = 0; i < _animations.Length; i++)
			{
				AnimatorData.MotionData motionData = dataIndex.GetMotionData(_animations[i].clip);
				if (motionData == null)
				{
					continue;
				}
				if (motionData.m_rootMotionRotation != 0f && motionData.m_rootMotionLength > 0f)
				{
					float angle = motionData.m_rootMotionRotation * LogicMain.FrameTime / motionData.m_rootMotionLength;
					_controller.transform.Rotate(Vector3.up, angle);
				}
				for (int j = 0; j < motionData.m_events.Count; j++)
				{
					if (!(_animations[i].weight < 0.01f))
					{
						BlendedEvent blendedEvent = FindEvent(motionData.m_events[j].m_blendID);
						if (blendedEvent == null)
						{
							blendedEvent = new BlendedEvent(motionData.m_events[j]);
							m_currentEventIndex.Add(blendedEvent);
						}
						blendedEvent.m_time += motionData.m_events[j].m_time * _animations[i].weight;
						blendedEvent.m_weight += _animations[i].weight;
						if (blendedEvent.m_bestWeight < _animations[i].weight)
						{
							blendedEvent.m_bestWeight = _animations[i].weight;
							blendedEvent.m_type = motionData.m_events[j].m_type;
						}
					}
				}
			}
			for (int k = 0; k < m_currentEventIndex.Count; k++)
			{
				m_currentEventIndex[k].m_time /= m_currentEventIndex[k].m_weight;
			}
			ProcessEvents(_controller);
		}

		public void UpdateAsPrevious(AnimatorDataController _controller)
		{
			m_lastLoop = m_curLoop;
			if (m_length > 0.01f)
			{
				m_curTime += LogicMain.FrameTime / m_length;
				int num = (int)m_curTime;
				m_curTime -= num;
				m_curLoop += num;
			}
			ProcessEvents(_controller);
		}

		private void ProcessEvents(AnimatorDataController _controller)
		{
			UpdateCurrentEvents();
			if (m_lastLoop < m_curLoop)
			{
				for (int i = 0; i < m_currentEvents.Count; i++)
				{
					_controller.FireEvent(this, m_currentEvents[i], m_curLoop);
				}
				ResetCollectedEvents();
				UpdateCurrentEvents();
				if (m_lastLoop + 1 < m_curLoop)
				{
					for (int j = m_lastLoop + 1; j < m_curLoop; j++)
					{
						for (int k = 0; k < m_currentEvents.Count; k++)
						{
							_controller.FireEvent(this, m_currentEvents[k], m_curLoop);
						}
					}
					ResetCollectedEvents();
				}
			}
			for (int l = 0; l < m_currentEvents.Count; l++)
			{
				if (m_currentEvents[l].m_time <= m_curTime)
				{
					_controller.FireEvent(this, m_currentEvents[l], m_curLoop);
				}
			}
		}

		private void UpdateCurrentEvents()
		{
			m_currentEvents.ClearIfNotEmpty();
			for (int i = 0; i < m_currentEventIndex.Count; i++)
			{
				BlendedEvent blendedEvent = m_currentEventIndex[i];
				BlendedEvent blendedEvent2 = null;
				for (int j = 0; j < m_collectedEvents.Count; j++)
				{
					if (m_collectedEvents[j].m_blendID == blendedEvent.m_blendID)
					{
						blendedEvent2 = m_collectedEvents[j];
						blendedEvent2.m_type = blendedEvent.m_type;
						break;
					}
				}
				if (blendedEvent2 == null)
				{
					blendedEvent2 = new BlendedEvent(blendedEvent);
					m_collectedEvents.Add(blendedEvent2);
				}
				else
				{
					if (blendedEvent2.m_fired)
					{
						continue;
					}
					blendedEvent2.m_time = Mathf.Min(blendedEvent2.m_time, blendedEvent.m_time);
				}
				m_currentEvents.Add(blendedEvent2);
			}
			if (m_currentEvents.Count > 1)
			{
				m_currentEvents.Sort();
			}
		}

		private void ResetCollectedEvents()
		{
			for (int i = 0; i < m_collectedEvents.Count; i++)
			{
				BlendedEvent blendedEvent = m_collectedEvents[i];
				blendedEvent.m_fired = false;
				blendedEvent.m_time = float.PositiveInfinity;
			}
		}

		public static bool operator ==(StateData a, StateData b)
		{
			return a.m_nameHash == b.m_nameHash;
		}

		public static bool operator !=(StateData a, StateData b)
		{
			return a.m_nameHash != b.m_nameHash;
		}
	}

	private class BlendedEvent : IComparable<BlendedEvent>
	{
		public float m_time;

		public float m_weight;

		public AnimatorEventType m_type;

		public int m_blendID;

		public bool m_fired;

		public bool m_OneTimeEvent;

		public UnityEngine.Object m_payload;

		public float m_bestWeight;

		public BlendedEvent(AnimatorData.Event _e)
		{
			m_type = _e.m_type;
			m_blendID = _e.m_blendID;
			m_OneTimeEvent = _e.m_OneTimeEvent;
			m_payload = _e.m_payload;
		}

		public BlendedEvent(BlendedEvent _e)
		{
			m_time = _e.m_time;
			m_weight = _e.m_weight;
			m_type = _e.m_type;
			m_blendID = _e.m_blendID;
			m_fired = _e.m_fired;
			m_OneTimeEvent = _e.m_OneTimeEvent;
			m_payload = _e.m_payload;
			m_bestWeight = _e.m_bestWeight;
		}

		public int CompareTo(BlendedEvent other)
		{
			return m_time.CompareTo(other.m_time);
		}
	}

	[SerializeField]
	private AnimatorDataIndex m_dataIndex;

	private Animator m_animator;

	private StateData[] m_currentState;

	private StateData[] m_nextState;

	private AnimatorClipInfo[][] m_currentClipState;

	private AnimatorStateInfo[] m_currentAnimatorState;

	private AnimatorStateInfo[] m_nextAnimatorState;

	private bool[] m_forceClipStateRefresh;

	private List<IAnimatorEventListener> m_eventListeners = new List<IAnimatorEventListener>();

	private List<IAnimatorEventListener> m_eventListenersFire = new List<IAnimatorEventListener>();

	public static void Copy(AnimatorDataController _source, GameObject _dest)
	{
		AnimatorDataController animatorDataController = _dest.AddComponent<AnimatorDataController>();
		animatorDataController.m_dataIndex = _source.m_dataIndex;
	}

	private void Start()
	{
		m_animator = GetComponent<Animator>();
		if (m_animator == null)
		{
			Debug.LogWarning("AnimationEventManager needs an Animator component!");
			return;
		}
		m_currentState = new StateData[m_animator.layerCount];
		m_nextState = new StateData[m_animator.layerCount];
		m_currentClipState = new AnimatorClipInfo[m_animator.layerCount][];
		m_currentAnimatorState = new AnimatorStateInfo[m_animator.layerCount];
		m_nextAnimatorState = new AnimatorStateInfo[m_animator.layerCount];
		m_forceClipStateRefresh = new bool[m_animator.layerCount];
		m_dataIndex.LoadIfNotLoaded();
	}

	public AnimatorClipInfo[] GetCurrentAnimationClipState(int layer)
	{
		return m_currentClipState[layer];
	}

	public void UpdateStates()
	{
		if (m_animator == null)
		{
			return;
		}
		Profiling.Start("AnimatorDataController::UpdateStates", float.MaxValue);
		for (int i = 0; i < m_animator.layerCount; i++)
		{
			bool flag = m_animator.IsInTransition(i);
			AnimatorStateInfo currentAnimatorStateInfo = m_animator.GetCurrentAnimatorStateInfo(i);
			if (currentAnimatorStateInfo.fullPathHash != m_currentAnimatorState[i].fullPathHash || m_currentClipState[i].Length > 0 || m_forceClipStateRefresh[i])
			{
				m_currentClipState[i] = m_animator.GetCurrentAnimatorClipInfo(i);
				m_forceClipStateRefresh[i] = false;
			}
			m_currentAnimatorState[i] = currentAnimatorStateInfo;
			if (Match(m_currentState[i], m_currentAnimatorState[i]))
			{
				m_currentState[i].UpdateData(m_currentAnimatorState[i], m_currentClipState[i], this);
			}
			else
			{
				m_forceClipStateRefresh[i] = true;
				if (m_currentState[i] != StateData.Invalid)
				{
					m_currentState[i].UpdateAsPrevious(this);
				}
				if (Match(m_nextState[i], m_currentAnimatorState[i]))
				{
					m_currentState[i] = m_nextState[i];
					m_currentState[i].UpdateData(m_currentAnimatorState[i], m_currentClipState[i], this);
					m_nextState[i] = StateData.Invalid;
				}
				else
				{
					m_currentState[i] = StateData.Construct(i, m_currentAnimatorState[i], m_currentClipState[i], this);
				}
			}
			if (flag)
			{
				m_forceClipStateRefresh[i] = true;
				m_nextAnimatorState[i] = m_animator.GetNextAnimatorStateInfo(i);
				AnimatorClipInfo[] nextAnimatorClipInfo = m_animator.GetNextAnimatorClipInfo(i);
				if (Match(m_nextState[i], m_nextAnimatorState[i]))
				{
					m_nextState[i].UpdateData(m_nextAnimatorState[i], nextAnimatorClipInfo, this);
				}
				else
				{
					m_nextState[i] = StateData.Construct(i, m_nextAnimatorState[i], nextAnimatorClipInfo, this);
				}
			}
			else
			{
				m_nextState[i] = StateData.Invalid;
			}
		}
		Profiling.Stop();
	}

	public bool IsEventListenerRegistered(IAnimatorEventListener _listener)
	{
		return m_eventListeners.Contains(_listener);
	}

	public void RegisterEventListener(IAnimatorEventListener _listener)
	{
		m_eventListeners.Add(_listener);
	}

	public bool UnregisterEventListener(IAnimatorEventListener _listener)
	{
		return m_eventListeners.Remove(_listener);
	}

	public void Attach()
	{
	}

	public void Detach()
	{
		for (int i = 0; i < m_eventListeners.Count; i++)
		{
			IDetachableAnimatorEventListener detachableAnimatorEventListener = m_eventListeners[i] as IDetachableAnimatorEventListener;
			if (detachableAnimatorEventListener != null && detachableAnimatorEventListener.Detach())
			{
				m_eventListeners.RemoveAt(i);
				i--;
			}
		}
	}

	private void FireEvent(StateData _state, BlendedEvent _event, int _currLoop)
	{
		if (_event.m_OneTimeEvent && _currLoop > 0)
		{
			return;
		}
		_event.m_fired = true;
		if (Assets.Scripts.Offline.PortDebug.Enabled && (_event.m_type == AnimatorEventType.Hit || _event.m_type == AnimatorEventType.Combo || _event.m_type == AnimatorEventType.Parry || _event.m_type == AnimatorEventType.Dodge || _event.m_type == AnimatorEventType.ReactionStart || _event.m_type == AnimatorEventType.TurnEnd || _event.m_type == AnimatorEventType.TaskEnd))
		{
			Assets.Scripts.Offline.PortDebug.Log(string.Format("event {0} on {1} ({2} listeners)", _event.m_type, name, m_eventListeners.Count));
		}
		m_eventListenersFire.AddRange(m_eventListeners);
		AnimatorEvent @event = new AnimatorEvent(m_animator, _state.NameHash, _event.m_type, _event.m_blendID, _state.Layer, _event.m_payload);
		for (int i = 0; i < m_eventListenersFire.Count; i++)
		{
			IAnimatorEventListener animatorEventListener = m_eventListenersFire[i];
			if (animatorEventListener.OnAnimatorEvent(@event))
			{
				m_eventListeners.Remove(animatorEventListener);
			}
		}
		m_eventListenersFire.ClearIfNotEmpty();
	}

	public AnimatorData.Event GetEventByMotion(AnimationClip _motion, AnimatorEventType _eventType)
	{
		return m_dataIndex.GetEventByMotion(_motion, _eventType);
	}

	public List<AnimatorData.Event> GetEventsByMotion(AnimationClip _motion, AnimatorEventType _eventType)
	{
		return m_dataIndex.GetEventsByMotion(_motion, _eventType);
	}

	private static bool Match(StateData _state, AnimatorStateInfo _info)
	{
		return _state.NameHash == _info.fullPathHash;
	}
}
