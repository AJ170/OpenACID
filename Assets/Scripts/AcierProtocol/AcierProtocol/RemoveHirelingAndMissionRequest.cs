using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RemoveHirelingAndMissionRequest")]
	public class RemoveHirelingAndMissionRequest : IExtensible
	{
		private string _campaignId = "";

		private string _missionId = "";

		private string _assassinId = "";

		private int _rankIdx;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "campaignId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(3, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
