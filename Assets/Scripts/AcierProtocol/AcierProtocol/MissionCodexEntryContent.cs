using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionCodexEntryContent")]
	public class MissionCodexEntryContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private string _TypeGuid = "";

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
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
