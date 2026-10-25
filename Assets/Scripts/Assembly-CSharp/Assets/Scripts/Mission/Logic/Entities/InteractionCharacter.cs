using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class InteractionCharacter : NpcCharacter, ICharacterStatsCarrier, IDefinitionCarrier, ICharacterStateMachinesCarrier, IInteractionCharacterStateMachinesCarrier, IEntityStatsCarrier, IHeadIconCarrier, IInteractionCharacterStatsCarrier, IStateMachinesCarrier, IEntity, ICharacterVitalityCarrierEntity
	{
		protected readonly InteractionCharacterStats m_charStats;

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.InteractionCharacter;
			}
		}

		public override CharacterStats CharacterStats
		{
			get
			{
				return m_charStats;
			}
		}

		public InteractionCharacterStats InteractionCharacterStats
		{
			get
			{
				return m_charStats;
			}
		}

		public InteractionCharacterStateMachines InteractionCharacterStateMachines
		{
			get
			{
				return (InteractionCharacterStateMachines)CharacterStateMachines;
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

		public InteractionCharacter()
			: this((IEntity entity) => new InteractionCharacterStateMachines(entity), (IEntity entity) => new InteractionCharacterStats(entity))
		{
		}

		protected InteractionCharacter(Func<IEntity, Assets.Scripts.Mission.Logic.Components.StateMachines> _stateMachinesGenerator, Func<IEntity, InteractionCharacterStats> _statsGenerator)
			: base(_stateMachinesGenerator)
		{
			m_charStats = AddComponent(_statsGenerator(this));
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
