using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HeroicRankMapping")]
	public class HeroicRankMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Entry")]
		public class Entry : IExtensible
		{
			private int _rankIdx;

			private int _randomMissionsScalingLevelLower;

			private int _randomMissionsScalingLevelUpper;

			private float _missionsScalingMultiplier;

			private string _entitlement = "";

			private double _multiplierXp;

			private double _multiplierSc;

			private double _multiplierHt;

			private int _requiredLockPicks;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int rankIdx
			{
				get
				{
					return _rankIdx;
				}
				set
				{
					_rankIdx = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "randomMissionsScalingLevelLower", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int randomMissionsScalingLevelLower
			{
				get
				{
					return _randomMissionsScalingLevelLower;
				}
				set
				{
					_randomMissionsScalingLevelLower = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "randomMissionsScalingLevelUpper", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int randomMissionsScalingLevelUpper
			{
				get
				{
					return _randomMissionsScalingLevelUpper;
				}
				set
				{
					_randomMissionsScalingLevelUpper = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "missionsScalingMultiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float missionsScalingMultiplier
			{
				get
				{
					return _missionsScalingMultiplier;
				}
				set
				{
					_missionsScalingMultiplier = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "entitlement", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string entitlement
			{
				get
				{
					return _entitlement;
				}
				set
				{
					_entitlement = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "multiplierXp", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double multiplierXp
			{
				get
				{
					return _multiplierXp;
				}
				set
				{
					_multiplierXp = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "multiplierSc", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double multiplierSc
			{
				get
				{
					return _multiplierSc;
				}
				set
				{
					_multiplierSc = value;
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "multiplierHt", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double multiplierHt
			{
				get
				{
					return _multiplierHt;
				}
				set
				{
					_multiplierHt = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "requiredLockPicks", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int requiredLockPicks
			{
				get
				{
					return _requiredLockPicks;
				}
				set
				{
					_requiredLockPicks = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Entry[] _entries = new Entry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
		public Entry[] entries
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
}
