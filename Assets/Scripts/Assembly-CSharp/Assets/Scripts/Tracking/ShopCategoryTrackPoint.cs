using System;
using AcierProtocol;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Tracking
{
	public class ShopCategoryTrackPoint : TrackPoint
	{
		public EShopCategory Category { get; private set; }

		public DateTime OpenTime { get; private set; }

		public DateTime CloseTime { get; private set; }

		public bool Closed { get; private set; }

		public ShopCategoryTrackPoint()
		{
		}

		public ShopCategoryTrackPoint(EShopCategory _category)
		{
			Category = _category;
			OpenTime = DateTime.UtcNow;
		}

		public override void Serialize(DataSerializerBase _ser)
		{
			Category = (EShopCategory)_ser.SerializeEnum("Category", typeof(EShopCategory), (int)Category);
		}

		public override void Report(ITrackReporter _reporter, long _id)
		{
			_reporter.Add(_id, "type", ETrackingEventType.ShopScreen);
			_reporter.Add(_id, "open", OpenTime);
			_reporter.Add(_id, "name", "ShopScreen");
			_reporter.Add(_id, "category", Category);
			_reporter.Add(_id, "close", (!Closed) ? default(DateTime) : CloseTime);
			_reporter.Add(_id, "duration", (!Closed) ? new TimeSpan(0L) : (CloseTime - OpenTime));
		}

		public void CloseCategory()
		{
			CloseTime = DateTime.UtcNow;
			Closed = true;
		}
	}
}
