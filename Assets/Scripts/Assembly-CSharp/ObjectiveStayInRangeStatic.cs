using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Localization;
using Mission;
using UnityEngine;

public class ObjectiveStayInRangeStatic : ObjectiveStayInRange<ObjectiveStayInRangeStaticData>
{
	public MissionTarget Target;

	protected override bool InitialVisualSummaryState
	{
		get
		{
			return true;
		}
	}

	protected override float TargetRange
	{
		get
		{
			return (!(Target != null)) ? (-1f) : Target.Data.Range;
		}
	}

	protected override Vector3 TargetPosition
	{
		get
		{
			return (!(Target != null)) ? Vector3.zero : Target.transform.position;
		}
	}

	private ObjectiveStayInRangeStatic()
	{
	}

	public override void OnReadCompleted()
	{
		base.OnReadCompleted();
		Target = GetMissionObject(base.Data.TargetID) as MissionTarget;
	}

	public override void FrameUpdateImpl()
	{
		if (LogicMain.Player == null || Target == null)
		{
			return;
		}
		base.FrameUpdateImpl();
		if (VisualTrackerState == EProgressState.NotStarted)
		{
			return;
		}
		if (base.IsInside)
		{
			base.VisualTrackerState = EProgressState.InRange;
			return;
		}
		Invalidate();
		if (!(base.GetBackInRangeTimer <= (float)base.Data.TimerSeconds))
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.VisualSummaryState = false;
			base.Result = EObjectiveResult.Failed;
			Complete();
		}
	}

	public override StringMapper GetStringMapper()
	{
		StringMapper stringMapper = base.GetStringMapper();
		if (Target != null)
		{
			stringMapper.Add("target_distance_maximum", Target.Data.Range);
			stringMapper.Add("threshold", Target.Data.Range);
		}
		return stringMapper;
	}
}
