using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class NavMeshLayerSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly int m_LayerMask;

		public NavMeshLayerSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, int _layerMask)
			: base(ETriggerType.NavMeshLayer, _owner, _frequency, _categoryFlags, _targetSource)
		{
			m_LayerMask = _layerMask;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			IGameEntity gameEntity = TargetEntityId.TryGetEntity<IGameEntity>();
			if (gameEntity == null)
			{
				return false;
			}
			NavMeshHit hit;
			if (!_entity.UnityComponentCache.NavMeshAgent.SamplePathPosition(_entity.UnityComponentCache.NavMeshAgent.areaMask, float.MaxValue, out hit))
			{
				return false;
			}
			return (hit.mask & m_LayerMask) != 0;
		}
	}
}
