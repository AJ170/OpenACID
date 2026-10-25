using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SpawnProjectilesSkillEffect<TEntity, TPayload> : SkillEffectBase<TEntity, TPayload> where TEntity : IRulesCarrier, ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly ProjectileData.EProjectileType m_ProjectileType;

		private readonly SkillEffectsWrapper m_HitEffectsWrapper;

		private readonly Func<TEntity, EntityId, BBList<EnemyCharacter>> m_TargetsSelector;

		public SpawnProjectilesSkillEffect(IRule _owner, ETargetSource _targetSource, ProjectileData.EProjectileType _projectileType, SkillEffectsWrapper _hitEffectsWrapper, Func<TEntity, EntityId, BBList<EnemyCharacter>> _targetsSelector)
			: base(EEffectType.SpawnProjectiles, _owner, _targetSource)
		{
			m_ProjectileType = _projectileType;
			m_HitEffectsWrapper = _hitEffectsWrapper;
			m_TargetsSelector = _targetsSelector;
		}

		protected override IEffectInstance Apply(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			BBList<EnemyCharacter> bBList = m_TargetsSelector(_entity, TargetEntityId);
			for (int i = 0; i < bBList.Count; i++)
			{
				EnemyCharacter enemyCharacter = bBList[i];
				Projectile projectile = ProjectileHelper.SpawnProjectile(m_ProjectileType, _entity, enemyCharacter.Id, _stats);
				m_HitEffectsWrapper.Entity = _entity;
				m_HitEffectsWrapper.StatValueCollection = _stats.StatValueCollection;
				m_HitEffectsWrapper.Payload = new EntityIdSkillPayload(enemyCharacter.Id);
				EffectsWrapper wrapperClone = m_HitEffectsWrapper.Clone().Cast<EffectsWrapper>();
				projectile.ProjectileStateMachines.ProjectileFlight.HitAction = delegate
				{
					wrapperClone.ApplyEffects();
				};
			}
			return result;
		}
	}
}
