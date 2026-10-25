using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class HasTargetsNavMeshLayerSkillTrigger<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		public HasTargetsNavMeshLayerSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(ETriggerType.HasTargetsNavMeshLayer, _owner, _frequency, _categoryFlags, _targetSource)
		{
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
			NavMeshHit hit2;
			if (!_entity.UnityComponentCache.NavMeshAgent.SamplePathPosition(_entity.UnityComponentCache.NavMeshAgent.areaMask, float.MaxValue, out hit2))
			{
				return false;
			}
			return hit.mask == hit2.mask;
		}
	}
}
