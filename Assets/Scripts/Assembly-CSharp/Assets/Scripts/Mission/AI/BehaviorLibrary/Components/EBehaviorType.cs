namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public enum EBehaviorType
	{
		Invalid = 0,
		BehaviorAction = 1,
		BehaviorActionParameterized = 2,
		ParallelSelector = 3,
		ParallelSequence = 4,
		RandomSelector = 5,
		Selector = 6,
		Sequence = 7,
		Conditional = 8,
		ConditionalParameterized = 9,
		Branch = 10,
		Counter = 11,
		Inverter = 12,
		Timer = 13,
		Enforcer = 14,
		Wait = 15,
		Prolongator = 16
	}
}
