using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class BuffEffect<T> : TargetEffectBase<T> where T : IRulesCarrier
	{
		public class EffectInstance : EffectInstanceBase
		{
			private readonly IBuffsCarrier m_TargetEntity;

			private readonly IBuff m_Buff;

			private readonly Predicate<IBuffsCarrier> m_activityPredicate;

			public EffectInstance(IEffect _effect, IBuffsCarrier _targetEntity, IBuff _buff, Predicate<IBuffsCarrier> _activityPredicate)
				: base(_effect)
			{
				m_TargetEntity = _targetEntity;
				m_Buff = _buff;
				m_activityPredicate = _activityPredicate;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (m_Buff.IsRunning && m_activityPredicate != null && !m_activityPredicate(m_TargetEntity))
				{
					Abort(_entity);
				}
				return m_Buff.IsRunning;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_TargetEntity.Buffs.RemoveBuff(m_Buff);
			}
		}

		private readonly IBuffGenerator m_BuffGenerator;

		private readonly Predicate<IBuffsCarrier> m_ActivityPredicate;

		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public IBuffsCarrier TargetEntity
		{
			get
			{
				return TargetEntityId.GetEntity<IBuffsCarrier>();
			}
		}

		public BuffEffect(IRule _owner, IBuffGenerator _buffGenerator, ETargetSource _targetSource)
			: this(_owner, _buffGenerator, _targetSource, (Predicate<IBuffsCarrier>)null)
		{
		}

		public BuffEffect(IRule _owner, IBuffGenerator _buffGenerator, ETargetSource _targetSource, Predicate<IBuffsCarrier> _activityPredicate)
			: base(EEffectType.Buff, _owner, _targetSource)
		{
			m_BuffGenerator = _buffGenerator;
			m_ActivityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			return Apply(_entity, this, TargetEntity, m_BuffGenerator, m_ActivityPredicate);
		}

		public static IEffectInstance Apply(IRulesCarrier _rulesCarrier, IEffect _effect, IBuffsCarrier _targetEntity, IBuffGenerator _buffGenerator, Predicate<IBuffsCarrier> _activityPredicate)
		{
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(_effect);
			_buffGenerator.MakeBuffs(s_BuffList, _rulesCarrier, _targetEntity);
			for (int i = 0; i < s_BuffList.Count; i++)
			{
				_targetEntity.Buffs.AddBuff(s_BuffList[i]);
				wrapperEffectInstance.WrappedEffectInstances.Add(new EffectInstance(_effect, _targetEntity, s_BuffList[i], _activityPredicate));
			}
			return wrapperEffectInstance;
		}
	}
}
