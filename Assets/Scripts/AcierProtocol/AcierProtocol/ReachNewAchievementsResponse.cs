using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ReachNewAchievementsResponse")]
	public class ReachNewAchievementsResponse : IExtensible
	{
		private Achievements _achievements;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "achievements", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Achievements achievements
		{
			get
			{
				return _achievements;
			}
			set
			{
				_achievements = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
