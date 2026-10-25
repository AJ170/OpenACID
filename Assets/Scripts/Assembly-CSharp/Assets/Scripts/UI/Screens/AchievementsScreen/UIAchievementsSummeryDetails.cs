using System;
using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using InControl;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsSummeryDetails : MonoBehaviour
	{
		private const string k_gPlaySpriteName = "aUI_icon_googleplay";

		[SerializeField]
		private UIExtendedLabel m_headLine;

		[SerializeField]
		private UIAchievementsProgressDetails m_totalProgressDetails;

		[SerializeField]
		private UIResizeBox m_resizeBox;

		[SerializeField]
		private UIAlignmentBox[] m_subCategoryAlignmentBox;

		[SerializeField]
		private GameObject m_lastUnlockedChildContainer;

		[SerializeField]
		private UIAchievementsItemDetails m_lastUnlockedChild;

		[SerializeField]
		private UIShareOnSocialMediaBehaviour m_shareOnSocialMedia;

		[SerializeField]
		private UIScrollView m_categoryScrollView;

		[SerializeField]
		private UITable m_categoryInfoTable;

		[SerializeField]
		private UIExtendedButton m_googlePlayButton;

		private int m_subCategoriesPerRow;

		private UIAchievementsProgressDetails[][] m_subCategoryProgressDetails;

		public void UpdateSummery(AchievementCategoryData _categoryData)
		{
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			bool flag = true;
			Achievement achievement = _categoryData.LastClaimableAchievement;
			if (achievement == null)
			{
				flag = false;
				achievement = _categoryData.LastClaimedAchievement;
			}
			if (achievement != null)
			{
				m_lastUnlockedChildContainer.SetActive(true);
				AchievementMapping.Definition achievementMappingDefinition = achievementsManager.GetAchievementMappingDefinition(achievement.definitionId);
				m_lastUnlockedChild.UpdateChild(achievementMappingDefinition, !flag, flag, 0);
			}
			else
			{
				m_lastUnlockedChildContainer.SetActive(false);
			}
			if (!string.IsNullOrEmpty(_categoryData.CategoryId))
			{
				AchievementCategoryDefinition achievementCategoryDefinition = achievementsManager.GetAchievementCategoryDefinition(_categoryData.CategoryId);
				m_headLine.LocalizationKey = achievementCategoryDefinition.NameLocaKey.Key;
			}
			else
			{
				m_headLine.LocalizationKey = 5627;
			}
			m_totalProgressDetails.UpdateChild(_categoryData);
			int num = m_subCategoriesPerRow * m_subCategoryAlignmentBox.Length;
			int count = _categoryData.SubCategories.Count;
			int num2 = 0;
			for (int i = 0; i < count && i <= num - 1; i++)
			{
				AchievementCategoryData achievementCategoryData = _categoryData.SubCategories[i];
				if (achievementCategoryData.TotalCount > 0)
				{
					int num3 = num2 / m_subCategoriesPerRow;
					int num4 = num2 % m_subCategoriesPerRow;
					if (num4 == 0)
					{
						m_subCategoryAlignmentBox[num3].gameObject.SetActive(true);
					}
					UIAchievementsProgressDetails uIAchievementsProgressDetails = m_subCategoryProgressDetails[num3][num4];
					uIAchievementsProgressDetails.UpdateChild(achievementCategoryData);
					uIAchievementsProgressDetails.gameObject.SetActive(true);
					num2++;
				}
			}
			int num5 = num2 / m_subCategoriesPerRow;
			int num6 = num2 % m_subCategoriesPerRow;
			for (int j = num6; j < m_subCategoryProgressDetails[num5].Length; j++)
			{
				m_subCategoryProgressDetails[num5][j].gameObject.SetActive(false);
			}
			if (num6 > 0)
			{
				num5++;
			}
			for (int k = num5; k < m_subCategoryAlignmentBox.Length; k++)
			{
				m_subCategoryAlignmentBox[k].gameObject.SetActive(false);
			}
			EventDelegate.Add(m_resizeBox.OnResized, delegate
			{
				m_categoryInfoTable.repositionNow = true;
				UITable categoryInfoTable = m_categoryInfoTable;
				categoryInfoTable.onReposition = (UITable.OnReposition)Delegate.Combine(categoryInfoTable.onReposition, new UITable.OnReposition(OnReposition));
			}, true);
			m_resizeBox.ResizeNow = true;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_googlePlayButton.onClick, OnGooglePlayButtonKlicked);
			ClientAchievementsManager.ClientAchievementSignInCallback += GoogleSignInUpdate;
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_googlePlayButton.onClick, OnGooglePlayButtonKlicked);
			ClientAchievementsManager.ClientAchievementSignInCallback -= GoogleSignInUpdate;
		}

		private void GoogleSignInUpdate()
		{
			UIButtonLabelHandler component = m_googlePlayButton.GetComponent<UIButtonLabelHandler>();
			component.SetIcon(Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Animus), "aUI_icon_googleplay");
		}

		private void Awake()
		{
			UIAchievementsItemDetails lastUnlockedChild = m_lastUnlockedChild;
			lastUnlockedChild.OnItemClaimed = (Action<AchievementMapping.Definition>)Delegate.Combine(lastUnlockedChild.OnItemClaimed, new Action<AchievementMapping.Definition>(OnItemClaimed));
			m_subCategoryProgressDetails = new UIAchievementsProgressDetails[m_subCategoryAlignmentBox.Length][];
			for (int i = 0; i < m_subCategoryAlignmentBox.Length; i++)
			{
				UIAlignmentBox uIAlignmentBox = m_subCategoryAlignmentBox[i];
				m_subCategoriesPerRow = uIAlignmentBox.Children.Count;
				m_subCategoryProgressDetails[i] = new UIAchievementsProgressDetails[m_subCategoriesPerRow];
				for (int j = 0; j < uIAlignmentBox.Children.Count; j++)
				{
					m_subCategoryProgressDetails[i][j] = uIAlignmentBox.Children[j].GetComponent<UIAchievementsProgressDetails>();
				}
			}
			m_googlePlayButton.gameObject.SetActive(true);
			EventDelegate.Add(m_googlePlayButton.onClick, OnGooglePlayButtonKlicked);
		}

		private void OnItemClaimed(AchievementMapping.Definition _definition)
		{
			m_shareOnSocialMedia.OnAchievementClaimed(_definition);
		}

		private void OnDestroy()
		{
			if (m_lastUnlockedChild != null)
			{
				UIAchievementsItemDetails lastUnlockedChild = m_lastUnlockedChild;
				lastUnlockedChild.OnItemClaimed = (Action<AchievementMapping.Definition>)Delegate.Remove(lastUnlockedChild.OnItemClaimed, new Action<AchievementMapping.Definition>(OnItemClaimed));
			}
			m_subCategoryProgressDetails = new UIAchievementsProgressDetails[m_subCategoryAlignmentBox.Length][];
			for (int i = 0; i < m_subCategoryProgressDetails.Length; i++)
			{
				m_subCategoryProgressDetails[i].ClearIfNotEmpty();
			}
			m_subCategoryProgressDetails.ClearIfNotEmpty();
			m_subCategoryProgressDetails = null;
			EventDelegate.Remove(m_googlePlayButton.onClick, OnGooglePlayButtonKlicked);
			ClientAchievementsManager.ClientAchievementSignInCallback -= GoogleSignInUpdate;
		}

		private void OnReposition()
		{
			UITable categoryInfoTable = m_categoryInfoTable;
			categoryInfoTable.onReposition = (UITable.OnReposition)Delegate.Remove(categoryInfoTable.onReposition, new UITable.OnReposition(OnReposition));
			if (!m_lastUnlockedChild.IsWaitingForClaimAnimation)
			{
				m_categoryScrollView.ResetPosition();
			}
		}

		private void OnGooglePlayButtonKlicked()
		{
			InputManager.ClearInputState();
			ClientAchievementsManager.ShowAchievements();
		}
	}
}
