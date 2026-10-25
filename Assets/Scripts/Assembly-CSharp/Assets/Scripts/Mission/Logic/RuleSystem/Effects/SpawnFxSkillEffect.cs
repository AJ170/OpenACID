using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SpawnFxSkillEffect<TEntity, TPayload> : SkillEffectBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		public delegate Vector3 SpawnPosGetter(ISkillsCarrier _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats);

		private readonly SpawnPosGetter m_SpawnPosGetter;

		public SpawnFxSkillEffect(IRule _owner, ETargetSource _targetSource, Vector3 _positionOffset)
			: this(_owner, _targetSource, (SpawnPosGetter)((ISkillsCarrier _entity, EntityId _id, IBuffedStatsCollectionHandle _stats) => OffsetSpawnPosGetter(_entity, _id, _stats, _positionOffset)))
		{
		}

		public SpawnFxSkillEffect(IRule _owner, ETargetSource _targetSource, SpawnPosGetter _spawnPosGetter)
			: base(EEffectType.SpawnFx, _owner, _targetSource)
		{
			m_SpawnPosGetter = _spawnPosGetter;
		}

		protected override IEffectInstance Apply(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			Vector3 position = m_SpawnPosGetter(_entity, TargetEntityId, _stats);
			GameID gameID = Main.Instance.GameDB.FindGameIdFromGuid(_stats.Get_String(StatTypes.EStatType.Rule_FxGuid));
			EffectHelper.SpawnEffectInWorld(gameID, position);
			return result;
		}

		private static Vector3 OffsetSpawnPosGetter(ISkillsCarrier _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats, Vector3 _offset)
		{
			return _targetEntityId.GetEntity<IPositionEntity>().Position + _offset;
		}
	}
}
