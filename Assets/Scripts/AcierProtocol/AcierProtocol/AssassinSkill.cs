using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinSkill")]
	public class AssassinSkill : IExtensible
	{
		private int _level;

		private bool _unlocked;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int level
		{
			get
			{
				return _level;
			}
			set
			{
				_level = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "unlocked", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool unlocked
		{
			get
			{
				return _unlocked;
			}
			set
			{
				_unlocked = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
