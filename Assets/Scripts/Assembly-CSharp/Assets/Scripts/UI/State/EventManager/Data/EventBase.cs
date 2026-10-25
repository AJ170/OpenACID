using System;
using System.Text;
using AcierProtocol;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State.EventManager.Data
{
	public class EventBase
	{
		protected static readonly StringBuilder s_EventNameBuilder = new StringBuilder(512);

		private uint m_crc32;

		public uint CRC32
		{
			get
			{
				if (m_crc32 == 0)
				{
					m_crc32 = StringCRC32.Compute(ToString());
				}
				return m_crc32;
			}
		}

		public Event Event { get; protected set; }

		public bool LastValidState { get; private set; }

		public EventBase(Event _event)
		{
			Event = _event;
			LastValidState = false;
			ProcessEvent(_event);
		}

		public bool Validate()
		{
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			DateTime dateTime = ServerTimeManager.ToUtcTime(Event.startTimeUTC);
			DateTime dateTime2 = ServerTimeManager.ToUtcTime(Event.endTimeUTC);
			LastValidState = utcNow >= dateTime && utcNow < dateTime2;
			return LastValidState;
		}

		protected void ProcessEvent(Event _event)
		{
			Validate();
			OnProcessEvent(_event);
		}

		protected virtual void OnProcessEvent(Event _event)
		{
		}

		public override string ToString()
		{
			s_EventNameBuilder.Length = 0;
			s_EventNameBuilder.Append(GetType().Name);
			s_EventNameBuilder.Append("_");
			s_EventNameBuilder.Append(Event.name);
			s_EventNameBuilder.Append("_");
			s_EventNameBuilder.Append(ServerTimeManager.ToUtcTime(Event.startTimeUTC));
			s_EventNameBuilder.Append("_");
			s_EventNameBuilder.Append(ServerTimeManager.ToUtcTime(Event.endTimeUTC));
			s_EventNameBuilder.Append("_");
			s_EventNameBuilder.Append(Event.type);
			for (int i = 0; i < Event.properties.Length; i++)
			{
				Event.Property property = Event.properties[i];
				s_EventNameBuilder.Append("_p(");
				s_EventNameBuilder.Append(property.name);
				s_EventNameBuilder.Append("):");
				s_EventNameBuilder.Append(property.value);
				s_EventNameBuilder.Append(" ");
			}
			return s_EventNameBuilder.ToString();
		}
	}
}
