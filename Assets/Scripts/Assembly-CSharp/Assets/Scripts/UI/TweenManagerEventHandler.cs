using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.ScreenManager;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI
{
	public class TweenManagerEventHandler
	{
		private class TweenedComponentsEventCounter
		{
			public readonly BBList<UITweenedScreenComponent> List = new BBList<UITweenedScreenComponent>();

			public int CurrentOnFinishedCount { get; private set; }

			public int CurrentOnStartedCount { get; private set; }

			public void IncreaseStartedCount()
			{
				CurrentOnStartedCount++;
			}

			public void IncreaseFinishedCount()
			{
				CurrentOnFinishedCount++;
			}

			public bool ShouldFireStartedEvent()
			{
				return CurrentOnStartedCount <= 1;
			}

			public bool ShouldFireFinishedEvent()
			{
				return CurrentOnFinishedCount >= List.Count;
			}

			public void Reset()
			{
				int currentOnFinishedCount = (CurrentOnStartedCount = 0);
				CurrentOnFinishedCount = currentOnFinishedCount;
			}
		}

		public enum EventType
		{
			OnPlay = 0,
			OnFinished = 1
		}

		private readonly BBDictionary<int, TweenedComponentsEventCounter> m_tweenComponentsByTrigger = new BBDictionary<int, TweenedComponentsEventCounter>();

		private readonly BBDictionary<int, BBList<EventDelegate>> m_onFinishedEvents = new BBDictionary<int, BBList<EventDelegate>>();

		private readonly BBDictionary<int, BBList<EventDelegate>> m_onStartedEvents = new BBDictionary<int, BBList<EventDelegate>>();

		public TweenManagerEventHandler()
		{
			Array values = Enum.GetValues(typeof(UITweenedScreenComponent.ETriggerPhase));
			IEnumerator enumerator = values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Current != null)
				{
					m_onFinishedEvents.Add((int)enumerator.Current, new BBList<EventDelegate>());
					m_onStartedEvents.Add((int)enumerator.Current, new BBList<EventDelegate>());
				}
			}
		}

		public void AddEventListener(EventType _type, UITweenedScreenComponent.ETriggerPhase _phase, EventDelegate.Callback _callback)
		{
			switch (_type)
			{
			case EventType.OnPlay:
				EventDelegate.Add(m_onStartedEvents[(int)_phase], _callback);
				break;
			case EventType.OnFinished:
				EventDelegate.Add(m_onFinishedEvents[(int)_phase], _callback);
				break;
			}
		}

		public void RemoveEventListener(EventType _type, UITweenedScreenComponent.ETriggerPhase _phase, EventDelegate.Callback _callback)
		{
			switch (_type)
			{
			case EventType.OnPlay:
				EventDelegate.Remove(m_onStartedEvents[(int)_phase], _callback);
				break;
			case EventType.OnFinished:
				EventDelegate.Remove(m_onFinishedEvents[(int)_phase], _callback);
				break;
			}
		}

		public bool IsPlaying(UITweenedScreenComponent.ETriggerPhase _phase)
		{
			TweenedComponentsEventCounter tweenedComponentsEventCounter = m_tweenComponentsByTrigger[(int)_phase];
			return tweenedComponentsEventCounter.CurrentOnStartedCount > 0;
		}

		public void ClearAllEvents()
		{
			m_onFinishedEvents.Clear();
			m_onStartedEvents.Clear();
			foreach (KeyValuePair<int, TweenedComponentsEventCounter> item in m_tweenComponentsByTrigger)
			{
				for (int i = 0; i < item.Value.List.Count; i++)
				{
					UITweenedScreenComponent uITweenedScreenComponent = item.Value.List[i];
					uITweenedScreenComponent.onFinish.Clear();
					uITweenedScreenComponent.onPlay.Clear();
				}
			}
		}

		public void AddToTweenComponentsByTrigger(UITweenedScreenComponent _tweenComponent)
		{
			if (!m_tweenComponentsByTrigger.ContainsKey((int)_tweenComponent.Phase))
			{
				TweenedComponentsEventCounter value = new TweenedComponentsEventCounter();
				m_tweenComponentsByTrigger.Add((int)_tweenComponent.Phase, value);
			}
			TweenedComponentsEventCounter tweenedComponentsEventCounter = m_tweenComponentsByTrigger[(int)_tweenComponent.Phase];
			if (!tweenedComponentsEventCounter.List.Contains(_tweenComponent))
			{
				tweenedComponentsEventCounter.List.Add(_tweenComponent);
				EventDelegate.Add(_tweenComponent.onFinish, delegate
				{
					OnTweenFinished(_tweenComponent);
				});
				EventDelegate.Add(_tweenComponent.onPlay, delegate
				{
					OnTweenPlay(_tweenComponent);
				});
			}
		}

		private void OnTweenFinished(UITweenedScreenComponent _tweenComponent)
		{
			TweenedComponentsEventCounter tweenedComponentsEventCounter = m_tweenComponentsByTrigger[(int)_tweenComponent.Phase];
			tweenedComponentsEventCounter.IncreaseFinishedCount();
			if (tweenedComponentsEventCounter.ShouldFireFinishedEvent())
			{
				tweenedComponentsEventCounter.Reset();
				EventDelegate.Execute(m_onFinishedEvents[(int)_tweenComponent.Phase]);
			}
		}

		private void OnTweenPlay(UITweenedScreenComponent _tweenComponent)
		{
			TweenedComponentsEventCounter tweenedComponentsEventCounter = m_tweenComponentsByTrigger[(int)_tweenComponent.Phase];
			tweenedComponentsEventCounter.IncreaseStartedCount();
			if (tweenedComponentsEventCounter.ShouldFireStartedEvent())
			{
				EventDelegate.Execute(m_onStartedEvents[(int)_tweenComponent.Phase]);
			}
		}
	}
}
