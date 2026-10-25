using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;

public class ObjectiveVoid : ObjectiveBase<ObjectiveVoidData>
{
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
			return true;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.Fulfilled;
		}
	}

	private ObjectiveVoid()
	{
	}

	public override void FrameUpdateImpl()
	{
		base.FrameUpdateImpl();
		base.Result = EObjectiveResult.Fulfilled;
	}

	public override string ToString()
	{
		return "Empty objective";
	}
}
