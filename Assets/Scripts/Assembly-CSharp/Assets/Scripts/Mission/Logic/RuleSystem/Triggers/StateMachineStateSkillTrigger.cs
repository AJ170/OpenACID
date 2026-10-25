using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class StateMachineStateSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IStateMachinesCarrier where TPayload : ISkillPayload
	{
		public EStateMachineType StateMachineType { get; private set; }

		public int[] StateTypes { get; private set; }

		protected StateMachineStateSkillTrigger(ETriggerType _triggerType, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateMachineType _stateMachineType, int[] _stateTypes)
			: base(_triggerType, _owner, _frequency, _categories, _targetSource)
		{
			StateMachineType = _stateMachineType;
			StateTypes = _stateTypes;
		}

		public StateMachineStateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateMachineType _stateMachineType, int[] _stateTypes)
			: this(ETriggerType.StateMachineState, _owner, _frequency, _categories, _targetSource, _stateMachineType, _stateTypes)
		{
		}

		public StateMachineStateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateMachineType _stateMachineType, EStateType _stateType)
			: this(_owner, _frequency, _categories, _targetSource, _stateMachineType, new int[1] { (int)_stateType })
		{
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return TargetEntityId.GetEntity<IStateMachinesCarrier>().IsInState(StateMachineType, StateTypes);
		}
	}
}
