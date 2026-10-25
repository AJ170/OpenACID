using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class HirelingCharacterStateMachines : AssassinCharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IHirelingMainCarrier
	{
		public override AssassinApproachingTarget AssassinApproachingTarget { get; protected set; }

		public override IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; protected set; }

		public HirelingMain HirelingMain { get; private set; }

		public HirelingCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			HirelingMain = AddStateMachine(new HirelingMain((HirelingCharacter)_entity));
			AssassinApproachingTarget = AddStateMachine(new AssassinApproachingTarget((HirelingCharacter)_entity, HirelingMain));
			CharacterVitality = AddStateMachine(new CharacterVitality((HirelingCharacter)_entity));
		}
	}
}
