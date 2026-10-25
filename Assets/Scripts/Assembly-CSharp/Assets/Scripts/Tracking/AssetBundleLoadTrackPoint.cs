using System;
using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class AssetBundleLoadTrackPoint : TrackPoint
	{
		public DateTime StarTime { get; private set; }

		public DateTime EndTime { get; private set; }

		public string Name { get; private set; }

		public bool Success { get; private set; }

		public AssetBundleLoadTrackPoint()
		{
		}

		public AssetBundleLoadTrackPoint(DateTime _start, DateTime _end, string _name, bool _success)
		{
			StarTime = _start;
			EndTime = _end;
			Name = _name;
			Success = _success;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			StarTime = new DateTime(_ser.SerializeInt64("StarTime", StarTime.Ticks));
			EndTime = new DateTime(_ser.SerializeInt64("EndTime", EndTime.Ticks));
			Name = _ser.SerializeString("Name", Name);
			Success = _ser.SerializeBool("Success", Success);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "type", ETrackingEventType.AssetDownload);
			_reporter.Add(_id, "start", StarTime);
			_reporter.Add(_id, "end", EndTime);
			_reporter.Add(_id, "name", Name);
			_reporter.Add(_id, "success", Success);
		}
	}
}
