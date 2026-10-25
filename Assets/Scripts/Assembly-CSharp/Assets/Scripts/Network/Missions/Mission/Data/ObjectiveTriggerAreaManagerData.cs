using System;
using System.Collections.Generic;
using AcierProtocol;
using ProtoBuf;

namespace Assets.Scripts.Network.Missions.Mission.Data
{
	[Serializable]
	public class ObjectiveTriggerAreaManagerData : ObjectiveBaseData
	{
		public class TriggerAreaSetting
		{
			public EEffectSpawnerSoundType SoundTypeIn;

			public EEffectSpawnerSoundType SoundTypeOut;

			public int ID;
		}

		public List<TriggerAreaSetting> Targets { get; set; }

		public ObjectiveTriggerAreaManagerData()
		{
		}

		public ObjectiveTriggerAreaManagerData(ObjectiveTriggerAreaContent _missionSpawnEffectContent)
			: base(_missionSpawnEffectContent.ObjectiveBaseContent)
		{
			Targets = Convert(_missionSpawnEffectContent.Targets);
		}

		public override IExtensible SerializeProto()
		{
			ObjectiveTriggerAreaContent objectiveTriggerAreaContent = new ObjectiveTriggerAreaContent();
			objectiveTriggerAreaContent.ObjectiveBaseContent = (ObjectiveBaseContent)base.SerializeProto();
			objectiveTriggerAreaContent.Targets = Convert(Targets);
			return objectiveTriggerAreaContent;
		}

		private List<TriggerAreaSetting> Convert(ObjectiveTriggerAreaContent.TriggerAreaSetting[] _uniqueIds)
		{
			List<TriggerAreaSetting> list = new List<TriggerAreaSetting>();
			foreach (ObjectiveTriggerAreaContent.TriggerAreaSetting triggerAreaSetting in _uniqueIds)
			{
				list.Add(new TriggerAreaSetting
				{
					ID = triggerAreaSetting.ID,
					SoundTypeIn = triggerAreaSetting.SoundTypeIn,
					SoundTypeOut = triggerAreaSetting.SoundTypeOut
				});
			}
			return list;
		}

		private ObjectiveTriggerAreaContent.TriggerAreaSetting[] Convert(List<TriggerAreaSetting> _uniqueIds)
		{
			ObjectiveTriggerAreaContent.TriggerAreaSetting[] array = new ObjectiveTriggerAreaContent.TriggerAreaSetting[_uniqueIds.Count];
			for (int i = 0; i < _uniqueIds.Count; i++)
			{
				TriggerAreaSetting triggerAreaSetting = _uniqueIds[i];
				array[i] = new ObjectiveTriggerAreaContent.TriggerAreaSetting
				{
					ID = triggerAreaSetting.ID,
					SoundTypeIn = triggerAreaSetting.SoundTypeIn,
					SoundTypeOut = triggerAreaSetting.SoundTypeOut
				};
			}
			return array;
		}
	}
}
