using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveStayInRangeContent")]
	public class ObjectiveStayInRangeContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private long _TargetID;

		private int _TimerSeconds;

		private int _StayTimerSeconds;

		private bool _MaintainLineOfSight;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBaseContent ObjectiveBaseContent
		{
			get
			{
				return _ObjectiveBaseContent;
			}
			set
			{
				_ObjectiveBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "TargetID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long TargetID
		{
			get
			{
				return _TargetID;
			}
			set
			{
				_TargetID = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "TimerSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int TimerSeconds
		{
			get
			{
				return _TimerSeconds;
			}
			set
			{
				_TimerSeconds = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "StayTimerSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int StayTimerSeconds
		{
			get
			{
				return _StayTimerSeconds;
			}
			set
			{
				_StayTimerSeconds = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MaintainLineOfSight", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool MaintainLineOfSight
		{
			get
			{
				return _MaintainLineOfSight;
			}
			set
			{
				_MaintainLineOfSight = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
