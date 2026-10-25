using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinSkillTier_")]
	public class AssassinSkillTier_ : IExtensible
	{
		private int _tier;

		private AssassinSkills _skills;

		private int _pointsRemaining;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "tier", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int tier
		{
			get
			{
				return _tier;
			}
			set
			{
				_tier = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "skills", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSkills skills
		{
			get
			{
				return _skills;
			}
			set
			{
				_skills = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "pointsRemaining", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int pointsRemaining
		{
			get
			{
				return _pointsRemaining;
			}
			set
			{
				_pointsRemaining = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
