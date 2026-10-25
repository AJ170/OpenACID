using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionToken")]
	public class MissionToken : IExtensible
	{
		private string _token = "";

		private string _missionId = "";

		private string _userId = "";

		private string _assassinId = "";

		private string _hirelingId = "";

		private long _createdAt;

		private long _expiresAt;

		private string _missionXml = "";

		private MissionFile _missionFile;

		private MissionGoals _missionGoals;

		private InventoryElement _module;

		private MissionGoalLoot[] _missionGoalLoots = new MissionGoalLoot[0];

		private bool _hirelingIsFriend;

		private string _campaignId = "";

		private int _rank;

		private bool _hirelingCanBeFriend;

		private bool _eliteMission;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "token", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string token
		{
			get
			{
				return _token;
			}
			set
			{
				_token = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "missionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionId
		{
			get
			{
				return _missionId;
			}
			set
			{
				_missionId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "userId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userId
		{
			get
			{
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(6, IsRequired = false, Name = "hirelingId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string hirelingId
		{
			get
			{
				return _hirelingId;
			}
			set
			{
				_hirelingId = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "createdAt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long createdAt
		{
			get
			{
				return _createdAt;
			}
			set
			{
				_createdAt = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "expiresAt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long expiresAt
		{
			get
			{
				return _expiresAt;
			}
			set
			{
				_expiresAt = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "missionXml", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionXml
		{
			get
			{
				return _missionXml;
			}
			set
			{
				_missionXml = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "missionFile", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionFile missionFile
		{
			get
			{
				return _missionFile;
			}
			set
			{
				_missionFile = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "missionGoals", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionGoals missionGoals
		{
			get
			{
				return _missionGoals;
			}
			set
			{
				_missionGoals = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "module", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventoryElement module
		{
			get
			{
				return _module;
			}
			set
			{
				_module = value;
			}
		}

		[ProtoMember(13, Name = "missionGoalLoots", DataFormat = DataFormat.Default)]
		public MissionGoalLoot[] missionGoalLoots
		{
			get
			{
				return _missionGoalLoots;
			}
			set
			{
				Array.Resize(ref _missionGoalLoots, value.Length);
				value.CopyTo(_missionGoalLoots, 0);
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "hirelingIsFriend", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool hirelingIsFriend
		{
			get
			{
				return _hirelingIsFriend;
			}
			set
			{
				_hirelingIsFriend = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "campaignId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string campaignId
		{
			get
			{
				return _campaignId;
			}
			set
			{
				_campaignId = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "hirelingCanBeFriend", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool hirelingCanBeFriend
		{
			get
			{
				return _hirelingCanBeFriend;
			}
			set
			{
				_hirelingCanBeFriend = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "eliteMission", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool eliteMission
		{
			get
			{
				return _eliteMission;
			}
			set
			{
				_eliteMission = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
