using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class AssassinCharacter : Character, IAssassinInteractionCarrier, ICharacterStatsCarrier, IDefinitionCarrier, ICharacterDefinitionCarrier, IAvatarDefinitionCarrier, ICharacterStateMachinesCarrier, IAssassinCharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHeadIconCarrier, IHighlightCarrier, IPlayerStatsCarrier, IRulesCarrier, ISkillsCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, ICharacterConditionCarrierEntity, ICharacterVitalityCarrierEntity, IPlayerMainCarrierEntity, IPlayerProfileCarrierEntity, ISuperBuffsStatsCarrier
	{
		private readonly AvatarDefinition m_avatarDefinition;

		private readonly PlayerStats m_playerStats;

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.PlayerCharacter;
			}
		}

		public override Definition Definition
		{
			get
			{
				return m_avatarDefinition;
			}
		}

		public override CharacterDefinition CharacterDefinition
		{
			get
			{
				return m_avatarDefinition;
			}
		}

		public AvatarDefinition AvatarDefinition
		{
			get
			{
				return m_avatarDefinition;
			}
		}

		public Rules Rules { get; private set; }

		public override CharacterStats CharacterStats
		{
			get
			{
				return m_playerStats;
			}
		}

		public ICharacterConditionCarrier CharacterConditionCarrier
		{
			get
			{
				return AssassinCharacterStateMachines;
			}
		}

		public PlayerStats PlayerStats
		{
			get
			{
				return m_playerStats;
			}
		}

		public ISuperBuffStats SuperBuffStats
		{
			get
			{
				return m_playerStats;
			}
		}

		public AssassinCharacterStateMachines AssassinCharacterStateMachines { get; private set; }

		public IPlayerMainCarrier PlayerMainCarrier
		{
			get
			{
				return AssassinCharacterStateMachines;
			}
		}

		public IPlayerProfileCarrier PlayerProfileCarrier
		{
			get
			{
				return AssassinCharacterStateMachines;
			}
		}

		public ICharacterVitalityCarrier CharacterVitalityCarrier
		{
			get
			{
				return AssassinCharacterStateMachines;
			}
		}

		public Skills Skills { get; private set; }

		public KillFromHaystack KillFromHaystack { get; protected set; }

		public KillFromBench KillFromBench { get; protected set; }

		public KillFromRooftopGarden KillFromRooftopGarden { get; protected set; }

		public abstract AssassinInteraction AssassinInteraction { get; protected set; }

		public override bool CanBePooled
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public event CharacterDiedEventHandler CharacterDied;

		protected AssassinCharacter(Func<IEntity, Assets.Scripts.Mission.Logic.Components.StateMachines> _stateMachinesGenerator)
			: base(_stateMachinesGenerator)
		{
			AssassinCharacterStateMachines = (AssassinCharacterStateMachines)CharacterStateMachines;
			Rules = AddComponent<Rules>();
			m_avatarDefinition = AddComponent<AvatarDefinition>();
			m_playerStats = AddComponent<PlayerStats>();
			Skills = AddComponent<Skills>();
			KillFromHaystack = AddComponent<KillFromHaystack>();
			KillFromBench = AddComponent<KillFromBench>();
			KillFromRooftopGarden = AddComponent<KillFromRooftopGarden>();
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
