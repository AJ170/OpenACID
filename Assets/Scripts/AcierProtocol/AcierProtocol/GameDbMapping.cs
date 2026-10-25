using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GameDbMapping")]
	public class GameDbMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "SubDb")]
		public class SubDb : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "DbEntry")]
			public class DbEntry : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "KeyValuePair")]
				public class KeyValuePair : IExtensible
				{
					private string _name = "";

					private float _floatValue;

					private string _stringValue = "";

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string name
					{
						get
						{
							return _name;
						}
						set
						{
							_name = value;
						}
					}

					[ProtoMember(2, IsRequired = false, Name = "floatValue", DataFormat = DataFormat.FixedSize)]
					[DefaultValue(0f)]
					public float floatValue
					{
						get
						{
							return _floatValue;
						}
						set
						{
							_floatValue = value;
						}
					}

					[ProtoMember(3, IsRequired = false, Name = "stringValue", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string stringValue
					{
						get
						{
							return _stringValue;
						}
						set
						{
							_stringValue = value;
						}
					}

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private string _name = "";

				private string _gameIdGuid = "";

				private int _gameIdGuidHash;

				private KeyValuePair[] _properties = new KeyValuePair[0];

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string name
				{
					get
					{
						return _name;
					}
					set
					{
						_name = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "gameIdGuid", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string gameIdGuid
				{
					get
					{
						return _gameIdGuid;
					}
					set
					{
						_gameIdGuid = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "gameIdGuidHash", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int gameIdGuidHash
				{
					get
					{
						return _gameIdGuidHash;
					}
					set
					{
						_gameIdGuidHash = value;
					}
				}

				[ProtoMember(4, Name = "properties", DataFormat = DataFormat.Default)]
				public KeyValuePair[] properties
				{
					get
					{
						return _properties;
					}
					set
					{
						Array.Resize(ref _properties, value.Length);
						value.CopyTo(_properties, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private string _name = "";

			private DbEntry[] _entries = new DbEntry[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string name
			{
				get
				{
					return _name;
				}
				set
				{
					_name = value;
				}
			}

			[ProtoMember(2, Name = "entries", DataFormat = DataFormat.Default)]
			public DbEntry[] entries
			{
				get
				{
					return _entries;
				}
				set
				{
					Array.Resize(ref _entries, value.Length);
					value.CopyTo(_entries, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private SubDb[] _subDbs = new SubDb[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "subDbs", DataFormat = DataFormat.Default)]
		public SubDb[] subDbs
		{
			get
			{
				return _subDbs;
			}
			set
			{
				Array.Resize(ref _subDbs, value.Length);
				value.CopyTo(_subDbs, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
