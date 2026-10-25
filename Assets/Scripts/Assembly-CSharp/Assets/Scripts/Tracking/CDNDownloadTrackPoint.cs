using System;
using AcierProtocol;
using Assets.Scripts.UI.State;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class CDNDownloadTrackPoint : TrackPoint
	{
		public DateTime StartTime { get; private set; }

		public DateTime EndTime { get; private set; }

		public DateTime InstallTime { get; private set; }

		public string Url { get; private set; }

		public long FileSize { get; private set; }

		public CDNDownloadTrackPoint()
		{
		}

		public CDNDownloadTrackPoint(string _url, DateTime _start, DateTime _end, long _fileSize, DateTime _install)
		{
			StartTime = _start;
			EndTime = _end;
			InstallTime = _install;
			Url = _url;
			FileSize = _fileSize;
		}

		public CDNDownloadTrackPoint(string _url, DateTime _start, DateTime _end, long _fileSize)
		{
			StartTime = _start;
			EndTime = _end;
			InstallTime = ServerTimeManager.UnixTimeReference;
			Url = _url;
			FileSize = _fileSize;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			StartTime = new DateTime(_ser.SerializeInt64("StartTime", StartTime.Ticks));
			EndTime = new DateTime(_ser.SerializeInt64("EndTime", EndTime.Ticks));
			InstallTime = new DateTime(_ser.SerializeInt64("InstallTime", InstallTime.Ticks));
			Url = _ser.SerializeString("Url", Url);
			FileSize = _ser.SerializeInt64("FileSize", FileSize);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "type", ETrackingEventType.AssetDownload);
			_reporter.Add(_id, "url", Url);
			_reporter.Add(_id, "size", FileSize);
			_reporter.Add(_id, "start", StartTime);
			_reporter.Add(_id, "end", EndTime);
			_reporter.Add(_id, "duration", EndTime - StartTime);
		}
	}
}
