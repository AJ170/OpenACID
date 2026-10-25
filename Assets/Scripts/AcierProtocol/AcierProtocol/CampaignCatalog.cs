using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CampaignCatalog")]
	public class CampaignCatalog : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Campaign")]
		public class Campaign : IExtensible
		{
			private string _id = "";

			private string _name = "";

			private string[] _reqEntitlements = new string[0];

			private bool _hidden;

			private int _sortIndex;

			private MissionsAtLocation[] _missions = new MissionsAtLocation[0];

			private EarlyAccessInfo _earlyAccessInfo;

			private Resources _unlockCosts;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string id
			{
				get
				{
					return _id;
				}
				set
				{
					_id = value;
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

			[ProtoMember(3, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
			public string[] reqEntitlements
			{
				get
				{
					return _reqEntitlements;
				}
				set
				{
					Array.Resize(ref _reqEntitlements, value.Length);
					value.CopyTo(_reqEntitlements, 0);
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "hidden", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool hidden
			{
				get
				{
					return _hidden;
				}
				set
				{
					_hidden = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "sortIndex", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int sortIndex
			{
				get
				{
					return _sortIndex;
				}
				set
				{
					_sortIndex = value;
				}
			}

			[ProtoMember(6, Name = "missions", DataFormat = DataFormat.Default)]
			public MissionsAtLocation[] missions
			{
				get
				{
					return _missions;
				}
				set
				{
					Array.Resize(ref _missions, value.Length);
					value.CopyTo(_missions, 0);
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "earlyAccessInfo", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public EarlyAccessInfo earlyAccessInfo
			{
				get
				{
					return _earlyAccessInfo;
				}
				set
				{
					_earlyAccessInfo = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "unlockCosts", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resources unlockCosts
			{
				get
				{
					return _unlockCosts;
				}
				set
				{
					_unlockCosts = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Campaign[] _campaigns = new Campaign[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "campaigns", DataFormat = DataFormat.Default)]
		public Campaign[] campaigns
		{
			get
			{
				return _campaigns;
			}
			set
			{
				Array.Resize(ref _campaigns, value.Length);
				value.CopyTo(_campaigns, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
