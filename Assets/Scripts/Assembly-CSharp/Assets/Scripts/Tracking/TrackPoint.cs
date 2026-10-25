using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public abstract class TrackPoint
	{
		public abstract void Serialize(DataSerializerBase _ser);

		public abstract void Report(ITrackReporter _reporter, long _id);
	}
}
