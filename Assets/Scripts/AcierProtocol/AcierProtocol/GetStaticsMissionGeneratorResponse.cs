using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetStaticsMissionGeneratorResponse")]
	public class GetStaticsMissionGeneratorResponse : IExtensible
	{
		private StaticsMissionGenerator _staticsMissionGenerator;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "staticsMissionGenerator", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public StaticsMissionGenerator staticsMissionGenerator
		{
			get
			{
				return _staticsMissionGenerator;
			}
			set
			{
				_staticsMissionGenerator = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
