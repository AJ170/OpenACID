using System;
using System.Collections.Generic;
using AcierProtocol;

namespace Assets.Scripts.Tracking
{
	public abstract class TrackReport
	{
		public long[] TrackIds { get; private set; }

		public DateTime UTCTimeStamp { get; private set; }

		protected TrackReport(List<long> _ids)
		{
			TrackIds = _ids.ToArray();
			UTCTimeStamp = DateTime.UtcNow;
		}

		public bool HasId(long _id)
		{
			return TrackIds.Contains(_id);
		}
	}
}
