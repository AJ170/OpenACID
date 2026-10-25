using AcierProtocol;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network.Missions.Mission.Data;
using Mission;

public class ObjectiveStayAlive : ObjectiveBase<ObjectiveStayAliveData>
{
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
			return Mission.EObjectiveResult.Fulfilled;
		}
	}

	protected override EProgressState InitialVisualTrackerState
	{
		get
		{
			return EProgressState.InProgress;
		}
	}

	public ObjectiveStayAlive()
	{
		base.Data.HideDescription = true;
	}

	protected override void ShowFailMessage()
	{
		base.ShowFailMessage();
		LogicMain.LogicInterface.OnSendInfo(EInfoEventType.TextAnnotation, 7747, InfoEvent.ETextAnnotationMode.MessageOpen, -1, null, true);
	}

	public override void PrepareRevive()
	{
		IsActive = Result == Mission.EObjectiveResult.Failed;
		base.PrepareRevive();
	}

	public override void TurnUpdateImpl()
	{
		if (LogicMain.PlayerEntity.GetIsDead())
		{
			base.VisualTrackerState = EProgressState.Failed;
			base.Result = Mission.EObjectiveResult.Failed;
			base.VisualSummaryState = false;
			Complete();
		}
	}

	public override string ToString()
	{
		return "Stay alive.";
	}
}
