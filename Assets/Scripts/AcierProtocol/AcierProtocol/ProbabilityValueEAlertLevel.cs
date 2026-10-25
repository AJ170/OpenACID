using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProbabilityValueEAlertLevel")]
	public class ProbabilityValueEAlertLevel : IExtensible
	{
		private float _Probability;

		private EAlertLevel _Value;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "Probability", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Probability
		{
			get
			{
				return _Probability;
			}
			set
			{
				_Probability = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "Value", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EAlertLevel.Anonymous)]
		public EAlertLevel Value
		{
			get
			{
				return _Value;
			}
			set
			{
				_Value = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
