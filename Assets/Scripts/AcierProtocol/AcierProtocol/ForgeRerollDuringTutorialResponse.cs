using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ForgeRerollDuringTutorialResponse")]
	public class ForgeRerollDuringTutorialResponse : IExtensible
	{
		private InventoryElement _rolledElement;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rolledElement", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventoryElement rolledElement
		{
			get
			{
				return _rolledElement;
			}
			set
			{
				_rolledElement = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
