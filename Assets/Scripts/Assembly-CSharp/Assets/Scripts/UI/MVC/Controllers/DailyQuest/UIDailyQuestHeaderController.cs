using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.DailyQuest;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.MVC.UIView.Events;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.DailyQuest
{
	public class UIDailyQuestHeaderController : UITabButtonHeaderController
	{
		[CheckAssigned]
		[SerializeField]
		private UIEventHeaderViewPresenter m_headerView;

		[CheckAssigned]
		[SerializeField]
		private UIProgressBar[] m_questProgressBars;

		[SerializeField]
		[CheckAssigned]
		private UIRewardItemController[] m_rewardItemController;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_rewardAlignmentBox;

		private UIRewardItemController m_currentSelected;

		private UIDailyQuestModel m_dailyQuestModel;

		public override void DeselectRewardItem()
		{
			if (m_currentSelected != null)
			{
				m_currentSelected.IsHighlighted = false;
				m_currentSelected.Close();
			}
		}

		public override void Initialize(UITabButtonModel _model)
		{
			m_currentSelected = null;
			UpdateController(_model);
			base.IsInitialized = true;
		}

		public override void UpdateController(UITabButtonModel _model)
		{
			m_dailyQuestModel = _model as UIDailyQuestModel;
			bool flag = m_dailyQuestModel != null && m_dailyQuestModel.RankQuest != null;
			m_rewardAlignmentBox.gameObject.SetActive(flag);
			StringId key = 8294;
			if (flag)
			{
				key = 8293;
			}
			m_headerView.HeaderDescription = LocalizationManager.Instance.Get(key);
			if (m_dailyQuestModel == null || m_dailyQuestModel.RankQuest != null)
			{
				UpdateQuestStatus();
				if (flag)
				{
					m_rewardAlignmentBox.RepositionNow = true;
				}
			}
		}

		public override void Show()
		{
			base.Show();
			m_headerView.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			m_headerView.gameObject.SetActive(false);
		}

		public void ClaimRewards(bool _isSentToInventory)
		{
			for (int i = 0; i < m_rewardItemController.Length; i++)
			{
				m_rewardItemController[i].ClaimReward(_isSentToInventory);
			}
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			for (int i = 0; i < m_rewardItemController.Length; i++)
			{
				UIRewardItemController obj = m_rewardItemController[i];
				obj.OnClicked = (Action<UIRewardItemController>)Delegate.Combine(obj.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
			}
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			m_dailyQuestModel = null;
			for (int i = 0; i < m_rewardItemController.Length; i++)
			{
				if (m_rewardItemController[i] != null)
				{
					UIRewardItemController obj = m_rewardItemController[i];
					obj.OnClicked = (Action<UIRewardItemController>)Delegate.Remove(obj.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
				}
			}
		}

		private void UpdateQuestStatus()
		{
			if (m_dailyQuestModel == null || m_dailyQuestModel.RankQuest == null)
			{
				return;
			}
			ProgressionQuests.RankQuests.Quest[] questChain = m_dailyQuestModel.RankQuest.questChain;
			for (int i = 0; i < m_rewardItemController.Length; i++)
			{
				if (questChain.Length > i && questChain[i] != null)
				{
					ProgressionQuests.RankQuests.Quest quest = questChain[i];
					m_rewardItemController[i].Initialize(quest.reward);
					m_rewardItemController[i].IsCheckMarkVisible = quest.status == EQuestStatus.Claimed;
					if (m_questProgressBars.Length > i && m_questProgressBars[i] != null)
					{
						m_questProgressBars[i].value = (float)quest.progress / (float)quest.numberOfTimesToPerform;
					}
				}
				else
				{
					m_rewardItemController[i].Hide();
				}
			}
		}

		private void OnRewardItemClicked(UIRewardItemController _controller)
		{
			if (m_currentSelected != null && m_currentSelected != _controller)
			{
				m_currentSelected.IsHighlighted = false;
			}
			m_currentSelected = _controller;
			Action onItemClicked = base.OnItemClicked;
			if (onItemClicked != null)
			{
				onItemClicked();
			}
		}
	}
}
