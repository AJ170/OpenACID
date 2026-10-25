using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EventsUserFriendlyMapping")]
	public class EventsUserFriendlyMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ClientThemeAssetMappingEntry")]
		public class ClientThemeAssetMappingEntry : IExtensible
		{
			private string _themeLabel = "";

			private string _screenImageGuiId = "";

			private string _bannerImageGuiId = "";

			private string _notificationImageGuId = "";

			private string _bannerBackgroundLargeGuId = "";

			private string _bannerBackgroundSmallGuId = "";

			private string _bannerFontColor1GuId = "";

			private string _bannerFontColor2GuId = "";

			private string _bannerFontColor3GuId = "";

			private string _bannerFontColor4GuId = "";

			private string _bannerTintingColorGuId = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "themeLabel", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string themeLabel
			{
				get
				{
					return _themeLabel;
				}
				set
				{
					_themeLabel = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "screenImageGuiId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string screenImageGuiId
			{
				get
				{
					return _screenImageGuiId;
				}
				set
				{
					_screenImageGuiId = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "bannerImageGuiId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerImageGuiId
			{
				get
				{
					return _bannerImageGuiId;
				}
				set
				{
					_bannerImageGuiId = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "notificationImageGuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string notificationImageGuId
			{
				get
				{
					return _notificationImageGuId;
				}
				set
				{
					_notificationImageGuId = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "bannerBackgroundLargeGuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerBackgroundLargeGuId
			{
				get
				{
					return _bannerBackgroundLargeGuId;
				}
				set
				{
					_bannerBackgroundLargeGuId = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "bannerBackgroundSmallGuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerBackgroundSmallGuId
			{
				get
				{
					return _bannerBackgroundSmallGuId;
				}
				set
				{
					_bannerBackgroundSmallGuId = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "bannerFontColor1GuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerFontColor1GuId
			{
				get
				{
					return _bannerFontColor1GuId;
				}
				set
				{
					_bannerFontColor1GuId = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "bannerFontColor2GuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerFontColor2GuId
			{
				get
				{
					return _bannerFontColor2GuId;
				}
				set
				{
					_bannerFontColor2GuId = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "bannerFontColor3GuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerFontColor3GuId
			{
				get
				{
					return _bannerFontColor3GuId;
				}
				set
				{
					_bannerFontColor3GuId = value;
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "bannerFontColor4GuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerFontColor4GuId
			{
				get
				{
					return _bannerFontColor4GuId;
				}
				set
				{
					_bannerFontColor4GuId = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "bannerTintingColorGuId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string bannerTintingColorGuId
			{
				get
				{
					return _bannerTintingColorGuId;
				}
				set
				{
					_bannerTintingColorGuId = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "DisplayMessageMappingEntry")]
		public class DisplayMessageMappingEntry : IExtensible
		{
			private string _messageLabel = "";

			private int _locaKey;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "messageLabel", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string messageLabel
			{
				get
				{
					return _messageLabel;
				}
				set
				{
					_messageLabel = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "locaKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int locaKey
			{
				get
				{
					return _locaKey;
				}
				set
				{
					_locaKey = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "LootTemplateNameMappingEntry")]
		public class LootTemplateNameMappingEntry : IExtensible
		{
			private string _lootTemplateLabel = "";

			private string _lootTemplateName = "";

			private int _sortingIdx;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "lootTemplateLabel", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string lootTemplateLabel
			{
				get
				{
					return _lootTemplateLabel;
				}
				set
				{
					_lootTemplateLabel = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "lootTemplateName", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string lootTemplateName
			{
				get
				{
					return _lootTemplateName;
				}
				set
				{
					_lootTemplateName = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "sortingIdx", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int sortingIdx
			{
				get
				{
					return _sortingIdx;
				}
				set
				{
					_sortingIdx = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "CounterMappingEntry")]
		public class CounterMappingEntry : IExtensible
		{
			private string _counterLabel = "";

			private ECounterType _counter;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "counterLabel", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string counterLabel
			{
				get
				{
					return _counterLabel;
				}
				set
				{
					_counterLabel = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "counter", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ECounterType.InvalidCounter)]
			public ECounterType counter
			{
				get
				{
					return _counter;
				}
				set
				{
					_counter = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ClientThemeAssetMappingEntry[] _clientThemeAssetMapping = new ClientThemeAssetMappingEntry[0];

		private DisplayMessageMappingEntry[] _displayMessageMapping = new DisplayMessageMappingEntry[0];

		private DisplayMessageMappingEntry[] _displaySubMessageMapping = new DisplayMessageMappingEntry[0];

		private LootTemplateNameMappingEntry[] _lootTemplateNameMapping = new LootTemplateNameMappingEntry[0];

		private CounterMappingEntry[] _counterNameMapping = new CounterMappingEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "clientThemeAssetMapping", DataFormat = DataFormat.Default)]
		public ClientThemeAssetMappingEntry[] clientThemeAssetMapping
		{
			get
			{
				return _clientThemeAssetMapping;
			}
			set
			{
				Array.Resize(ref _clientThemeAssetMapping, value.Length);
				value.CopyTo(_clientThemeAssetMapping, 0);
			}
		}

		[ProtoMember(2, Name = "displayMessageMapping", DataFormat = DataFormat.Default)]
		public DisplayMessageMappingEntry[] displayMessageMapping
		{
			get
			{
				return _displayMessageMapping;
			}
			set
			{
				Array.Resize(ref _displayMessageMapping, value.Length);
				value.CopyTo(_displayMessageMapping, 0);
			}
		}

		[ProtoMember(3, Name = "displaySubMessageMapping", DataFormat = DataFormat.Default)]
		public DisplayMessageMappingEntry[] displaySubMessageMapping
		{
			get
			{
				return _displaySubMessageMapping;
			}
			set
			{
				Array.Resize(ref _displaySubMessageMapping, value.Length);
				value.CopyTo(_displaySubMessageMapping, 0);
			}
		}

		[ProtoMember(4, Name = "lootTemplateNameMapping", DataFormat = DataFormat.Default)]
		public LootTemplateNameMappingEntry[] lootTemplateNameMapping
		{
			get
			{
				return _lootTemplateNameMapping;
			}
			set
			{
				Array.Resize(ref _lootTemplateNameMapping, value.Length);
				value.CopyTo(_lootTemplateNameMapping, 0);
			}
		}

		[ProtoMember(5, Name = "counterNameMapping", DataFormat = DataFormat.Default)]
		public CounterMappingEntry[] counterNameMapping
		{
			get
			{
				return _counterNameMapping;
			}
			set
			{
				Array.Resize(ref _counterNameMapping, value.Length);
				value.CopyTo(_counterNameMapping, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
