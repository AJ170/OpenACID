using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ShopTable")]
	public class ShopTable : IExtensible
	{
		private ulong _version;

		private ShopBundle[] _bundles = new ShopBundle[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0f)]
		public ulong version
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

		[ProtoMember(2, Name = "bundles", DataFormat = DataFormat.Default)]
		public ShopBundle[] bundles
		{
			get
			{
				return _bundles;
			}
			set
			{
				Array.Resize(ref _bundles, value.Length);
				value.CopyTo(_bundles, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
