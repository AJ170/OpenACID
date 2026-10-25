using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "OpenMysteryBoxRequest")]
	public class OpenMysteryBoxRequest : IExtensible
	{
		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private EMysteryBoxType _mysteryBoxType = EMysteryBoxType.MysteryBoxItemType;

		private string _itemId = "";

		private bool _startChain;

		private bool _overspending;

		private string _assassinId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Quality.EQuality.Mundane)]
		public Quality.EQuality quality
		{
			get
			{
				return _quality;
			}
			set
			{
				_quality = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "mysteryBoxType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMysteryBoxType.MysteryBoxItemType)]
		public EMysteryBoxType mysteryBoxType
		{
			get
			{
				return _mysteryBoxType;
			}
			set
			{
				_mysteryBoxType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "itemId", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "startChain", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool startChain
		{
			get
			{
				return _startChain;
			}
			set
			{
				_startChain = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "overspending", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool overspending
		{
			get
			{
				return _overspending;
			}
			set
			{
				_overspending = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
