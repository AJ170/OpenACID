using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.UI.Ingame.Behaviours.Misc;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.LoadoutScreen
{
	public class UIObjectiveBehaviour : MonoBehaviour
	{
		public GameObject TableGameObject;

		public GameObject Header;

		public UITable ChallengeTable;

		public List<UICheckboxBehaviour> ChallengeEntries;

		private Action m_callback;

		public void SetActive(bool _isVisible)
		{
			if (Header != null)
			{
				Header.SetActive(_isVisible);
			}
			if (TableGameObject != null)
			{
				TableGameObject.SetActive(_isVisible);
			}
			for (int i = 0; i < ChallengeEntries.Count; i++)
			{
				ChallengeEntries[i].gameObject.SetActive(_isVisible);
			}
		}

		public void SetData(UIMissionGoals.GoalData[] _texts, Action _repositionMissionTable, bool _hasToBeDone, bool _useDots = false, bool _hasToBeChecked = false, UICheckboxBehaviour.ECheckedState? _overrideState = null)
		{
			m_callback = _repositionMissionTable;
			UIMissionGoals.GoalData[] array = ((_texts != null) ? _texts.Where((UIMissionGoals.GoalData x) => !x.Hidden) : null);
			for (int i = 0; i < ChallengeEntries.Count; i++)
			{
				bool flag = array != null && i < array.Length;
				UICheckboxBehaviour uICheckboxBehaviour = ChallengeEntries[i];
				uICheckboxBehaviour.gameObject.SetActive(flag);
				string text = ((!flag) ? string.Empty : array[i].Name);
				UICheckboxBehaviour.ECheckedState eCheckedState = (_useDots ? UICheckboxBehaviour.ECheckedState.Dot : UICheckboxBehaviour.ECheckedState.None);
				UICheckboxBehaviour.ECheckedState eCheckedState2 = ((!flag || _useDots) ? eCheckedState : GetState(array[i].ObjectiveId, _hasToBeDone));
				if (_hasToBeChecked && !_useDots && eCheckedState2 != UICheckboxBehaviour.ECheckedState.Success)
				{
					eCheckedState2 = UICheckboxBehaviour.ECheckedState.Fail;
				}
				if (eCheckedState2 == UICheckboxBehaviour.ECheckedState.Success && !_useDots && _overrideState.HasValue)
				{
					eCheckedState2 = _overrideState.Value;
				}
				uICheckboxBehaviour.SetData(text, eCheckedState2);
			}
			ChallengeTable.repositionNow = true;
			UITable challengeTable = ChallengeTable;
			challengeTable.onReposition = (UITable.OnReposition)Delegate.Combine(challengeTable.onReposition, new UITable.OnReposition(OnReposition));
		}

		private void OnReposition()
		{
			UITable challengeTable = ChallengeTable;
			challengeTable.onReposition = (UITable.OnReposition)Delegate.Remove(challengeTable.onReposition, new UITable.OnReposition(OnReposition));
			if (m_callback != null)
			{
				m_callback();
			}
		}

		public static UICheckboxBehaviour.ECheckedState GetState(long _id, bool _hasToBeDone)
		{
			if (!UbiCore.Utils.Singleton<MissionManager>.IsAvailable)
			{
				return UICheckboxBehaviour.ECheckedState.None;
			}
			BBList<MissionTracker.ObjectiveData> collectedObjectiveData = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionTracker.CollectedObjectiveData;
			if (collectedObjectiveData == null)
			{
				return UICheckboxBehaviour.ECheckedState.None;
			}
			for (int i = 0; i < collectedObjectiveData.Count; i++)
			{
				MissionTracker.ObjectiveData objectiveData = collectedObjectiveData[i];
				for (int j = 0; j < objectiveData.ObjectivesBaseData.Count; j++)
				{
					IObjectiveBase objectiveBase = objectiveData.ObjectivesBaseData[j];
					if (objectiveBase.MissionData.ID == _id)
					{
						if (_hasToBeDone && objectiveBase.IsActive)
						{
							return UICheckboxBehaviour.ECheckedState.None;
						}
						return (!objectiveBase.VisualSummaryState) ? UICheckboxBehaviour.ECheckedState.Fail : UICheckboxBehaviour.ECheckedState.Success;
					}
				}
			}
			return UICheckboxBehaviour.ECheckedState.None;
		}
	}
}
