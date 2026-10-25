using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMissionInfoRequest")]
	public class GetMissionInfoRequest : IExtensible
	{
		private string _assassinId = "";

		private string _campaignId = "";

		private string _missionId = "";

		private string _hirelingId = "";

		private int _rank;

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

		[ProtoMember(2, IsRequired = false, Name = "campaignId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "missionId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "hirelingId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
