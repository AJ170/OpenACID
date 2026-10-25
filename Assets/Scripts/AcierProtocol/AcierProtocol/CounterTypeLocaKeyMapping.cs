using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CounterTypeLocaKeyMapping")]
	public class CounterTypeLocaKeyMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "CounterTypeLocaKeyEntry")]
		public class CounterTypeLocaKeyEntry : IExtensible
		{
			private ECounterType _counter;

			private int _titleLocaKey;

			private int _descriptionLocaKey;

			private int[] _placeholderLocaKeys = new int[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "counter", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(2, IsRequired = false, Name = "titleLocaKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int titleLocaKey
			{
				get
				{
					return _titleLocaKey;
				}
				set
				{
					_titleLocaKey = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "descriptionLocaKey", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int descriptionLocaKey
			{
				get
				{
					return _descriptionLocaKey;
				}
				set
				{
					_descriptionLocaKey = value;
				}
			}

			[ProtoMember(4, Name = "placeholderLocaKeys", DataFormat = DataFormat.TwosComplement)]
			public int[] placeholderLocaKeys
			{
				get
				{
					return _placeholderLocaKeys;
				}
				set
				{
					Array.Resize(ref _placeholderLocaKeys, value.Length);
					value.CopyTo(_placeholderLocaKeys, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private CounterTypeLocaKeyEntry[] _counterTypeLocaKeyEntries = new CounterTypeLocaKeyEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "counterTypeLocaKeyEntries", DataFormat = DataFormat.Default)]
		public CounterTypeLocaKeyEntry[] counterTypeLocaKeyEntries
		{
			get
			{
				return _counterTypeLocaKeyEntries;
			}
			set
			{
				Array.Resize(ref _counterTypeLocaKeyEntries, value.Length);
				value.CopyTo(_counterTypeLocaKeyEntries, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
