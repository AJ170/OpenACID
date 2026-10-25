using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SkillWheelEntry")]
	public class SkillWheelEntry : IExtensible
	{
		private int _tierId;

		private int _position;

		private Assassin.EClassTypeId _classType = Assassin.EClassTypeId.None;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "tierId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int tierId
		{
			get
			{
				return _tierId;
			}
			set
			{
				_tierId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "position", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "classType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId classType
		{
			get
			{
				return _classType;
			}
			set
			{
				_classType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
