using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SpawnProjectileSkillEffect<TEntity, TPayload> : SkillEffectBase<TEntity, TPayload> where TEntity : IRulesCarrier, ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private class EffectInstance : WrapperEffectInstance
		{
			private bool m_waiting;

			public EffectInstance(IEffect _effect)
				: base(_effect)
			{
				m_waiting = true;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				return base.Update(_entity) || m_waiting;
			}

			public void Done()
			{
				m_waiting = false;
			}
		}

		private readonly ProjectileData.EProjectileType m_projectileType;

		private readonly SkillEffectsWrapper m_hitEffectsWrapper;

		private readonly SkillEffectsWrapper m_explodeEffectsWrapper;

		public SpawnProjectileSkillEffect(IRule _owner, ETargetSource _targetSource, ProjectileData.EProjectileType _projectileType, SkillEffectsWrapper _hitEffectsWrapper, SkillEffectsWrapper _explodeEffectsWrapper = null)
			: base(EEffectType.SpawnProjectile, _owner, _targetSource)
		{
			m_projectileType = _projectileType;
			m_hitEffectsWrapper = _hitEffectsWrapper;
			m_explodeEffectsWrapper = _explodeEffectsWrapper;
		}

		protected override IEffectInstance Apply(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			Projectile projectile = ProjectileHelper.SpawnProjectile(m_projectileType, _entity, TargetEntityId, _stats);
			Assets.Scripts.Mission.Logic.StateMachines.ProjectileFlight projectileFlight = projectile.ProjectileStateMachines.ProjectileFlight;
			EntityIdProjectileSkillPayload payload = new EntityIdProjectileSkillPayload((EntityIdSkillPayload)Payload, projectile);
			EffectInstance wrapperEffectInstance = new EffectInstance(this);
			if (m_hitEffectsWrapper != null)
			{
				m_hitEffectsWrapper.Entity = _entity;
				m_hitEffectsWrapper.StatValueCollection = _stats.StatValueCollection;
				m_hitEffectsWrapper.Payload = payload;
				EffectsWrapper hitWrapperClone = m_hitEffectsWrapper.Clone().Cast<EffectsWrapper>();
				projectileFlight.HitAction = delegate
				{
					hitWrapperClone.ApplyEffects(wrapperEffectInstance.WrappedEffectInstances);
				};
			}
			if (m_explodeEffectsWrapper != null)
			{
				m_explodeEffectsWrapper.Entity = _entity;
				m_explodeEffectsWrapper.StatValueCollection = _stats.StatValueCollection;
				m_explodeEffectsWrapper.Payload = payload;
				EffectsWrapper explodeWrapperClone = m_explodeEffectsWrapper.Clone().Cast<EffectsWrapper>();
				projectileFlight.ExplodeAction = delegate
				{
					explodeWrapperClone.ApplyEffects(wrapperEffectInstance.WrappedEffectInstances);
					wrapperEffectInstance.Done();
				};
			}
			else
			{
				projectileFlight.ExplodeAction = delegate
				{
					wrapperEffectInstance.Done();
				};
			}
			return wrapperEffectInstance;
		}
	}
}
