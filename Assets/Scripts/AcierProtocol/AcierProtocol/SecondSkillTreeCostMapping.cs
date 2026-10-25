using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SecondSkillTreeCostMapping")]
	public class SecondSkillTreeCostMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "SecondSkillTreeCostEntry")]
		public class SecondSkillTreeCostEntry : IExtensible
		{
			private int _unlockNumber;

			private Resources _costOptions;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "unlockNumber", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int unlockNumber
			{
				get
				{
					return _unlockNumber;
				}
				set
				{
					_unlockNumber = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "costOptions", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resources costOptions
			{
				get
				{
					return _costOptions;
				}
				set
				{
					_costOptions = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private SecondSkillTreeCostEntry[] _secondSkillTreeUnlockCosts = new SecondSkillTreeCostEntry[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "secondSkillTreeUnlockCosts", DataFormat = DataFormat.Default)]
		public SecondSkillTreeCostEntry[] secondSkillTreeUnlockCosts
		{
			get
			{
				return _secondSkillTreeUnlockCosts;
			}
			set
			{
				Array.Resize(ref _secondSkillTreeUnlockCosts, value.Length);
				value.CopyTo(_secondSkillTreeUnlockCosts, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
