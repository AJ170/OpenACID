using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;

public class ObjectiveTimer : ObjectiveBase<ObjectiveTimerData>, IObjectiveTimer
{
	private float m_currentTime;

	public bool HasTimer
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
			return ToResult(base.Data.InitialResultWrapper.Result);
		}
	}

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return ToSummaryState(base.Data.InitialResultWrapper.Tracker);
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return ToTrackerState(base.Data.InitialResultWrapper.Tracker);
		}
	}

	private float CurrentTime
	{
		get
		{
			return m_currentTime;
		}
		set
		{
			if (!(Math.Abs(m_currentTime - value) < float.Epsilon))
			{
				m_currentTime = value;
				UpdateDescription();
			}
		}
	}

	private ObjectiveTimer()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7742, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void OnPreActivate()
	{
		base.OnPreActivate();
		CurrentTime = 0f;
	}

	public override void FrameUpdateImpl()
	{
		float currentTime = CurrentTime + LogicMain.FrameTime;
		CurrentTime = currentTime;
		if (!(CurrentTime < (float)base.Data.TimeInSeconds))
		{
			ObjectiveResultWrapper completionResultWrapper = base.Data.CompletionResultWrapper;
			base.VisualTrackerState = ToTrackerState(completionResultWrapper.Tracker);
			base.VisualSummaryState = ToSummaryState(completionResultWrapper.Tracker);
			base.Result = ToResult(completionResultWrapper.Result);
		}
	}

	protected override void Complete()
	{
		if (CurrentTime < (float)base.Data.TimeInSeconds)
		{
			ObjectiveResultWrapper terminationResultWrapper = base.Data.TerminationResultWrapper;
			base.VisualTrackerState = ToTrackerState(terminationResultWrapper.Tracker);
			base.VisualSummaryState = ToSummaryState(terminationResultWrapper.Tracker);
			base.Result = ToResult(terminationResultWrapper.Result);
		}
		base.Complete();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_currentTime = 0f;
	}

	public int GetTime()
	{
		return base.Data.TimeInSeconds - (int)CurrentTime;
	}

	public override string ToString()
	{
		return string.Format("Timer: {0}s", base.Data.TimeInSeconds);
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		TimeSpan timeSpan = TimeSpan.FromSeconds(GetTime());
		stringMapper.Add("minutes", timeSpan.Minutes);
		stringMapper.Add("seconds", timeSpan.Seconds);
		stringMapper.Add("total_seconds", timeSpan.TotalSeconds);
		stringMapper.Add("TotalSeconds", timeSpan.TotalSeconds);
		return stringMapper;
	}

	private static bool ToSummaryState(EObjectiveProgress _tracker)
	{
		switch (_tracker)
		{
		case EObjectiveProgress.Succeeded_EObjectiveProgress:
		case EObjectiveProgress.InRange_EObjectiveProgress:
		case EObjectiveProgress.InProgress_EObjectiveProgress:
			return true;
		default:
			return false;
		}
	}

	private static EProgressState ToTrackerState(EObjectiveProgress _tracker)
	{
		switch (_tracker)
		{
		case EObjectiveProgress.NotStarted_EObjectiveProgress:
			return EProgressState.NotStarted;
		case EObjectiveProgress.Succeeded_EObjectiveProgress:
			return EProgressState.Fulfilled;
		case EObjectiveProgress.Failed_EObjectiveProgress:
			return EProgressState.Failed;
		case EObjectiveProgress.InRange_EObjectiveProgress:
			return EProgressState.InRange;
		case EObjectiveProgress.InProgress_EObjectiveProgress:
			return EProgressState.InProgress;
		default:
			throw new ArgumentOutOfRangeException("_tracker");
		}
	}

	private Mission.EObjectiveResult ToResult(AcierProtocol.EObjectiveResult _initialResult)
	{
		switch (_initialResult)
		{
		case AcierProtocol.EObjectiveResult.Undefined_EObjectiveResult:
			return Mission.EObjectiveResult.Undefined;
		case AcierProtocol.EObjectiveResult.Succeeded_EObjectiveResult:
			return Mission.EObjectiveResult.Fulfilled;
		case AcierProtocol.EObjectiveResult.Failed_EObjectiveResult:
			return Mission.EObjectiveResult.Failed;
		default:
			throw new ArgumentOutOfRangeException("_initialResult");
		}
	}
}
