using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.HirelingScreen
{
	public class HirelingDetail : MonoBehaviour
	{
		public UIExtendedLabel ItemPOW;

		public UIExtendedLabel AssassinsName;

		public UIExtendedLabel Username;

		public UIExtendedLabel ClassName;

		public UIPortraitTexture AssassinPortrait;

		public UIExtendedLabel Reward;

		public UISprite RewardSprite;

		public UIExtendedLabel ActiveSkill;

		public UIExtendedLabel PassiveSkill;

		public UITexture ActiveSkillIcon;

		public UITexture PassiveSkillIcon;

		public UIExtendedLabel ActiveSkillDesc;

		public UIExtendedLabel PassiveSkillDesc;

		public UIExtendedLabel Relation;

		[SerializeField]
		private UIHeroicPortraitTexture m_heroicPortrait;

		[SerializeField]
		private UITable m_skillTable;

		[SerializeField]
		private UIScrollView m_scrollView;

		[SerializeField]
		private UISprite m_relationSymbol;

		[SerializeField]
		private UIExtendedLabel m_rewardTypeLabel;

		[SerializeField]
		private UIExtendedLabel m_noRewardLabel;

		[SerializeField]
		private GameObject m_rewardAvailableContainer;

		private HirelingAssassin.EHirelingType m_relationType;

		public void UpdateTextureData(HirelingAssassin _data)
		{
			UIPortraitUtil.UpdatePortraitByColorDef(_data, AssassinPortrait, true);
			UIPortraitUtil.UpdateHeroicPortraitByHirelingAssassin(_data, _data.rankIdx, m_heroicPortrait);
		}

		public void UpdateData(HirelingAssassin _hirelingAssassinData)
		{
			if (_hirelingAssassinData != null)
			{
				HirelingAssassinsData hirelingAssassinsData = new HirelingAssassinsData(_hirelingAssassinData);
				ItemPOW.text = LocalizationManager.Instance.GetInteger(hirelingAssassinsData.ItemPow);
				AssassinsName.text = hirelingAssassinsData.AssassinsName;
				ClassName.text = hirelingAssassinsData.ClassName;
				m_relationType = hirelingAssassinsData.RelationType;
				UpdateRewardSeperately();
				Relation.text = hirelingAssassinsData.Relation;
				m_relationSymbol.enabled = hirelingAssassinsData.ShowRelationSymbol;
				LocalizationManager instance = LocalizationManager.Instance;
				ClassDescription classDescription = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AssassinClassDescriptions[(int)hirelingAssassinsData.ClassType];
				Username.text = (string.IsNullOrEmpty(hirelingAssassinsData.Username) ? instance.Get(10650) : instance.Get(5387, new StringMapper { { "Player_Name", hirelingAssassinsData.Username } }));
				ActiveSkill.text = instance.Get(classDescription.HirelingDescriptionData.ActiveNameId);
				if (hirelingAssassinsData.ActiveSkill != null)
				{
					StringId activeDescriptionId = classDescription.HirelingDescriptionData.ActiveDescriptionId;
					ActiveSkillDesc.text = instance.Get(activeDescriptionId, hirelingAssassinsData.ActiveSkill.RuleDefinition.GetStringMapperForRules(activeDescriptionId, (int)hirelingAssassinsData.ItemPow));
					ActiveSkillIcon.mainTexture = hirelingAssassinsData.ActiveSkill.IconUp.GetObject<Texture2D>();
				}
				PassiveSkill.text = instance.Get(classDescription.HirelingDescriptionData.PassiveNameId);
				if (hirelingAssassinsData.PassiveSkill != null)
				{
					StringId passiveDescriptionId = classDescription.HirelingDescriptionData.PassiveDescriptionId;
					PassiveSkillDesc.text = instance.Get(passiveDescriptionId, hirelingAssassinsData.PassiveSkill.RuleDefinition.GetStringMapperForRules(passiveDescriptionId, (int)hirelingAssassinsData.ItemPow));
					PassiveSkillIcon.mainTexture = hirelingAssassinsData.PassiveSkill.IconUp.GetObject<Texture2D>();
				}
				PassiveSkillDesc.Update();
				ActiveSkillDesc.Update();
				m_heroicPortrait.alpha = 0.01f;
				m_skillTable.repositionNow = true;
				m_scrollView.ResetPosition();
			}
		}

		public void UpdateRewardSeperately()
		{
			bool active = FriendUtil.IsFriendListFull();
			m_noRewardLabel.gameObject.SetActive(active);
			m_noRewardLabel.SetLocalizationKey(9699);
			m_rewardAvailableContainer.SetActive(!FriendUtil.IsFriendListFull());
			MissionSelectionManager missionSelectionManager = Singleton<StateManager>.Instance.MissionSelectionManager;
			switch (m_relationType)
			{
			case HirelingAssassin.EHirelingType.RandomGenerated:
				m_rewardAvailableContainer.SetActive(false);
				m_noRewardLabel.gameObject.SetActive(true);
				m_noRewardLabel.SetLocalizationKey(10651);
				break;
			case HirelingAssassin.EHirelingType.Random:
			{
				AcierProtocol.Resources addFriendReward = Singleton<StateManager>.Instance.TemplateManager.Globals.addFriendReward;
				if (addFriendReward.resources.Length > 0)
				{
					Reward.text = LocalizationManager.Instance.GetInteger(addFriendReward.resources[0].amount);
				}
				m_rewardTypeLabel.text = LocalizationManager.Instance.Get(8134);
				break;
			}
			case HirelingAssassin.EHirelingType.Friend:
			{
				int num = missionSelectionManager.ContractHirelingReward;
				if (Singleton<StateManager>.Instance.MissionSelectionManager.IsFromCampaignMenu)
				{
					num = missionSelectionManager.CampaignHirelingReward;
				}
				Reward.text = LocalizationManager.Instance.GetInteger(num);
				m_rewardTypeLabel.text = LocalizationManager.Instance.Get(8135);
				break;
			}
			default:
				m_rewardAvailableContainer.SetActive(false);
				break;
			}
		}
	}
}
