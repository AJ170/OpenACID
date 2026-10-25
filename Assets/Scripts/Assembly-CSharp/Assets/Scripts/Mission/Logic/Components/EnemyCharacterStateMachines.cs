using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Database;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class EnemyCharacterStateMachines : CharacterStateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, ICharacterAwarenessCarrier, ICharacterConditionCarrier, ICharacterDrawWeaponCarrier, ICharacterDrawMeleeWeaponCarrier, ICharacterDrawRangedWeaponCarrier, ICharacterVitalityCarrier, IEnemyMainCarrier
	{
		public CharacterAwareness CharacterAwareness { get; private set; }

		public CharacterDrawWeaponBase[] CharacterDrawWeapon { get; private set; }

		public CharacterDrawWeaponMelee CharacterDrawMeleeWeapon
		{
			get
			{
				return (CharacterDrawWeaponMelee)CharacterDrawWeapon[0];
			}
		}

		public CharacterDrawWeaponRanged CharacterDrawRangedWeapon
		{
			get
			{
				return (CharacterDrawWeaponRanged)CharacterDrawWeapon[1];
			}
		}

		public EnemyMain EnemyMain { get; private set; }

		public IStateMachine<ICharacterVitalityCarrierEntity> CharacterVitality { get; private set; }

		public CharacterCondition CharacterCondition { get; private set; }

		public EnemyCharacterStateMachines(IEntity _entity)
			: base(_entity)
		{
			CharacterCondition = AddStateMachine(new CharacterCondition((Character)_entity));
			EnemyMain = AddStateMachine(new EnemyMain((EnemyCharacter)_entity));
			CharacterAwareness = AddStateMachine(new CharacterAwareness((NpcCharacter)_entity));
			CharacterVitality = AddStateMachine(new EnemyVitality((EnemyCharacter)_entity));
			CharacterDrawWeapon = new CharacterDrawWeaponBase[2];
			CharacterDrawWeapon[0] = AddStateMachine(new CharacterDrawWeaponMelee((EnemyCharacter)_entity, EWeaponType.Sword));
		}

		public override void OnRegistered()
		{
			EnemyCharacter enemyCharacter = (EnemyCharacter)Entity;
			NpcData.INpcDefinition definition = enemyCharacter.NpcDefinition.Definition;
			if (GameID.IsValid(definition.RangedWeaponGameID))
			{
				ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(definition.RangedWeaponGameID);
				ItemDef.EWeaponTypes weaponType = item.WeaponType;
				EWeaponType type = WeaponHandler.WeaponTypesToType(weaponType);
				CharacterDrawWeapon[1] = AddStateMachine(new CharacterDrawWeaponRanged(enemyCharacter, type));
			}
			base.OnRegistered();
		}
	}
}
