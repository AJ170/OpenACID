namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	public class TutorialForgeAddToForgeTwo : TutorialForgeAddToForge
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxLootAgain.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeAddToForgeTwo";
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
			return 6930;
		}
	}
}
