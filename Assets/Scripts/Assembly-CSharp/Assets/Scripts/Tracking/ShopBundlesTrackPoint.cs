using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class ShopBundlesTrackPoint : TimedTrackPoint
	{
		public string Id { get; private set; }

		public string TriggeredFrom { get; private set; }

		public ShopBundlesTrackPoint()
		{
		}

		public ShopBundlesTrackPoint(string _triggeredFrom, string _id)
		{
			TriggeredFrom = _triggeredFrom;
			Id = _id;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			TriggeredFrom = _ser.SerializeString("TriggeredFrom", TriggeredFrom);
			Id = _ser.SerializeString("Id", Id);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.ShopBundles);
			_reporter.Add(_id, "triggered_from", TriggeredFrom);
			_reporter.Add(_id, "shop_bundle_id", Id);
		}
	}
}
