using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ClaimAchievementRequest")]
	public class ClaimAchievementRequest : IExtensible
	{
		private string _achievementId = "";

		private int _rankIdx;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "achievementId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string achievementId
		{
			get
			{
				return _achievementId;
			}
			set
			{
				_achievementId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "rankIdx", DataFormat = DataFormat.TwosComplement)]
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
