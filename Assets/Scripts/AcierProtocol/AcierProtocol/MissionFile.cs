using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionFile")]
	public class MissionFile : IExtensible
	{
		private SerializedNode _RootNode;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "RootNode", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public SerializedNode RootNode
		{
			get
			{
				return _RootNode;
			}
			set
			{
				_RootNode = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
