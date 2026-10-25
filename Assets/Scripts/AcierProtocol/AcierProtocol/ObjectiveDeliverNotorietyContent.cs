using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveDeliverNotorietyContent")]
	public class ObjectiveDeliverNotorietyContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private bool _FleeOnTransgression;

		private long _FleeTargetID;

		private EAlertLevel _MaxNotoriety;

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

		[ProtoMember(1, IsRequired = false, Name = "FleeOnTransgression", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool FleeOnTransgression
		{
			get
			{
				return _FleeOnTransgression;
			}
			set
			{
				_FleeOnTransgression = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "FleeTargetID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long FleeTargetID
		{
			get
			{
				return _FleeTargetID;
			}
			set
			{
				_FleeTargetID = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MaxNotoriety", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
