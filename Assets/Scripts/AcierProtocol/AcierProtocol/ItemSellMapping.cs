using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ItemSellMapping")]
	public class ItemSellMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "QualityMapping")]
		public class QualityMapping : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "LevelMapping")]
			public class LevelMapping : IExtensible
			{
				private int _level;

				private long _amount;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int level
				{
					get
					{
						return _level;
					}
					set
					{
						_level = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "amount", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0L)]
				public long amount
				{
					get
					{
						return _amount;
					}
					set
					{
						_amount = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private LevelMapping[] _levelMappings = new LevelMapping[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(2, Name = "levelMappings", DataFormat = DataFormat.Default)]
			public LevelMapping[] levelMappings
			{
				get
				{
					return _levelMappings;
				}
				set
				{
					Array.Resize(ref _levelMappings, value.Length);
					value.CopyTo(_levelMappings, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private double _salePercentage;

		private QualityMapping[] _qualityMapping = new QualityMapping[0];

		private long _version;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "salePercentage", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double salePercentage
		{
			get
			{
				return _salePercentage;
			}
			set
			{
				_salePercentage = value;
			}
		}

		[ProtoMember(2, Name = "qualityMapping", DataFormat = DataFormat.Default)]
		public QualityMapping[] qualityMapping
		{
			get
			{
				return _qualityMapping;
			}
			set
			{
				Array.Resize(ref _qualityMapping, value.Length);
				value.CopyTo(_qualityMapping, 0);
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
