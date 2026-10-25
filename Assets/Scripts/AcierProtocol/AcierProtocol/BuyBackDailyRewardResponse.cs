using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "BuyBackDailyRewardResponse")]
	public class BuyBackDailyRewardResponse : IExtensible
	{
		private DailyRewardDay[] _days = new DailyRewardDay[0];

		private int _currentDay;

		private int _maxDay;

		private long _nextDayTimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "days", DataFormat = DataFormat.Default)]
		public DailyRewardDay[] days
		{
			get
			{
				return _days;
			}
			set
			{
				Array.Resize(ref _days, value.Length);
				value.CopyTo(_days, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "currentDay", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int currentDay
		{
			get
			{
				return _currentDay;
			}
			set
			{
				_currentDay = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "maxDay", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxDay
		{
			get
			{
				return _maxDay;
			}
			set
			{
				_maxDay = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "nextDayTimeStamp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long nextDayTimeStamp
		{
			get
			{
				return _nextDayTimeStamp;
			}
			set
			{
				_nextDayTimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
