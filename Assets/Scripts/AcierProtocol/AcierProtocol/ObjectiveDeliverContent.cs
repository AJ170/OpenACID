using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveDeliverContent")]
	public class ObjectiveDeliverContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
