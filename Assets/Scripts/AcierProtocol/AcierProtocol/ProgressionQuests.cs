using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProgressionQuests")]
	public class ProgressionQuests : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RankQuests")]
		public class RankQuests : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Quest")]
			public class Quest : IExtensible
			{
				private uint _progress;

				private uint _numberOfTimesToPerform;

				private uint _descriptionLocaKey;

				private EQuestStatus _status;

				private MissionGoalLoot _reward;

				private int _titleLocaKey;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "progress", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0L)]
				public uint progress
				{
					get
					{
						return _progress;
					}
					set
					{
						_progress = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "numberOfTimesToPerform", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0L)]
				public uint numberOfTimesToPerform
				{
					get
					{
						return _numberOfTimesToPerform;
					}
					set
					{
						_numberOfTimesToPerform = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "descriptionLocaKey", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0L)]
				public uint descriptionLocaKey
				{
					get
					{
						return _descriptionLocaKey;
					}
					set
					{
						_descriptionLocaKey = value;
					}
				}

				[ProtoMember(4, IsRequired = false, Name = "status", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(EQuestStatus.Created)]
				public EQuestStatus status
				{
					get
					{
						return _status;
					}
					set
					{
						_status = value;
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "reward", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public MissionGoalLoot reward
				{
					get
					{
						return _reward;
					}
					set
					{
						_reward = value;
					}
				}

				[ProtoMember(6, IsRequired = false, Name = "titleLocaKey", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int titleLocaKey
				{
					get
					{
						return _titleLocaKey;
					}
					set
					{
						_titleLocaKey = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _rankIdx;

			private Quest[] _questChain = new Quest[0];

			private uint _currentQuestIndex;

			private bool _questChainProgress;

			private ulong _expiresAtTimeStamp;

			private MissionGoalLoot _rewardCache;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int rankIdx
			{
				get
				{
					return _rankIdx;
				}
				set
				{
					_rankIdx = value;
				}
			}

			[ProtoMember(2, Name = "questChain", DataFormat = DataFormat.Default)]
			public Quest[] questChain
			{
				get
				{
					return _questChain;
				}
				set
				{
					Array.Resize(ref _questChain, value.Length);
					value.CopyTo(_questChain, 0);
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "currentQuestIndex", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint currentQuestIndex
			{
				get
				{
					return _currentQuestIndex;
				}
				set
				{
					_currentQuestIndex = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "questChainProgress", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool questChainProgress
			{
				get
				{
					return _questChainProgress;
				}
				set
				{
					_questChainProgress = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "expiresAtTimeStamp", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0f)]
			public ulong expiresAtTimeStamp
			{
				get
				{
					return _expiresAtTimeStamp;
				}
				set
				{
					_expiresAtTimeStamp = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "rewardCache", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public MissionGoalLoot rewardCache
			{
				get
				{
					return _rewardCache;
				}
				set
				{
					_rewardCache = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private RankQuests[] _rankQuests = new RankQuests[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "rankQuests", DataFormat = DataFormat.Default)]
		public RankQuests[] rankQuests
		{
			get
			{
				return _rankQuests;
			}
			set
			{
				Array.Resize(ref _rankQuests, value.Length);
				value.CopyTo(_rankQuests, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
