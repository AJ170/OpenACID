using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers.InfoPopup;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Modules
{
	public class TutorialModulesEquip : TutorialInfoPopUpBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialModulesEquip";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Modules;
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
			ModuleInfoPopupHelper.OnModuleChangedCallback += new SimpleDelegate.CallbackFunction(OnEquip);
		}

		protected override void RemoveActionCallback()
		{
			ModuleInfoPopupHelper.OnModuleChangedCallback -= new SimpleDelegate.CallbackFunction(OnEquip);
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			ModuleInfoPopupHelper.OnModuleChangedCallback -= new SimpleDelegate.CallbackFunction(OnEquip);
		}

		private void OnEquip(params object[] _params)
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideModules(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 7317;
		}

		public override int GetDescriptionText()
		{
			return 7323;
		}
	}
}
