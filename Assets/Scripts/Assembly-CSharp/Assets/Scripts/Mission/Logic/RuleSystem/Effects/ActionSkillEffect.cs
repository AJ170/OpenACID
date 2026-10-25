using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ActionSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public delegate IEffectInstance Action(IEffect _effect, T _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, TPayload _payload);

		private readonly Action m_action;

		public ActionSkillEffect(IRule _owner, ETargetSource _targetSource, Action _action)
			: base(EEffectType.Action, _owner, _targetSource)
		{
			m_action = _action;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			return m_action(this, _entity, _stats, TargetEntityId, base.TypedPayload);
		}
	}
}
