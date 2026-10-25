using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FriendAssassin")]
	public class FriendAssassin : IExtensible
	{
		private string _id;

		private Assassin.EClassTypeId _classTypeId = Assassin.EClassTypeId.None;

		private Assassin.EClassTypeId _extendedClassTypeId = Assassin.EClassTypeId.None;

		private AssassinName _assassinName;

		private string _headGuid = "";

		private string _humanoidGuid = "";

		private string _colorSetting = "";

		private string _insigniaGuid = "";

		private long _itemPower;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "id", DataFormat = DataFormat.Default)]
		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "classTypeId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId classTypeId
		{
			get
			{
				return _classTypeId;
			}
			set
			{
				_classTypeId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "extendedClassTypeId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId extendedClassTypeId
		{
			get
			{
				return _extendedClassTypeId;
			}
			set
			{
				_extendedClassTypeId = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "assassinName", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinName assassinName
		{
			get
			{
				return _assassinName;
			}
			set
			{
				_assassinName = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "headGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string headGuid
		{
			get
			{
				return _headGuid;
			}
			set
			{
				_headGuid = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "humanoidGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string humanoidGuid
		{
			get
			{
				return _humanoidGuid;
			}
			set
			{
				_humanoidGuid = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "colorSetting", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string colorSetting
		{
			get
			{
				return _colorSetting;
			}
			set
			{
				_colorSetting = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "insigniaGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string insigniaGuid
		{
			get
			{
				return _insigniaGuid;
			}
			set
			{
				_insigniaGuid = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "itemPower", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long itemPower
		{
			get
			{
				return _itemPower;
			}
			set
			{
				_itemPower = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
