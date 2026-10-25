using System;
using AcierProtocol;
using Mission.Helpers;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class MissionTargetData : MissionBaseData
	{
		public float Range = 10f;

		public int TargetKind { get; set; }

		public MissionTargetData()
		{
		}

		public MissionTargetData(MissionTargetContent _missionTargetContent)
			: base(_missionTargetContent.MissionBaseContent)
		{
			Range = _missionTargetContent.Range;
			TargetKind = (int)_missionTargetContent.TargetKind.FromMask();
		}

		public override IExtensible SerializeProto()
		{
			MissionTargetContent missionTargetContent = new MissionTargetContent();
			missionTargetContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionTargetContent.Range = Range;
			MissionTargetContent missionTargetContent2 = missionTargetContent;
			ETemplateTargetKind[] array = ((ETemplateTargetKind)TargetKind).ToFlagList();
			foreach (ETemplateTargetKind item in array)
			{
				missionTargetContent2.TargetKind = missionTargetContent2.TargetKind.Add(item);
			}
			return missionTargetContent2;
		}
	}
}
