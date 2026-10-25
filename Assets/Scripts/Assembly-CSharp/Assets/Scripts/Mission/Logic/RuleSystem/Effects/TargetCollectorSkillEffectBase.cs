using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class TargetCollectorSkillEffectBase<T, TPayload> : SkillEffectBase<T, TPayload>, IMultipleTargetsCarrier, IEffect, IMultipleTargetsEffect where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public delegate void TargetCollector(T _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, TPayload _payload, BBList<EntityId> _targetEntityIds);

		private readonly TargetCollector m_TargetCollector;

		public BBList<EntityId> TargetEntityIds { get; private set; }

		protected TargetCollectorSkillEffectBase(EEffectType _type, IRule _owner, ETargetSource _targetSource, TargetCollector _targetCollector)
			: base(_type, _owner, _targetSource)
		{
			m_TargetCollector = _targetCollector;
			TargetEntityIds = new BBList<EntityId>();
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			TargetEntityIds.ClearIfNotEmpty();
			m_TargetCollector(_entity, _stats, TargetEntityId, base.TypedPayload, TargetEntityIds);
			return result;
		}
	}
}
