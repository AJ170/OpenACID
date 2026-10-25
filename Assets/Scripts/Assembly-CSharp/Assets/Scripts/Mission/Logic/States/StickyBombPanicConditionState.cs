using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.States
{
	public class StickyBombPanicConditionState : State<Character>
	{
		public class Payload : IPayload
		{
			private Vector3 m_targetPosition;

			public Vector3 TargetPosition
			{
				get
				{
					return m_targetPosition;
				}
				set
				{
					m_targetPosition = value;
					Valid = true;
				}
			}

			public bool Valid { get; private set; }

			public void Reset()
			{
				Valid = false;
			}
		}

		public StickyBombPanicConditionState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<Character> _stateMachine)
		{
			return new Payload();
		}
	}
}
