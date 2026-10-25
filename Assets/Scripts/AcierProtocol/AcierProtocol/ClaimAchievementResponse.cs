using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ClaimAchievementResponse")]
	public class ClaimAchievementResponse : IExtensible
	{
		private Achievement _claimedAchievement;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "claimedAchievement", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Achievement claimedAchievement
		{
			get
			{
				return _claimedAchievement;
			}
			set
			{
				_claimedAchievement = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
