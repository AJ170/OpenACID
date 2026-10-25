namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public interface IBehaviorRuntimeData
	{
		BehaviorReturnCode ReturnCode { get; set; }

		void Reset();
	}
}
