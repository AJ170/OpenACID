using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "StaticObjectConfigurationData")]
	public class StaticObjectConfigurationData : IExtensible
	{
		private string _guid = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string guid
		{
			get
			{
				return _guid;
			}
			set
			{
				_guid = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
