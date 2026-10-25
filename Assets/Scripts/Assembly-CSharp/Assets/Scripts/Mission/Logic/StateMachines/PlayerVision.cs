using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public class PlayerVision : StateMachineBase<PlayerCharacter, PlayerVision>
	{
		public override EStateMachineType Type
		{
			get
			{
				return EStateMachineType.PlayerVision;
			}
		}

		public override EStateType InitialState
		{
			get
			{
				return EStateType.PlayerVision_Normal;
			}
		}

		public PlayerVision(PlayerCharacter _entity)
			: base(_entity)
		{
		}

		public static void InitPlayerVision()
		{
			StateMachineBase<PlayerCharacter, PlayerVision>.InitStateMachineBase();
			StateMachineBase<PlayerCharacter, PlayerVision>.AddState<State<PlayerCharacter>>(EStateType.PlayerVision_Normal, OnDeactivateEagleEye, null, OnUpdate);
			StateMachineBase<PlayerCharacter, PlayerVision>.AddState<State<PlayerCharacter>>(EStateType.PlayerVision_EagleEye, OnActivateEagleEye, null, OnUpdate);
			StateMachineBase<PlayerCharacter, PlayerVision>.AddTransition(EStateType.PlayerVision_Normal, EStateType.PlayerVision_EagleEye);
			StateMachineBase<PlayerCharacter, PlayerVision>.AddTransition(EStateType.PlayerVision_EagleEye, EStateType.PlayerVision_Normal);
		}

		private static void OnActivateEagleEye(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _prevstate, IState<PlayerCharacter> _currentstate)
		{
			_statemachine.Entity.Vision.Activate(true);
		}

		private static void OnDeactivateEagleEye(IStateMachine<PlayerCharacter> _statemachine, IPayload _payload, IState<PlayerCharacter> _prevstate, IState<PlayerCharacter> _currentstate)
		{
			_statemachine.Entity.Vision.Activate(false);
		}

		private static void OnUpdate(IStateMachine<PlayerCharacter> stateMachine, IPayload payload, IState<PlayerCharacter> currentState)
		{
			EStateType eStateType = ((!stateMachine.Entity.EntityStats.Get_bool(StatTypes.EStatType.Player_EagleVision)) ? EStateType.PlayerVision_Normal : EStateType.PlayerVision_EagleEye);
			if (stateMachine.CurrentStateType != eStateType)
			{
				stateMachine.ScheduleStateChange(eStateType);
			}
		}
	}
}
