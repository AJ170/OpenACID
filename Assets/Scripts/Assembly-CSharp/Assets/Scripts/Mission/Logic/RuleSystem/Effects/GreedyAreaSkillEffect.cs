using AcierProtocol;
using Assets.Scripts.Crowd;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class GreedyAreaSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		protected class EffectInstance : EffectInstanceBase, SkillAreaHelper.IAreaInstance, IGreedyAreaEffectInstance
		{
			private const float ScanClamping = 2f;

			private const float EdgeDecisionDistance = 3f;

			private const float EnemyDistanceFactor = 0.7f;

			private const float ZoneRadius = 0.8f;

			private const int ZoneCount = 9;

			private const float CrowdRadius = 4f;

			private const EntityTypes.EType TargetType = EntityTypes.EType.NpcCharacter;

			private int m_lastTurnUpdate = -1;

			private readonly int m_endTurn;

			private readonly float m_attractRadiusSqr;

			private int m_greedyCount;

			private readonly int m_maxGreedyCount;

			public BBList<SkillAreaHelper.AffectedNpc> AffectedCharacters { get; set; }

			public BBList<SkillAreaHelper.Zone> Zones { get; set; }

			public bool[,] ZoneConnections { get; set; }

			public Vector3 AreaCenter { get; set; }

			public EffectInstance(GreedyAreaSkillEffect<T, TPayload> _effect, IPositionEntity _entity, IBuffedStatsCollectionHandle _stats)
				: base(_effect)
			{
				int num = _stats.Get_int(StatTypes.EStatType.Rule_GreedyDuration);
				float num2 = _stats.Get_float(StatTypes.EStatType.Rule_GreedyAttractionRadius);
				float throwRange = _stats.Get_float(StatTypes.EStatType.Rule_ThrowRange);
				m_maxGreedyCount = _stats.Get_int(StatTypes.EStatType.Rule_ThrowTargets);
				ScanForAreaCenter(_entity, throwRange);
				m_endTurn = LogicMain.CurrentTurn + num;
				m_attractRadiusSqr = num2 * num2;
				AffectedCharacters = new BBList<SkillAreaHelper.AffectedNpc>();
				this.FindZones(9, 0.8f);
				Assets.Scripts.Crowd.Crowd.Instance.AddHotspot(new CrowdHotspotData
				{
					Position = AreaCenter,
					DynamicWeight = 1.5f,
					EndTime = LogicMain.CurrentTime + (float)num * 0.1f,
					Radius = num2,
					Flee = false,
					TriggerRadius = 4f,
					TriggerDuration = 3f,
					TriggerReactionType = 21
				});
			}

			public override bool Update(IRulesCarrier _entity)
			{
				int currentTurn = LogicMain.CurrentTurn;
				if (m_lastTurnUpdate == currentTurn)
				{
					return true;
				}
				m_lastTurnUpdate = currentTurn;
				if (m_greedyCount >= m_maxGreedyCount)
				{
					return CleanupIfDone();
				}
				AffectedCharacters.ClearIfNotEmpty();
				int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.NpcCharacter);
				for (int i = 0; i < entityCount; i++)
				{
					NpcCharacter entity = LogicMain.EntityManager.GetEntity<NpcCharacter>(EntityTypes.EType.NpcCharacter, i);
					if (entity.Id == EntityId.InvalidId)
					{
						continue;
					}
					float num = entity.Position.DistanceSquared(AreaCenter);
					if (num >= m_attractRadiusSqr || !MayGetGreedy(entity) || !this.CouldPotentiallyReachArea(entity))
					{
						continue;
					}
					float num2 = SkillAreaHelper.DummyPath.CalculateLength();
					if (!(num2 * num2 >= m_attractRadiusSqr))
					{
						if (entity is EnemyCharacter)
						{
							num2 *= 0.7f;
						}
						AffectedCharacters.Add(new SkillAreaHelper.AffectedNpc(entity, num2));
					}
				}
				if (AffectedCharacters.Count <= 0)
				{
					return CleanupIfDone();
				}
				AffectedCharacters.Sort();
				int a = Mathf.Min(m_maxGreedyCount - m_greedyCount, AffectedCharacters.Count);
				a = Mathf.Max(a, 0);
				for (int j = 0; j < a; j++)
				{
					int num3 = this.FindRandomZoneIdx();
					if (num3 < 0)
					{
						break;
					}
					m_greedyCount++;
					NpcCharacter npc = AffectedCharacters[j].Npc;
					CharacterCondition characterCondition = npc.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
					GreedyConditionState.Payload payload = characterCondition.GetPayload<GreedyConditionState.Payload>(EStateType.CharacterCondition_Greedy);
					payload.Set(this, num3);
					AdditiveNumberBuff buff = new AdditiveNumberBuff(EBuffType.Greedy, m_endTurn - currentTurn, 1f);
					npc.Buffs.AddBuff(buff);
					ICharacterAwarenessCarrier characterAwarenessCarrier = npc.StateMachines as ICharacterAwarenessCarrier;
					if (characterAwarenessCarrier != null)
					{
						EStateType currentStateType = characterAwarenessCarrier.CharacterAwareness.CurrentStateType;
						if (currentStateType == EStateType.CharacterAwareness_Alarmed)
						{
							characterAwarenessCarrier.CharacterAwareness.ChangeStateForced(EStateType.CharacterAwareness_Unaware);
						}
					}
					characterCondition.ExternalChange(EStateType.CharacterCondition_Greedy);
				}
				return CleanupIfDone();
			}

			public override void Abort(IRulesCarrier _entity)
			{
			}

			public void FindNewTarget(GreedyConditionState.Payload _payload)
			{
				int num = (_payload.ZoneIdxPrev = _payload.ZoneIdxCurrent);
				int num2 = num;
				if (num2 == 0)
				{
					num2 = this.FindRandomZoneIdx(1, Zones.Count - 1, num2);
					if (num2 < 0)
					{
						num2 = _payload.ZoneIdxCurrent;
					}
				}
				else
				{
					int[] array = new int[3];
					int num3 = 0;
					int num4 = ((num2 <= 1) ? (Zones.Count - 1) : (num2 - 1));
					if (this.CanReachZone(num4, num2))
					{
						array[num3++] = num4;
					}
					int num5 = ((num2 >= Zones.Count - 1) ? 1 : (num2 + 1));
					if (this.CanReachZone(num5, num2))
					{
						array[num3++] = num5;
					}
					if (this.CanReachZone(0, num2))
					{
						array[num3++] = 0;
					}
					if (num3 > 0)
					{
						num2 = array[LogicMain.Random.Rand(0, num3 - 1)];
						Zones[num2].IsFree = false;
					}
				}
				_payload.ZoneIdxCurrent = num2;
				Vector2 vector = Random.insideUnitCircle * Zones[num2].Radius;
				_payload.Target = Zones[num2].Position + new Vector3(vector.x, 0f, vector.y);
				NavMeshHit hit;
				if (NavMesh.SamplePosition(_payload.Target, out hit, 2f, SkillAreaHelper.WalkableMask))
				{
					_payload.Target = hit.position;
				}
			}

			public void ReachedTarget(GreedyConditionState.Payload _payload)
			{
				if (_payload.ZoneIdxPrev >= 0)
				{
					if (_payload.ZoneIdxPrev != _payload.ZoneIdxCurrent)
					{
						Zones[_payload.ZoneIdxPrev].IsFree = true;
					}
					_payload.ZoneIdxPrev = -1;
				}
			}

			private static bool MayGetGreedy(NpcCharacter _npc)
			{
				if (!(_npc.CharacterStats is ICharacterConditionStats))
				{
					return false;
				}
				if (_npc.CharacterStats.Get_int(StatTypes.EStatType.Character_Condition_IsGreedy) > 0)
				{
					return false;
				}
				if (_npc.NpcStats.Get_bool(StatTypes.EStatType.Npc_IgnoreCoins))
				{
					return false;
				}
				if (_npc.GetKnowinglyInCombat())
				{
					return false;
				}
				if (_npc.NpcDefinition.Definition.Category == ENpcCategory.Courtesan_ENpcCategory || _npc.NpcDefinition.Definition.Category == ENpcCategory.Doctor_ENpcCategory)
				{
					return false;
				}
				CharacterCondition characterCondition = _npc.CharacterStateMachines.Cast<ICharacterConditionCarrier>().CharacterCondition;
				return characterCondition.IsStateChangeAllowed;
			}

			private bool CleanupIfDone()
			{
				if (m_endTurn > LogicMain.CurrentTurn)
				{
					return true;
				}
				int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.NpcCharacter);
				for (int i = 0; i < entityCount; i++)
				{
					NpcCharacter entity = LogicMain.EntityManager.GetEntity<NpcCharacter>(EntityTypes.EType.NpcCharacter, i);
					ICharacterConditionCarrier characterConditionCarrier = entity.StateMachines as ICharacterConditionCarrier;
					if (characterConditionCarrier != null)
					{
						GreedyConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<GreedyConditionState.Payload>(EStateType.CharacterCondition_Greedy);
						if (payload.AreaInstance == this)
						{
							payload.Reset();
						}
					}
				}
				return false;
			}

			private void ScanForAreaCenter(IPositionEntity _entity, float _throwRange)
			{
				Vector3 vector = GreedyAreaSkillEffect<T, TPayload>.ComputeZoneDirection();
				Vector3 position = _entity.Position;
				if (NavMesh.SamplePosition(position, out SkillAreaHelper.DummyHit, 0.5f, NavMeshLayers.WalkableLayerMaskPreset))
				{
					position = SkillAreaHelper.DummyHit.position;
				}
				if (NavigationHelper.SafeRaycast(position, position + vector * (_throwRange + 2f), out SkillAreaHelper.DummyHit, NavMeshLayers.WalkableLayerMaskPreset))
				{
					float num = Vector3.Distance(position, SkillAreaHelper.DummyHit.position);
					if (num > 3f)
					{
						num = Mathf.Min(num - 2f, _throwRange);
						num = Mathf.Max(2f, num);
						AreaCenter = _entity.Position + Vector3.up + vector * num;
						return;
					}
				}
				position = _entity.Position + Vector3.up;
				if (Physics.Raycast(position, vector, out SkillAreaHelper.DummyRayHit, _throwRange + 2f, Layers.StaticCollidersLayersMaskPreset))
				{
					float num2 = Vector3.Distance(position, SkillAreaHelper.DummyRayHit.point);
					num2 = Mathf.Min(num2 - 2f, _throwRange);
					num2 = Mathf.Max(2f, num2);
					AreaCenter = position + vector * num2;
				}
				else
				{
					AreaCenter = position + vector * _throwRange;
				}
			}
		}

		public GreedyAreaSkillEffect(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.GreedyArea, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			IPositionEntity entity = TargetEntityId.GetEntity<IPositionEntity>();
			return new EffectInstance(this, entity, _stats);
		}

		public static Vector3 ComputeZoneDirection()
		{
			UnityEngine.Camera main = UnityEngine.Camera.main;
			Vector3 forward = main.transform.forward;
			forward.y = 0f;
			float magnitude = forward.magnitude;
			if (magnitude > 0.001f)
			{
				return forward / magnitude;
			}
			return Vector3.forward;
		}
	}
}
