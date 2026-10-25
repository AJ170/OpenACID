using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetMissionInfoResponse")]
	public class GetMissionInfoResponse : IExtensible
	{
		private MissionGoals _missionGoals;

		private MissionReward _missionRewardPreview;

		private IExtension extensionObject;

		[ProtoMember(18, IsRequired = false, Name = "missionGoals", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionGoals missionGoals
		{
			get
			{
				return _missionGoals;
			}
			set
			{
				_missionGoals = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "missionRewardPreview", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionReward missionRewardPreview
		{
			get
			{
				return _missionRewardPreview;
			}
			set
			{
				_missionRewardPreview = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
