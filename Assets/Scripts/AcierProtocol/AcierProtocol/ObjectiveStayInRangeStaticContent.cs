using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveStayInRangeStaticContent")]
	public class ObjectiveStayInRangeStaticContent : IExtensible
	{
		private ObjectiveStayInRangeContent _ObjectiveStayInRangeContent;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
