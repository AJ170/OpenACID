namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class StateMachineBaseBase
	{
		public static bool IsInStateChange { get; protected set; }

		public abstract EStateMachineType Type { get; }

		protected static void InitStateMachineBaseBase()
		{
		}
	}
}
