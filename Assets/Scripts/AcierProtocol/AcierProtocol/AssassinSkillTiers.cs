using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinSkillTiers")]
	public class AssassinSkillTiers : IExtensible
	{
		private AssassinSkillTier_[] _skillTiers = new AssassinSkillTier_[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "skillTiers", DataFormat = DataFormat.Default)]
		public AssassinSkillTier_[] skillTiers
		{
			get
			{
				return _skillTiers;
			}
			set
			{
				Array.Resize(ref _skillTiers, value.Length);
				value.CopyTo(_skillTiers, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
