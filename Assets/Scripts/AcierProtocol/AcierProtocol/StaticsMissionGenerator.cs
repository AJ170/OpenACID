using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "StaticsMissionGenerator")]
	public class StaticsMissionGenerator : IExtensible
	{
		private int _levelSliceWidth;

		private MissionConfigurations _missionConfigurations;

		private MissionCatalog _missionCatalog;

		private MissionAlternatives _missionAlternatives;

		private AssassinLevelToMissionBudget _assassinLevelToMissionBudget;

		private MissionBlackList _missionBlackList;

		private RandomNumberBlob _randomNumberBlob;

		private HeroicRankMapping _heroicRankMapping;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "levelSliceWidth", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int levelSliceWidth
		{
			get
			{
				return _levelSliceWidth;
			}
			set
			{
				_levelSliceWidth = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "missionConfigurations", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionConfigurations missionConfigurations
		{
			get
			{
				return _missionConfigurations;
			}
			set
			{
				_missionConfigurations = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "missionCatalog", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionCatalog missionCatalog
		{
			get
			{
				return _missionCatalog;
			}
			set
			{
				_missionCatalog = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "missionAlternatives", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAlternatives missionAlternatives
		{
			get
			{
				return _missionAlternatives;
			}
			set
			{
				_missionAlternatives = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "assassinLevelToMissionBudget", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinLevelToMissionBudget assassinLevelToMissionBudget
		{
			get
			{
				return _assassinLevelToMissionBudget;
			}
			set
			{
				_assassinLevelToMissionBudget = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "missionBlackList", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBlackList missionBlackList
		{
			get
			{
				return _missionBlackList;
			}
			set
			{
				_missionBlackList = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "randomNumberBlob", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public RandomNumberBlob randomNumberBlob
		{
			get
			{
				return _randomNumberBlob;
			}
			set
			{
				_randomNumberBlob = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "heroicRankMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public HeroicRankMapping heroicRankMapping
		{
			get
			{
				return _heroicRankMapping;
			}
			set
			{
				_heroicRankMapping = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
