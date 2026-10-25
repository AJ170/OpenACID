using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class EffectBase<T> : IEffect where T : IRulesCarrier
	{
		public EEffectType Type { get; private set; }

		public IRule Owner { get; private set; }

		public Predicate<IEffect, IRulesCarrier> Predicate { get; set; }

		public ETriggerCategoryFlags Categories { get; set; }

		public bool IsAbortable { get; set; }

		protected EffectBase(EEffectType _type, IRule _owner)
		{
			Type = _type;
			Owner = _owner;
			Categories = ETriggerCategoryFlags.MainStates;
			IsAbortable = true;
		}

		public IEffectInstance Apply(IRulesCarrier _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance effectInstance = Apply((T)_entity, _stats);
			object result;
			if (IsAbortable)
			{
				IEffectInstance effectInstance2 = effectInstance;
				result = effectInstance2;
			}
			else
			{
				result = null;
			}
			return (IEffectInstance)result;
		}

		public virtual void Prepare(IRulesCarrier _entity)
		{
		}

		protected abstract IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats);

		public virtual void Reset()
		{
		}

		public virtual bool GetIsCategoryMatching(IRulesCarrier _entity)
		{
			return Categories.Include(ETriggerCategoryFlags.None).Contains(_entity.Rules.CurrentTriggerCategoryFlags);
		}
	}
}
