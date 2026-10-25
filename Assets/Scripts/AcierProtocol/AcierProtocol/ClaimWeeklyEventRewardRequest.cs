using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ClaimWeeklyEventRewardRequest")]
	public class ClaimWeeklyEventRewardRequest : IExtensible
	{
		private string _challengeId = "";

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "challengeId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string challengeId
		{
			get
			{
				return _challengeId;
			}
			set
			{
				_challengeId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
