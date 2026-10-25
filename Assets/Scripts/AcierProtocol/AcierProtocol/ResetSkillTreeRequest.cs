using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ResetSkillTreeRequest")]
	public class ResetSkillTreeRequest : IExtensible
	{
		private string _assassinId;

		private AssassinSkillInTraining.SkillClass _skillClass = AssassinSkillInTraining.SkillClass.None_SkillClass;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "assassinId", DataFormat = DataFormat.Default)]
		public string assassinId
		{
			get
			{
				return _assassinId;
			}
			set
			{
				_assassinId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "skillClass", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(AssassinSkillInTraining.SkillClass.None_SkillClass)]
		public AssassinSkillInTraining.SkillClass skillClass
		{
			get
			{
				return _skillClass;
			}
			set
			{
				_skillClass = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
