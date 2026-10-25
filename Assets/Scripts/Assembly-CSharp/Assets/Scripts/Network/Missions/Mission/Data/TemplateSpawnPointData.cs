using System;
using AcierProtocol;
using Mission.Helpers;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class TemplateSpawnPointData : MissionSpawnEntityData, IMissionTemplateData
	{
		public int MaxEntityCount = 4;

		public ESpawnPointKind SpawnPointKind { get; set; }

		public TemplateSpawnPointData()
		{
		}

		public TemplateSpawnPointData(TemplateSpawnPointContent _templateSpawnPointContent)
			: base(_templateSpawnPointContent.MissionSpawnEntityContent)
		{
			SpawnPointKind = _templateSpawnPointContent.SpawnPointKind.FromMask();
			MaxEntityCount = _templateSpawnPointContent.MaxEntityCount;
		}

		public override string ToString()
		{
			return string.Format("TemplateSpawnPoint {0}", SpawnPointKind);
		}

		public override IExtensible SerializeProto()
		{
			TemplateSpawnPointContent templateSpawnPointContent = new TemplateSpawnPointContent();
			templateSpawnPointContent.MaxEntityCount = MaxEntityCount;
			templateSpawnPointContent.MissionSpawnEntityContent = (MissionSpawnEntityContent)base.SerializeProto();
			TemplateSpawnPointContent templateSpawnPointContent2 = templateSpawnPointContent;
			templateSpawnPointContent2.SpawnPointKind = templateSpawnPointContent2.SpawnPointKind.AddRange(SpawnPointKind.ToFlagList());
			return templateSpawnPointContent2;
		}
	}
}
