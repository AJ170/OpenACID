using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Selection;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PlayerCharacterStateMachines : AssassinCharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IPlayerAlertlevelCarrier, IPlayerVisionCarrier
	{
		public override AssassinApproachingTarget AssassinApproachingTarget { get; protected set; }

		public override IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; protected set; }

		public PlayerVision PlayerVision { get; private set; }

		public PlayerAlertLevel PlayerAlertLevel { get; private set; }

		public PlayerCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			PlayerVision = AddStateMachine(new PlayerVision((PlayerCharacter)_entity));
			PlayerAlertLevel = AddStateMachine(new PlayerAlertLevel((PlayerCharacter)_entity));
			AssassinApproachingTarget = AddStateMachine(new AssassinApproachingTarget((PlayerCharacter)_entity, UbiCore.Utils.Singleton<SelectionManager>.Instance));
			CharacterVitality = AddStateMachine(new PlayerVitality((PlayerCharacter)_entity));
		}
	}
}
