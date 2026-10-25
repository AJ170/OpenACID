using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.Screens.Popup.SocialMediaShareAchievementPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIShareOnSocialMediaBehaviour : MonoBehaviour
	{
		private const float c_showPopupDelayTime = 2f;

		private float m_popupShowTimer;

		private PopUpConfiguration m_popupConfigToBeShown;

		private readonly BBSimpleList<PopUpConfiguration> m_popupQueue = new BBSimpleList<PopUpConfiguration>();

		public void OnAchievementClaimed(AchievementMapping.Definition _achivement)
		{
			if (Main.Instance.FacebookAchievementsEnabled && (FacebookManager.CanShareAchievements || WeiboManager.CanShareAchievements))
			{
				CreateShareToSocialMediaPopup(_achivement);
			}
			Update();
		}

		private void OnDisable()
		{
			m_popupConfigToBeShown = null;
			m_popupQueue.Clear();
		}

		private void Update()
		{
			if (!Singleton<UIScreenManager>.Instance.IsPopupOpen())
			{
				m_popupShowTimer -= RealTime.deltaTime;
				if (m_popupShowTimer <= 0f && m_popupConfigToBeShown != null)
				{
					Singleton<UIScreenManager>.Instance.ShowPopUpScreen(m_popupConfigToBeShown);
					m_popupConfigToBeShown = null;
				}
				else if (m_popupQueue.Count > 0 && m_popupConfigToBeShown == null)
				{
					m_popupConfigToBeShown = m_popupQueue[0];
					m_popupQueue.RemoveAt(0);
					m_popupShowTimer = 2f;
				}
			}
		}

		private void CreateShareToSocialMediaPopup(AchievementMapping.Definition _achivement)
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			AchievementCategoryData achievementCategoryData = achievementsManager.GetAchievementCategoryData(_achivement.categoryId);
			Achievement value = null;
			achievementCategoryData.IdToAchievements.TryGetValue(_achivement.id, out value);
			AchievementRank achievementRank = value.GetLastClaimedAchievementRank();
			if (achievementRank == null)
			{
				achievementRank = value.GetNextAvailableAchievementRank();
			}
			PopUpConfiguration popUpConfiguration = new PopUpConfiguration();
			popUpConfiguration.ShowCloseButton = true;
			popUpConfiguration.OnCloseButtonClicked = delegate
			{
				Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
			};
			popUpConfiguration.Aligntarget = UIPopup.EContentAlignTarget.Top;
			popUpConfiguration.Heading = LocalizationManager.Instance.Get((!FacebookManager.IsAvailable) ? 9782 : 6596);
			popUpConfiguration.Buttons = new PopUpButtonConfiguration[1]
			{
				new PopUpButtonConfiguration
				{
					Label = LocalizationManager.Instance.Get(6598),
					ButtonType = PopUpButtonConfiguration.EButtonType.eSuggestedClick,
					OnButtonPressed = delegate
					{
						Singleton<UIScreenManager>.Instance.HideCurrentPopupScreen();
						if (FacebookManager.IsAvailable)
						{
							FacebookManager.ShareAchievement(_achivement, achievementRank.idx);
						}
						else if (WeiboManager.IsWeiboAvailable)
						{
							WeiboManager.ShareAchievement(_achivement, achievementRank.idx);
						}
					}
				}
			};
			popUpConfiguration.Data = new SocialMediaShareAchievementContent.SocialMediaAchievementData
			{
				RankIndex = achievementRank.idx,
				AchievementMappingDefinition = _achivement
			};
			PopUpConfiguration value2 = popUpConfiguration;
			m_popupQueue.Add(value2);
		}
	}
}
