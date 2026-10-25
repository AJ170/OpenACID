using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.States
{
	public class JackBoxConditionState : State<Character>
	{
		public class Payload : IPayload
		{
			public Vector3 TargetPosition { get; private set; }

			public Vector3 LookAtPosition { get; private set; }

			public bool Valid { get; private set; }

			public void Set(Vector3 _target, Vector3 _lookAt)
			{
				TargetPosition = _target;
				LookAtPosition = _lookAt;
				Valid = true;
			}

			public void Reset()
			{
				Valid = false;
			}
		}

		public JackBoxConditionState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<Character> _stateMachine)
		{
			return new Payload();
		}
	}
}
