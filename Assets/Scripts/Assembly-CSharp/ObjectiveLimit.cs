using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UbiCore.Utils;

public class ObjectiveLimit : ObjectiveBase<ObjectiveLimitData>, IMonitoredHidingBehaviour, IObjectiveValue
{
	private int m_initialCount;

	private int m_value;

	private int m_displayValue;

	public int DisplayValue
	{
		get
		{
			return m_displayValue;
		}
		protected set
		{
			if (m_displayValue != value)
			{
				m_displayValue = value;
				UpdateDescription();
				OnValueChanged();
			}
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
				UpdateDescription();
				OnValueChanged();
			}
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return (!base.Data.InvertedResult) ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Undefined;
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return !base.Data.InvertedResult;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.InvertedResult ? EProgressState.InProgress : EProgressState.Fulfilled;
		}
	}

	private int CurrentCount
	{
		get
		{
			int num = -m_initialCount;
			switch (base.Data.LimitType)
			{
			case ELimitType.Roof:
				num += LogicMain.MissionResult.EnterRoofCount;
				break;
			case ELimitType.Ground:
				num += LogicMain.MissionResult.EnterGroundCount;
				break;
			case ELimitType.Hiding:
				num += LogicMain.MissionResult.UsedHidingSpotCount;
				break;
			case ELimitType.Fleeing:
				num += LogicMain.MissionResult.FledCount;
				break;
			case ELimitType.Skills:
				num += LogicMain.MissionResult.UsedSkillsCount;
				break;
			case ELimitType.Combat:
				num += LogicMain.MissionResult.CombatCount;
				break;
			case ELimitType.KilledUnitsInCombat:
				num += LogicMain.MissionResult.EnemiesKilledInCombatCount;
				break;
			case ELimitType.KilledUnitsTotal:
				num += LogicMain.MissionResult.EnemiesKilledCount;
				break;
			case ELimitType.KilledUnitsByAssassination:
				num += LogicMain.MissionResult.AssassinationCount;
				break;
			case ELimitType.HotspotHaystack:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Haystack);
				break;
			case ELimitType.HotspotBench:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Bench);
				break;
			case ELimitType.HotspotRooftopGarden:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_RooftopGarden);
				break;
			case ELimitType.HotspotCivilianCover:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_CivilianCover);
				break;
			case ELimitType.HotspotDoctor:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Doctor);
				break;
			case ELimitType.HotspotCaptives:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Captives);
				break;
			case ELimitType.HotspotChests:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Chests);
				break;
			case ELimitType.HotspotPigeonCoup:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_PigeonCoup);
				break;
			case ELimitType.HotspotCourtesans:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Courtesans);
				break;
			case ELimitType.HotspotThieves:
				num += LogicMain.MissionResult.GetUsedHotspotCount(EHotspot.EHotspot_Thieves);
				break;
			case ELimitType.NotorietyAnonymous:
				num += LogicMain.MissionResult.GetNotoriety(EAlertLevel.Anonymous);
				break;
			case ELimitType.NotorietyAlterted:
				num += LogicMain.MissionResult.GetNotoriety(EAlertLevel.Alerted);
				break;
			case ELimitType.NotorietyExposed:
				num += LogicMain.MissionResult.GetNotoriety(EAlertLevel.Exposed);
				break;
			case ELimitType.LowestBaseHealthPercent:
				num = (int)((!(LogicMain.MissionResult.LowestBaseHealthMax > 0f)) ? 100f : (LogicMain.MissionResult.LowestBaseHealth / LogicMain.MissionResult.LowestBaseHealthMax * 100f));
				break;
			case ELimitType.LostCaptives:
				num += LogicMain.MissionResult.LostCaptivesCount;
				break;
			case ELimitType.ExecutedKillMoves:
				num += LogicMain.MissionResult.EnemiesKilledWithCombo;
				break;
			case ELimitType.SuccessfullyParried:
				num += LogicMain.MissionResult.SuccessfullyParried;
				break;
			case ELimitType.SwitchedTarget:
				num += LogicMain.MissionResult.SwitchedTarget;
				break;
			case ELimitType.SuccessfullHirelingAttacks:
				num += LogicMain.MissionResult.SuccessfullHirelingAttacks;
				break;
			}
			return num;
		}
	}

	public int CurrentDisplayedCount
	{
		get
		{
			ELimitType limitType = base.Data.LimitType;
			if (limitType == ELimitType.LowestBaseHealthPercent)
			{
				if (LogicMain.CurrentTurn <= 1)
				{
					return 100;
				}
				return (int)((!(LogicMain.MissionResult.LowestBaseHealthMax > 0f)) ? 100f : (LogicMain.PlayerEntity.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_Health) / LogicMain.MissionResult.LowestBaseHealthMax * 100f));
			}
			return CurrentCount;
		}
	}

	public bool PreventFailingOnIncrease
	{
		get
		{
			return base.Data.PreventFailingOnIncrease;
		}
	}

	public int Available
	{
		get
		{
			ELimitType limitType = base.Data.LimitType;
			if (limitType == ELimitType.HotspotCaptives || limitType == ELimitType.LostCaptives)
			{
				return LogicMain.MissionInfo.CaptiveCount;
			}
			return 0;
		}
	}

	public event ValueChangedDelegate ValueChanged;

	private ObjectiveLimit()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		switch (base.Data.LimitType)
		{
		case ELimitType.Combat:
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7736, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
			break;
		case ELimitType.NotorietyExposed:
		case ELimitType.NotorietyAlterted:
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7737, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
			break;
		case ELimitType.LowestBaseHealthPercent:
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7748, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
			break;
		default:
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7744, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
			break;
		}
	}

	public override void FrameUpdateImpl()
	{
		if (IsActive && LogicMain.PlayerEntity != null)
		{
			Value = CurrentCount;
			DisplayValue = CurrentDisplayedCount;
			if (EvaluateResult())
			{
				Complete();
			}
		}
	}

	private bool EvaluateResult()
	{
		if (!((!base.Data.FlipComparer) ? ((float)CurrentCount >= base.Data.Limit) : ((float)CurrentCount < base.Data.Limit)))
		{
			return false;
		}
		base.VisualTrackerState = ((!base.Data.InvertedResult) ? EProgressState.Failed : EProgressState.Fulfilled);
		base.VisualSummaryState = base.Data.InvertedResult;
		base.Result = (base.Data.InvertedResult ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Failed);
		return true;
	}

	public bool MonitorsLimitType(ELimitType _limitType)
	{
		return _limitType == base.Data.LimitType || (base.Data.LimitType == ELimitType.Hiding && IsHidingLimitType(_limitType));
	}

	public override void OnActivate()
	{
		m_initialCount = CurrentCount;
		m_value = 0;
		switch (base.Data.LimitType)
		{
		case ELimitType.LowestBaseHealthPercent:
			LogicMain.MissionResult.ResetHealthStats();
			break;
		case ELimitType.Hiding:
			UbiCore.Utils.Singleton<MissionManager>.Instance.ObjectivesRelyingOnHidingSpotsCount++;
			break;
		}
		base.OnActivate();
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		ELimitType limitType = base.Data.LimitType;
		if (limitType == ELimitType.Hiding)
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.ObjectivesRelyingOnHidingSpotsCount--;
		}
	}

	public override string ToString()
	{
		return string.Format("{0}{1} less {2}", (!base.Data.FlipComparer) ? string.Empty : "Not ", base.Data.LimitType, base.Data.Limit);
	}

	public bool WillFailOnIncrease(int _increase)
	{
		return (float)(CurrentCount + _increase) >= base.Data.Limit;
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current", CurrentDisplayedCount);
		stringMapper.Add("limit", base.Data.Limit);
		stringMapper.Add("limit+1", base.Data.Limit + 1f);
		stringMapper.Add("limit-1", base.Data.Limit - 1f);
		stringMapper.Add("available", Available);
		stringMapper.Add("Count", base.Data.Limit);
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

	private static bool IsHidingLimitType(ELimitType _limitType)
	{
		return _limitType == ELimitType.Hiding || _limitType == ELimitType.HotspotHaystack || _limitType == ELimitType.HotspotBench || _limitType == ELimitType.HotspotRooftopGarden || _limitType == ELimitType.HotspotCivilianCover || _limitType == ELimitType.HotspotCourtesans || _limitType == ELimitType.HotspotThieves;
	}
}
