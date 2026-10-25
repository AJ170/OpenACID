using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EndMissionResponse")]
	public class EndMissionResponse : IExtensible
	{
		private MissionReward _missionReward;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionReward missionReward
		{
			get
			{
				return _missionReward;
			}
			set
			{
				_missionReward = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
