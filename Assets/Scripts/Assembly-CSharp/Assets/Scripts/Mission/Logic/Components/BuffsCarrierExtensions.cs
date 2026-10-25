using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public static class BuffsCarrierExtensions
	{
		public static void TriggerCharacterConditionChange(this IBuffsCarrier _buffsCarrier)
		{
			IStateMachinesCarrier stateMachinesCarrier = _buffsCarrier as IStateMachinesCarrier;
			if (stateMachinesCarrier != null)
			{
				ICharacterConditionCarrier characterConditionCarrier = stateMachinesCarrier.StateMachines as ICharacterConditionCarrier;
				if (characterConditionCarrier != null)
				{
					characterConditionCarrier.CharacterCondition.ExternalChange();
				}
			}
		}
	}
}
