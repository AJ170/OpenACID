using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class RuleBase<T> : IRule where T : IRulesCarrier
	{
		protected static readonly TriggerSet s_TriggerSet = new TriggerSet();

		private readonly RuleData.IRuleDefinition m_Definition;

		private readonly BBList<ITrigger> m_Triggers = new BBList<ITrigger>();

		private readonly BBList<ITrigger> m_MandatoryTriggers = new BBList<ITrigger>();

		private readonly BBList<IEffect> m_Effects = new BBList<IEffect>();

		private readonly ReadOnlyCollection<ITrigger> m_TriggersAccess;

		private readonly ReadOnlyCollection<ITrigger> m_MandatoryTriggersAccess;

		private readonly ReadOnlyCollection<IEffect> m_EffectsAccess;

		private readonly BuffedReadonlyStatValueCollection m_StatValueCollection;

		private static readonly BBList<Predicate<IEffect, IRulesCarrier>> s_effectConditionsList = new BBList<Predicate<IEffect, IRulesCarrier>>();

		private static readonly BBList<Predicate<ITrigger, IRulesCarrier>> s_triggerConditionsList = new BBList<Predicate<ITrigger, IRulesCarrier>>();

		private static readonly BBList<IEffect> s_UnfilteredEffects = new BBList<IEffect>();

		public Enum<RuleTypes> Type
		{
			get
			{
				return m_Definition.Type;
			}
		}

		public RuleData.IRuleDefinition Definition
		{
			get
			{
				return m_Definition;
			}
		}

		public IList<float> Parameters { get; private set; }

		public BuffedReadonlyStatValueCollection StatValueCollection
		{
			get
			{
				return m_StatValueCollection;
			}
		}

		public ReadOnlyCollection<ITrigger> Triggers
		{
			get
			{
				return m_TriggersAccess;
			}
		}

		public ReadOnlyCollection<ITrigger> MandatoryTriggers
		{
			get
			{
				return m_MandatoryTriggersAccess;
			}
		}

		public ReadOnlyCollection<IEffect> Effects
		{
			get
			{
				return m_EffectsAccess;
			}
		}

		protected RuleBase(RuleData.IRuleDefinition _definition, IList<float> _parameters)
		{
			m_TriggersAccess = new ReadOnlyCollection<ITrigger>(m_Triggers);
			m_MandatoryTriggersAccess = new ReadOnlyCollection<ITrigger>(m_MandatoryTriggers);
			m_EffectsAccess = new ReadOnlyCollection<IEffect>(m_Effects);
			m_Definition = _definition;
			m_StatValueCollection = new BuffedReadonlyStatValueCollection(m_Definition.StatValueCollection, _definition.Id);
			Parameters = _parameters;
		}

		public bool GetAreCategoryFlagsMatching(IRulesCarrier _entity)
		{
			for (int i = 0; i < m_Triggers.Count; i++)
			{
				ITrigger trigger = m_Triggers[i];
				if (!trigger.GetIsCategoryMatching(_entity))
				{
					return false;
				}
			}
			return true;
		}

		public bool IsFulfilled(IRulesCarrier _entity, Predicate<ITrigger, IRulesCarrier> _triggerCondition, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return IsFulfilledInternal((T)_entity, null, _triggerCondition, out _availabilityFlags);
		}

		public bool MeetsActivityConditions(IRulesCarrier _entity, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return ComputeFulfilledTriggers((T)_entity, Triggers, null, out _availabilityFlags, RulesHelper.ActivityConditionTriggerPredicate);
		}

		public void Apply(IRulesCarrier _entity, BBList<IEffect> _cumulatedEffects)
		{
			s_UnfilteredEffects.ClearIfNotEmpty();
			ApplyInternal((T)_entity, s_UnfilteredEffects);
			if (s_UnfilteredEffects.Count > 0)
			{
				FilterEffects((T)_entity, s_UnfilteredEffects, _cumulatedEffects);
			}
		}

		protected virtual void PrepareTrigger(T _entity, ITrigger _trigger)
		{
		}

		protected virtual void PrepareEffect(T _entity, IEffect _effect)
		{
			_effect.Prepare(_entity);
		}

		protected virtual bool IsFulfilledInternal(T _entity, TriggerSet _fulfilledTriggers, Predicate<ITrigger, IRulesCarrier> _triggerCondition, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return ComputeFulfilledTriggerConditionTriggers(_entity, Triggers, _fulfilledTriggers, out _availabilityFlags, _triggerCondition);
		}

		protected virtual void ApplyInternal(T _entity, BBList<IEffect> _unfilteredEffects)
		{
			ESkillAvailabilityFlags _availabilityFlags;
			if (IsFulfilledInternal(_entity, null, null, out _availabilityFlags))
			{
				_unfilteredEffects.AddRange(Effects);
			}
		}

		protected TTrigger AddTrigger<TTrigger>(TTrigger _trigger) where TTrigger : ITrigger
		{
			m_Triggers.Add(_trigger);
			if (_trigger.IsMandatory)
			{
				m_MandatoryTriggers.Add(_trigger);
			}
			return _trigger;
		}

		protected TriggerBase<TEntity> AddMandatoryTrigger<TEntity>(TriggerBase<TEntity> _trigger) where TEntity : IRulesCarrier
		{
			_trigger.IsMandatory = true;
			return AddTrigger(_trigger);
		}

		protected TTrigger AddMandatoryTrigger<TTrigger, TEntity>(TTrigger _trigger) where TTrigger : TriggerBase<TEntity> where TEntity : IRulesCarrier
		{
			_trigger.IsMandatory = true;
			return AddTrigger(_trigger);
		}

		protected TEffect AddEffect<TEffect>(TEffect _effect) where TEffect : IEffect
		{
			m_Effects.Add(_effect);
			return _effect;
		}

		protected TEffect AddEffectFront<TEffect>(TEffect _effect) where TEffect : IEffect
		{
			m_Effects.Insert(0, _effect);
			return _effect;
		}

		protected bool ComputeFulfilledTriggerConditionTriggers(T _entity, IList<ITrigger> _sourceTriggers, TriggerSet _fulfilledTriggers, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return ComputeFulfilledTriggers(_entity, _sourceTriggers, _fulfilledTriggers, out _availabilityFlags, RulesHelper.TriggerConditionTriggerPredicate);
		}

		protected bool ComputeFulfilledTriggerConditionTriggers(T _entity, IList<ITrigger> _sourceTriggers, TriggerSet _fulfilledTriggers, out ESkillAvailabilityFlags _availabilityFlags, Predicate<ITrigger, IRulesCarrier> _triggerCondition1)
		{
			return ComputeFulfilledTriggers(_entity, _sourceTriggers, _fulfilledTriggers, out _availabilityFlags, _triggerCondition1, RulesHelper.TriggerConditionTriggerPredicate);
		}

		protected void FilterEffects(T _entity, IList<IEffect> _sourceEffects, BBList<IEffect> _cumulatedEffects)
		{
			FilterEffects(_entity, _sourceEffects, _cumulatedEffects, (Predicate<IEffect, IRulesCarrier>)null);
		}

		protected void FilterEffects(T _entity, IList<IEffect> _sourceEffects, BBList<IEffect> _cumulatedEffects, Predicate<IEffect, IRulesCarrier> _effectCondition)
		{
			s_effectConditionsList.ClearIfNotEmpty();
			s_effectConditionsList.Add(RulesHelper.CategoryMatchingEffectPredicate);
			if (_effectCondition != null)
			{
				s_effectConditionsList.Add(_effectCondition);
			}
			FilterEffects(_entity, _sourceEffects, _cumulatedEffects, s_effectConditionsList);
		}

		private bool ComputeFulfilledTriggers(T _entity, IList<ITrigger> _sourceTriggers, TriggerSet _fulfilledTriggers, out ESkillAvailabilityFlags _availabilityFlags, Predicate<ITrigger, IRulesCarrier> _triggerCondition1)
		{
			return ComputeFulfilledTriggers(_entity, _sourceTriggers, _fulfilledTriggers, out _availabilityFlags, _triggerCondition1, null);
		}

		private bool ComputeFulfilledTriggers(T _entity, IList<ITrigger> _sourceTriggers, TriggerSet _fulfilledTriggers, out ESkillAvailabilityFlags _availabilityFlags, Predicate<ITrigger, IRulesCarrier> _triggerCondition1, Predicate<ITrigger, IRulesCarrier> _triggerCondition2)
		{
			s_triggerConditionsList.ClearIfNotEmpty();
			s_triggerConditionsList.Add(RulesHelper.CategoryMatchingTriggerPredicate);
			if (_triggerCondition1 != null)
			{
				s_triggerConditionsList.Add(_triggerCondition1);
			}
			if (_triggerCondition2 != null)
			{
				s_triggerConditionsList.Add(_triggerCondition2);
			}
			return ComputeFulfilledTriggers(_entity, _sourceTriggers, _fulfilledTriggers, out _availabilityFlags, s_triggerConditionsList);
		}

		private bool ComputeFulfilledTriggers(T _entity, IList<ITrigger> _sourceTriggers, TriggerSet _fulfilledTriggers, out ESkillAvailabilityFlags _availabilityFlags, BBList<Predicate<ITrigger, IRulesCarrier>> _triggerConditions)
		{
			_availabilityFlags = ESkillAvailabilityFlags.Unknown;
			if (_fulfilledTriggers != null)
			{
				_fulfilledTriggers.Clear();
			}
			using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = StatValueCollection.Apply(_entity as ISuperBuffsStatsCarrier))
			{
				for (int i = 0; i < _sourceTriggers.Count; i++)
				{
					ITrigger trigger = _sourceTriggers[i];
					bool flag = false;
					if (trigger.Predicate != null && !trigger.Predicate(trigger, _entity, buffedStatsCollectionHandle))
					{
						flag = true;
						if (_fulfilledTriggers != null)
						{
							_fulfilledTriggers.AddTrigger(trigger);
						}
					}
					if (_triggerConditions != null)
					{
						for (int j = 0; j < _triggerConditions.Count; j++)
						{
							Predicate<ITrigger, IRulesCarrier> predicate = _triggerConditions[j];
							if (!predicate(trigger, _entity))
							{
								flag = true;
								if (_fulfilledTriggers == null)
								{
									break;
								}
								_fulfilledTriggers.AddTrigger(trigger);
							}
						}
					}
					if (flag)
					{
						continue;
					}
					trigger.Reset();
					PrepareTrigger(_entity, trigger);
					if ((trigger.Sign != 0) ? trigger.IsActive(_entity, buffedStatsCollectionHandle) : (!trigger.IsActive(_entity, buffedStatsCollectionHandle)))
					{
						if (trigger.Sign == ETriggerSign.Positive)
						{
							_availabilityFlags |= trigger.GetAvailabilityFlags(_entity);
						}
						if (_fulfilledTriggers == null)
						{
							return false;
						}
					}
					else if (_fulfilledTriggers != null)
					{
						_fulfilledTriggers.AddTrigger(trigger);
					}
				}
			}
			return _fulfilledTriggers == null || _fulfilledTriggers.Triggers.Count == m_Triggers.Count;
		}

		private void FilterEffects(T _entity, IList<IEffect> _sourceEffects, BBList<IEffect> _cumulatedEffects, BBList<Predicate<IEffect, IRulesCarrier>> _effectConditions)
		{
			for (int i = 0; i < _sourceEffects.Count; i++)
			{
				IEffect effect = _sourceEffects[i];
				effect.Reset();
				PrepareEffect(_entity, effect);
				if (effect.Predicate != null && !effect.Predicate(effect, _entity))
				{
					continue;
				}
				if (_effectConditions != null)
				{
					bool flag = false;
					for (int j = 0; j < _effectConditions.Count; j++)
					{
						Predicate<IEffect, IRulesCarrier> predicate = _effectConditions[j];
						if (!predicate(effect, _entity))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
				}
				_cumulatedEffects.Add(m_Effects[i]);
			}
		}
	}
}
