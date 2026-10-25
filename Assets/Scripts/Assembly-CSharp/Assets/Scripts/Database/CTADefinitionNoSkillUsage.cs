using System.Collections.ObjectModel;
using AssetBundleData;
using SharedBaseLib.Serialization;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Database
{
	public class CTADefinitionNoSkillUsage : CTADefinitionBase
	{
		public int MissionsSkillsNeverUsed { get; protected set; }

		public int MissionsSkillsUsedBefore { get; protected set; }

		public CTADefinitionNoSkillUsage(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			MissionsSkillsNeverUsed = _s.SerializeInt("MissionsSkillsNeverUsed", MissionsSkillsNeverUsed);
			MissionsSkillsUsedBefore = _s.SerializeInt("MissionsSkillsUsedBefore", MissionsSkillsUsedBefore);
		}

		protected override EResult CheckTrigger()
		{
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			int num = ((instance.CTACounterGet(LocalStorage.CTACounterUsedSkillsOutOfTutorial) > 0) ? MissionsSkillsUsedBefore : MissionsSkillsNeverUsed);
			ReadOnlyCollection<LocalStorage.ICTAMissionData> readOnlyCollection = UbiCore.Utils.Singleton<LocalStorage>.Instance.CTAMissionDataGet();
			if (readOnlyCollection.Count < num || UsedSkills(readOnlyCollection, num))
			{
				return EResult.Proceed;
			}
			return EResult.Fullfilled;
		}

		private static bool UsedSkills(ReadOnlyCollection<LocalStorage.ICTAMissionData> _missionData, int _totalMissions)
		{
			for (int i = Mathf.Max(_missionData.Count - _totalMissions, 0); i < _missionData.Count; i++)
			{
				if (_missionData[i].UsedSkill)
				{
					return true;
				}
			}
			return false;
		}
	}
}
