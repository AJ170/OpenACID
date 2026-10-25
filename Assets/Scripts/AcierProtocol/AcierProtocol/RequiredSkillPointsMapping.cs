using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "RequiredSkillPointsMapping")]
	public class RequiredSkillPointsMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Class")]
		public class Class : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Tier")]
			public class Tier : IExtensible
			{
				private int _idx;

				private int _unlockTier;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "idx", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int idx
				{
					get
					{
						return _idx;
					}
					set
					{
						_idx = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "unlockTier", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int unlockTier
				{
					get
					{
						return _unlockTier;
					}
					set
					{
						_unlockTier = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _unlockClass;

			private Tier[] _tiers = new Tier[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "unlockClass", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int unlockClass
			{
				get
				{
					return _unlockClass;
				}
				set
				{
					_unlockClass = value;
				}
			}

			[ProtoMember(2, Name = "tiers", DataFormat = DataFormat.Default)]
			public Tier[] tiers
			{
				get
				{
					return _tiers;
				}
				set
				{
					Array.Resize(ref _tiers, value.Length);
					value.CopyTo(_tiers, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Class _primary;

		private Class _secondary;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "primary", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Class primary
		{
			get
			{
				return _primary;
			}
			set
			{
				_primary = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "secondary", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Class secondary
		{
			get
			{
				return _secondary;
			}
			set
			{
				_secondary = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
