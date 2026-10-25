using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LevelSkillPointMapping")]
	public class LevelSkillPointMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "LevelSkillPointEntry")]
		public class LevelSkillPointEntry : IExtensible
		{
			private int _level;

			private int _skillPoint;

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

			[ProtoMember(2, IsRequired = false, Name = "skillPoint", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int skillPoint
			{
				get
				{
					return _skillPoint;
				}
				set
				{
					_skillPoint = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private LevelSkillPointEntry[] _skillPointsPerLevel = new LevelSkillPointEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "skillPointsPerLevel", DataFormat = DataFormat.Default)]
		public LevelSkillPointEntry[] skillPointsPerLevel
		{
			get
			{
				return _skillPointsPerLevel;
			}
			set
			{
				Array.Resize(ref _skillPointsPerLevel, value.Length);
				value.CopyTo(_skillPointsPerLevel, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
