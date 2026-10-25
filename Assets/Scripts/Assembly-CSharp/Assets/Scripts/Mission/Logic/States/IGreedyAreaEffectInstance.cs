namespace Assets.Scripts.Mission.Logic.States
{
	public interface IGreedyAreaEffectInstance
	{
		void FindNewTarget(GreedyConditionState.Payload _payload);

		void ReachedTarget(GreedyConditionState.Payload _payload);
	}
}
