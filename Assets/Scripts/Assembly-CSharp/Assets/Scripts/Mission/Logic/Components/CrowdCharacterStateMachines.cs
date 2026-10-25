using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class CrowdCharacterStateMachines : CharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterConditionCarrier
	{
		public CharacterCondition CharacterCondition { get; private set; }

		public CrowdCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			CharacterCondition = AddStateMachine(new CharacterCondition((Character)_entity));
		}
	}
}
