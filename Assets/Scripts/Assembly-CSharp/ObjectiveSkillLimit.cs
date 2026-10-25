using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;

public class ObjectiveSkillLimit : ObjectiveBase<ObjectiveSkillLimitData>, IObjectiveValue
{
	[Skill]
	public string Skill;

	private Enum<SkillTypes> SkillType;

	private int m_value;

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.Fulfilled;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return true;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	public int Value
	{
		get
		{
			return m_value;
		}
		protected set
		{
			if (m_value != value)
			{
				m_value = value;
				OnValueChanged();
			}
		}
	}

	public event ValueChangedDelegate ValueChanged;

	private ObjectiveSkillLimit()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7743, InfoEvent.ETextAnnotationMode.MessageOpen, -1, new StringMapper { 
		{
			"failwarning_noskill",
			LocalizationManager.Instance.Get(SkillType.GetDefinition().NameLocaKey.Key)
		} }, true);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		SkillType = new Enum<SkillTypes>(base.Data.SkillName);
		m_value = 0;
	}

	public override void FrameUpdateImpl()
	{
		if (!IsActive || LogicMain.PlayerEntity == null)
		{
			return;
		}
		Value = LogicMain.MissionResult.GetSkillUsageCount(LogicMain.PlayerEntity.Skills.GetBuffedSkillDefinition(SkillType).Type.Name);
		if (base.Data.Invert)
		{
			if (Value < base.Data.Limit)
			{
				base.VisualTrackerState = EProgressState.Failed;
				base.Result = Mission.EObjectiveResult.Failed;
				base.VisualSummaryState = false;
				Complete();
			}
		}
		else if (Value > base.Data.Limit)
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = Mission.EObjectiveResult.Failed;
			base.VisualSummaryState = false;
			Complete();
		}
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		Skill = base.Data.SkillName;
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current", Value);
		stringMapper.Add("limit", base.Data.Limit);
		stringMapper.Add("limit+1", base.Data.Limit + 1);
		stringMapper.Add("limit-1", base.Data.Limit - 1);
		stringMapper.Add("count", base.Data.Limit);
		return stringMapper;
	}

	protected virtual void OnValueChanged()
	{
		ValueChangedDelegate valueChanged = this.ValueChanged;
		if (valueChanged != null)
		{
			valueChanged();
		}
	}
}
