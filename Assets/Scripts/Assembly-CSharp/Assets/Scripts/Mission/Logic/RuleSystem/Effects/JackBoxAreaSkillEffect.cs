using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class JackBoxAreaSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		protected class EffectInstance : EffectInstanceBase, SkillAreaHelper.IAreaInstance
		{
			private const float ZoneRadius = 0.5f;

			private const int ZoneCount = 9;

			private const EntityTypes.EType TargetType = EntityTypes.EType.EnemyCharacter;

			private int m_lastTurnUpdate = -1;

			private readonly int m_startTurn;

			private readonly int m_endTurn;

			private readonly float m_attractRadiusSqr;

			private int m_affectedCount;

			private readonly int m_maxAffectedCount;

			public BBList<SkillAreaHelper.AffectedNpc> AffectedCharacters { get; set; }

			public BBList<SkillAreaHelper.Zone> Zones { get; set; }

			public bool[,] ZoneConnections { get; set; }

			public Vector3 AreaCenter { get; set; }

			public EffectInstance(JackBoxAreaSkillEffect<T, TPayload> _effect, IPositionEntity _entity, IBuffedStatsCollectionHandle _stats)
				: base(_effect)
			{
				int num = _stats.Get_int(StatTypes.EStatType.Rule_JackBoxStartDelay);
				int num2 = _stats.Get_int(StatTypes.EStatType.Rule_ProjectileDelay);
				float num3 = _stats.Get_float(StatTypes.EStatType.Rule_AttractionRange);
				m_maxAffectedCount = _stats.Get_int(StatTypes.EStatType.Rule_ThrowTargets);
				AreaCenter = _entity.Position;
				m_startTurn = LogicMain.CurrentTurn + num;
				m_endTurn = LogicMain.CurrentTurn + num2;
				m_attractRadiusSqr = num3 * num3;
				AffectedCharacters = new BBList<SkillAreaHelper.AffectedNpc>();
				this.FindZones(9, 0.5f);
				Zones[0].IsFree = false;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				int currentTurn = LogicMain.CurrentTurn;
				if (currentTurn < m_startTurn || m_lastTurnUpdate == currentTurn)
				{
					return true;
				}
				m_lastTurnUpdate = currentTurn;
				if (m_affectedCount >= m_maxAffectedCount)
				{
					return ExitIfDone();
				}
				AffectedCharacters.ClearIfNotEmpty();
				int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.EnemyCharacter);
				NpcCharacter entity;
				for (int i = 0; i < entityCount; i++)
				{
					entity = LogicMain.EntityManager.GetEntity<NpcCharacter>(EntityTypes.EType.EnemyCharacter, i);
					if (entity.Id == EntityId.InvalidId)
					{
						continue;
					}
					float num = entity.Position.DistanceSquared(AreaCenter);
					if (!(num >= m_attractRadiusSqr) && MayGetJacked(entity) && this.CouldPotentiallyReachArea(entity))
					{
						float num2 = SkillAreaHelper.DummyPath.CalculateLength();
						if (!(num2 * num2 >= m_attractRadiusSqr))
						{
							AffectedCharacters.Add(new SkillAreaHelper.AffectedNpc(entity, num2));
						}
					}
				}
				if (AffectedCharacters.Count <= 0)
				{
					return ExitIfDone();
				}
				AffectedCharacters.Sort();
				entity = null;
				int num3 = -1;
				for (int j = 0; j < AffectedCharacters.Count; j++)
				{
					entity = AffectedCharacters[j].Npc;
					num3 = this.FindClosestZoneIdx(entity.Position);
					if (num3 >= 0)
					{
						break;
					}
				}
				if (num3 < 0)
				{
					return ExitIfDone();
				}
				m_affectedCount++;
				CharacterCondition characterCondition = entity.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				JackBoxConditionState.Payload payload = characterCondition.GetPayload<JackBoxConditionState.Payload>(EStateType.CharacterCondition_JackBoxed);
				payload.Set(Zones[num3].Position, AreaCenter);
				AdditiveNumberBuff buff = new AdditiveNumberBuff(EBuffType.JackInTheBox, m_endTurn - currentTurn + 1, 1f);
				entity.Buffs.AddBuff(buff);
				ICharacterAwarenessCarrier characterAwarenessCarrier = entity.StateMachines as ICharacterAwarenessCarrier;
				if (characterAwarenessCarrier != null)
				{
					EStateType currentStateType = characterAwarenessCarrier.CharacterAwareness.CurrentStateType;
					if (currentStateType == EStateType.CharacterAwareness_Alarmed)
					{
						characterAwarenessCarrier.CharacterAwareness.ChangeStateForced(EStateType.CharacterAwareness_Unaware);
					}
				}
				characterCondition.ExternalChange(EStateType.CharacterCondition_JackBoxed);
				return ExitIfDone();
			}

			public override void Abort(IRulesCarrier _entity)
			{
			}

			private static bool MayGetJacked(NpcCharacter _npc)
			{
				if (!(_npc.CharacterStats is ICharacterConditionStats))
				{
					return false;
				}
				if (_npc.CharacterStats.Get_int(StatTypes.EStatType.Character_Condition_IsJackBoxed) > 0)
				{
					return false;
				}
				if (_npc.GetKnowinglyInCombat())
				{
					return false;
				}
				if (_npc.NpcDefinition.Definition.Category == ENpcCategory.Courtesan_ENpcCategory)
				{
					return false;
				}
				CharacterCondition characterCondition = _npc.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				return characterCondition.IsStateChangeAllowed;
			}

			private bool ExitIfDone()
			{
				return m_endTurn > LogicMain.CurrentTurn;
			}
		}

		public JackBoxAreaSkillEffect(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.JackArea, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			IPositionEntity entity = TargetEntityId.GetEntity<IPositionEntity>();
			return new EffectInstance(this, entity, _stats);
		}
	}
}
