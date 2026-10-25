using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class EntityInRangeSkillTriggerBase<T, TPayload> : CategorySkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly int m_LayerMask;

		private readonly bool m_PlaneDistance;

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private static NavMeshPath s_Path__lazy;

		private static NavMeshPath s_Path
		{
			get
			{
				return s_Path__lazy ?? (s_Path__lazy = new NavMeshPath());
			}
		}

		protected EntityInRangeSkillTriggerBase(ETriggerType _type, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, int _layerMask, bool _planeDistance)
			: base(_type, _owner, _frequency, _categoryFlags, _targetSource)
		{
			m_LayerMask = _layerMask;
			m_PlaneDistance = _planeDistance;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && TargetEntityId.IsValid;
		}

		protected float ComputeSqrDistance(IGameEntity _entity)
		{
			Vector3 position = TargetEntityId.GetEntity<IGameObjectEntity>().Position;
			if (m_LayerMask == 0)
			{
				return (!m_PlaneDistance) ? (_entity.Position - position).sqrMagnitude : (_entity.Position - position).LengthXZSquared();
			}
			NavMesh.CalculatePath(_entity.Position, position, m_LayerMask, s_Path);
			if (s_Path.status == NavMeshPathStatus.PathComplete)
			{
				float num = s_Path.CalculateLength();
				return num * num;
			}
			return float.MaxValue;
		}
	}
}
