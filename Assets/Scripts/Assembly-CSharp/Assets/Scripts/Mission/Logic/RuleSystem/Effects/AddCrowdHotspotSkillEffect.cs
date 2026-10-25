using Assets.Scripts.Animation;
using Assets.Scripts.Crowd;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class AddCrowdHotspotSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly float m_DynmamicWeight;

		private readonly float m_Duration;

		private readonly float m_Radius;

		private readonly bool m_Flee;

		private readonly float m_TriggerRadius;

		private readonly float m_TriggerDuration;

		private readonly ReactionType m_TriggerReactionType;

		public AddCrowdHotspotSkillEffect(IRule _owner, ETargetSource _targetSource, float _dynamicWeight, float _duration, float _radius, bool _flee, float _triggerRadius, float _triggerDuration, ReactionType _triggerReactionType)
			: base(EEffectType.AddCrowdHotspot, _owner, _targetSource)
		{
			m_DynmamicWeight = _dynamicWeight;
			m_Duration = _duration;
			m_Radius = _radius;
			m_Flee = _flee;
			m_TriggerRadius = _triggerRadius;
			m_TriggerDuration = _triggerDuration;
			m_TriggerReactionType = _triggerReactionType;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			Assets.Scripts.Crowd.Crowd.Instance.AddHotspot(new CrowdHotspotData
			{
				Position = TargetEntityId.GetEntity<IPositionEntity>().Position,
				DynamicWeight = m_DynmamicWeight,
				EndTime = LogicMain.CurrentTime + m_Duration,
				Radius = m_Radius,
				Flee = m_Flee,
				TriggerRadius = m_TriggerRadius,
				TriggerDuration = m_TriggerDuration,
				TriggerReactionType = (int)m_TriggerReactionType
			});
			return base.Apply(_entity, _stats);
		}
	}
}
