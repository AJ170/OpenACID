using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UserSettings")]
	public class UserSettings : IExtensible
	{
		private PushNotificationSetting[] _pushNotificationSettings = new PushNotificationSetting[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "pushNotificationSettings", DataFormat = DataFormat.Default)]
		public PushNotificationSetting[] pushNotificationSettings
		{
			get
			{
				return _pushNotificationSettings;
			}
			set
			{
				Array.Resize(ref _pushNotificationSettings, value.Length);
				value.CopyTo(_pushNotificationSettings, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
