using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class SkillRuleBase<T> : RuleBase<T>, IRule, ISkillRule where T : IRulesCarrier, ISkillsCarrier
	{
		private readonly MaximumTriggeredSkillsTrigger<T, ISkillPayload> m_MaximumTriggeredSkillsTrigger;

		public Enum<SkillTypes> SkillType
		{
			get
			{
				return Definition.SkillType;
			}
		}

		public SkillData.ISkillDefinition SkillDefinition
		{
			get
			{
				return UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(SkillType);
			}
		}

		public ETriggerCategoryFlags SkillCategories
		{
			get
			{
				return SkillDefinition.Categories;
			}
		}

		public ETriggerFrequency SkillFrequency
		{
			get
			{
				return SkillDefinition.Frequency;
			}
		}

		protected SkillRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters, SkillFiredTrigger<T> _skillFiredTrigger)
			: base(_ruleDefinition, _parameters)
		{
			if (_skillFiredTrigger != null)
			{
				AddMandatoryTrigger(_skillFiredTrigger);
			}
			AddMandatoryTrigger(new CategorySkillTrigger<T>(this, SkillFrequency, SkillCategories));
			AddMandatoryTrigger(new SkillRankTrigger<T, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner));
			AddMandatoryTrigger(new TargetPredicateSkillTrigger<T, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, RuleOwnerAlive)
			{
				ActivityCondition = true
			});
			if ((ESkillActivationType.Slots & SkillDefinition.ActivationType) != 0)
			{
				m_MaximumTriggeredSkillsTrigger = AddMandatoryTrigger<MaximumTriggeredSkillsTrigger<T, ISkillPayload>, T>(new MaximumTriggeredSkillsTrigger<T, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner, 0, ESkillActivationType.Slots));
				AddMandatoryTrigger(new BlockingSkillActiveTrigger<T, ISkillPayload>(this, SkillFrequency, SkillCategories, ETargetSource.RuleOwner)
				{
					Sign = ETriggerSign.Negative,
					DelayedCheck = true
				});
			}
			AddEffect(new SkillFiredSkillEffect(this, SkillType));
		}

		protected SkillRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: this(_ruleDefinition, _parameters, (SkillFiredTrigger<T>)null)
		{
			AddMandatoryTrigger(new SkillFiredTrigger<T>(this, SkillType));
		}

		public TPayload GetSkillPayload<TPayload>(ISkillsCarrier _entity) where TPayload : class, ISkillPayload
		{
			return _entity.Skills.GetPayload<TPayload>(SkillType);
		}

		public void ActivateCategoryCheckDuringActivity(Predicate<ITrigger, IRulesCarrier, IBuffedStatsCollectionHandle> _predicate = null)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(MandatoryTriggers.Count((ITrigger t) => t is CategorySkillTrigger<T>) == 1))
			{
				TriggerBase<T> triggerBase = AddMandatoryTrigger(new CategorySkillTrigger<T>(this, SkillFrequency, SkillCategories.Include(ETriggerCategoryFlags.None))
				{
					ActivityCondition = true
				});
				if (_predicate != null)
				{
					triggerBase.Predicate = _predicate;
				}
			}
		}

		public void ActivateMaximumTriggeredSkillsCheckDuringActivity()
		{
			m_MaximumTriggeredSkillsTrigger.ActivityCondition = true;
		}

		protected override void PrepareTrigger(T _entity, ITrigger _trigger)
		{
			ISkillTrigger skillTrigger = _trigger as ISkillTrigger;
			if (skillTrigger != null)
			{
				skillTrigger.Payload = GetSkillPayload<ISkillPayload>(_entity);
			}
			base.PrepareTrigger(_entity, _trigger);
		}

		protected override void PrepareEffect(T _entity, IEffect _effect)
		{
			ISkillEffect skillEffect = _effect as ISkillEffect;
			if (skillEffect != null)
			{
				skillEffect.Payload = GetSkillPayload<ISkillPayload>(_entity);
			}
			base.PrepareEffect(_entity, _effect);
		}

		protected bool RuleOwnerAlive(T _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetId)
		{
			Character character = _targetId.TryGetEntity<Character>();
			return character != null && !character.GetIsDead();
		}
	}
}
