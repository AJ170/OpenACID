using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "VerifyAppRequest")]
	public class VerifyAppRequest : IExtensible
	{
		private string _appReceipt = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "appReceipt", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string appReceipt
		{
			get
			{
				return _appReceipt;
			}
			set
			{
				_appReceipt = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
