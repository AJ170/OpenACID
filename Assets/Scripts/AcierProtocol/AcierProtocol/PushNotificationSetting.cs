using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PushNotificationSetting")]
	public class PushNotificationSetting : IExtensible
	{
		private EPushNotificationGroup _pushNotificationGroup = EPushNotificationGroup.SkillTrainingGroup;

		private bool _value;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "pushNotificationGroup", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EPushNotificationGroup.SkillTrainingGroup)]
		public EPushNotificationGroup pushNotificationGroup
		{
			get
			{
				return _pushNotificationGroup;
			}
			set
			{
				_pushNotificationGroup = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
