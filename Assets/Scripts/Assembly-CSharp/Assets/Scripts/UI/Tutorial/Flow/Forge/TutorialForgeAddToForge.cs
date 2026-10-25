using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeAddToForge : TutorialInfoPopUpBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeAddToForge";
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
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6928;
		}

		protected override void AddActionCallback()
		{
			ItemInfoPopupHelper.OnAddToForgeCallback += new SimpleDelegate.CallbackFunction(OnAddedToForge);
		}

		protected override void RemoveActionCallback()
		{
			ItemInfoPopupHelper.OnAddToForgeCallback -= new SimpleDelegate.CallbackFunction(OnAddedToForge);
		}

		private void OnAddedToForge(params object[] _params)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}
	}
}
