using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.Screens.MysteryBox;
using Assets.Scripts.UI.Screens.MysteryBoxScreen;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxLootAgain : TutorialButtonAndConfirmationBase
	{
		[SerializeField]
		private ChainManager m_ChainManager;

		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxLootAgain.ToTutorialFlowId();

		private long m_addedVirtualHardCurrency;

		private MysteryBoxItemContainer itemContainer;

		public override string Name
		{
			get
			{
				return "Mystery Box Loot Again";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25MysteryBox;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected override void OnScreenInit()
		{
			m_ChainManager.ItemContainersAdded.Add(OnItemContainersAdded);
			base.OnScreenInit();
		}

		protected override void OnScreenDestroy()
		{
			if (itemContainer != null)
			{
				itemContainer.OnAnimationFinished.Remove(OnItemAnimationFinished);
				itemContainer = null;
			}
			m_ChainManager.ItemContainersAdded.Remove(OnItemContainersAdded);
			base.OnScreenDestroy();
		}

		public override void Activate()
		{
			base.IsActive = true;
			UIMysteryBoxScreen componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIMysteryBoxScreen>();
			if (componentInChildren != null)
			{
				Resource overspendingCost = componentInChildren.OverspendingCost;
				m_addedVirtualHardCurrency = overspendingCost.amount;
			}
			AcierProtocol.Resources mysteryBoxTutorialRewardAfterCompletion = Singleton<StateManager>.Instance.TemplateManager.Globals.mysteryBoxTutorialRewardAfterCompletion;
			m_addedVirtualHardCurrency += TutorialHelper.GetCostOfType(mysteryBoxTutorialRewardAfterCompletion, EResourceType.HardCurrency);
			UIWalletHandler walletHandler = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler;
			walletHandler.SetVirtualAddedAmount(EResourceType.HardCurrency, m_addedVirtualHardCurrency);
			walletHandler.ProfileSyncEnabled = false;
			walletHandler.ForceUpdateResourceType(EResourceType.HardCurrency);
			base.gameObject.SetActive(true);
			CollectBlockers();
			InitializeBlockers();
			for (int i = 0; i < m_blockers.Count; i++)
			{
				FullscreenBlocker blocker = m_blockers[i];
				ActivateBlocker(blocker);
			}
		}

		private void OnItemContainersAdded(object[] _params)
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialCategoryActive(Category))
			{
				List<MysteryBoxItemContainer> list = _params[0] as List<MysteryBoxItemContainer>;
				if (list != null && list.Count > 0)
				{
					itemContainer = list[0];
					itemContainer.OnAnimationFinished.Add(OnItemAnimationFinished);
				}
			}
		}

		private void OnItemAnimationFinished(object[] _params)
		{
			itemContainer.OnAnimationFinished.Remove(OnItemAnimationFinished);
			base.Activate();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.MysteryBox;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6900;
		}

		protected override void OnModalScreenPoppedInternal()
		{
			AcierProtocol.Resources mysteryBoxTutorialRewardAfterCompletion = Singleton<StateManager>.Instance.TemplateManager.Globals.mysteryBoxTutorialRewardAfterCompletion;
			m_addedVirtualHardCurrency = TutorialHelper.GetCostOfType(mysteryBoxTutorialRewardAfterCompletion, EResourceType.HardCurrency);
			UIWalletHandler walletHandler = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.WalletHandler;
			walletHandler.SetVirtualAddedAmount(EResourceType.HardCurrency, m_addedVirtualHardCurrency);
			walletHandler.ForceUpdateResourceType(EResourceType.HardCurrency);
			base.OnModalScreenPoppedInternal();
		}

		protected override UIPopup.EContentType GetPopupType()
		{
			return UIPopup.EContentType.EarlyAccess;
		}
	}
}
