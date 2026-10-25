using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class BuffSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly IBuffGenerator m_BuffGenerator;

		private readonly Predicate<IBuffsCarrier> m_ActivityPredicate;

		public IBuffsCarrier TargetEntity
		{
			get
			{
				return TargetEntityId.GetEntity<IBuffsCarrier>();
			}
		}

		public BuffSkillEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator)
			: this(_owner, _targetSource, _buffGenerator, (Predicate<IBuffsCarrier>)null)
		{
		}

		public BuffSkillEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator, Predicate<IBuffsCarrier> _activityPredicate)
			: base(EEffectType.Buff, _owner, _targetSource)
		{
			m_BuffGenerator = _buffGenerator;
			m_ActivityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			return BuffEffect<T>.Apply(_entity, this, TargetEntity, m_BuffGenerator, m_ActivityPredicate);
		}
	}
}
