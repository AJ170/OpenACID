namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeSelectItemTwo : TutorialForgeSelectItem
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeSelectItemTwo";
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override int GetHeaderText()
		{
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6929;
		}

		public override string GetItemToForge()
		{
			return TutorialHelper.TutorialForgeItemTwo;
		}
	}
}
