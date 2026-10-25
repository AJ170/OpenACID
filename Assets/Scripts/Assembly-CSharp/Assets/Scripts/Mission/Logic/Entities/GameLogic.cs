using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class GameLogic : Entity, IDefinitionCarrier, IGameLogicStateMachinesCarrier, IEntityStatsCarrier, IEntityTrackingCarrier, IGameDefinitionCarrier, IGameStatsCarrier, IMissionResultCarrier, IStateMachinesCarrier, IEntity, IUpdateableEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.GameLogic;
			}
		}

		public override EntityStats EntityStats
		{
			get
			{
				return GameStats;
			}
		}

		public GameStats GameStats { get; private set; }

		public override Definition Definition
		{
			get
			{
				return GameDefinition;
			}
		}

		public GameDefinition GameDefinition { get; private set; }

		public LogicInterface LogicInterface { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.StateMachines StateMachines
		{
			get
			{
				return GameLogicStateMachines;
			}
		}

		public GameLogicStateMachines GameLogicStateMachines { get; private set; }

		public MissionResult MissionResult { get; private set; }

		public EntityTracking EntityTracking { get; private set; }

		public GameLogic()
		{
			GameDefinition = AddComponent<GameDefinition>();
			GameStats = AddComponent<GameStats>();
			LogicInterface = AddComponent<LogicInterface>();
			GameLogicStateMachines = AddComponent<GameLogicStateMachines>();
			MissionResult = AddComponent<MissionResult>();
			EntityTracking = AddComponent<EntityTracking>();
		}
	}
}
