using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class ProfileStateSkillTrigger<T, TPayload> : StateMachineStateSkillTrigger<T, TPayload> where T : ISkillsCarrier, IPlayerProfileCarrierEntity where TPayload : ISkillPayload
	{
		public ProfileStateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int[] _stateTypes)
			: base(ETriggerType.ProfileStateMachineState, _owner, _frequency, _categories, _targetSource, EStateMachineType.PlayerProfile, _stateTypes)
		{
		}

		public ProfileStateSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, EStateType _stateType)
			: this(_owner, _frequency, _categories, _targetSource, new int[1] { (int)_stateType })
		{
		}
	}
}
