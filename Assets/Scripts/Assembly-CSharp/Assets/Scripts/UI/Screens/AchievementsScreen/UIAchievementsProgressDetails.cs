using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsProgressDetails : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_label;

		[SerializeField]
		private UIExtendedLabel m_progressValue;

		[SerializeField]
		private UISprite m_progressImage;

		public void UpdateChild(AchievementCategoryData _currentCategorySelectedData)
		{
			if (!string.IsNullOrEmpty(_currentCategorySelectedData.CategoryId))
			{
				AchievementCategoryDefinition achievementCategoryDefinition = Singleton<StateManager>.Instance.AchievementsManager.GetAchievementCategoryDefinition(_currentCategorySelectedData.CategoryId);
				m_label.LocalizationKey = achievementCategoryDefinition.NameLocaKey.Key;
			}
			m_progressValue.text = UIUtil.FormatPercentValue(_currentCategorySelectedData.Progress).ToString();
			m_progressImage.fillAmount = _currentCategorySelectedData.Progress;
		}

		private void Awake()
		{
		}
	}
}
