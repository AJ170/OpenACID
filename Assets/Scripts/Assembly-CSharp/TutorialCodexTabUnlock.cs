using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Tutorial.Flow;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

public class TutorialCodexTabUnlock : TutorialTabUnlockBase
{
	private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

	public override string Name
	{
		get
		{
			return "TutorialCodexTabUnlock";
		}
	}

	public override ETutorialCategory Category
	{
		get
		{
			return ETutorialCategory.TutCatMs26Codex;
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
		return TutorialHelper.ConditionAllowShowGuideCodexTab(this);
	}

	public override EScreenID GetScreenId()
	{
		return EScreenID.TabUnlockScreen;
	}
}
