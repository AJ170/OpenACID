using System.Collections.Generic;
using System.Text;
using AcierProtocol;

namespace Assets.Scripts.Tracking
{
	public class ProtobufReport : TrackReport
	{
		public TrackingEvent[] Events { get; private set; }

		public ProtobufReport(List<long> _ids, List<TrackingEvent> _events)
			: base(_ids)
		{
			Events = _events.ToArray();
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProtobufReport: \n");
			for (int i = 0; i < Events.Length; i++)
			{
				long num = ((i >= base.TrackIds.Length) ? (-1) : base.TrackIds[i]);
				TrackingEvent trackingEvent = Events[i];
				stringBuilder.AppendFormat("TrackId: {0} \nEvent Type: {1} \n", num, trackingEvent.eventType);
				for (int j = 0; j < trackingEvent.trackingParams.Length; j++)
				{
					TrackingEvent.TrackingParam trackingParam = trackingEvent.trackingParams[j];
					stringBuilder.AppendFormat("Param Type: {0} Value: {1}\n", trackingParam.key, trackingParam.value);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
