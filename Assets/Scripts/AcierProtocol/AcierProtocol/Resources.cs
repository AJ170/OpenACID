using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Resources")]
	public class Resources : IExtensible
	{
		private Resource[] _resources = new Resource[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "resources", DataFormat = DataFormat.Default)]
		public Resource[] resources
		{
			get
			{
				return _resources;
			}
			set
			{
				Array.Resize(ref _resources, value.Length);
				value.CopyTo(_resources, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
