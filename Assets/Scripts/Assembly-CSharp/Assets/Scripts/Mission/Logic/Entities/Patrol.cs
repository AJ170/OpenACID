using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Selection;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Patrol : GameEntity, IDefinitionCarrier, IPatrolDefinitionCarrier, IPatrolStateMachinesCarrier, IEntityStatsCarrier, IEscortCarrier, IHeadIconCarrier, IVictimCarrier, IPatrolCarrier, IPatrolStatsCarrier, IStateMachinesCarrier, IEntity, IPositionProvider, IPositionEntity, ISelectable
	{
		public override Definition Definition
		{
			get
			{
				return PatrolDefinition;
			}
		}

		public PatrolDefinition PatrolDefinition { get; private set; }

		public override EntityStats EntityStats
		{
			get
			{
				return PatrolStats;
			}
		}

		public PatrolStats PatrolStats { get; private set; }

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Patrol;
			}
		}

		public Assets.Scripts.Mission.Logic.Components.StateMachines StateMachines
		{
			get
			{
				return PatrolStateMachines;
			}
		}

		public PatrolStateMachines PatrolStateMachines { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.Patrol PatrolComponent { get; private set; }

		public Escort EscortComponent
		{
			get
			{
				return VictimComponent as Escort;
			}
		}

		public Captive CaptiveComponent
		{
			get
			{
				return VictimComponent as Captive;
			}
		}

		public IVictim VictimComponent { get; private set; }

		public HeadIcon HeadIcon { get; private set; }

		public Patrol()
		{
			PatrolDefinition = AddComponent<PatrolDefinition>();
			PatrolComponent = AddComponent<Assets.Scripts.Mission.Logic.Components.Patrol>();
			PatrolStateMachines = AddComponent<PatrolStateMachines>();
			PatrolStats = AddComponent<PatrolStats>();
			HeadIcon = AddComponent<HeadIcon>();
		}

		public Escort AddEscorteeComponent()
		{
			VictimComponent = AddComponent<Escort>();
			return EscortComponent;
		}

		public Captive AddCaptiveComponent()
		{
			VictimComponent = AddComponent<Captive>();
			return CaptiveComponent;
		}
	}
}
