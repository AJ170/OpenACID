using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ProbabilityValueETemplateTargetKind")]
	public class ProbabilityValueETemplateTargetKind : IExtensible
	{
		private float _Probability;

		private ETemplateTargetKind _Value = ETemplateTargetKind.Escape_ETemplateTargetKind;

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
		[DefaultValue(ETemplateTargetKind.Escape_ETemplateTargetKind)]
		public ETemplateTargetKind Value
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
