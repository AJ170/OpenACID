using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public abstract class CombatCharacterVitalityBase<TEntity, TThis> : CharacterVitalityBase<TEntity, TThis> where TEntity : Character, ICharacterVitalityCarrierEntity, IParticipantCarrier where TThis : CharacterVitalityBase<TEntity, TThis>
	{
		protected CombatCharacterVitalityBase(TEntity _entity)
			: base(_entity)
		{
		}

		protected static void InitCombatCharacterVitalityBase()
		{
			CharacterVitalityBase<TEntity, TThis>.InitCharacterVitalityBase();
			StateMachineBase<TEntity, TThis>.OverrideStateHandlers(EStateType.Character_Alive, null, null, OnUpdateAlive);
			StateMachineBase<TEntity, TThis>.OverrideStateHandlers(EStateType.Character_Dead, OnEnterDead, null, null);
		}

		public new static void OnEnterDead(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _prevState, IState<TEntity> _currentState)
		{
			CharacterVitalityBase<TEntity, TThis>.OnEnterDead(_stateMachine, _payload, _prevState, _currentState);
			IBuffsCarrier entity = _stateMachine.Entity;
			if (entity != null)
			{
				entity.Buffs.RemoveBuffs(BuffHelpers.CancelBuffOnDeath, int.MaxValue);
			}
		}

		public static void OnUpdateAlive(IStateMachine<TEntity> _stateMachine, IPayload _payload, IState<TEntity> _currentState)
		{
			TEntity entity = _stateMachine.Entity;
			ApplyDamageOverTime(entity, StatTypes.EStatType.Character_Conflict_BleedingDamage);
			ApplyDamageOverTime(entity, StatTypes.EStatType.Character_Conflict_BleedingDamageRelative, LogicMain.PlayerEntity.PlayerStats.Get_float(StatTypes.EStatType.Player_TotalDamage));
			if (entity.GetHasZeroHealth())
			{
				_stateMachine.ChangeState(EStateType.Character_Dead);
			}
		}

		public static void ApplyDamageOverTime(Character _target, StatTypes.EStatType _damageStat, float _factor = 1f)
		{
			if (_target.GetDeadOrAboutToDie())
			{
				return;
			}
			EntityStats characterStats = _target.CharacterStats;
			float num = characterStats.Get_float(_damageStat) * _factor;
			if (num <= 0f)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (num < 1.7014117E+38f)
			{
				CombatHelper.ApplyGenericDamage(null, _target, EApplyDamageFlags.IsSkillOrigin, num);
				flag = _target.GetHasZeroHealth();
				if (!flag)
				{
				}
			}
			else
			{
				_target.EntityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, 0f);
				flag = true;
				flag2 = true;
			}
			if (flag)
			{
				_target.Cast<ICharacterVitalityCarrierEntity>().AddKillTypeFlag(EKillType.DoT);
				GrantKillToken(_damageStat);
			}
			if (!flag || !_target.IsAttached)
			{
				return;
			}
			AnimationTaskManager taskManager = _target.UnityComponentCache.AnimationController.TaskManager;
			if (_target.GetIsInParcour())
			{
				Messenger.Instance.Broadcast(new ParcourCancelEvent(_target.Id, ParcourCancelEvent.ECancelType.Die));
				taskManager.CreateAnimationTaskKnockdowned(null, _target, false, false);
				return;
			}
			AnimationTaskBase currentTask = taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
			if (currentTask != null && currentTask.TaskType == AnimationTaskType.Knockdowned)
			{
				currentTask.Cast<AnimationTaskKnockdowned>().OnGotHit();
				taskManager.CreateAnimationTaskGetHit(null, (AttackMeleeType)(1 + LogicMain.Random.Rand(0, 3)));
			}
			else if (flag2 || currentTask == null || currentTask.CanCancel())
			{
				if (flag2)
				{
					taskManager.CancelCurrentTaskForced(AnimationTaskManager.ELayer.Base);
				}
				UnityComponentCache unityComponentCache = _target.UnityComponentCache;
				Vector3 forward = unityComponentCache.Transform.forward;
				float num2 = Vector3.Dot(forward, unityComponentCache.NavMeshAgent.velocity.normalized);
				if (num2 > 0.1f)
				{
					IPositionProvider source = new SimplePositionProvider(_target.Position - forward);
					EffectsHelper.CharacterKnockdown(source, _target, 50);
				}
				else
				{
					taskManager.CreateAnimationTaskGetHit(null, (AttackMeleeType)(1 + LogicMain.Random.Rand(0, 3)));
				}
			}
			else if (currentTask.TaskType != AnimationTaskType.Dead)
			{
				_target.EntityStats.Set_float(StatTypes.EStatType.Character_Conflict_Health, 0.0001f);
				_target.Buffs.AddBuff(new AdditiveNumberBuff(_damageStat, float.MaxValue));
			}
		}

		private static void GrantKillToken(StatTypes.EStatType _damageStat)
		{
			if (_damageStat == StatTypes.EStatType.Character_Conflict_BleedingDamage || _damageStat == StatTypes.EStatType.Character_Conflict_BleedingDamageRelative)
			{
				StatTypes.EStatType eStatType = StatTypes.EStatType.Player_LastTurnKillTokenFromBleeding;
				int num = 9;
				PlayerStats playerStats = LogicMain.PlayerEntity.PlayerStats;
				int num2 = playerStats.Get_int(eStatType);
				if (num2 - LogicMain.CurrentTurn >= num && CombatHelper.IncreaseKillTokenCount(true))
				{
					playerStats.Set_int(eStatType, LogicMain.CurrentTurn);
				}
			}
		}
	}
}
