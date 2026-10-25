using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "PushNotificationTemplates")]
	public class PushNotificationTemplates : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Language")]
		public class Language : IExtensible
		{
			private string _lang = "";

			private string _title = "";

			private string _message = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "lang", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string lang
			{
				get
				{
					return _lang;
				}
				set
				{
					_lang = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "title", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string title
			{
				get
				{
					return _title;
				}
				set
				{
					_title = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "message", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string message
			{
				get
				{
					return _message;
				}
				set
				{
					_message = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "PushNotificationTemplate")]
		public class PushNotificationTemplate : IExtensible
		{
			private EPushNotificationType _type = EPushNotificationType.SkillTrainingFinished;

			private Language[] _languages = new Language[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EPushNotificationType.SkillTrainingFinished)]
			public EPushNotificationType type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(2, Name = "languages", DataFormat = DataFormat.Default)]
			public Language[] languages
			{
				get
				{
					return _languages;
				}
				set
				{
					Array.Resize(ref _languages, value.Length);
					value.CopyTo(_languages, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private PushNotificationTemplate[] _templates = new PushNotificationTemplate[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "templates", DataFormat = DataFormat.Default)]
		public PushNotificationTemplate[] templates
		{
			get
			{
				return _templates;
			}
			set
			{
				Array.Resize(ref _templates, value.Length);
				value.CopyTo(_templates, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
