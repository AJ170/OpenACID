using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Modules
{
	internal class TutorialModulesGoBack : TutorialGotoBase
	{
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
				return EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();
			}
		}

		public override void Activate()
		{
			m_buttonWidget = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager.GetButtonByHeaderType(UIButtonHeader.EHeaderType.MissionsCampaign).GetComponent<UIWidget>();
			base.Activate();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.Mission;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this, true) && Singleton<UIScreenManager>.Instance.CurrentScreenId == EScreenID.InventoryScreen;
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
			return 7321;
		}

		protected override void OnScreenChangedInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, null);
		}
	}
}
