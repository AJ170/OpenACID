using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class InviteFriendTrackPoint : TimedTrackPoint
	{
		public string MessageType { get; private set; }

		public InviteFriendTrackPoint()
		{
		}

		public InviteFriendTrackPoint(string _messageType)
		{
			MessageType = _messageType;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			MessageType = _ser.SerializeString("MessageType", MessageType);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.InviteFriend);
			_reporter.Add(_id, "message_type", MessageType);
		}
	}
}
