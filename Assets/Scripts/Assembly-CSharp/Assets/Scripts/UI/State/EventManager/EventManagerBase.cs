using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager
{
	public abstract class EventManagerBase
	{
		private static readonly Type[] s_Params = new Type[1] { typeof(EventManagerBase) };

		public SimpleDelegate OnChanged;

		protected bool m_hasChanged;

		protected BBList<EventBase> m_Events = new BBList<EventBase>();

		protected BBList<EventBase> m_oldEvents = new BBList<EventBase>();

		public ReadOnlyCollection<EventBase> Events
		{
			get
			{
				return m_Events.AsReadOnly();
			}
		}

		public abstract EEventType ManagerType { get; }

		protected EventManagerBase()
		{
			OnChanged = new SimpleDelegate(this, s_Params);
		}

		public abstract void ProcessEvent(Event _event);

		public virtual void StartProcessEvents()
		{
			m_hasChanged = false;
			BBList<EventBase> oldEvents = m_oldEvents;
			m_oldEvents = m_Events;
			m_Events = oldEvents;
			oldEvents.ClearIfNotEmpty();
		}

		public virtual void EndProcessEvents()
		{
			if (m_oldEvents != null)
			{
				m_hasChanged = m_hasChanged || m_oldEvents.Count != m_Events.Count;
				m_oldEvents.ClearIfNotEmpty();
			}
			if (m_hasChanged && OnChanged != null)
			{
				OnChanged.Trigger(this);
			}
			m_hasChanged = false;
		}

		public abstract void Update();

		public bool HasValidEvent()
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				if (m_Events[i].LastValidState)
				{
					return true;
				}
			}
			return false;
		}

		protected bool HasInOldEvents(EventBase _event)
		{
			bool result = false;
			for (int i = 0; i < m_oldEvents.Count; i++)
			{
				EventBase eventBase = m_oldEvents[i];
				if (eventBase.CRC32 == _event.CRC32)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
