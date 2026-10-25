using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class EventProgressionChallengeTrackPoint : TimedTrackPoint
	{
		public string TriggeredFrom { get; private set; }

		public EventProgressionChallengeTrackPoint()
		{
		}

		public EventProgressionChallengeTrackPoint(string _triggeredFrom)
		{
			TriggeredFrom = _triggeredFrom;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			TriggeredFrom = _ser.SerializeString("TriggeredFrom", TriggeredFrom);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.EventProgression);
			_reporter.Add(_id, "triggered_from", TriggeredFrom);
		}
	}
}
