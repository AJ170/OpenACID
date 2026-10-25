using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SetTutorialStepRequest")]
	public class SetTutorialStepRequest : IExtensible
	{
		private ETutorialCategory _category;

		private int _counter;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ETutorialCategory.TutCatIntroMission)]
		public ETutorialCategory category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "counter", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int counter
		{
			get
			{
				return _counter;
			}
			set
			{
				_counter = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
