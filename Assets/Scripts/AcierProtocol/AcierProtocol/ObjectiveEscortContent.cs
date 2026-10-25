using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveEscortContent")]
	public class ObjectiveEscortContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private int _EnemyCountCap;

		private float _EscortAreaRadius;

		private int _MaxSurviveDurationSec;

		private int _ResumePatrolDelaySec;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveTargetContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveTargetContent ObjectiveTargetContent
		{
			get
			{
				return _ObjectiveTargetContent;
			}
			set
			{
				_ObjectiveTargetContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "EnemyCountCap", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int EnemyCountCap
		{
			get
			{
				return _EnemyCountCap;
			}
			set
			{
				_EnemyCountCap = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "EscortAreaRadius", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float EscortAreaRadius
		{
			get
			{
				return _EscortAreaRadius;
			}
			set
			{
				_EscortAreaRadius = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "MaxSurviveDurationSec", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int MaxSurviveDurationSec
		{
			get
			{
				return _MaxSurviveDurationSec;
			}
			set
			{
				_MaxSurviveDurationSec = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "ResumePatrolDelaySec", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int ResumePatrolDelaySec
		{
			get
			{
				return _ResumePatrolDelaySec;
			}
			set
			{
				_ResumePatrolDelaySec = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
