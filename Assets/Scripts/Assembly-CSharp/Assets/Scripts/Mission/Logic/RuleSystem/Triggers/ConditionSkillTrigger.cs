using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class ConditionSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : Character, ISkillsCarrier, IStateMachinesCarrier where TPayload : ISkillPayload
	{
		private readonly int[] m_stateTypes;

		public ConditionSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateType _stateType)
			: this(_owner, _frequency, _categories, _targetSource, new int[1] { (int)_stateType })
		{
		}

		public ConditionSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int[] _stateTypes)
			: base(ETriggerType.ConditionState, _owner, _frequency, _categories, _targetSource)
		{
			m_stateTypes = _stateTypes;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			Character character = TargetEntityId.TryGetEntity<Character>();
			if (character == null)
			{
				return false;
			}
			ICharacterConditionCarrier characterConditionCarrier = character.StateMachines as ICharacterConditionCarrier;
			return characterConditionCarrier != null && character.IsInState(EStateMachineType.CharacterCondition, m_stateTypes);
		}
	}
}
