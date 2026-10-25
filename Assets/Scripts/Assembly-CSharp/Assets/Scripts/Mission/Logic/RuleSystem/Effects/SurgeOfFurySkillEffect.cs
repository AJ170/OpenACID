using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SurgeOfFurySkillEffect : SweepSkillEffectWrapperBase<PlayerCharacter, ISkillPayload, PlayerCharacter>
	{
		protected class EffectInstance : SweepEffectInstanceBase
		{
			private float m_lastProgress;

			private int m_killTokenIncrease;

			private readonly float m_effectRadius;

			private readonly int m_knockdownTurns;

			private readonly float m_knockdownChance;

			public EffectInstance(IEffect _effect, PlayerCharacter _entity, SkillEffectsWrapper _wrapper)
				: base(_effect, _entity, _wrapper)
			{
				m_lastProgress = 0f;
				m_killTokenIncrease = 0;
				TrySetFleeingAllowed(false);
				using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = Effect.Owner.StatValueCollection.Apply(m_entity))
				{
					m_effectRadius = buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_EffectRadius);
					m_knockdownTurns = buffedStatsCollectionHandle.Get_int(StatTypes.EStatType.Rule_KnockdownDuration);
					m_knockdownChance = buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_KnockdownChance);
				}
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (base.Update(_entity))
				{
					return true;
				}
				TrySetFleeingAllowed(true);
				return false;
			}

			private void TrySetFleeingAllowed(bool _allowed)
			{
				CombatController instance = CombatController.Instance;
				if (instance != null)
				{
					instance.IsFleeingAllowed = _allowed;
				}
			}

			protected override void OnSweepUpdate()
			{
				float num = 360f * Mathf.Max(m_lastProgress - 0.125f, 0f);
				float num2 = 360f * m_progress;
				Vector3 position = m_entity.Position;
				Quaternion quaternion = Quaternion.Inverse(m_entity.Rotation);
				IEnumerable<EnemyCharacter> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<EnemyCharacter>(position, m_effectRadius);
				foreach (EnemyCharacter item in enumerable)
				{
					if (!item.GetDeadOrAboutToDie())
					{
						Vector3 vector = quaternion * (item.Position - position);
						float num3 = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
						num3 = 180f - num3;
						if (!(num3 < num) && !(num3 > num2))
						{
							m_targetList.Add(item.Id);
						}
					}
				}
				m_lastProgress = m_progress;
			}

			protected override void OnSweepHitPostEffect(EntityId _targetId)
			{
				EnemyCharacter entity = _targetId.GetEntity<EnemyCharacter>();
				bool deadOrAboutToDie = entity.GetDeadOrAboutToDie();
				CombatHelper.IncreaseKillTokenCountMultiEnemy(deadOrAboutToDie, ref m_killTokenIncrease);
				if (!entity.IsAttached)
				{
					return;
				}
				CharacterCondition characterCondition = entity.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				if (CombatHelper.DiceRoll(m_knockdownChance) || deadOrAboutToDie || characterCondition.CurrentStateType == EStateType.CharacterCondition_Knockdowned)
				{
					IBuff buff = new AdditiveNumberBuff(EBuffType.Knockdowned, m_knockdownTurns, 1f);
					entity.Buffs.AddBuff(buff);
					AnimationTaskKnockdowned.Parameters parameters = new AnimationTaskKnockdowned.Parameters();
					parameters.Source = m_entity;
					parameters.Target = entity;
					AnimationTaskKnockdowned.Parameters externalParameters = parameters;
					characterCondition.ExternalChange(EStateType.CharacterCondition_Knockdowned, externalParameters);
				}
				else
				{
					entity.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskGetHit(m_entity, AttackMeleeType.WaveOfFury);
				}
				if (!deadOrAboutToDie)
				{
					CharacterAwareness characterAwareness = entity.EnemyCharacterStateMachines.CharacterAwareness;
					if (characterAwareness.CurrentStateType != EStateType.CharacterAwareness_Alarmed)
					{
						characterAwareness.ScheduleStateChangeForced(EStateType.CharacterAwareness_Alarmed);
					}
				}
			}
		}

		private const float StartAngle = 180f;

		private const float SweepAngle = 360f;

		private const float SweepDuration = 0.1f;

		private const float SweepExtraWidth = 0.125f;

		public SurgeOfFurySkillEffect(IRule _owner, BBList<IEffect> _effects)
			: base(_owner, ETargetSource.RuleOwner, _effects, 0.1f)
		{
		}

		protected override IEffectInstance CreateEffectInstance(PlayerCharacter _entity, SkillEffectsWrapper _wrapper)
		{
			return new EffectInstance(this, _entity, _wrapper);
		}
	}
}
