using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveNotorietyContent")]
	public class ObjectiveNotorietyContent : IExtensible
	{
		private ObjectiveBaseContent _ObjectiveBaseContent;

		private EAlertLevel _MaxNotoriety;

		private bool _FailOnTransgression;

		private int _StayBelowSec;

		private ECompare _Comparer = ECompare.Less;

		private bool _ResetIfInvalidated;

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

		[ProtoMember(9, IsRequired = false, Name = "MaxNotoriety", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EAlertLevel.Anonymous)]
		public EAlertLevel MaxNotoriety
		{
			get
			{
				return _MaxNotoriety;
			}
			set
			{
				_MaxNotoriety = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "FailOnTransgression", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool FailOnTransgression
		{
			get
			{
				return _FailOnTransgression;
			}
			set
			{
				_FailOnTransgression = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "StayBelowSec", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int StayBelowSec
		{
			get
			{
				return _StayBelowSec;
			}
			set
			{
				_StayBelowSec = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "Comparer", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ECompare.Less)]
		public ECompare Comparer
		{
			get
			{
				return _Comparer;
			}
			set
			{
				_Comparer = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ResetIfInvalidated", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool ResetIfInvalidated
		{
			get
			{
				return _ResetIfInvalidated;
			}
			set
			{
				_ResetIfInvalidated = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
