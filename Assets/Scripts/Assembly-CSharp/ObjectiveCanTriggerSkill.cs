using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;

public class ObjectiveCanTriggerSkill : ObjectiveBase<ObjectiveCanTriggerSkillData>
{
	[Skill]
	public string Skill;

	private Enum<SkillTypes> m_skillType;

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

	protected override EObjectiveResult InitialResult
	{
		get
		{
			return EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	private ObjectiveCanTriggerSkill()
	{
		if (string.IsNullOrEmpty(base.Data.SkillName))
		{
			m_skillType = Enum<SkillTypes>.Invalid;
		}
		else
		{
			m_skillType = new Enum<SkillTypes>(base.Data.SkillName);
		}
	}

	public override void FrameUpdateImpl()
	{
		ESkillAvailabilityFlags _availabilityFlags;
		if (IsActive && LogicMain.PlayerEntity != null && LogicMain.LogicInterface.CanBeTriggeredOnSelectedEntity(m_skillType, out _availabilityFlags))
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.Result = EObjectiveResult.Fulfilled;
			base.VisualSummaryState = true;
			Complete();
		}
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		Skill = base.Data.SkillName;
		if (string.IsNullOrEmpty(Skill))
		{
			m_skillType = Enum<SkillTypes>.Invalid;
		}
		else
		{
			m_skillType = new Enum<SkillTypes>(Skill);
		}
	}
}
