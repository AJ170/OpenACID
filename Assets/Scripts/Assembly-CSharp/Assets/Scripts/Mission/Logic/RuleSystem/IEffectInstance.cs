using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IEffectInstance
	{
		IEffect Effect { get; }

		bool DemandAbortAll { get; }

		IEffectInstaceResult Result { get; }

		bool Update(IRulesCarrier _entity);

		void Abort(IRulesCarrier _entity);
	}
}
