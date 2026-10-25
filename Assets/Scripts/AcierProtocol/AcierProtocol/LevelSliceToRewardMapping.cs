using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LevelSliceToRewardMapping")]
	public class LevelSliceToRewardMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RewardMappingSlice")]
		public class RewardMappingSlice : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "RewardMapping")]
			public class RewardMapping : IExtensible
			{
				private string _objective = "";

				private int _xp;

				private int _softCurrency;

				private int _heroTokens;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "objective", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string objective
				{
					get
					{
						return _objective;
					}
					set
					{
						_objective = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "xp", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int xp
				{
					get
					{
						return _xp;
					}
					set
					{
						_xp = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "softCurrency", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int softCurrency
				{
					get
					{
						return _softCurrency;
					}
					set
					{
						_softCurrency = value;
					}
				}

				[ProtoMember(4, IsRequired = false, Name = "heroTokens", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int heroTokens
				{
					get
					{
						return _heroTokens;
					}
					set
					{
						_heroTokens = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _slice;

			private RewardMapping[] _rewardMapping = new RewardMapping[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "slice", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int slice
			{
				get
				{
					return _slice;
				}
				set
				{
					_slice = value;
				}
			}

			[ProtoMember(2, Name = "rewardMapping", DataFormat = DataFormat.Default)]
			public RewardMapping[] rewardMapping
			{
				get
				{
					return _rewardMapping;
				}
				set
				{
					Array.Resize(ref _rewardMapping, value.Length);
					value.CopyTo(_rewardMapping, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "SliceDifferenceMapping")]
		public class SliceDifferenceMapping : IExtensible
		{
			private int _sliceDifference;

			private float _xpMultiplier;

			private float _softCurrencyMultiplier;

			private float _heroTokenMultiplier;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "sliceDifference", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int sliceDifference
			{
				get
				{
					return _sliceDifference;
				}
				set
				{
					_sliceDifference = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "xpMultiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float xpMultiplier
			{
				get
				{
					return _xpMultiplier;
				}
				set
				{
					_xpMultiplier = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "softCurrencyMultiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float softCurrencyMultiplier
			{
				get
				{
					return _softCurrencyMultiplier;
				}
				set
				{
					_softCurrencyMultiplier = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "heroTokenMultiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float heroTokenMultiplier
			{
				get
				{
					return _heroTokenMultiplier;
				}
				set
				{
					_heroTokenMultiplier = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private RewardMappingSlice[] _slices = new RewardMappingSlice[0];

		private SliceDifferenceMapping[] _sliceDifferenceMapping = new SliceDifferenceMapping[0];

		private long _version;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "slices", DataFormat = DataFormat.Default)]
		public RewardMappingSlice[] slices
		{
			get
			{
				return _slices;
			}
			set
			{
				Array.Resize(ref _slices, value.Length);
				value.CopyTo(_slices, 0);
			}
		}

		[ProtoMember(2, Name = "sliceDifferenceMapping", DataFormat = DataFormat.Default)]
		public SliceDifferenceMapping[] sliceDifferenceMapping
		{
			get
			{
				return _sliceDifferenceMapping;
			}
			set
			{
				Array.Resize(ref _sliceDifferenceMapping, value.Length);
				value.CopyTo(_sliceDifferenceMapping, 0);
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
