namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class SkillTreeProfileFacadeMock : ISkillTreeProfileFacade
	{
		public int SkillPoints { get; private set; }

		public int PointsSpent { get; private set; }

		public void Init(int _skillPoints, int _pointsSpent)
		{
			SkillPoints = _skillPoints;
			PointsSpent = _pointsSpent;
		}
	}
}
