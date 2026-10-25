using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.System;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	internal class TutorialForgeConfirmToForge : TutorialUIPopupBase
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideMysteryBoxInfoContinue.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeConfirmToForge";
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

		public override void Deactivate()
		{
			if (IsActive)
			{
				UIInventoryScreen componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIInventoryScreen>();
				componentInChildren.InventoryHandler.ResetTutorialItem();
				base.Deactivate();
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 5664;
		}

		public override int GetDescriptionText()
		{
			return 5664;
		}

		protected override void OnClickInternal()
		{
		}

		protected override int GetHightlightLayer()
		{
			return Layers.UISharedLayer;
		}

		protected override int GetHightlightSortingOrder()
		{
			return 3;
		}

		protected override int GetButtonIndexToHighlight()
		{
			return 0;
		}
	}
}
