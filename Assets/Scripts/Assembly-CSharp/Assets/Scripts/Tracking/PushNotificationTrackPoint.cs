using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class PushNotificationTrackPoint : TimedTrackPoint
	{
		public bool IsServerTriggered { get; private set; }

		public PushNotificationTrackPoint()
		{
		}

		public PushNotificationTrackPoint(bool _isServerTriggered)
		{
			IsServerTriggered = _isServerTriggered;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			IsServerTriggered = _ser.SerializeBool("IsServerTriggered", IsServerTriggered);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.PushNotifications);
			_reporter.Add(_id, "server_triggered", IsServerTriggered);
		}
	}
}
