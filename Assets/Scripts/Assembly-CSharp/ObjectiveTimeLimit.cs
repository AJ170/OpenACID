using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UnityEngine;

public class ObjectiveTimeLimit : ObjectiveBase<ObjectiveTimeLimitData>, IObjectiveTimer
{
	private float m_currentTime;

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return !base.Data.WaitForTimerEnd;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return (!base.Data.WaitForTimerEnd) ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Undefined;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

	public bool HasTimer
	{
		get
		{
			return true;
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

	private ObjectiveTimeLimit()
	{
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7742, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void PrepareRevive()
	{
		base.PrepareRevive();
		CurrentTime = Mathf.Max(CurrentTime - (float)base.Data.TimeForRetry, 0f);
	}

	public override void FrameUpdateImpl()
	{
		float currentTime = CurrentTime + LogicMain.FrameTime;
		CurrentTime = currentTime;
		if (CurrentTime < (float)base.Data.TimeInSeconds)
		{
			return;
		}
		if (base.Data.Invert)
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.VisualSummaryState = true;
			if (base.Data.WaitForTimerEnd)
			{
				base.Result = Mission.EObjectiveResult.Fulfilled;
			}
		}
		else
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			base.Result = Mission.EObjectiveResult.Failed;
		}
		Complete();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		m_currentTime = 0f;
	}

	protected override void Complete()
	{
		if (Result == Mission.EObjectiveResult.Undefined)
		{
			base.VisualTrackerState = ((!base.Data.Invert) ? EProgressState.Fulfilled : EProgressState.Failed);
			base.VisualSummaryState = !base.Data.Invert;
		}
		base.Complete();
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

	public override string ToString()
	{
		if (!base.Data.Invert)
		{
			return string.Format("Timer: {0}s", base.Data.TimeInSeconds);
		}
		return string.Format("Counter: T-{0}s", base.Data.TimeInSeconds);
	}

	public int GetTime()
	{
		return base.Data.TimeInSeconds - (int)CurrentTime;
	}
}
