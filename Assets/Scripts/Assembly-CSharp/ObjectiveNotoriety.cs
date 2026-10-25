using AcierProtocol;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using Mission;
using UbiCore.Utils;
using UnityEngine;

public class ObjectiveNotoriety : ObjectiveBase<ObjectiveNotorietyData>, IObjectiveTimer
{
	private float m_timer;

	private EAlertLevel m_currentAlertLevel;

	private int m_remainingTime;

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
			return EProgressState.InProgress;
		}
	}

	private int RemainingTime
	{
		get
		{
			return m_remainingTime;
		}
		set
		{
			if (m_remainingTime != value)
			{
				m_remainingTime = value;
				UpdateDescription();
			}
		}
	}

	private bool IsMatch
	{
		get
		{
			return base.Data.Comparer.IsTrue(m_currentAlertLevel, base.Data.MaxNotoriety);
		}
	}

	public bool HasTimer
	{
		get
		{
			return false;
		}
	}

	private bool HasTimerInternal
	{
		get
		{
			return base.Data.StayBelowSec >= 0 && IsMatch;
		}
	}

	private ObjectiveNotoriety()
	{
	}

	protected override void ShowFailMessage()
	{
		if (!IsMatch)
		{
			base.ShowFailMessage();
			LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7737, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		ResetTimer();
		m_remainingTime = 0;
		m_currentAlertLevel = EAlertLevel.Anonymous;
	}

	public override void PrepareRevive()
	{
		base.PrepareRevive();
		IStatValue statValue = LogicMain.PlayerEntity.EntityStats.GetStatValue(StatTypes.EStatType.Character_Conspicuous_Notoriety);
		switch (base.Data.MaxNotoriety)
		{
		case EAlertLevel.Anonymous:
			statValue.Value_float = 0f;
			break;
		case EAlertLevel.Exposed:
			statValue.Value_float = Mathf.Min(0.5f, statValue.Value_float);
			break;
		case EAlertLevel.Alerted:
			break;
		}
	}

	public override void FrameUpdateImpl()
	{
		if (!IsActive || LogicMain.PlayerEntity == null)
		{
			return;
		}
		m_currentAlertLevel = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel;
		if (IsMatch)
		{
			RemainingTime = GetTime();
			if (HasTimerInternal && RemainingTime <= 0)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.VisualSummaryState = true;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				UpdateDescription();
			}
		}
		else if (base.Data.ResetIfInvalidated)
		{
			ResetTimer();
			base.VisualTrackerState = InitialVisualTrackerState;
			base.VisualSummaryState = InitialVisualSummaryState;
			base.Result = InitialResult;
			UpdateDescription();
		}
		else if (base.Data.FailOnTransgression)
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = Mission.EObjectiveResult.Failed;
			Complete();
		}
		else
		{
			ResetTimer();
		}
	}

	protected override void Complete()
	{
		if (base.Data.ResetIfInvalidated)
		{
			switch (VisualTrackerState)
			{
			case EProgressState.Failed:
				base.Result = Mission.EObjectiveResult.Failed;
				break;
			case EProgressState.Fulfilled:
				base.Result = Mission.EObjectiveResult.Fulfilled;
				break;
			}
		}
		base.Complete();
	}

	public override StringMapper GetStringMapper()
	{
		string value = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AlertLevelDescriptions[(int)base.Data.MaxNotoriety].DescriptionId);
		string value2 = LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.AlertLevelDescriptions[(int)m_currentAlertLevel].DescriptionId);
		int time = GetTime();
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("alert_level_current", value2);
		stringMapper.Add("alert_level_maximum", value);
		stringMapper.Add("timer", (time <= 0) ? string.Empty : (": " + time));
		stringMapper.Add("alert_timer", (time <= 0) ? string.Empty : (": " + time));
		stringMapper.Add("Current", value2);
		stringMapper.Add("Threshold", value);
		stringMapper.Add("Seconds", (time <= 0) ? string.Empty : (": " + time));
		return stringMapper;
	}

	private void ResetTimer()
	{
		m_timer = LogicMain.CurrentTime;
		UpdateDescription();
	}

	public override string ToString()
	{
		return string.Format("{0} {1}", base.Data.Comparer, base.Data.MaxNotoriety);
	}

	public int GetTime()
	{
		return base.Data.StayBelowSec - Mathf.FloorToInt(LogicMain.CurrentTime - m_timer);
	}
}
