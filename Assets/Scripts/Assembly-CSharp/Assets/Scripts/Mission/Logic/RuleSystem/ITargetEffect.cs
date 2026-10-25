using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ITargetEffect : ITargetCarrier, IEffect
	{
		ETargetSource TargetSource { get; }

		void ComputeTargetEntityId(IRulesCarrier _rulesCarrier);
	}
}
