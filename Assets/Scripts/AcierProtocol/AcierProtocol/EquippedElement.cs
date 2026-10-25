using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EquippedElement")]
	public class EquippedElement : IExtensible
	{
		private InventoryElement _element;

		private EInventorySlot _slot = EInventorySlot.SlotWeapon;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "element", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventoryElement element
		{
			get
			{
				return _element;
			}
			set
			{
				_element = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "slot", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EInventorySlot.SlotWeapon)]
		public EInventorySlot slot
		{
			get
			{
				return _slot;
			}
			set
			{
				_slot = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
