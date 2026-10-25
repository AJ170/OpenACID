using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PurchaseInGameItemRequest")]
	public class PurchaseInGameItemRequest : IExtensible
	{
		private string _itemId = "";

		private string _receiverId = "";

		private Resources _costs;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "itemId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string itemId
		{
			get
			{
				return _itemId;
			}
			set
			{
				_itemId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "receiverId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiverId
		{
			get
			{
				return _receiverId;
			}
			set
			{
				_receiverId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "costs", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources costs
		{
			get
			{
				return _costs;
			}
			set
			{
				_costs = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
