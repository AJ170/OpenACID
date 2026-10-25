using AcierProtocol;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveDeliverNotoriety : ObjectiveTarget<ObjectiveDeliverNotorietyData>
{
	private const float FleeTragetCloseDist = 1f;

	private const float FleeTragetCloseDistSqr = 1f;

	private const EStateType c_Alerted = EStateType.CharacterInteractionMain_MissionAlerted;

	private EAlertLevel m_currentNotoriety;

	private EAlertLevel CurrentNotoriety
	{
		get
		{
			return m_currentNotoriety;
		}
		set
		{
			if (m_currentNotoriety != value)
			{
				m_currentNotoriety = value;
				UpdateDescription();
			}
		}
	}

	private InteractionCharacter InteractionCharacter
	{
		get
		{
			return base.RelevantEntities[0].GetEntity<InteractionCharacter>();
		}
	}

	public MissionTarget FleeTarget { get; set; }

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return false;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.NotStarted;
		}
	}

	private ObjectiveDeliverNotoriety()
	{
	}

	public override void OnPostRead(MissionBaseData _nodeData)
	{
		base.OnPostRead(_nodeData);
		if (base.Data.FleeTargetID != -2)
		{
			FleeTarget = GetMissionObject(base.Data.FleeTargetID) as MissionTarget;
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_currentNotoriety = EAlertLevel.Anonymous;
		InteractionCharacter entity = base.RelevantEntities[0].GetEntity<InteractionCharacter>();
		entity.InteractionCharacterStats.Set_bool(StatTypes.EStatType.MissionCharacter_FleeOnTransgression, FleeTarget != null && base.Data.FleeOnTransgression);
		if (FleeTarget != null)
		{
			entity.InteractionCharacterStats.Set_Vector3(StatTypes.EStatType.MissionCharacter_FleePosition, FleeTarget.transform.position);
		}
		float value = 0f;
		switch (base.Data.MaxNotoriety)
		{
		case EAlertLevel.Alerted:
			value = 0.5f;
			break;
		case EAlertLevel.Exposed:
			value = 0.99f;
			break;
		}
		entity.InteractionCharacterStats.Set_float(StatTypes.EStatType.MissionCharacter_MaxNotoriety, value);
	}

	public override void FrameUpdateImpl()
	{
		if (!IsActive)
		{
			return;
		}
		Mission.EObjectiveResult result = Mission.EObjectiveResult.Fulfilled;
		EProgressState visualTrackerState = EProgressState.Fulfilled;
		bool visualSummaryState = true;
		CurrentNotoriety = (UbiCore.Utils.Singleton<UiMain>.Instance.IsRunning ? UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel : EAlertLevel.Anonymous);
		InteractionCharacterMain interactionCharacterMain = InteractionCharacter.InteractionCharacterStateMachines.InteractionCharacterMain;
		if (!InteractionCharacter.EntityStats.Get_bool(StatTypes.EStatType.Interaction_Triggered))
		{
			if (interactionCharacterMain.CurrentStateType == EStateType.CharacterInteractionMain_MissionAlerted)
			{
				if (base.Data.FleeOnTransgression && Vector3.SqrMagnitude(FleeTarget.transform.position - InteractionCharacter.Position) < 1f)
				{
					result = Mission.EObjectiveResult.Failed;
					visualTrackerState = EProgressState.Failed;
					visualSummaryState = false;
				}
			}
			else if (CurrentNotoriety >= base.Data.MaxNotoriety)
			{
				result = Mission.EObjectiveResult.Undefined;
				visualTrackerState = EProgressState.NotStarted;
				visualSummaryState = false;
			}
		}
		base.VisualTrackerState = visualTrackerState;
		base.VisualSummaryState = visualSummaryState;
		base.Result = result;
	}

	public override StringMapper GetStringMapper()
	{
		EAlertLevel key = ((base.RelevantEntities.Count > 0) ? CurrentNotoriety : EAlertLevel.Anonymous);
		string value = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AlertLevelDescriptions[(int)base.Data.MaxNotoriety].DescriptionId);
		string value2 = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AlertLevelDescriptions[(int)key].DescriptionId);
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("notoriety_current", value2);
		stringMapper.Add("notoriety_maximum", value);
		stringMapper.Add("Current", value2);
		stringMapper.Add("Threshold", value);
		return stringMapper;
	}

	protected override bool IsEntityToBeRegistered(GameEntity _entity, IMissionEntity _missionEntity)
	{
		return _entity is InteractionCharacter;
	}

	public override string ToString()
	{
		string text = base.ToString() + "\n";
		string arg = ((!base.Data.FleeOnTransgression) ? "Reject" : "Flee");
		text += string.Format("{0} if notoriety at least {1}", arg, base.Data.MaxNotoriety);
		if (base.Data.FleeOnTransgression)
		{
			text += string.Format("\nFlee to: {0}", FleeTarget);
		}
		return text;
	}
}
