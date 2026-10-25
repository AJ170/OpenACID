using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionSpawnObjectContent")]
	public class MissionSpawnObjectContent : IExtensible
	{
		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private string _TypeGuid = "";

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionSpawnEntityContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnEntityContent MissionSpawnEntityContent
		{
			get
			{
				return _MissionSpawnEntityContent;
			}
			set
			{
				_MissionSpawnEntityContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "TypeGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string TypeGuid
		{
			get
			{
				return _TypeGuid;
			}
			set
			{
				_TypeGuid = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
