using System;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public abstract class TimedTrackPoint : TrackPoint
	{
		public DateTime UTCTimeStamp { get; private set; }

		protected TimedTrackPoint()
		{
			UTCTimeStamp = DateTime.UtcNow;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			UTCTimeStamp = new DateTime(_ser.SerializeInt64("UTCTimeStamp", UTCTimeStamp.Ticks));
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "time", UTCTimeStamp);
		}
	}
}
