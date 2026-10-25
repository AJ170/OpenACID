using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinLevelToMissionBudget")]
	public class AssassinLevelToMissionBudget : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "LocationBudgetMapping")]
		public class LocationBudgetMapping : IExtensible
		{
			private LocationSet _location;

			private BudgetMappingSlice[] _slices = new BudgetMappingSlice[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "location", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LocationSet location
			{
				get
				{
					return _location;
				}
				set
				{
					_location = value;
				}
			}

			[ProtoMember(2, Name = "slices", DataFormat = DataFormat.Default)]
			public BudgetMappingSlice[] slices
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "BudgetMappingSlice")]
		public class BudgetMappingSlice : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "BudgetMapping")]
			public class BudgetMapping : IExtensible
			{
				private string _objective = "";

				private int[] _budgets = new int[0];

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

				[ProtoMember(2, Name = "budgets", DataFormat = DataFormat.TwosComplement)]
				public int[] budgets
				{
					get
					{
						return _budgets;
					}
					set
					{
						Array.Resize(ref _budgets, value.Length);
						value.CopyTo(_budgets, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _slice;

			private BudgetMapping[] _budgetMappings = new BudgetMapping[0];

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

			[ProtoMember(2, Name = "budgetMappings", DataFormat = DataFormat.Default)]
			public BudgetMapping[] budgetMappings
			{
				get
				{
					return _budgetMappings;
				}
				set
				{
					Array.Resize(ref _budgetMappings, value.Length);
					value.CopyTo(_budgetMappings, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private int _levelSliceWidth;

		private long _version;

		private LocationBudgetMapping[] _locations = new LocationBudgetMapping[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "levelSliceWidth", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int levelSliceWidth
		{
			get
			{
				return _levelSliceWidth;
			}
			set
			{
				_levelSliceWidth = value;
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

		[ProtoMember(4, Name = "locations", DataFormat = DataFormat.Default)]
		public LocationBudgetMapping[] locations
		{
			get
			{
				return _locations;
			}
			set
			{
				Array.Resize(ref _locations, value.Length);
				value.CopyTo(_locations, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
