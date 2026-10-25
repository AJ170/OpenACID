using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "StoryMissionRewardMapping")]
	public class StoryMissionRewardMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "RankEntry")]
			public class RankEntry : IExtensible
			{
				private int _rankIdx;

				private int _xp;

				private int _softCurrency;

				private LootTemplate[] _lootTemplates = new LootTemplate[0];

				private int _heroTokens;

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

				[ProtoMember(2, IsRequired = false, Name = "xp", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int xp
				{
					get
					{
						return _xp;
					}
					set
					{
						_xp = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "softCurrency", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int softCurrency
				{
					get
					{
						return _softCurrency;
					}
					set
					{
						_softCurrency = value;
					}
				}

				[ProtoMember(4, Name = "lootTemplates", DataFormat = DataFormat.Default)]
				public LootTemplate[] lootTemplates
				{
					get
					{
						return _lootTemplates;
					}
					set
					{
						Array.Resize(ref _lootTemplates, value.Length);
						value.CopyTo(_lootTemplates, 0);
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "heroTokens", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int heroTokens
				{
					get
					{
						return _heroTokens;
					}
					set
					{
						_heroTokens = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private string _name = "";

			private RankEntry[] _completionRewards = new RankEntry[0];

			private RankEntry[] _challengeRewards = new RankEntry[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string name
			{
				get
				{
					return _name;
				}
				set
				{
					_name = value;
				}
			}

			[ProtoMember(2, Name = "completionRewards", DataFormat = DataFormat.Default)]
			public RankEntry[] completionRewards
			{
				get
				{
					return _completionRewards;
				}
				set
				{
					Array.Resize(ref _completionRewards, value.Length);
					value.CopyTo(_completionRewards, 0);
				}
			}

			[ProtoMember(3, Name = "challengeRewards", DataFormat = DataFormat.Default)]
			public RankEntry[] challengeRewards
			{
				get
				{
					return _challengeRewards;
				}
				set
				{
					Array.Resize(ref _challengeRewards, value.Length);
					value.CopyTo(_challengeRewards, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
		{
			get
			{
				return _entries;
			}
			set
			{
				Array.Resize(ref _entries, value.Length);
				value.CopyTo(_entries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
