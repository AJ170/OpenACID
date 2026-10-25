namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData
{
	public class TimerRuntimeData : SimpleRuntimeData
	{
		public float EndTime { get; set; }

		public TimerRuntimeData()
		{
			EndTime = -1f;
		}
	}
}
