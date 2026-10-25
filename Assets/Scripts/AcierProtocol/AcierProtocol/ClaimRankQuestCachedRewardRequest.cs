using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ClaimRankQuestCachedRewardRequest")]
	public class ClaimRankQuestCachedRewardRequest : IExtensible
	{
		private int _rankIdx;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rankIdx
		{
			get
			{
				return _rankIdx;
			}
			set
			{
				_rankIdx = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
