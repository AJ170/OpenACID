using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinSkills")]
	public class AssassinSkills : IExtensible
	{
		private AssassinSkill _activeSkill;

		private AssassinSkill _passiveOneSkill;

		private AssassinSkill _passiveTwoSkill;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "activeSkill", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkill activeSkill
		{
			get
			{
				return _activeSkill;
			}
			set
			{
				_activeSkill = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "passiveOneSkill", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkill passiveOneSkill
		{
			get
			{
				return _passiveOneSkill;
			}
			set
			{
				_passiveOneSkill = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "passiveTwoSkill", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkill passiveTwoSkill
		{
			get
			{
				return _passiveTwoSkill;
			}
			set
			{
				_passiveTwoSkill = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
