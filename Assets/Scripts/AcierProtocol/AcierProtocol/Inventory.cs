using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Inventory")]
	public class Inventory : IExtensible
	{
		private int _maxSize;

		private InventoryElement[] _elements = new InventoryElement[0];

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "maxSize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxSize
		{
			get
			{
				return _maxSize;
			}
			set
			{
				_maxSize = value;
			}
		}

		[ProtoMember(3, Name = "elements", DataFormat = DataFormat.Default)]
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
