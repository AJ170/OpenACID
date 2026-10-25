using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionAchievementEntryContent")]
	public class MissionAchievementEntryContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private string _Achievement = "";

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Achievement", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Achievement
		{
			get
			{
				return _Achievement;
			}
			set
			{
				_Achievement = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
