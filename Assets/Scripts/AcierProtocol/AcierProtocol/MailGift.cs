using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailGift")]
	public class MailGift : IExtensible
	{
		private string _senderName = "";

		private string _senderId = "";

		private Resources _resources;

		private EModuleOrigin _origin;

		private InventoryElements _elements;

		private string[] _entitlements = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "senderName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string senderName
		{
			get
			{
				return _senderName;
			}
			set
			{
				_senderName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "senderId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string senderId
		{
			get
			{
				return _senderId;
			}
			set
			{
				_senderId = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "resources", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources resources
		{
			get
			{
				return _resources;
			}
			set
			{
				_resources = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "origin", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleOrigin.ModuleOrigin_DailyReward)]
		public EModuleOrigin origin
		{
			get
			{
				return _origin;
			}
			set
			{
				_origin = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "elements", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventoryElements elements
		{
			get
			{
				return _elements;
			}
			set
			{
				_elements = value;
			}
		}

		[ProtoMember(9, Name = "entitlements", DataFormat = DataFormat.Default)]
		public string[] entitlements
		{
			get
			{
				return _entitlements;
			}
			set
			{
				Array.Resize(ref _entitlements, value.Length);
				value.CopyTo(_entitlements, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
