using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionWaypointContent")]
	public class MissionWaypointContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private ENpcIdleCategory _IdleCategory;

		private float _IdleTime;

		private ERotationAtWaypoint _RotationType;

		private EMovementSpeed _MinSpeed;

		private EMovementSpeed _MaxSpeed;

		private bool _WarpTarget;

		private EFormationIterationType _ActivationIterationType;

		private ERouteIterationStart _ActivationIterationStartType = ERouteIterationStart.Reset_ERouteIterationStart;

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

		[ProtoMember(9, IsRequired = false, Name = "IdleCategory", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ENpcIdleCategory.NpcTypeDefault)]
		public ENpcIdleCategory IdleCategory
		{
			get
			{
				return _IdleCategory;
			}
			set
			{
				_IdleCategory = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "IdleTime", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float IdleTime
		{
			get
			{
				return _IdleTime;
			}
			set
			{
				_IdleTime = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "RotationType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERotationAtWaypoint.None)]
		public ERotationAtWaypoint RotationType
		{
			get
			{
				return _RotationType;
			}
			set
			{
				_RotationType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MinSpeed", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMovementSpeed.WalkSlow)]
		public EMovementSpeed MinSpeed
		{
			get
			{
				return _MinSpeed;
			}
			set
			{
				_MinSpeed = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "MaxSpeed", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMovementSpeed.WalkSlow)]
		public EMovementSpeed MaxSpeed
		{
			get
			{
				return _MaxSpeed;
			}
			set
			{
				_MaxSpeed = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "WarpTarget", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool WarpTarget
		{
			get
			{
				return _WarpTarget;
			}
			set
			{
				_WarpTarget = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "ActivationIterationType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFormationIterationType.BackAndForth)]
		public EFormationIterationType ActivationIterationType
		{
			get
			{
				return _ActivationIterationType;
			}
			set
			{
				_ActivationIterationType = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "ActivationIterationStartType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ERouteIterationStart.Reset_ERouteIterationStart)]
		public ERouteIterationStart ActivationIterationStartType
		{
			get
			{
				return _ActivationIterationStartType;
			}
			set
			{
				_ActivationIterationStartType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
