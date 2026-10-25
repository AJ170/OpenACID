using System;
using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class NpcCharacter : Character, ICharacterAiCarrier, ICharacterStatsCarrier, IDefinitionCarrier, ICharacterDefinitionCarrier, INpcDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, INavAgentSurrogateCarrier, INpcStatsCarrier, ISpacialEntityCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		private readonly CharacterAi m_characterAi;

		private readonly NpcDefinition m_npcDefinition;

		private readonly NavAgentSurrogate m_navAgentSurrogate;

		private readonly SelectionCollider m_selectionCollider;

		public override Definition Definition
		{
			get
			{
				return m_npcDefinition;
			}
		}

		public override CharacterDefinition CharacterDefinition
		{
			get
			{
				return m_npcDefinition;
			}
		}

		public NpcDefinition NpcDefinition
		{
			get
			{
				return m_npcDefinition;
			}
		}

		public CharacterAi CharacterAi
		{
			get
			{
				return m_characterAi;
			}
		}

		public NavAgentSurrogate NavAgentSurrogate
		{
			get
			{
				return m_navAgentSurrogate;
			}
		}

		public Patrol Patrol
		{
			get
			{
				return NpcStats.Patrol;
			}
		}

		public SelectionCollider SelectionCollider
		{
			get
			{
				return m_selectionCollider;
			}
		}

		public NpcStats NpcStats
		{
			get
			{
				return (NpcStats)CharacterStats;
			}
		}

		protected NpcCharacter(Func<IEntity, Assets.Scripts.Mission.Logic.Components.StateMachines> _stateMachinesGenerator)
			: base(_stateMachinesGenerator)
		{
			m_npcDefinition = AddComponent<NpcDefinition>();
			m_characterAi = AddComponent<CharacterAi>();
			m_navAgentSurrogate = AddComponent<NavAgentSurrogate>();
			m_selectionCollider = AddComponent<SelectionCollider>();
		}
	}
}
