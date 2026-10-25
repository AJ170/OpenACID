using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "DifficultyConfigurationFloat")]
	public class DifficultyConfigurationFloat : IExtensible
	{
		private ProbabilityValueFloat[] _probabilityValue = new ProbabilityValueFloat[0];

		private int _difficultyLevel;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "probabilityValue", DataFormat = DataFormat.Default)]
		public ProbabilityValueFloat[] probabilityValue
		{
			get
			{
				return _probabilityValue;
			}
			set
			{
				Array.Resize(ref _probabilityValue, value.Length);
				value.CopyTo(_probabilityValue, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "difficultyLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int difficultyLevel
		{
			get
			{
				return _difficultyLevel;
			}
			set
			{
				_difficultyLevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
