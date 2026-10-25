using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ExclusiveContentMapping")]
	public class ExclusiveContentMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ExclusiveContentMappingEntry")]
		public class ExclusiveContentMappingEntry : IExtensible
		{
			private EExclusiveContentType _type;

			private EMailTemplateType _mailTemplateType = EMailTemplateType.WelcomeMail;

			private string _entitlementGuidStr = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EExclusiveContentType.ExclusiveContent_AppleWinterOutfit)]
			public EExclusiveContentType type
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

			[ProtoMember(2, IsRequired = false, Name = "mailTemplateType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMailTemplateType.WelcomeMail)]
			public EMailTemplateType mailTemplateType
			{
				get
				{
					return _mailTemplateType;
				}
				set
				{
					_mailTemplateType = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "entitlementGuidStr", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string entitlementGuidStr
			{
				get
				{
					return _entitlementGuidStr;
				}
				set
				{
					_entitlementGuidStr = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ExclusiveContentMappingEntry[] _exclusiveContentEntries = new ExclusiveContentMappingEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "exclusiveContentEntries", DataFormat = DataFormat.Default)]
		public ExclusiveContentMappingEntry[] exclusiveContentEntries
		{
			get
			{
				return _exclusiveContentEntries;
			}
			set
			{
				Array.Resize(ref _exclusiveContentEntries, value.Length);
				value.CopyTo(_exclusiveContentEntries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
