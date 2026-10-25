using System;
using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class ScreenTrackPoint : TrackPoint
	{
		public string Name { get; private set; }

		public DateTime OpenTime { get; private set; }

		public DateTime CloseTime { get; private set; }

		public bool Closed { get; private set; }

		public TimeSpan Duration
		{
			get
			{
				return (!Closed) ? TimeSpan.Zero : (CloseTime - OpenTime);
			}
		}

		public ScreenTrackPoint()
		{
		}

		public ScreenTrackPoint(string _name)
		{
			Name = _name;
			OpenTime = DateTime.UtcNow;
		}

		public void CloseScreen()
		{
			CloseTime = DateTime.UtcNow;
			Closed = true;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			Name = _ser.SerializeString("Name", Name);
			OpenTime = new DateTime(_ser.SerializeInt64("OpenTime", OpenTime.Ticks));
			CloseTime = new DateTime(_ser.SerializeInt64("CloseTime", CloseTime.Ticks));
			Closed = _ser.SerializeBool("Closed", Closed);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "type", ETrackingEventType.Screen);
			_reporter.Add(_id, "name", Name);
			_reporter.Add(_id, "open", OpenTime);
			_reporter.Add(_id, "close", (!Closed) ? default(DateTime) : CloseTime);
			_reporter.Add(_id, "duration", (!Closed) ? new TimeSpan(0L) : (CloseTime - OpenTime));
		}
	}
}
