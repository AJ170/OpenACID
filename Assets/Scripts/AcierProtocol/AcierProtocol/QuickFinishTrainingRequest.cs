using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "QuickFinishTrainingRequest")]
	public class QuickFinishTrainingRequest : IExtensible
	{
		private string _assassinId = "";

		private int _calculatedPrice;

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

		[ProtoMember(2, IsRequired = false, Name = "calculatedPrice", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int calculatedPrice
		{
			get
			{
				return _calculatedPrice;
			}
			set
			{
				_calculatedPrice = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
