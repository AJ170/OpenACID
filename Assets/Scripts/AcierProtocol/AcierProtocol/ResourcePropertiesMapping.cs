using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ResourcePropertiesMapping")]
	public class ResourcePropertiesMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ResourcePropertiesEntry")]
		public class ResourcePropertiesEntry : IExtensible
		{
			private EResourceType _resourceType = EResourceType.HardCurrency;

			private long _resourceCap;

			private bool _spendable;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "resourceType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EResourceType.HardCurrency)]
			public EResourceType resourceType
			{
				get
				{
					return _resourceType;
				}
				set
				{
					_resourceType = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "resourceCap", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long resourceCap
			{
				get
				{
					return _resourceCap;
				}
				set
				{
					_resourceCap = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "spendable", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool spendable
			{
				get
				{
					return _spendable;
				}
				set
				{
					_spendable = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ResourcePropertiesEntry[] _resourceProperties = new ResourcePropertiesEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "resourceProperties", DataFormat = DataFormat.Default)]
		public ResourcePropertiesEntry[] resourceProperties
		{
			get
			{
				return _resourceProperties;
			}
			set
			{
				Array.Resize(ref _resourceProperties, value.Length);
				value.CopyTo(_resourceProperties, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
