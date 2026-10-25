using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveGetInRangeStaticContent")]
	public class ObjectiveGetInRangeStaticContent : IExtensible
	{
		private ObjectiveGetInRangeContent _ObjectiveGetInRangeContent;

		private string _TypeGuid = "";

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

		[ProtoMember(9, IsRequired = false, Name = "TypeGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string TypeGuid
		{
			get
			{
				return _TypeGuid;
			}
			set
			{
				_TypeGuid = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
