using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UnequipElementRequest")]
	public class UnequipElementRequest : IExtensible
	{
		private string _assassinId = "";

		private string _elementId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string assassinId
		{
			get
			{
				return _assassinId;
			}
			set
			{
				_assassinId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "elementId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string elementId
		{
			get
			{
				return _elementId;
			}
			set
			{
				_elementId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
