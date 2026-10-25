using System;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class MissionTextAnnotationData : MissionBaseData
	{
		public int TextId = -1;

		public int HeaderTextId = -1;

		public bool WaitForConfirmation;

		public EMissionTextAnnotationType Type;

		public string Texture;

		public MissionTextAnnotationData()
		{
		}

		public MissionTextAnnotationData(MissionTextAnnotationContent _missionTextAnnotationContent)
			: base(_missionTextAnnotationContent.MissionBaseContent)
		{
			TextId = _missionTextAnnotationContent.TextId;
			WaitForConfirmation = _missionTextAnnotationContent.WaitForConfirmation;
			Type = _missionTextAnnotationContent.Type;
			HeaderTextId = _missionTextAnnotationContent.HeaderTextId;
			Texture = _missionTextAnnotationContent.Texture;
		}

		public override IExtensible SerializeProto()
		{
			MissionTextAnnotationContent missionTextAnnotationContent = new MissionTextAnnotationContent();
			missionTextAnnotationContent.MissionBaseContent = (MissionBaseContent)base.SerializeProto();
			missionTextAnnotationContent.TextId = TextId;
			missionTextAnnotationContent.HeaderTextId = HeaderTextId;
			missionTextAnnotationContent.WaitForConfirmation = WaitForConfirmation;
			missionTextAnnotationContent.Type = Type;
			missionTextAnnotationContent.Texture = Texture;
			return missionTextAnnotationContent;
		}
	}
}
