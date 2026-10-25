using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class TargetCollectorBuffSkillEffect<T, TPayload> : TargetCollectorSkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly IBuffGenerator m_BuffGenerator;

		private readonly Predicate<IBuffsCarrier> m_ActivityPredicate;

		private readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public TargetCollectorBuffSkillEffect(IRule _owner, ETargetSource _targetSource, TargetCollector _targetCollector, IBuffGenerator _buffGenerator, Predicate<IBuffsCarrier> _activityPredicate)
			: base(EEffectType.Buff, _owner, _targetSource, _targetCollector)
		{
			m_BuffGenerator = _buffGenerator;
			m_ActivityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(this);
			for (int i = 0; i < TargetEntityIds.Count; i++)
			{
				IBuffsCarrier buffsCarrier = (IBuffsCarrier)TargetEntityIds[i].Entity;
				m_BuffGenerator.MakeBuffs(s_BuffList, _entity, buffsCarrier);
				for (int j = 0; j < s_BuffList.Count; j++)
				{
					buffsCarrier.Buffs.AddBuff(s_BuffList[j]);
					wrapperEffectInstance.WrappedEffectInstances.Add(new BuffEffect<T>.EffectInstance(this, buffsCarrier, s_BuffList[j], m_ActivityPredicate));
				}
			}
			return wrapperEffectInstance;
		}
	}
}
