using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class WrapperEffectInstance : EffectInstanceBase
	{
		private readonly BBList<IEffectInstance> m_WrappedEffectInstances = new BBList<IEffectInstance>();

		public BBList<IEffectInstance> WrappedEffectInstances
		{
			get
			{
				return m_WrappedEffectInstances;
			}
		}

		public WrapperEffectInstance(IEffect _effect)
			: base(_effect)
		{
		}

		public override bool Update(IRulesCarrier _entity)
		{
			bool result = false;
			for (int i = 0; i < m_WrappedEffectInstances.Count; i++)
			{
				IEffectInstance effectInstance = m_WrappedEffectInstances[i];
				if (effectInstance.Update(_entity))
				{
					result = true;
				}
			}
			return result;
		}

		public override void Abort(IRulesCarrier _entity)
		{
			for (int i = 0; i < m_WrappedEffectInstances.Count; i++)
			{
				IEffectInstance effectInstance = m_WrappedEffectInstances[i];
				if (effectInstance.Update(_entity))
				{
					effectInstance.Abort(_entity);
				}
			}
		}
	}
}
