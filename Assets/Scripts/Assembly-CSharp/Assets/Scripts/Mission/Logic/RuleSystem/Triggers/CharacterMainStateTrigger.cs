using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class CharacterMainStateTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : Character, ISkillsCarrier, IStateMachinesCarrier where TPayload : ISkillPayload
	{
		public int[] StateTypes { get; private set; }

		public CharacterMainStateTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int[] _stateTypes)
			: base(ETriggerType.MainStateMachineState, _owner, _frequency, _categories, _targetSource)
		{
			StateTypes = _stateTypes;
		}

		public CharacterMainStateTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource)
			: base(ETriggerType.MainStateMachineState, _owner, _frequency, _categories, _targetSource)
		{
			BBList<int> bBList = new BBList<int>();
			for (int i = 0; i < 11; i++)
			{
				ETriggerCategoryFlags eTriggerCategoryFlags = ((ETriggerCategory)i).ToFlags();
				if ((_categories & eTriggerCategoryFlags) == eTriggerCategoryFlags)
				{
					EStateType eStateType = eTriggerCategoryFlags.ToPlayerMainState();
					if (eStateType != 0)
					{
						bBList.Add((int)eStateType);
					}
				}
			}
			StateTypes = bBList.ToArray();
		}

		public CharacterMainStateTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateType _stateType)
			: this(_owner, _frequency, _categories, _targetSource, new int[1] { (int)_stateType })
		{
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			Character entity = TargetEntityId.GetEntity<Character>();
			if (entity == null)
			{
				return false;
			}
			EStateMachineType mainStateMachineType = entity.GetMainStateMachineType();
			if (mainStateMachineType == EStateMachineType.Invalid)
			{
				return false;
			}
			return entity.IsInState(mainStateMachineType, StateTypes);
		}
	}
}
