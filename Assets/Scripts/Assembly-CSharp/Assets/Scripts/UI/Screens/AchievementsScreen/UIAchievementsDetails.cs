using Assets.Scripts.Achievements;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AchievementsScreen
{
	public class UIAchievementsDetails : MonoBehaviour
	{
		[SerializeField]
		private BBTweenAlpha m_alphaTweener;

		[SerializeField]
		private UIAchievementsItemsDetails m_achievementsItems;

		[SerializeField]
		private UIAchievementsSummeryDetails m_achievementsSummery;

		private string m_lastCategoryId;

		private void Awake()
		{
		}

		public void UpdateAchievementDetails(AchievementCategoryData _categoryDate, bool _shouldReposition = true)
		{
			if (m_lastCategoryId == null || m_lastCategoryId != _categoryDate.CategoryId)
			{
				m_alphaTweener.StartTweenPlayForward();
			}
			m_lastCategoryId = _categoryDate.CategoryId;
			bool flag = string.IsNullOrEmpty(_categoryDate.CategoryId);
			m_achievementsItems.gameObject.SetActive(!flag);
			m_achievementsSummery.gameObject.SetActive(flag);
			if (flag)
			{
				m_achievementsSummery.UpdateSummery(_categoryDate);
			}
			else
			{
				m_achievementsItems.UpdateAchievementList(_categoryDate, _shouldReposition);
			}
		}
	}
}
