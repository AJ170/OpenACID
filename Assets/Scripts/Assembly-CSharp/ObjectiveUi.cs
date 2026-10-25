using AcierProtocol;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;

public class ObjectiveUi : ObjectiveBase<ObjectiveUiData>
{
	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return base.Data.WaitForInput ? EProgressState.InProgress : EProgressState.Fulfilled;
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
			return (!base.Data.WaitForInput) ? Mission.EObjectiveResult.Fulfilled : Mission.EObjectiveResult.Undefined;
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		switch (base.Data.Action)
		{
		case EAction.Hide:
			LogicMain.LogicInterface.Tutorial_WidgetVisible(base.Data.TargetPath, false);
			break;
		case EAction.Block:
			LogicMain.LogicInterface.Tutorial_WidgetBlocked(base.Data.TargetPath, true);
			break;
		case EAction.Highlight:
			LogicMain.LogicInterface.Tutorial_WidgetHighlight(base.Data.TargetPath, true);
			break;
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		switch (base.Data.Action)
		{
		case EAction.Hide:
			LogicMain.LogicInterface.Tutorial_WidgetVisible(base.Data.TargetPath, true);
			break;
		case EAction.Block:
			LogicMain.LogicInterface.Tutorial_WidgetBlocked(base.Data.TargetPath, false);
			break;
		case EAction.Highlight:
			LogicMain.LogicInterface.Tutorial_WidgetHighlight(base.Data.TargetPath, false);
			break;
		}
	}

	public override void FrameUpdateImpl()
	{
	}

	public void OnWidgetPress(string _path)
	{
		if (base.Data.TargetPath.StartsWith(_path) && base.Data.WaitForInput)
		{
			base.VisualTrackerState = EProgressState.Fulfilled;
			base.Result = Mission.EObjectiveResult.Fulfilled;
		}
	}
}
