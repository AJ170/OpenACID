using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.States
{
	public class FleeingConditionState : State<Character>
	{
		public enum Reason
		{
			Invalid = 0,
			Fleeing = 1,
			WalkFromCombat = 2
		}

		public class Payload : IPayload
		{
			public Reason Reason { get; set; }

			public void Reset()
			{
				Reason = Reason.Invalid;
			}
		}

		public FleeingConditionState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<Character> _stateMachine)
		{
			return new Payload();
		}
	}
}
