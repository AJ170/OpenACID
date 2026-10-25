using AcierProtocol;
using Assets.Scripts.Native;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class GamePadTrackPoint : TimedTrackPoint
	{
		public string UsedIn { get; private set; }

		public string DeviceID { get; private set; }

		public GamePadTrackPoint()
		{
		}

		public GamePadTrackPoint(string _usedIn)
		{
			UsedIn = _usedIn;
			DeviceID = NativeTools.DeviceUniqueId;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			UsedIn = _ser.SerializeString("UsedIn", UsedIn);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.Gamepad);
			_reporter.Add(_id, "used_in", UsedIn);
		}
	}
}
