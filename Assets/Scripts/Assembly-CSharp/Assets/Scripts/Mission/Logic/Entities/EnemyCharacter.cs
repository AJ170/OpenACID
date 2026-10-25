using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class EnemyCharacter : NpcCharacter, IParticipantCarrier, ICharacterStatsCarrier, IDefinitionCarrier, IEnemyNpcStatsCarrier, ICharacterStateMachinesCarrier, IEnemyCharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHeadIconCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, ICharacterConditionCarrierEntity, ICharacterVitalityCarrierEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.EnemyCharacter;
			}
		}

		public override CharacterStats CharacterStats
		{
			get
			{
				return EnemyNpcStats;
			}
		}

		public EnemyNpcStats EnemyNpcStats { get; private set; }

		public ICharacterConditionCarrier CharacterConditionCarrier
		{
			get
			{
				return EnemyCharacterStateMachines;
			}
		}

		public CombatParticipantProxy CombatParticipantProxy { get; private set; }

		public EnemyCharacterStateMachines EnemyCharacterStateMachines
		{
			get
			{
				return (EnemyCharacterStateMachines)CharacterStateMachines;
			}
		}

		public ICharacterVitalityCarrier CharacterVitalityCarrier
		{
			get
			{
				return (EnemyCharacterStateMachines)CharacterStateMachines;
			}
		}

		public event CharacterDiedEventHandler CharacterDied;

		public EnemyCharacter()
			: base((IEntity entity) => new EnemyCharacterStateMachines(entity))
		{
			EnemyNpcStats = AddComponent<EnemyNpcStats>();
			CombatParticipantProxy = AddComponent<CombatParticipantProxy>();
		}

		public void InvokeCharacterDied()
		{
			CharacterDiedEventHandler characterDied = this.CharacterDied;
			if (characterDied != null)
			{
				characterDied(this);
			}
		}
	}
}
