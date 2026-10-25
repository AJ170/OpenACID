using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class EffectInstanceBase : IEffectInstance
	{
		public IEffect Effect { get; private set; }

		public virtual bool DemandAbortAll
		{
			get
			{
				return false;
			}
		}

		public virtual IEffectInstaceResult Result
		{
			get
			{
				return null;
			}
		}

		protected EffectInstanceBase(IEffect _effect)
		{
			Effect = _effect;
		}

		public abstract bool Update(IRulesCarrier _entity);

		public abstract void Abort(IRulesCarrier _entity);
	}
}
