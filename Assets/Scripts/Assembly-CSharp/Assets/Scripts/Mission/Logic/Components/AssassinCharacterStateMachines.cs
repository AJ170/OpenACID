using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class AssassinCharacterStateMachines : CharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, IPlayerApproachingTargetCarrier, ICharacterConditionCarrier, ICharacterDrawWeaponCarrier, ICharacterDrawMeleeWeaponCarrier, ICharacterVitalityCarrier, IPlayerMainCarrier, IPlayerProfileCarrier
	{
		public PlayerMain PlayerMain { get; private set; }

		public PlayerProfile PlayerProfile { get; private set; }

		public abstract AssassinApproachingTarget AssassinApproachingTarget { get; protected set; }

		public CharacterDrawWeaponBase[] CharacterDrawWeapon { get; private set; }

		public CharacterDrawWeaponMelee CharacterDrawMeleeWeapon
		{
			get
			{
				return (CharacterDrawWeaponMelee)CharacterDrawWeapon[0];
			}
		}

		public PlayerDisguise PlayerDisguise { get; private set; }

		public PlayerStalkingZone PlayerStalkingZone { get; private set; }

		public abstract IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; protected set; }

		public CharacterCondition CharacterCondition { get; private set; }

		protected AssassinCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			CharacterCondition = AddStateMachine(new CharacterCondition((AssassinCharacter)_entity));
			PlayerMain = AddStateMachine(new PlayerMain((AssassinCharacter)_entity));
			PlayerProfile = AddStateMachine(new PlayerProfile((AssassinCharacter)_entity));
			CharacterDrawWeapon = new CharacterDrawWeaponBase[1];
			CharacterDrawWeapon[0] = AddStateMachine(new CharacterDrawWeaponMelee((AssassinCharacter)_entity, EWeaponType.Sword));
			PlayerDisguise = AddStateMachine(new PlayerDisguise((AssassinCharacter)_entity));
			PlayerStalkingZone = AddStateMachine(new PlayerStalkingZone((AssassinCharacter)_entity));
		}
	}
}
