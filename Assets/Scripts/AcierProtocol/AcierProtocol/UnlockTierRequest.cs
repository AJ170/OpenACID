using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UnlockTierRequest")]
	public class UnlockTierRequest : IExtensible
	{
		private string _assassinId = "";

		private int _tierId;

		private AssassinSkillInTraining.SkillClass _skillClass = AssassinSkillInTraining.SkillClass.None_SkillClass;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
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

		[ProtoMember(2, IsRequired = false, Name = "tierId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int tierId
		{
			get
			{
				return _tierId;
			}
			set
			{
				_tierId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "skillClass", DataFormat = DataFormat.TwosComplement)]
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
