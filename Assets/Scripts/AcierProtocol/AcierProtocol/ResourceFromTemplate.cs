using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ResourceFromTemplate")]
	public class ResourceFromTemplate : IExtensible
	{
		private Resource _resource;

		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private LangKey _nameLangKey;

		private LangKey _descriptionLangKey;

		private string _debugRollCode = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "resource", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource resource
		{
			get
			{
				return _resource;
			}
			set
			{
				_resource = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Quality.EQuality.Mundane)]
		public Quality.EQuality quality
		{
			get
			{
				return _quality;
			}
			set
			{
				_quality = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "nameLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey nameLangKey
		{
			get
			{
				return _nameLangKey;
			}
			set
			{
				_nameLangKey = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "descriptionLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey descriptionLangKey
		{
			get
			{
				return _descriptionLangKey;
			}
			set
			{
				_descriptionLangKey = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "debugRollCode", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string debugRollCode
		{
			get
			{
				return _debugRollCode;
			}
			set
			{
				_debugRollCode = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
