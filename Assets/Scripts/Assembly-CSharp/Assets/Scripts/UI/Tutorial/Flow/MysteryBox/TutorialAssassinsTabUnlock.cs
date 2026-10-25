using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	public class TutorialAssassinsTabUnlock : TutorialTabUnlockBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialAssassinsTabUnlock";
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

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.TabUnlockScreen;
		}
	}
}
