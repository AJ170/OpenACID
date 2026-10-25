using Assets.Scripts.Mission.Environment;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Selection;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class GameMain : StateMachineBase<GameLogic, GameMain>
	{
		public override EStateType InitialState
		{
			get
			{
				return EStateType.Game_Running;
			}
		}

		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.GameMain;
			}
		}

		public GameMain(GameLogic _entity)
			: base(_entity)
		{
		}

		public static void InitGameMain()
		{
			StateMachineBase<GameLogic, GameMain>.InitStateMachineBase();
			StateMachineBase<GameLogic, GameMain>.AddState<State<GameLogic>>(EStateType.Game_Running, null, null, OnUpdateRunning);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			PigeonSpawner.OnMissionLoaded();
		}

		private static void OnUpdateRunning(IStateMachine<GameLogic> stateMachine, IPayload payload, IState<GameLogic> currentState)
		{
			UbiCore.Utils.Singleton<SelectionManager>.Instance.UpdateSelection();
			if (!CombatController.IsActive)
			{
				int num = stateMachine.Entity.GameStats.Get_int(StatTypes.EStatType.Game_RecentlyKilledEnemyTurn);
				if (LogicMain.CurrentTurn - num >= 50)
				{
					stateMachine.Entity.GameStats.Set_int(StatTypes.EStatType.Game_RecentlyKilledEnemyCount, 0);
				}
			}
		}
	}
}
