using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public abstract class TriggerBase<T> : ITrigger where T : IRulesCarrier
	{
		protected ETriggerCategoryFlags m_Categories;

		public ETriggerFrequency Frequency { get; protected set; }

		public virtual ETriggerCategoryFlags Categories
		{
			get
			{
				return m_Categories;
			}
			protected set
			{
				m_Categories = value.Include(ETriggerCategoryFlags.NonInteractive);
			}
		}

		public ETriggerType Type { get; private set; }

		public ETriggerSign Sign { get; set; }

		public IRule Owner { get; private set; }

		public Predicate<ITrigger, IRulesCarrier, IBuffedStatsCollectionHandle> Predicate { get; set; }

		public bool IsFrequencyMatching
		{
			get
			{
				return Frequency == ETriggerFrequency.Framewise || LogicMain.IsInTurnUpdate;
			}
		}

		public bool TriggerCondition { get; set; }

		public bool IsMandatory { get; set; }

		protected TriggerBase(ETriggerType _type, IRule _owner)
		{
			Type = _type;
			Owner = _owner;
			Sign = ETriggerSign.Positive;
			TriggerCondition = true;
			Categories = ETriggerCategoryFlags.None;
		}

		public virtual ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			return ESkillAvailabilityFlags.Unknown;
		}

		public virtual bool GetIsCategoryMatching(IRulesCarrier _entity)
		{
			return Categories.Include(ETriggerCategoryFlags.NonInteractive | ETriggerCategoryFlags.NoBirth).Contains(_entity.Rules.CurrentTriggerCategoryFlags);
		}

		public bool IsActive(IRulesCarrier _entity, IBuffedStatsCollectionHandle _stats)
		{
			return IsActive((T)_entity, _stats);
		}

		public virtual void Reset()
		{
		}

		protected virtual bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return IsFrequencyMatching && GetIsCategoryMatching(_entity);
		}
	}
}
