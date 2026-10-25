namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData
{
	public class CounterRuntimeData : SimpleRuntimeData
	{
		public int Value { get; set; }

		public float ResetTime { get; set; }

		public override void Reset()
		{
			base.Reset();
		}
	}
}
