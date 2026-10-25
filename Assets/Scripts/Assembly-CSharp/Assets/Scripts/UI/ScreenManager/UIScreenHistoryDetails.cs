using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIScreenHistoryDetails
	{
		public enum EUIScreenGroups
		{
			None = 0,
			CampaignMission = 1,
			ContractMission = 2
		}

		public class ScreenHistory
		{
			public EScreenID ScreenId;

			public MissionSelectionManager.EMissionMenu MenuType;
		}

		private readonly Dictionary<int, ScreenHistory> m_lastScreenForScreenGroup = new Dictionary<int, ScreenHistory>();

		public UIScreenHistoryDetails()
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged += new SimpleDelegate.CallbackFunction(OnMenuTypeChanged);
		}

		public static EUIScreenGroups GetGroupIdByMenuType(MissionSelectionManager.EMissionMenu _menuType)
		{
			if (MissionSelectionManager.IsCampaignMenuType(_menuType))
			{
				return EUIScreenGroups.CampaignMission;
			}
			return EUIScreenGroups.ContractMission;
		}

		public static EUIScreenGroups GetGroupIdByScreenId(EScreenID _screenId, MissionSelectionManager.EMissionMenu _menuType)
		{
			if (_screenId == EScreenID.MissionLoadout || _screenId == EScreenID.MissionHireling || _screenId == EScreenID.Mission)
			{
				return GetGroupIdByMenuType(_menuType);
			}
			return EUIScreenGroups.None;
		}

		~UIScreenHistoryDetails()
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.OnSubMenuChanged -= new SimpleDelegate.CallbackFunction(OnMenuTypeChanged);
		}

		public ScreenHistory GetLastScreenByScreenGroup(EUIScreenGroups _groupId)
		{
			ScreenHistory value = null;
			if (m_lastScreenForScreenGroup.TryGetValue((int)_groupId, out value))
			{
				ValidateHistoryScreenId(ref value);
			}
			return value;
		}

		private void OnMenuTypeChanged(params object[] _params)
		{
			if (!(Singleton<UIScreenManager>.Instance.CurrentScreen == null))
			{
				MissionSelectionManager.EMissionMenu subMenuType = Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType;
				EUIScreenGroups groupIdByMenuType = GetGroupIdByMenuType(subMenuType);
				if (!m_lastScreenForScreenGroup.ContainsKey((int)groupIdByMenuType))
				{
					m_lastScreenForScreenGroup[(int)groupIdByMenuType] = new ScreenHistory();
				}
				m_lastScreenForScreenGroup[(int)groupIdByMenuType].ScreenId = Singleton<UIScreenManager>.Instance.CurrentScreen.ScreenId;
				m_lastScreenForScreenGroup[(int)groupIdByMenuType].MenuType = subMenuType;
			}
		}

		public void OnScreenChange(EScreenID _screenId)
		{
			EUIScreenGroups groupIdByScreenId = GetGroupIdByScreenId(_screenId, Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType);
			if (!m_lastScreenForScreenGroup.ContainsKey((int)groupIdByScreenId))
			{
				m_lastScreenForScreenGroup[(int)groupIdByScreenId] = new ScreenHistory();
			}
			m_lastScreenForScreenGroup[(int)groupIdByScreenId].ScreenId = _screenId;
		}

		public void FixHistoryAfterMission()
		{
			ScreenHistory value = null;
			if (m_lastScreenForScreenGroup.TryGetValue(1, out value))
			{
				if (value.MenuType != 0)
				{
					value.MenuType = MissionSelectionManager.EMissionMenu.CampaignMissions;
				}
				value.ScreenId = EScreenID.Mission;
			}
			if (m_lastScreenForScreenGroup.TryGetValue(2, out value))
			{
				value.MenuType = MissionSelectionManager.EMissionMenu.ContractMissions;
				value.ScreenId = EScreenID.Mission;
			}
		}

		private void ValidateHistoryScreenId(ref ScreenHistory _screenHistory)
		{
			EScreenID screenId = _screenHistory.ScreenId;
			if ((screenId == EScreenID.MissionLoadout || screenId == EScreenID.MissionHireling || screenId == EScreenID.Mission) && !MissionSelectionManager.IsCampaignMenuType(_screenHistory.MenuType))
			{
				AcierProtocol.Mission contractMission = Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission;
				MissionCollection.RankMissions currrentRankMissions = Singleton<StateManager>.Instance.ProfileManager.CurrrentRankMissions;
				if (contractMission != null && currrentRankMissions.GetMissionByID(contractMission.id) == null)
				{
					_screenHistory.ScreenId = EScreenID.Mission;
					_screenHistory.MenuType = MissionSelectionManager.EMissionMenu.ContractMissions;
				}
			}
		}
	}
}
