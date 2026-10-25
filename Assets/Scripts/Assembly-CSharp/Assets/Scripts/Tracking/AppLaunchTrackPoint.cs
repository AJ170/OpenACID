using AcierProtocol;

namespace Assets.Scripts.Tracking
{
	public class AppLaunchTrackPoint : TimedTrackPoint
	{
		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.AppLaunch);
		}
	}
}
