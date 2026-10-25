using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UnityEngine;

public abstract class ObjectiveStayInRange<TData> : ObjectiveBase<TData>, IObjectiveTimer where TData : ObjectiveStayInRangeData, new()
{
	private bool m_isInside;

	private float m_inverseTime;

	private float m_inverseSecondaryTime;

	private double m_curDistance;

	public float GetBackInRangeTimer { get; private set; }

	public bool HasTimer
	{
		get
		{
			bool flag = !IsInside && GetRangeTimer() >= 0f;
			bool flag2 = IsInside && GetStayTimer() >= 0f;
			return VisualTrackerState != 0 && (flag || flag2);
		}
	}

	public float StayInRangeTimer { get; private set; }

	public virtual int FailMessageId
	{
		get
		{
			return 7738;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.NotStarted;
		}
	}

	protected override Mission.EObjectiveResult InitialResult
	{
		get
		{
			return Mission.EObjectiveResult.Undefined;
		}
	}

	protected bool IsInside
	{
		get
		{
			return m_isInside;
		}
		set
		{
			if (m_isInside != value)
			{
				m_isInside = value;
				if (!m_isInside)
				{
					GetBackInRangeTimer = 0f;
				}
				else if (VisualTrackerState == EProgressState.NotStarted)
				{
					base.VisualTrackerState = EProgressState.InRange;
				}
			}
		}
	}

	protected abstract Vector3 TargetPosition { get; }

	protected abstract float TargetRange { get; }

	private bool HasStayTimer
	{
		get
		{
			return base.Data.StayTimerSeconds >= 0;
		}
	}

	private float InverseTime
	{
		get
		{
			return m_inverseTime;
		}
		set
		{
			if (!(Math.Abs(m_inverseTime - value) < float.Epsilon))
			{
				m_inverseTime = value;
				UpdateDescription();
			}
		}
	}

	private float InverseSecondaryTime
	{
		get
		{
			return m_inverseSecondaryTime;
		}
		set
		{
			if (!(Math.Abs(m_inverseSecondaryTime - value) < float.Epsilon))
			{
				m_inverseSecondaryTime = value;
				UpdateDescription();
			}
		}
	}

	private double CurDistance
	{
		get
		{
			return m_curDistance;
		}
		set
		{
			if (!(Math.Abs(m_curDistance - value) < double.Epsilon))
			{
				m_curDistance = value;
				UpdateDescription();
			}
		}
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, FailMessageId, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void FrameUpdateImpl()
	{
		if (LogicMain.Player == null)
		{
			return;
		}
		IsInside = GetComputeIsInRange();
		InverseTime = GetRangeTimer();
		InverseSecondaryTime = GetStayTimer();
		if (!IsInside)
		{
			GetBackInRangeTimer += LogicMain.FrameTime;
		}
		if (VisualTrackerState != 0)
		{
			if (HasStayTimer)
			{
				StayInRangeTimer += LogicMain.FrameTime;
			}
			if (HasStayTimer && GetStayTimer() < 0f)
			{
				base.VisualTrackerState = EProgressState.Fulfilled;
				base.Result = Mission.EObjectiveResult.Fulfilled;
				Complete();
			}
		}
		UpdateTargetPositions();
	}

	public override void OnPreActivate()
	{
		m_isInside = false;
		m_inverseTime = 0f;
		m_inverseSecondaryTime = 0f;
		m_curDistance = 0.0;
		GetBackInRangeTimer = 0f;
		StayInRangeTimer = 0f;
		base.OnPreActivate();
	}

	protected virtual bool GetComputeIsInRange()
	{
		float sqrMagnitude = (LogicMain.Player.transform.position - TargetPosition).sqrMagnitude;
		CurDistance = Math.Sqrt(sqrMagnitude);
		float num = TargetRange * TargetRange;
		return sqrMagnitude <= num;
	}

	public int GetTime()
	{
		return (!IsInside) ? Mathf.CeilToInt(GetRangeTimer()) : Mathf.CeilToInt(GetStayTimer());
	}

	public override string ToString()
	{
		return "Range: " + TargetRange;
	}

	protected override void Complete()
	{
		switch (VisualTrackerState)
		{
		case EProgressState.InProgress:
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = Mission.EObjectiveResult.Failed;
			break;
		case EProgressState.InRange:
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.Result = Mission.EObjectiveResult.Fulfilled;
			break;
		}
		base.Complete();
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		stringMapper.Add("current_distance", CurDistance);
		stringMapper.Add("minutes_to_reach", (int)(InverseTime / 60f));
		stringMapper.Add("seconds_to_reach", (int)(InverseTime % 60f));
		stringMapper.Add("total_seconds_to_reach", (int)InverseTime);
		stringMapper.Add("minutes_to_stay", (int)(InverseSecondaryTime / 60f));
		stringMapper.Add("seconds_to_stay", (int)(InverseSecondaryTime % 60f));
		stringMapper.Add("total_seconds_to_stay", (int)InverseSecondaryTime);
		stringMapper.Add("Current", CurDistance);
		stringMapper.Add("Minutes", (int)(InverseTime / 60f));
		stringMapper.Add("Seconds", (int)(InverseTime % 60f));
		stringMapper.Add("TotalSeconds", (int)InverseTime);
		stringMapper.Add("SecondaryMinutes", (int)(InverseSecondaryTime / 60f));
		stringMapper.Add("SecondarySeconds", (int)(InverseSecondaryTime % 60f));
		stringMapper.Add("SecondaryTotalSeconds", (int)InverseSecondaryTime);
		return stringMapper;
	}

	protected void Invalidate()
	{
		if (VisualTrackerState != EProgressState.InProgress)
		{
			GetBackInRangeTimer = 0f;
		}
		base.VisualTrackerState = EProgressState.InProgress;
	}

	private float GetRangeTimer()
	{
		return (float)base.Data.TimerSeconds - GetBackInRangeTimer;
	}

	private float GetStayTimer()
	{
		return (float)base.Data.StayTimerSeconds - StayInRangeTimer;
	}

	private void UpdateTargetPositions()
	{
		TargetPositions.ClearIfNotEmpty();
		if (ObjectiveData.MapIcon != 0)
		{
			TargetPositions.Add(TargetPosition);
		}
	}
}
