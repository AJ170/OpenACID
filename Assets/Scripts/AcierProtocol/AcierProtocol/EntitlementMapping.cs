using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EntitlementMapping")]
	public class EntitlementMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private string _gameIdGuidStr = "";

			private string _name = "";

			private string[] _requiredIds = new string[0];

			private int _requiredLvl;

			private bool _onlyGrantAsReward;

			private int _gameIdGuid;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "gameIdGuidStr", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string gameIdGuidStr
			{
				get
				{
					return _gameIdGuidStr;
				}
				set
				{
					_gameIdGuidStr = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

			[ProtoMember(3, Name = "requiredIds", DataFormat = DataFormat.Default)]
			public string[] requiredIds
			{
				get
				{
					return _requiredIds;
				}
				set
				{
					Array.Resize(ref _requiredIds, value.Length);
					value.CopyTo(_requiredIds, 0);
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "requiredLvl", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int requiredLvl
			{
				get
				{
					return _requiredLvl;
				}
				set
				{
					_requiredLvl = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "onlyGrantAsReward", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool onlyGrantAsReward
			{
				get
				{
					return _onlyGrantAsReward;
				}
				set
				{
					_onlyGrantAsReward = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "gameIdGuid", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int gameIdGuid
			{
				get
				{
					return _gameIdGuid;
				}
				set
				{
					_gameIdGuid = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private string _reqEntHireling = "";

		private string _reqEntForge = "";

		private string _reqEntMysteryBox = "";

		private string _reqEntSkillTree = "";

		private string _reqEntShop = "";

		private string _reqEntEquipment = "";

		private string _reqEntEquipmentModule = "";

		private string _reqEntContracts = "";

		private string _reqEntEquipmentOutfit = "";

		private string _reqEntEquipmentInsignia = "";

		private string _reqEntFriending = "";

		private string _reqEntDailyQuests = "";

		private string _reqEntChallenges = "";

		private string _reqEntMailsButton = "";

		private string _reqEntRevive = "";

		private string _reqEntBerserker = "";

		private string _reqEntShadowblade = "";

		private string _reqEntTrickster = "";

		private string _reqEntThief = "";

		private string _reqEntProgressionEvents = "";

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

		[ProtoMember(10, IsRequired = false, Name = "reqEntHireling", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntHireling
		{
			get
			{
				return _reqEntHireling;
			}
			set
			{
				_reqEntHireling = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "reqEntForge", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntForge
		{
			get
			{
				return _reqEntForge;
			}
			set
			{
				_reqEntForge = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "reqEntMysteryBox", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntMysteryBox
		{
			get
			{
				return _reqEntMysteryBox;
			}
			set
			{
				_reqEntMysteryBox = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "reqEntSkillTree", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntSkillTree
		{
			get
			{
				return _reqEntSkillTree;
			}
			set
			{
				_reqEntSkillTree = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "reqEntShop", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntShop
		{
			get
			{
				return _reqEntShop;
			}
			set
			{
				_reqEntShop = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "reqEntEquipment", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntEquipment
		{
			get
			{
				return _reqEntEquipment;
			}
			set
			{
				_reqEntEquipment = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "reqEntEquipmentModule", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntEquipmentModule
		{
			get
			{
				return _reqEntEquipmentModule;
			}
			set
			{
				_reqEntEquipmentModule = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "reqEntContracts", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntContracts
		{
			get
			{
				return _reqEntContracts;
			}
			set
			{
				_reqEntContracts = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "reqEntEquipmentOutfit", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntEquipmentOutfit
		{
			get
			{
				return _reqEntEquipmentOutfit;
			}
			set
			{
				_reqEntEquipmentOutfit = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "reqEntEquipmentInsignia", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntEquipmentInsignia
		{
			get
			{
				return _reqEntEquipmentInsignia;
			}
			set
			{
				_reqEntEquipmentInsignia = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "reqEntFriending", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntFriending
		{
			get
			{
				return _reqEntFriending;
			}
			set
			{
				_reqEntFriending = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "reqEntDailyQuests", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntDailyQuests
		{
			get
			{
				return _reqEntDailyQuests;
			}
			set
			{
				_reqEntDailyQuests = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "reqEntChallenges", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntChallenges
		{
			get
			{
				return _reqEntChallenges;
			}
			set
			{
				_reqEntChallenges = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "reqEntMailsButton", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntMailsButton
		{
			get
			{
				return _reqEntMailsButton;
			}
			set
			{
				_reqEntMailsButton = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "reqEntRevive", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntRevive
		{
			get
			{
				return _reqEntRevive;
			}
			set
			{
				_reqEntRevive = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "reqEntBerserker", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntBerserker
		{
			get
			{
				return _reqEntBerserker;
			}
			set
			{
				_reqEntBerserker = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "reqEntShadowblade", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntShadowblade
		{
			get
			{
				return _reqEntShadowblade;
			}
			set
			{
				_reqEntShadowblade = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "reqEntTrickster", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntTrickster
		{
			get
			{
				return _reqEntTrickster;
			}
			set
			{
				_reqEntTrickster = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "reqEntThief", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntThief
		{
			get
			{
				return _reqEntThief;
			}
			set
			{
				_reqEntThief = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "reqEntProgressionEvents", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string reqEntProgressionEvents
		{
			get
			{
				return _reqEntProgressionEvents;
			}
			set
			{
				_reqEntProgressionEvents = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
