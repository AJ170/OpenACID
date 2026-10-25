using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Mission
{
	public class MissionTracker
	{
		public class ObjectiveData
		{
			public long PrimaryObjectiveID;

			public readonly BBList<IObjectiveBase> ObjectivesBaseData = new BBList<IObjectiveBase>();

			public List<IObjectiveBase> GetVisibleObjectives()
			{
				List<IObjectiveBase> list = new List<IObjectiveBase>();
				for (int i = 0; i < ObjectivesBaseData.Count; i++)
				{
					IObjectiveBase objectiveBase = ObjectivesBaseData[i];
					if (!objectiveBase.ObjectiveData.HideDescription)
					{
						list.Add(objectiveBase);
					}
				}
				return list;
			}

			public int ComputeModifierHash()
			{
				string text = string.Empty;
				int num = (string.IsNullOrEmpty(ObjectivesBaseData[0].SecondaryDescription) ? 1 : 0);
				for (int i = num; i < ObjectivesBaseData.Count; i++)
				{
					text += ObjectivesBaseData[i].MissionData.IDStr;
				}
				return text.GetHashCode();
			}
		}

		public delegate void MissionTrackerChanged();

		public readonly BBList<ObjectiveData> CollectedObjectiveData = new BBList<ObjectiveData>();

		public bool IsActive { get; private set; }

		public event MissionTrackerChanged OnMissionTrackerChanged = delegate
		{
		};

		public void OnLoaded()
		{
			if (LogicMain.LogicInterface.MissionType == EMissionType.Mission_EMissionType)
			{
				IsActive = true;
				UpdateObjectives();
			}
		}

		public void OnUnload()
		{
			if (IsActive)
			{
				IsActive = false;
				CollectedObjectiveData.ClearIfNotEmpty();
			}
		}

		public ObjectiveData GetLastPhaseObjectiveData()
		{
			if (CollectedObjectiveData.Count > 0)
			{
				int index = CollectedObjectiveData.Count - 1;
				return CollectedObjectiveData[index];
			}
			return null;
		}

		public ObjectiveData GetObjectiveData(long _primaryObjectiveID)
		{
			for (int i = 0; i < CollectedObjectiveData.Count; i++)
			{
				ObjectiveData objectiveData = CollectedObjectiveData[i];
				if (objectiveData.PrimaryObjectiveID == _primaryObjectiveID)
				{
					return objectiveData;
				}
			}
			return null;
		}

		public void UpdateObjectives()
		{
			bool flag = false;
			IObjectiveBase objectiveBase = null;
			BBList<IObjectiveBase> missionObjectives = UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.MissionObjectives;
			if (missionObjectives.Count == 0)
			{
				return;
			}
			for (int i = 0; i < missionObjectives.Count; i++)
			{
				IObjectiveBase objectiveBase2 = missionObjectives[i];
				if (objectiveBase2.ObjectiveData.IsPrimaryObjective)
				{
					objectiveBase = objectiveBase2;
					break;
				}
			}
			if (objectiveBase == null)
			{
				return;
			}
			int num = CollectedObjectiveData.Count - 1;
			if (num < 0 || CollectedObjectiveData[num].PrimaryObjectiveID != objectiveBase.ObjectiveData.ID)
			{
				ObjectiveData objectiveData = new ObjectiveData();
				objectiveData.PrimaryObjectiveID = objectiveBase.ObjectiveData.ID;
				ObjectiveData objectiveData2 = objectiveData;
				objectiveData2.ObjectivesBaseData.Add(objectiveBase);
				CollectedObjectiveData.Add(objectiveData2);
				flag = true;
			}
			num = CollectedObjectiveData.Count - 1;
			List<IObjectiveBase> objectivesBaseData = CollectedObjectiveData[num].ObjectivesBaseData;
			int count = objectivesBaseData.Count;
			for (int j = 0; j < missionObjectives.Count; j++)
			{
				IObjectiveBase objectiveBase3 = missionObjectives[j];
				ObjectiveBaseData objectiveData3 = objectiveBase3.ObjectiveData;
				bool flag2 = false;
				for (int k = 0; k < count; k++)
				{
					if (objectiveData3.ID == objectivesBaseData[k].ObjectiveData.ID)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					objectivesBaseData.Add(objectiveBase3);
					flag = true;
				}
			}
			if (this.OnMissionTrackerChanged != null && flag)
			{
				this.OnMissionTrackerChanged();
			}
		}
	}
}
