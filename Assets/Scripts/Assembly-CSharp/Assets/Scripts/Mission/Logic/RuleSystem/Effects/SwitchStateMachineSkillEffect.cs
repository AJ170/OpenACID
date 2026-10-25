using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SwitchStateMachineSkillEffect<T, TPayload, TStatePayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload where TStatePayload : class, IPayload
	{
		private readonly EStateMachineType m_StateMachineType;

		private readonly EStateType m_StateType;

		private readonly bool m_Forced;

		private readonly Action<TStatePayload, TPayload> m_PayloadAction;

		public SwitchStateMachineSkillEffect(IRule _owner, ETargetSource _targetSource, EStateMachineType _stateMachineType, EStateType _stateType, bool _forced, Action<TStatePayload, TPayload> _payloadAction)
			: base(EEffectType.SwitchStateMachine, _owner, _targetSource)
		{
			m_StateMachineType = _stateMachineType;
			m_StateType = _stateType;
			m_Forced = _forced;
			m_PayloadAction = _payloadAction;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			Assets.Scripts.Mission.Logic.Components.StateMachines stateMachines = TargetEntityId.GetEntity<IStateMachinesCarrier>().StateMachines;
			IStateMachine stateMachine = stateMachines.GetStateMachine(m_StateMachineType);
			if (m_PayloadAction != null)
			{
				m_PayloadAction(stateMachine.GetPayload<TStatePayload>(m_StateType), base.TypedPayload);
			}
			if (m_Forced)
			{
				stateMachine.ScheduleStateChangeForced(m_StateType);
			}
			else
			{
				stateMachine.ScheduleStateChange(m_StateType);
			}
			return result;
		}
	}
}
