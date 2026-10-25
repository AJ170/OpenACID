using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class CallToActionTrackPoint : TimedTrackPoint
	{
		public string Name { get; private set; }

		public bool Accept { get; private set; }

		public bool Closed { get; private set; }

		public CallToActionTrackPoint()
		{
		}

		public CallToActionTrackPoint(string _name, bool _accept, bool _closed)
		{
			Name = _name;
			Accept = _accept;
			Closed = _closed;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			base.Serialize(_ser);
			Name = _ser.SerializeString("Name", Name);
			Accept = _ser.SerializeBool("Accept", Accept);
			Closed = _ser.SerializeBool("Closed", Closed);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			base.Report(_reporter, _id);
			_reporter.Add(_id, "type", ETrackingEventType.CtaPopupResult);
			_reporter.Add(_id, "name", Name);
			_reporter.Add(_id, "accept", Accept);
			_reporter.Add(_id, "closed", Closed);
		}
	}
}
