using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ForgeRerollDuringTutorialRequest")]
	public class ForgeRerollDuringTutorialRequest : IExtensible
	{
		private Resource _expectedCost;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "expectedCost", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource expectedCost
		{
			get
			{
				return _expectedCost;
			}
			set
			{
				_expectedCost = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
