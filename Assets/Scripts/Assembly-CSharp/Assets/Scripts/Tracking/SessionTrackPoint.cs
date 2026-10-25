using System;
using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class SessionTrackPoint : TrackPoint
	{
		public DateTime StartTime { get; private set; }

		public DateTime EndTime { get; private set; }

		public SessionTrackPoint()
		{
		}

		public SessionTrackPoint(DateTime _start, DateTime _end)
		{
			StartTime = _start;
			EndTime = _end;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			StartTime = new DateTime(_ser.SerializeInt64("StartTime", StartTime.Ticks));
			EndTime = new DateTime(_ser.SerializeInt64("EndTime", EndTime.Ticks));
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "type", ETrackingEventType.Session);
			_reporter.Add(_id, "start", StartTime);
			_reporter.Add(_id, "end", EndTime);
		}
	}
}
