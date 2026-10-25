using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ReadAchievementResponse")]
	public class ReadAchievementResponse : IExtensible
	{
		private Achievement _readAchievement;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "readAchievement", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Achievement readAchievement
		{
			get
			{
				return _readAchievement;
			}
			set
			{
				_readAchievement = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
