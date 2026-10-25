using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinSkillInTraining")]
	public class AssassinSkillInTraining : IExtensible
	{
		[ProtoContract(Name = "SkillClass")]
		public enum SkillClass
		{
			[ProtoEnum(Name = "None_SkillClass", Value = 1)]
			None_SkillClass = 1,
			[ProtoEnum(Name = "BaseClass", Value = 2)]
			BaseClass = 2,
			[ProtoEnum(Name = "ExtendedClass", Value = 3)]
			ExtendedClass = 3
		}

		[ProtoContract(Name = "SkillType")]
		public enum SkillType
		{
			[ProtoEnum(Name = "Active", Value = 0)]
			Active = 0,
			[ProtoEnum(Name = "PassiveOne", Value = 1)]
			PassiveOne = 1,
			[ProtoEnum(Name = "PassiveTwo", Value = 2)]
			PassiveTwo = 2,
			[ProtoEnum(Name = "None_SkillType", Value = 3)]
			None_SkillType = 3
		}

		private SkillClass _skillClass = SkillClass.None_SkillClass;

		private SkillType _skillType;

		private long _startTime;

		private long _remainingTimeInMs;

		private long _totalTimeInMs;

		private int _tierId;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "skillClass", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(SkillClass.None_SkillClass)]
		public SkillClass skillClass
		{
			get
			{
				return _skillClass;
			}
			set
			{
				_skillClass = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "skillType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(SkillType.Active)]
		public SkillType skillType
		{
			get
			{
				return _skillType;
			}
			set
			{
				_skillType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "startTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long startTime
		{
			get
			{
				return _startTime;
			}
			set
			{
				_startTime = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "remainingTimeInMs", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long remainingTimeInMs
		{
			get
			{
				return _remainingTimeInMs;
			}
			set
			{
				_remainingTimeInMs = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "totalTimeInMs", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long totalTimeInMs
		{
			get
			{
				return _totalTimeInMs;
			}
			set
			{
				_totalTimeInMs = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "tierId", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
