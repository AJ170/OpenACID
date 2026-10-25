using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxReturnToInventory : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxInventoryReturn.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Mystery Box Return To Inventory";
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

		public override void Activate()
		{
			base.Activate();
			string[] itemsInFrontTechnicalNames = new string[1] { TutorialHelper.TutorialEquipItem };
			TutorialHelper.GetInventoryHandler().PreInitSortingComparer(new TutorialHelper.TutorialItemCompare(itemsInFrontTechnicalNames));
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnFinishCategory);
		}

		private void OnFinishCategory(IAsyncResult _ar)
		{
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			UIWalletHandler walletHandler = instance.SceneData.UIMenuBars.WalletHandler;
			walletHandler.ResetVirtualAddedAmounts();
			walletHandler.ProfileSyncEnabled = true;
			walletHandler.ForceUpdateResourceType(EResourceType.HardCurrency);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this, true);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.MysteryBox;
		}

		public override int GetHeaderText()
		{
			return 5664;
		}

		public override int GetDescriptionText()
		{
			return 5664;
		}
	}
}
