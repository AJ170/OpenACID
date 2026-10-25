using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using UnityEngine;

namespace Assets.Scripts.UI.Missions
{
	public class UIMissionScreenSelector : UIScreenChanger
	{
		[SerializeField]
		private MissionSelectionManager.EMissionMenu m_missionMenu;

		protected override void DoReactOnClick()
		{
			MissionSelectionManager.EMissionMenu eMissionMenu = m_missionMenu;
			EScreenID eScreenID = EScreenID.Mission;
			UIScreenHistoryDetails.EUIScreenGroups groupId = UIScreenHistoryDetails.EUIScreenGroups.ContractMission;
			if (MissionSelectionManager.IsCampaignMenuType(m_missionMenu))
			{
				groupId = UIScreenHistoryDetails.EUIScreenGroups.CampaignMission;
			}
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			UIScreenHistoryDetails.ScreenHistory lastScreenByScreenGroup = instance.LastScreenHistory.GetLastScreenByScreenGroup(groupId);
			if (lastScreenByScreenGroup != null)
			{
				eMissionMenu = lastScreenByScreenGroup.MenuType;
				eScreenID = lastScreenByScreenGroup.ScreenId;
			}
			if (eScreenID == instance.CurrentScreenId && eMissionMenu == Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType)
			{
				eMissionMenu = m_missionMenu;
				eScreenID = EScreenID.Mission;
			}
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = eMissionMenu;
			instance.ChangeScreen(eScreenID, new UITweenHandle());
		}
	}
}
