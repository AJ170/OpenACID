using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	internal class TutorialForgeClaim : TutorialInfoPopUpBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideForgeClaim.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeClaim";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected override void AddActionCallback()
		{
			ItemInfoPopupHelper.OnClameItemCallback += new SimpleDelegate.CallbackFunction(OnClaimItem);
		}

		protected override void RemoveActionCallback()
		{
			ItemInfoPopupHelper.OnClameItemCallback -= new SimpleDelegate.CallbackFunction(OnClaimItem);
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			ItemInfoPopupHelper.OnClameItemCallback -= new SimpleDelegate.CallbackFunction(OnClaimItem);
		}

		private void OnClaimItem(params object[] _params)
		{
			m_textPopup.MoveOut();
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnFinishedForgeTutorial);
		}

		private void OnFinishedForgeTutorial(IAsyncResult _ar)
		{
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForgeAfterForge(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6934;
		}
	}
}
