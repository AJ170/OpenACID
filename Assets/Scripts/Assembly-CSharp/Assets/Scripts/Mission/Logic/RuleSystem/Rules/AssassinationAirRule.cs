using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Motion;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public class AssassinationAirRule : AssassinationRuleBase
	{
		public AssassinationAirRule(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters, EStateType.PlayerMain_AssassinationAir)
		{
			AddMandatoryTrigger(new EntityInMinRangeSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, NavMeshLayers.WalkableLayerMaskPreset, true, MinRangeGetter));
			AddMandatoryTrigger(new TargetPredicateSkillTrigger<AssassinCharacter, IEntityIdSkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.Payload, CanJump));
			AddEffect(new AddCrowdHotspotSkillEffect<AssassinCharacter, IEntityIdSkillPayload>(this, ETargetSource.RuleOwner, 0f, 20f, 15f, true, 0f, 0f, ReactionType.Invalid));
		}

		private bool CanJump(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetId)
		{
			Entity entity = _targetId.Entity;
			Vector3 position = _entity.Position;
			Vector3 position2 = (entity as EnemyCharacter).Position;
			return ParcourHelper.IsJumpPathClear(position, position2);
		}

		private float MinRangeGetter(AssassinCharacter _entity, EntityId _targetId, IBuffedStatsCollectionHandle _stats)
		{
			Vector3 v = _targetId.GetEntity<IPositionEntity>().Position - _entity.Position;
			float f = Mathf.Atan(v.LengthXZ() / v.y) * 57.29578f;
			if (Mathf.Abs(f) > 50f)
			{
				return float.MaxValue;
			}
			return 0f;
		}
	}
}
