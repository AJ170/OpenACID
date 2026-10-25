using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveGetInRangeMobileContent")]
	public class ObjectiveGetInRangeMobileContent : IExtensible
	{
		private ObjectiveGetInRangeContent _ObjectiveGetInRangeContent;

		private float _Range;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveGetInRangeContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveGetInRangeContent ObjectiveGetInRangeContent
		{
			get
			{
				return _ObjectiveGetInRangeContent;
			}
			set
			{
				_ObjectiveGetInRangeContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Range", DataFormat = DataFormat.FixedSize)]
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
