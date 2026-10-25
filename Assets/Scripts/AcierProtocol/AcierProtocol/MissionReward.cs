using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionReward")]
	public class MissionReward : IExtensible
	{
		private InventoryElement[] _elements = new InventoryElement[0];

		private ResourceFromTemplate[] _resources = new ResourceFromTemplate[0];

		private string[] _entitlements = new string[0];

		private int _skillPoints;

		private long _baseXp;

		private long _baseSc;

		private long _baseHt;

		private long _xpWithoutChallenge;

		private long _scWithoutChallenge;

		private long _htWithoutChallenge;

		private long _xpFromPickpocketing;

		private long _scFromPickpocketing;

		private long _htFromPickpocketing;

		private long _scFromHireling;

		private long _htFromHireling;

		private long _finalXp;

		private long _finalSc;

		private long _finalHt;

		private MissionGoalLoot[] _challengeLoots = new MissionGoalLoot[0];

		private bool _fullInventory;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "elements", DataFormat = DataFormat.Default)]
		public InventoryElement[] elements
		{
			get
			{
				return _elements;
			}
			set
			{
				Array.Resize(ref _elements, value.Length);
				value.CopyTo(_elements, 0);
			}
		}

		[ProtoMember(2, Name = "resources", DataFormat = DataFormat.Default)]
		public ResourceFromTemplate[] resources
		{
			get
			{
				return _resources;
			}
			set
			{
				Array.Resize(ref _resources, value.Length);
				value.CopyTo(_resources, 0);
			}
		}

		[ProtoMember(3, Name = "entitlements", DataFormat = DataFormat.Default)]
		public string[] entitlements
		{
			get
			{
				return _entitlements;
			}
			set
			{
				Array.Resize(ref _entitlements, value.Length);
				value.CopyTo(_entitlements, 0);
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "skillPoints", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int skillPoints
		{
			get
			{
				return _skillPoints;
			}
			set
			{
				_skillPoints = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "baseXp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long baseXp
		{
			get
			{
				return _baseXp;
			}
			set
			{
				_baseXp = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "baseSc", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long baseSc
		{
			get
			{
				return _baseSc;
			}
			set
			{
				_baseSc = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "baseHt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long baseHt
		{
			get
			{
				return _baseHt;
			}
			set
			{
				_baseHt = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "xpWithoutChallenge", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long xpWithoutChallenge
		{
			get
			{
				return _xpWithoutChallenge;
			}
			set
			{
				_xpWithoutChallenge = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "scWithoutChallenge", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long scWithoutChallenge
		{
			get
			{
				return _scWithoutChallenge;
			}
			set
			{
				_scWithoutChallenge = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "htWithoutChallenge", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long htWithoutChallenge
		{
			get
			{
				return _htWithoutChallenge;
			}
			set
			{
				_htWithoutChallenge = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "xpFromPickpocketing", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long xpFromPickpocketing
		{
			get
			{
				return _xpFromPickpocketing;
			}
			set
			{
				_xpFromPickpocketing = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "scFromPickpocketing", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long scFromPickpocketing
		{
			get
			{
				return _scFromPickpocketing;
			}
			set
			{
				_scFromPickpocketing = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "htFromPickpocketing", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long htFromPickpocketing
		{
			get
			{
				return _htFromPickpocketing;
			}
			set
			{
				_htFromPickpocketing = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "scFromHireling", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long scFromHireling
		{
			get
			{
				return _scFromHireling;
			}
			set
			{
				_scFromHireling = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "htFromHireling", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long htFromHireling
		{
			get
			{
				return _htFromHireling;
			}
			set
			{
				_htFromHireling = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "finalXp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long finalXp
		{
			get
			{
				return _finalXp;
			}
			set
			{
				_finalXp = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "finalSc", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long finalSc
		{
			get
			{
				return _finalSc;
			}
			set
			{
				_finalSc = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "finalHt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long finalHt
		{
			get
			{
				return _finalHt;
			}
			set
			{
				_finalHt = value;
			}
		}

		[ProtoMember(13, Name = "challengeLoots", DataFormat = DataFormat.Default)]
		public MissionGoalLoot[] challengeLoots
		{
			get
			{
				return _challengeLoots;
			}
			set
			{
				Array.Resize(ref _challengeLoots, value.Length);
				value.CopyTo(_challengeLoots, 0);
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "fullInventory", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool fullInventory
		{
			get
			{
				return _fullInventory;
			}
			set
			{
				_fullInventory = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
