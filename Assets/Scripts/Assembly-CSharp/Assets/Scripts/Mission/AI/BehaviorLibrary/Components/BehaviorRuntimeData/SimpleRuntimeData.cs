namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components.BehaviorRuntimeData
{
	public class SimpleRuntimeData : IBehaviorRuntimeData
	{
		public BehaviorReturnCode ReturnCode { get; set; }

		public virtual void Reset()
		{
			ReturnCode = BehaviorReturnCode.NotCalled;
		}
	}
}
