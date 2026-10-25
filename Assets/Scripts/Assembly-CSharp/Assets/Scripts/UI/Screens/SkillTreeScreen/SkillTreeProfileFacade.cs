using AcierProtocol;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	internal class SkillTreeProfileFacade : ISkillTreeProfileFacade
	{
		public int SkillPoints
		{
			get
			{
				return Assassin.skillPoints;
			}
		}

		public int PointsSpent
		{
			get
			{
				return Assassin.PointSpent();
			}
		}

		public Assassin Assassin
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			}
		}

		public SkillTreeProfileFacade(SkillTreeData _data)
		{
		}
	}
}
