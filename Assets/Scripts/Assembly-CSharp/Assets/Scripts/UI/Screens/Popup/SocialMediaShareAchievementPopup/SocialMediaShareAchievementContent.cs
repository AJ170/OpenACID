using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.SocialMediaShareAchievementPopup
{
	public class SocialMediaShareAchievementContent : PopUpContentView<SocialMediaShareAchievementContent.SocialMediaAchievementData>
	{
		public class SocialMediaAchievementData : PopUpContentData
		{
			public AchievementMapping.Definition AchievementMappingDefinition;

			public int RankIndex;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.SocialMediaShareAchievement;
				}
			}
		}

		[SerializeField]
		private UIExtendedLabel m_nameLabel;

		[SerializeField]
		private UIExtendedLabel m_promptLabel;

		[SerializeField]
		private UIAchievementTexture m_achievementTexture;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		[SerializeField]
		private Texture[] m_awardRankFrames;

		private void Awake()
		{
		}

		protected override void Render(SocialMediaAchievementData _data)
		{
			AchievementMapping.Definition achievementMappingDefinition = _data.AchievementMappingDefinition;
			AchievementsManager achievementsManager = Singleton<StateManager>.Instance.AchievementsManager;
			AchievementDefinition achievementDefinition = achievementsManager.GetAchievementDefinition(achievementMappingDefinition);
			m_nameLabel.text = LocalizationManager.Instance.Get(achievementDefinition.NameLocaKey.Key);
			m_promptLabel.text = LocalizationManager.Instance.Get((!FacebookManager.IsAvailable) ? 9783 : 6597);
			if (achievementMappingDefinition.ranks.Length == 1)
			{
				m_achievementTexture.SetMaterialData(achievementDefinition.Icon.AccessObject<Texture>(), m_awardRankFrames[2], 0f);
			}
			else
			{
				m_achievementTexture.SetMaterialData(achievementDefinition.Icon.AccessObject<Texture>(), m_awardRankFrames[_data.RankIndex - 1], 0f);
			}
		}

		public override void UpdateLayout()
		{
			EventDelegate.Add(m_vBox.OnReposition, delegate
			{
				EventDelegate.Execute(onLayoutFinished);
			}, true);
			m_vBox.RepositionNow = true;
		}
	}
}
