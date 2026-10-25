using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SetTutorialFinishedRequest")]
	public class SetTutorialFinishedRequest : IExtensible
	{
		private ETutorialCategory _category;

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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
