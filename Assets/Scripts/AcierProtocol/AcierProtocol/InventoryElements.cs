using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "InventoryElements")]
	public class InventoryElements : IExtensible
	{
		private InventoryElement[] _elements = new InventoryElement[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "elements", DataFormat = DataFormat.Default)]
		public InventoryElement[] elements
		{
			get
			{
				return _elements;
			}
			set
			{
				Array.Resize(ref _elements, value.Length);
				value.CopyTo(_elements, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
