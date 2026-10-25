using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ShareMissionRequest")]
	public class ShareMissionRequest : IExtensible
	{
		private string _missionId = "";

		private string _receiverId = "";

		private string _missionFullName = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(2, IsRequired = false, Name = "receiverId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiverId
		{
			get
			{
				return _receiverId;
			}
			set
			{
				_receiverId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "missionFullName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionFullName
		{
			get
			{
				return _missionFullName;
			}
			set
			{
				_missionFullName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
