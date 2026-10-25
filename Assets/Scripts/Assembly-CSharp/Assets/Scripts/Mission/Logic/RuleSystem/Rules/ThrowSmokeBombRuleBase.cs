using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class ThrowSmokeBombRuleBase : SkillRuleBase<AssassinCharacter>
	{
		protected class AddCrowdHotspotEffect : AddCrowdHotspotSkillEffect<AssassinCharacter, IEntityIdSkillPayload>
		{
			public AddCrowdHotspotEffect(IRule _owner, ETargetSource _targetSource)
				: base(_owner, _targetSource, 0f, 15f, 20f, true, 7.5f, 5f, ReactionType.SmokeBombReaction)
			{
			}
		}

		protected ThrowSmokeBombRuleBase(RuleData.IRuleDefinition _definition, IList<float> _parameters)
			: base(_definition, _parameters)
		{
		}

		protected void CollectTargetsAndSetLastKnownPos(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, ISkillPayload _payload, BBList<EntityId> _targetEntityIds)
		{
			float num;
			using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = StatValueCollection.Apply(_entity))
			{
				num = buffedStatsCollectionHandle.Get_float(StatTypes.EStatType.Rule_EffectRadius);
				RulesHelper.LinearHirelingLevelValueProcessor2_float(_entity, buffedStatsCollectionHandle, num);
			}
			Vector3 position = _targetEntityId.GetEntity<IPositionEntity>().Position;
			IEnumerable<Character> enumerable = LogicMain.EntityManager.CollectEntitiesInRadius<Character>(position, num);
			float radius = Mathf.Min(num, 4f);
			foreach (Character item in enumerable)
			{
				if (!(item is AssassinCharacter) && item.EntityStats is ICharacterConditionStats && !item.GetIsDead())
				{
					_targetEntityIds.Add(item.Id);
					ICharacterConditionCarrier characterConditionCarrier = item.StateMachines as ICharacterConditionCarrier;
					if (characterConditionCarrier != null)
					{
						LastKnownAreaState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<LastKnownAreaState.Payload>(EStateType.CharacterCondition_SmokeBombed);
						payload.Set(position, radius);
					}
				}
			}
		}

		protected IEffectInstance OnHaltCombat(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, IEntityIdSkillPayload _payload)
		{
			CombatController instance = CombatController.Instance;
			if (CombatController.IsActive && instance.PlayerExitCombatMode == ExitCombatMode.Invalid)
			{
				instance.PlayerExitCombatMode = ExitCombatMode.EndOfCombatImminent;
			}
			return null;
		}

		protected bool AllReadyForExitCondition(IEffect _effect, AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, ISkillPayload _payload)
		{
			if (!CombatController.IsActive)
			{
				return true;
			}
			CombatController instance = CombatController.Instance;
			int num = instance.ParticipantCount();
			for (int i = 0; i < num; i++)
			{
				if (!instance.GetParticipant(i).ReadyForExitCombat())
				{
					return false;
				}
			}
			return num > 0;
		}

		protected abstract Vector3 SpawnPosGetter(ISkillsCarrier _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats);
	}
}
