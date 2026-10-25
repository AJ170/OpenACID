using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ModifyProfileNameResponse")]
	public class ModifyProfileNameResponse : IExtensible
	{
		private string _username = "";

		private string _password = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "username", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "password", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
