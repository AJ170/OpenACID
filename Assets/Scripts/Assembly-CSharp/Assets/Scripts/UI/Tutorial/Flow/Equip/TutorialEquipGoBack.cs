using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Equip
{
	internal class TutorialEquipGoBack : TutorialGoBackBase
	{
		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Equip;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return EFlowId.GuideMysteryBoxLootAgain.ToTutorialFlowId();
			}
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
			return 4299;
		}

		public override int GetDescriptionText()
		{
			return 4619;
		}
	}
}
