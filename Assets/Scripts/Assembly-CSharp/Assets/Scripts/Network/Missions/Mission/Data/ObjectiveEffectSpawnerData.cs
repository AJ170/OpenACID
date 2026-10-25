using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class ObjectiveEffectSpawnerData : ObjectiveBaseData
	{
		public class ObjectDelay
		{
			public int DelayMs;

			public EEffectSpawnerSoundType SoundType;

			public int ID;
		}

		public List<ObjectDelay> UniqueIds { get; set; }

		public List<int> HideObjectIds { get; set; }

		public ObjectiveEffectSpawnerData()
		{
		}

		public ObjectiveEffectSpawnerData(ObjectiveEffectSpawnerContent _missionSpawnEffectContent)
			: base(_missionSpawnEffectContent.ObjectiveBaseContent)
		{
			UniqueIds = Convert(_missionSpawnEffectContent.UniqueIds);
			HideObjectIds = new List<int>(_missionSpawnEffectContent.HideObjectIds);
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveEffectSpawnerContent objectiveEffectSpawnerContent = new ObjectiveEffectSpawnerContent();
			objectiveEffectSpawnerContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveEffectSpawnerContent.UniqueIds = Convert(UniqueIds);
			objectiveEffectSpawnerContent.HideObjectIds = HideObjectIds.ToArray();
			return objectiveEffectSpawnerContent;
		}

		private List<ObjectDelay> Convert(ObjectiveEffectSpawnerContent.ObjectDelay[] _uniqueIds)
		{
			List<ObjectDelay> list = new List<ObjectDelay>();
			foreach (ObjectiveEffectSpawnerContent.ObjectDelay objectDelay in _uniqueIds)
			{
				list.Add(new ObjectDelay
				{
					DelayMs = objectDelay.DelayMs,
					ID = objectDelay.ID,
					SoundType = objectDelay.SoundType
				});
			}
			return list;
		}

		private ObjectiveEffectSpawnerContent.ObjectDelay[] Convert(List<ObjectDelay> _uniqueIds)
		{
			ObjectiveEffectSpawnerContent.ObjectDelay[] array = new ObjectiveEffectSpawnerContent.ObjectDelay[_uniqueIds.Count];
			for (int i = 0; i < _uniqueIds.Count; i++)
			{
				ObjectDelay objectDelay = _uniqueIds[i];
				array[i] = new ObjectiveEffectSpawnerContent.ObjectDelay
				{
					DelayMs = objectDelay.DelayMs,
					ID = objectDelay.ID,
					SoundType = objectDelay.SoundType
				};
			}
			return array;
		}
	}
}
