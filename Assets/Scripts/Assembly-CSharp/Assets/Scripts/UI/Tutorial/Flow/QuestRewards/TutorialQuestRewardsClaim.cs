using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.MVC.Controllers.Events;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.QuestRewards
{
	public class TutorialQuestRewardsClaim : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		[SerializeField]
		private UIEventScreenChild m_eventScreenChild;

		private int m_rank = -1;

		private UIEventItemController m_questItem;

		public override string Name
		{
			get
			{
				return "TutorialQuestRewardsClaim";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26QuestRewards;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				if ((bool)m_questItem)
				{
					UIEventItemController questItem = m_questItem;
					questItem.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Remove(questItem.OnItemClaimed, new Action<UIEventItemController, bool>(OnClaimedEvent));
				}
				base.Deactivate();
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_questItem != null && m_questItem.OnItemClaimed != null)
			{
				UIEventItemController questItem = m_questItem;
				questItem.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Remove(questItem.OnItemClaimed, new Action<UIEventItemController, bool>(OnClaimedEvent));
			}
			m_questItem = null;
		}

		protected override void OnSetupInternal()
		{
			Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank = m_rank;
			m_eventScreenChild.SelectDailyQuestButton();
			UIEventItemController[] itemController = m_eventScreenChild.ItemController;
			for (int i = 0; i < itemController.Length; i++)
			{
				if (itemController[i].IsClaimable)
				{
					m_questItem = itemController[i];
					break;
				}
			}
			UIEventItemController questItem = m_questItem;
			questItem.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Combine(questItem.OnItemClaimed, new Action<UIEventItemController, bool>(OnClaimedEvent));
			m_buttonWidget = m_questItem.ClaimButtonWidget;
			base.OnSetupInternal();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowQuestRewards(this, out m_rank);
		}

		private void OnClaimedEvent(UIEventItemController _uiEventItemController, bool _b)
		{
			UIEventItemController questItem = m_questItem;
			questItem.OnItemClaimed = (Action<UIEventItemController, bool>)Delegate.Remove(questItem.OnItemClaimed, new Action<UIEventItemController, bool>(OnClaimedEvent));
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, null);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.EventProgressionScreen;
		}

		public override int GetHeaderText()
		{
			return 7147;
		}

		public override int GetDescriptionText()
		{
			return 7148;
		}
	}
}
