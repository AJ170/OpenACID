using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxGotoInventory : TutorialGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialMysteryBoxGotoInventory";
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
			TutorialHelper.GetInventoryHandler().PreInitSortingComparer(new TutorialHelper.TutorialMysteryBoxItemCompare());
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6897;
		}
	}
}
