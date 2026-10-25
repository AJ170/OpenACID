using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class BirthTrigger<T> : CategoryTriggerBase<T> where T : Character, IRulesCarrier
	{
		public override ETriggerCategoryFlags Categories
		{
			get
			{
				return m_Categories;
			}
			protected set
			{
				m_Categories = value.Include(ETriggerCategoryFlags.NonInteractive | ETriggerCategoryFlags.NoBirth);
			}
		}

		public BirthTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(ETriggerType.Birth, _owner, _frequency, _categoryFlags)
		{
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && ((IEntityStatsCarrier)_entity).EntityStats.Get_int(StatTypes.EStatType.Character_BirthTurn) == LogicMain.CurrentTurn;
		}
	}
}
