using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailTemplates")]
	public class MailTemplates : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MailTemplate")]
		public class MailTemplate : IExtensible
		{
			private EMailTemplateType _type = EMailTemplateType.WelcomeMail;

			private LangKey _subject;

			private LangKey _body;

			private LootTemplate[] _templates = new LootTemplate[0];

			private bool _fullInventory;

			private EMailPriority _priority;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMailTemplateType.WelcomeMail)]
			public EMailTemplateType type
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

			[ProtoMember(2, IsRequired = false, Name = "subject", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LangKey subject
			{
				get
				{
					return _subject;
				}
				set
				{
					_subject = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "body", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LangKey body
			{
				get
				{
					return _body;
				}
				set
				{
					_body = value;
				}
			}

			[ProtoMember(4, Name = "templates", DataFormat = DataFormat.Default)]
			public LootTemplate[] templates
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

			[ProtoMember(5, IsRequired = false, Name = "fullInventory", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool fullInventory
			{
				get
				{
					return _fullInventory;
				}
				set
				{
					_fullInventory = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "priority", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMailPriority.MailPriorityLow)]
			public EMailPriority priority
			{
				get
				{
					return _priority;
				}
				set
				{
					_priority = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private MailTemplate[] _templates = new MailTemplate[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "templates", DataFormat = DataFormat.Default)]
		public MailTemplate[] templates
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
