using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveStayInRangeMobileContent")]
	public class ObjectiveStayInRangeMobileContent : IExtensible
	{
		private ObjectiveStayInRangeContent _ObjectiveStayInRangeContent;

		private long _ToTargetID;

		private float _Range;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveStayInRangeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveStayInRangeContent ObjectiveStayInRangeContent
		{
			get
			{
				return _ObjectiveStayInRangeContent;
			}
			set
			{
				_ObjectiveStayInRangeContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "ToTargetID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long ToTargetID
		{
			get
			{
				return _ToTargetID;
			}
			set
			{
				_ToTargetID = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Range", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Range
		{
			get
			{
				return _Range;
			}
			set
			{
				_Range = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
