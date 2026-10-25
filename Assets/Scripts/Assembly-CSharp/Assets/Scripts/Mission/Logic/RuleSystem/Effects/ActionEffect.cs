using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ActionEffect<T> : TargetEffectBase<T> where T : IRulesCarrier
	{
		public delegate IEffectInstance EffectAction(T _entity, IBuffedStatsCollectionHandle _stats, ActionEffect<T> _effect, EntityId _targetId);

		private readonly EffectAction m_Action;

		public ActionEffect(IRule _owner, ETargetSource _targetSource, EffectAction _action)
			: base(EEffectType.Action, _owner, _targetSource)
		{
			m_Action = _action;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return m_Action(_entity, _stats, this, TargetEntityId);
		}
	}
}
