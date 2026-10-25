using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class AssassinationRuleBase : ApproachSkillRuleBase
	{
		private const float EnemyRange = 15f;

		protected AssassinationRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters, EStateType _playerMainTargetState)
			: base(_ruleDefinition, _parameters)
		{
			AddMandatoryTrigger(new LivingEnemySkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload));
			AddMandatoryTrigger(new StateMachineStateSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, EStateMachineType.CharacterAwareness, EStateType.CharacterAwareness_Alarmed)
			{
				Sign = ETriggerSign.Negative
			});
			EMovementSpeed speed = base.ApproachSkillDefinition.Speed;
			if (speed != EMovementSpeed.Invalid_EMovementSpeed)
			{
				AddMandatoryTrigger(new ProfileStateSkillTrigger<AssassinCharacter, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, speed.ToStateType()));
			}
			AddMandatoryTrigger(new AssassinExposedTrigger(this, SkillFrequency, SkillCategories)
			{
				Sign = ETriggerSign.Negative
			});
			AddMandatoryTrigger(new ConditionSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, EStateType.CharacterCondition_Knockdowned)
			{
				Sign = ETriggerSign.Negative
			});
			AddEffect(new SwitchStateMachineSkillEffect<AssassinCharacter, IEntityIdSkillPayload, PlayerMain.TargetState.Payload>(this, ETargetSource.RuleOwner, EStateMachineType.PlayerMain, _playerMainTargetState, true, SetTarget));
			AddEffect(HeartseekerRule.MakeEffect(this, ETargetSource.RuleOwner));
			AddEffect(new TargetCollectorBuffSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.Payload, EnemiesInRangeCollector, BuffHelpers.MakeConfusedBuffGenerator(StatValueCollection), null));
		}

		protected void SetTarget(PlayerMain.TargetState.Payload _payload, IEntityIdSkillPayload _entityIdSkillPayload)
		{
			_payload.Target = _entityIdSkillPayload.Id.GetEntity<EnemyCharacter>();
			_payload.InteractionReason = SkillDefinition.Cast<SkillData.ApproachSkillDefinition>().InteractionReason;
		}

		private void EnemiesInRangeCollector(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _entityIdSkillPayload, BBList<EntityId> _targetEntityIds)
		{
			BBList<EnemyCharacter> bBList = new BBList<EnemyCharacter>();
			EnemyHelper.FindEnemiesInRange(_targetEntityId.GameEntity.Position, 15f, null, bBList);
			int count = bBList.Count;
			for (int i = 0; i < count; i++)
			{
				EnemyCharacter enemyCharacter = bBList[i];
				EntityId id = enemyCharacter.Id;
				if (id != _targetEntityId && enemyCharacter.CharacterStats.Get_int(StatTypes.EStatType.Character_Detection_Range) < 3)
				{
					_targetEntityIds.Add(id);
				}
			}
		}
	}
}
