using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HirelingUsageStatisticsAttachment")]
	public class HirelingUsageStatisticsAttachment : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "StatisticsEntry")]
		public class StatisticsEntry : IExtensible
		{
			private string _assassinId = "";

			private int _timesUsedByNonFriends;

			private Resource _rewardForTimesUsedByNonFriends;

			private int _timesUsedByFriends;

			private Resource _rewardForTimesUsedByFriends;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string assassinId
			{
				get
				{
					return _assassinId;
				}
				set
				{
					_assassinId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "timesUsedByNonFriends", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int timesUsedByNonFriends
			{
				get
				{
					return _timesUsedByNonFriends;
				}
				set
				{
					_timesUsedByNonFriends = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "rewardForTimesUsedByNonFriends", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource rewardForTimesUsedByNonFriends
			{
				get
				{
					return _rewardForTimesUsedByNonFriends;
				}
				set
				{
					_rewardForTimesUsedByNonFriends = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "timesUsedByFriends", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int timesUsedByFriends
			{
				get
				{
					return _timesUsedByFriends;
				}
				set
				{
					_timesUsedByFriends = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "rewardForTimesUsedByFriends", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource rewardForTimesUsedByFriends
			{
				get
				{
					return _rewardForTimesUsedByFriends;
				}
				set
				{
					_rewardForTimesUsedByFriends = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private StatisticsEntry[] _hirelingUsages = new StatisticsEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "hirelingUsages", DataFormat = DataFormat.Default)]
		public StatisticsEntry[] hirelingUsages
		{
			get
			{
				return _hirelingUsages;
			}
			set
			{
				Array.Resize(ref _hirelingUsages, value.Length);
				value.CopyTo(_hirelingUsages, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
