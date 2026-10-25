using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public sealed class ObjectiveSpawnData : ObjectiveBaseData
	{
		[Serializable]
		public class SpawnFrame
		{
			public float Delay;

			public long DestinationSpot;

			public string Guid;

			public bool IsCounted = true;

			public bool IsKillingMandatory;

			public long[] SpawnPoints = new long[0];

			public SpawnFrame(ObjectiveSpawnContent.SpawnContent _spawnContent)
			{
				Delay = _spawnContent.Delay;
				DestinationSpot = _spawnContent.DestinationSpot;
				Guid = _spawnContent.Guid;
				IsCounted = _spawnContent.IsCounted;
				IsKillingMandatory = _spawnContent.IsKillingMandatory;
				SpawnPoints = _spawnContent.SpawnPoints;
			}

			public SpawnFrame()
			{
			}
		}

		public SpawnFrame[] SpawnFrames { get; set; }

		public ObjectiveSpawnData()
		{
			SpawnFrames = new SpawnFrame[0];
		}

		public ObjectiveSpawnData(ObjectiveSpawnContent _data)
			: base(_data.ObjectiveBaseContent)
		{
			SpawnFrames = new SpawnFrame[0];
			ObjectiveSpawnContent.SpawnContent[] spawnFrames = _data.SpawnFrames;
			foreach (ObjectiveSpawnContent.SpawnContent spawnContent in spawnFrames)
			{
				SpawnFrames = SpawnFrames.Add(new SpawnFrame(spawnContent));
			}
		}

		public override IExtensible SerializeProto()
		{
			List<ObjectiveSpawnContent.SpawnContent> list = new List<ObjectiveSpawnContent.SpawnContent>();
			SpawnFrame[] spawnFrames = SpawnFrames;
			foreach (SpawnFrame spawnFrame in spawnFrames)
			{
				ObjectiveSpawnContent.SpawnContent spawnContent = new ObjectiveSpawnContent.SpawnContent();
				spawnContent.Delay = spawnFrame.Delay;
				spawnContent.DestinationSpot = spawnFrame.DestinationSpot;
				spawnContent.Guid = spawnFrame.Guid;
				spawnContent.IsCounted = spawnFrame.IsCounted;
				spawnContent.IsKillingMandatory = spawnFrame.IsKillingMandatory;
				ObjectiveSpawnContent.SpawnContent spawnContent2 = spawnContent;
				if (spawnFrame.SpawnPoints != null)
				{
					spawnContent2.SpawnPoints = spawnContent2.SpawnPoints.AddRange(spawnFrame.SpawnPoints);
				}
				list.Add(spawnContent2);
			}
			ObjectiveSpawnContent objectiveSpawnContent = new ObjectiveSpawnContent();
			objectiveSpawnContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			ObjectiveSpawnContent objectiveSpawnContent2 = objectiveSpawnContent;
			objectiveSpawnContent2.SpawnFrames = objectiveSpawnContent2.SpawnFrames.AddRange(list);
			return objectiveSpawnContent2;
		}
	}
}
