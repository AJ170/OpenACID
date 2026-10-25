using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveGetInRangeContent")]
	public class ObjectiveGetInRangeContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private long _TargetID;

		private bool _Inverse;

		private bool _ResetIfInvalidated;

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

		[ProtoMember(1, IsRequired = false, Name = "Inverse", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool Inverse
		{
			get
			{
				return _Inverse;
			}
			set
			{
				_Inverse = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "ResetIfInvalidated", DataFormat = DataFormat.Default)]
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
