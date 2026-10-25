using System;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.MVC.UIView.Events;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils.Development;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Events
{
	public class UIEventItemController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIEventItemViewPresenter m_itemView;

		[CheckAssigned]
		[SerializeField]
		private UIRewardItemController[] m_rewardItemsController;

		private bool m_isClaiming;

		private StringMapper m_progressMapper;

		private UIEventBaseModel m_modelAfterClaimingDone;

		public Action<UIEventItemController> OnItemClicked { get; set; }

		public Action<UIEventItemController, bool> OnItemClaimed { get; set; }

		public UIEventBaseModel Model { get; private set; }

		public bool IsClaimed { get; private set; }

		public bool IsClaimable
		{
			get
			{
				return Model != null && Model.IsClaimable;
			}
		}

		public UIWidget ClaimButtonWidget
		{
			get
			{
				return m_itemView.ClaimButton;
			}
		}

		public UIRewardItemController GetRewardControllerByIndex(int _rewardIndex)
		{
			if (m_rewardItemsController.Length > _rewardIndex)
			{
				return m_rewardItemsController[_rewardIndex];
			}
			return null;
		}

		public void Initialize(UIEventBaseModel _model)
		{
			Show();
			m_modelAfterClaimingDone = null;
			m_isClaiming = false;
			UpdateController(_model);
			ClearRewardItemHighlight();
			base.IsInitialized = true;
		}

		public void UpdateController(UIEventBaseModel _model)
		{
			if (m_isClaiming)
			{
				m_modelAfterClaimingDone = _model;
				return;
			}
			if (_model is UIEventItemModel)
			{
				m_itemView.CenterWidget.rightAnchor.Set(m_itemView.ViewRoot.transform, 1f, -446f);
			}
			else
			{
				m_itemView.CenterWidget.rightAnchor.Set(m_itemView.ViewRoot.transform, 1f, -238f);
			}
			m_itemView.CenterWidget.ForceUpdate(true);
			Model = _model;
			Model.UpdateData();
			UpdateRewardItems();
			IsClaimed = Model.IsClaimed;
			m_itemView.HeaderLabel = Model.HeaderText;
			m_itemView.ShowLockBackground = Model.IsLocked;
			m_itemView.ShowLockIcon = Model.IsLocked;
			m_itemView.ShowCheckBoxFrame = !Model.IsLocked;
			m_itemView.ShowChekBoxMarker = (IsClaimed || IsClaimable) && !Model.IsLocked;
			for (int i = 0; i < m_rewardItemsController.Length; i++)
			{
				m_rewardItemsController[i].IsCheckMarkVisible = false;
			}
			UpdateCounter();
		}

		public void DeselectRewardItem()
		{
			for (int i = 0; i < m_rewardItemsController.Length; i++)
			{
				UIRewardItemController uIRewardItemController = m_rewardItemsController[i];
				uIRewardItemController.IsHighlighted = false;
				uIRewardItemController.Close();
			}
		}

		public void ClearRewardItemHighlight()
		{
			for (int i = 0; i < m_rewardItemsController.Length; i++)
			{
				m_rewardItemsController[i].IsHighlighted = false;
			}
		}

		public void OnClaimClick()
		{
			if (!Model.IsClaimable || !Model.CanClaimItem)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIOnScreenMessage>().ShowMessage(LocalizationManager.Instance.Get(4578));
				return;
			}
			m_isClaiming = true;
			UIEventScreenChild.IsWaitingForItemClaim = true;
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = false;
			Model.OnClaimItem(delegate(bool _successful, bool _wasItemSentToInventory, bool _wasExtraItemSentToInventory)
			{
				if (_successful)
				{
					IsClaimed = true;
					for (int i = 0; i < m_rewardItemsController.Length; i++)
					{
						if (Model.ShouldPlayClaimAnimationForReward(i))
						{
							m_rewardItemsController[i].ClaimReward(_wasItemSentToInventory);
						}
					}
					Action<UIEventItemController, bool> onItemClaimed = OnItemClaimed;
					if (onItemClaimed != null)
					{
						onItemClaimed(this, _wasExtraItemSentToInventory);
					}
					if (!Singleton<UIScreenManager>.Instance.SceneData.RecivedItemMgr.IsCoinAnimationPlaying)
					{
						Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
					}
				}
				else
				{
					Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler.ProfileSyncEnabled = true;
					IsClaimed = false;
				}
				m_isClaiming = false;
				if (m_modelAfterClaimingDone != null)
				{
					UpdateController(m_modelAfterClaimingDone);
				}
				UIEventScreenChild.IsWaitingForItemClaim = false;
			});
		}

		public override void Show()
		{
			base.Show();
			base.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			base.gameObject.SetActive(false);
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			m_progressMapper = new StringMapper
			{
				{ "value", 0 },
				{ "max_value", 0 }
			};
			for (int i = 0; i < m_rewardItemsController.Length; i++)
			{
				UIRewardItemController uIRewardItemController = m_rewardItemsController[i];
				uIRewardItemController.UseLargeCoinItemTextureAsClaimReward = true;
				uIRewardItemController.OnClicked = (Action<UIRewardItemController>)Delegate.Combine(uIRewardItemController.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
			}
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			for (int i = 0; i < m_rewardItemsController.Length; i++)
			{
				UIRewardItemController uIRewardItemController = m_rewardItemsController[i];
				if (uIRewardItemController != null)
				{
					uIRewardItemController.OnClicked = (Action<UIRewardItemController>)Delegate.Remove(uIRewardItemController.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
				}
			}
		}

		protected override void OnDisableUnity()
		{
			if (m_itemView != null)
			{
				m_itemView.SetTweenToEnd();
			}
		}

		private void OnRewardItemClicked(UIRewardItemController _controller)
		{
			Action<UIEventItemController> onItemClicked = OnItemClicked;
			if (onItemClicked != null)
			{
				onItemClicked(this);
			}
		}

		private void UpdateRewardItems()
		{
			int num = Model.RewardItemCount();
			for (int i = 0; i < num; i++)
			{
				if (i < m_rewardItemsController.Length)
				{
					Model.InitializeRewardItem(i, m_rewardItemsController[i]);
				}
			}
			for (int j = num; j < m_rewardItemsController.Length; j++)
			{
				m_rewardItemsController[j].Hide();
			}
			m_itemView.AlignRewardContainer();
		}

		private void UpdateCounter()
		{
			LocalizationManager instance = LocalizationManager.Instance;
			m_itemView.ChangeViewState(Model.IsClaimable);
			m_itemView.IsClaimButtonEnabled = Model.IsClaimable && Model.CanClaimItem;
			if (!Model.IsClaimable)
			{
				m_itemView.HeaderDescription = Model.HeaderDescription;
				m_progressMapper["value"] = instance.GetInteger(Model.CurrentValue);
				m_progressMapper["max_value"] = instance.GetInteger(Model.MaxValue);
				string eventProgressText = instance.Get(5630, m_progressMapper);
				m_itemView.EventProgress = (float)Model.CurrentValue / (float)Model.MaxValue;
				m_itemView.EventProgressText = eventProgressText;
				m_itemView.ShowProgressThumb = Model.CurrentValue < Model.MaxValue;
			}
		}
	}
}
