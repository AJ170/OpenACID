using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.States
{
	internal class LastKnownAreaState : State<Character>
	{
		public class Payload : IPayload
		{
			public Vector3 Position { get; private set; }

			public float Radius { get; private set; }

			public bool Valid { get; private set; }

			public void Set(Vector3 _position, float _radius)
			{
				Position = _position;
				Radius = _radius;
				Valid = true;
			}

			public void Reset()
			{
				Valid = false;
			}
		}

		public LastKnownAreaState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<Character> _stateMachine)
		{
			return new Payload();
		}
	}
}
