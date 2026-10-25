using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetStaticsMissionGeneratorRequest")]
	public class GetStaticsMissionGeneratorRequest : IExtensible
	{
		private bool _resetCatalog;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "resetCatalog", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool resetCatalog
		{
			get
			{
				return _resetCatalog;
			}
			set
			{
				_resetCatalog = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
