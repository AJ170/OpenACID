using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetForeignAssassinResponse")]
	public class GetForeignAssassinResponse : IExtensible
	{
		private Assassin _assassin;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assassin", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Assassin assassin
		{
			get
			{
				return _assassin;
			}
			set
			{
				_assassin = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
