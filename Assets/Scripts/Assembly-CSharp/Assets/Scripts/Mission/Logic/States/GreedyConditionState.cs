using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.States
{
	public class GreedyConditionState : State<Character>
	{
		public class Payload : IPayload
		{
			public IGreedyAreaEffectInstance AreaInstance { get; set; }

			public int ZoneIdxCurrent { get; set; }

			public int ZoneIdxPrev { get; set; }

			public Vector3 Target { get; set; }

			public void Reset()
			{
				AreaInstance = null;
			}

			public void Set(IGreedyAreaEffectInstance _areaInstance, int _zoneIdx)
			{
				AreaInstance = _areaInstance;
				ZoneIdxCurrent = _zoneIdx;
				ZoneIdxPrev = -1;
				FindNewTarget();
			}

			public void FindNewTarget()
			{
				if (AreaInstance != null)
				{
					AreaInstance.FindNewTarget(this);
				}
			}

			public void ReachedTarget()
			{
				if (AreaInstance != null)
				{
					AreaInstance.ReachedTarget(this);
				}
			}
		}

		public GreedyConditionState(EStateType _stateType, OnEnterHandler _onEnterHandler, OnExitHandler _onExitHandler, OnUpdateHandler _onUpdateHandler)
			: base(_stateType, _onEnterHandler, _onExitHandler, _onUpdateHandler)
		{
		}

		public override IPayload CreatePayload(IStateMachine<Character> _stateMachine)
		{
			return new Payload();
		}
	}
}
