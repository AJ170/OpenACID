using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionFormationBaseContent")]
	public class MissionFormationBaseContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private EFormationIterationDirection _IterationDirection;

		private EFormationIterationType _IterationType;

		private EPatrolType _Type;

		private bool _FixedLeaderSlot;

		private float _Radius;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "IterationDirection", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFormationIterationDirection.Down)]
		public EFormationIterationDirection IterationDirection
		{
			get
			{
				return _IterationDirection;
			}
			set
			{
				_IterationDirection = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "IterationType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFormationIterationType.BackAndForth)]
		public EFormationIterationType IterationType
		{
			get
			{
				return _IterationType;
			}
			set
			{
				_IterationType = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EPatrolType.Military)]
		public EPatrolType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "FixedLeaderSlot", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool FixedLeaderSlot
		{
			get
			{
				return _FixedLeaderSlot;
			}
			set
			{
				_FixedLeaderSlot = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "Radius", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Radius
		{
			get
			{
				return _Radius;
			}
			set
			{
				_Radius = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
