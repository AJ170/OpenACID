using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ChallengeObjective")]
	public class ChallengeObjective : IExtensible
	{
		private RankSetting[] _supportedRanks = new RankSetting[0];

		private int[] _blacklistedModifier = new int[0];

		private LootFilter _lootFilter;

		private bool _forced;

		private EChallengeType _challengeType;

		private ObjectiveBase _objectiveBase;

		private IExtension extensionObject;

		[ProtoMember(40, Name = "supportedRanks", DataFormat = DataFormat.Default)]
		public RankSetting[] supportedRanks
		{
			get
			{
				return _supportedRanks;
			}
			set
			{
				Array.Resize(ref _supportedRanks, value.Length);
				value.CopyTo(_supportedRanks, 0);
			}
		}

		[ProtoMember(41, Name = "blacklistedModifier", DataFormat = DataFormat.TwosComplement)]
		public int[] blacklistedModifier
		{
			get
			{
				return _blacklistedModifier;
			}
			set
			{
				Array.Resize(ref _blacklistedModifier, value.Length);
				value.CopyTo(_blacklistedModifier, 0);
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "lootFilter", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootFilter lootFilter
		{
			get
			{
				return _lootFilter;
			}
			set
			{
				_lootFilter = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "forced", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool forced
		{
			get
			{
				return _forced;
			}
			set
			{
				_forced = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "challengeType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EChallengeType.Invalid_EChallengeType)]
		public EChallengeType challengeType
		{
			get
			{
				return _challengeType;
			}
			set
			{
				_challengeType = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "objectiveBase", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBase objectiveBase
		{
			get
			{
				return _objectiveBase;
			}
			set
			{
				_objectiveBase = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
