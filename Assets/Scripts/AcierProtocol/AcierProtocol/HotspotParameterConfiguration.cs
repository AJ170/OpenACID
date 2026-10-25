using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HotspotParameterConfiguration")]
	public class HotspotParameterConfiguration : IExtensible
	{
		private int _budget;

		private EHotspotParameter _name = EHotspotParameter.WorldHotspotCountHaystack;

		private DifficultyConfigurationInteger[] _difficultyValues = new DifficultyConfigurationInteger[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "budget", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int budget
		{
			get
			{
				return _budget;
			}
			set
			{
				_budget = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EHotspotParameter.WorldHotspotCountHaystack)]
		public EHotspotParameter name
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

		[ProtoMember(3, Name = "difficultyValues", DataFormat = DataFormat.Default)]
		public DifficultyConfigurationInteger[] difficultyValues
		{
			get
			{
				return _difficultyValues;
			}
			set
			{
				Array.Resize(ref _difficultyValues, value.Length);
				value.CopyTo(_difficultyValues, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
