using System;
using System.Globalization;
using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Tracking
{
	public class ProtoBufReporter : ITrackReporter
	{
		private sealed class Event
		{
			public ETrackingEventType Type;

			public BBList<TrackingEvent.TrackingParam> Params = new BBList<TrackingEvent.TrackingParam>(8);
		}

		private readonly BBDictionary<long, Event> m_events = new BBDictionary<long, Event>(128);

		public void Add(long _id, string _name, int _int)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _int.ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, bool _bool)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _bool.ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, string _string)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _string));
		}

		public void Add(long _id, string _name, DateTime _time)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _time.ToJavaTimeStamp().ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, TimeSpan _span)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, ((long)_span.TotalMilliseconds).ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, float _float)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _float.ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, double _double)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _double.ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, long _long)
		{
			Event e = CreateOrGetEvent(_id);
			AddParamater(e, GenerateParam(_name, _long.ToString(CultureInfo.InvariantCulture)));
		}

		public void Add(long _id, string _name, Enum _enum)
		{
			Event @event = CreateOrGetEvent(_id);
			if (_enum is ETrackingEventType)
			{
				@event.Type = (ETrackingEventType)(object)_enum;
			}
			else
			{
				AddParamater(@event, GenerateParam(_name, _enum.ToString(CultureInfo.InvariantCulture)));
			}
		}

		public TrackReport MakeReport()
		{
			return new ProtobufReport(m_events.Keys.ToList(), m_events.Values.Select((Event _x) => new TrackingEvent
			{
				eventType = _x.Type,
				trackingParams = _x.Params.ToArray()
			}));
		}

		public void Clear()
		{
			m_events.Clear();
		}

		private Event CreateOrGetEvent(long _id)
		{
			if (!m_events.ContainsKey(_id))
			{
				m_events[_id] = new Event();
			}
			return m_events[_id];
		}

		private TrackingEvent.TrackingParam GenerateParam(string _key, string _value)
		{
			TrackingEvent.TrackingParam trackingParam = new TrackingEvent.TrackingParam();
			trackingParam.key = _key;
			trackingParam.value = _value;
			return trackingParam;
		}

		private static void AddParamater(Event _e, TrackingEvent.TrackingParam _param)
		{
			_e.Params.Add(_param);
		}
	}
}
