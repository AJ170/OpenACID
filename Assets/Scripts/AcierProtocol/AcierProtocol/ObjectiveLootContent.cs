using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveLootContent")]
	public class ObjectiveLootContent : IExtensible
	{
		private ObjectiveTargetContent _ObjectiveTargetContent;

		private bool _IdentityActivatesMapIcon;

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

		[ProtoMember(1, IsRequired = false, Name = "IdentityActivatesMapIcon", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool IdentityActivatesMapIcon
		{
			get
			{
				return _IdentityActivatesMapIcon;
			}
			set
			{
				_IdentityActivatesMapIcon = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
