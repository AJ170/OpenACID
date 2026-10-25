using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ExplicitStatsUpdateSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public IEntityStatsCarrier TargetEntity
		{
			get
			{
				return TargetEntityId.Entity;
			}
		}

		public ExplicitStatsUpdateSkillEffect(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.ExplicitStatsUpdate, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			TargetEntity.EntityStats.UpdateComputedStatsExplicit();
			return result;
		}
	}
}
