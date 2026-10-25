using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.Network.Missions.Helpers;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public static class RulesHelper
	{
		private static readonly BBList<IEffect> s_Effects = new BBList<IEffect>();

		private static readonly BBSimpleList<Enum<RuleTypes>> s_RulesList = new BBSimpleList<Enum<RuleTypes>>();

		private static readonly bool[] s_RulesSet = new bool[Enum<RuleTypes>.Count];

		private static int HirelingLevel
		{
			get
			{
				AvatarData.IAvatarDefinition hirelingDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetHirelingDefinition();
				return (hirelingDefinition != null) ? hirelingDefinition.Get_int(StatTypes.EStatType.Player_ItemPower) : 0;
			}
		}

		public static ETriggerCategory ToTriggerCategory(this EStateType _mainStateType)
		{
			switch (_mainStateType)
			{
			case EStateType.CombatCharacterMain_Roaming:
				return ETriggerCategory.Roaming;
			case EStateType.CombatCharacterMain_Combat:
				return ETriggerCategory.InCombat;
			case EStateType.PlayerMain_HidingInObject:
				return ETriggerCategory.HidingInObject;
			case EStateType.PlayerMain_HidingInPatrol:
				return ETriggerCategory.HidingInPatrol;
			case EStateType.PlayerMain_BeingChased:
				return ETriggerCategory.BeingChased;
			case EStateType.CombatCharacterMain_Parcour:
				return ETriggerCategory.Parcour;
			default:
				return ETriggerCategory.Invalid;
			}
		}

		public static EStateType ToPlayerMainState(this ETriggerCategory _triggerCategory)
		{
			return _triggerCategory.ToFlags().ToPlayerMainState();
		}

		public static EStateType ToPlayerMainState(this ETriggerCategoryFlags _flags)
		{
			switch (_flags)
			{
			case ETriggerCategoryFlags.Roaming:
				return EStateType.CombatCharacterMain_Roaming;
			case ETriggerCategoryFlags.InCombat:
				return EStateType.CombatCharacterMain_Combat;
			case ETriggerCategoryFlags.HidingInObject:
				return EStateType.PlayerMain_HidingInObject;
			case ETriggerCategoryFlags.HidingInPatrol:
				return EStateType.PlayerMain_HidingInPatrol;
			case ETriggerCategoryFlags.BeingChased:
				return EStateType.PlayerMain_BeingChased;
			case ETriggerCategoryFlags.Parcour:
				return EStateType.CombatCharacterMain_Parcour;
			default:
				return EStateType.Invalid;
			}
		}

		public static ETriggerCategoryFlags ToFlags(this ETriggerCategory _category)
		{
			return (ETriggerCategoryFlags)(1 << (int)_category);
		}

		public static bool HasCategory(this ETriggerCategoryFlags _flags, ETriggerCategory _category)
		{
			return (_flags & _category.ToFlags()) != 0;
		}

		public static bool Intersects(this ETriggerCategoryFlags _first, ETriggerCategoryFlags _second)
		{
			return (_first & _second) != 0;
		}

		public static bool Contains(this ETriggerCategoryFlags _baseSet, ETriggerCategoryFlags _testSet)
		{
			return (_baseSet & _testSet) == _testSet;
		}

		public static ETriggerCategoryFlags Include(this ETriggerCategoryFlags _flags, ETriggerCategoryFlags _includes)
		{
			return _flags | (ETriggerCategoryFlags.Exclusions & ~_includes);
		}

		public static bool CategoryMatchingLivingTargetPredicate(IEffect _effect, IRulesCarrier _entity)
		{
			if (!CategoryMatchingEffectPredicate(_effect, _entity))
			{
				return false;
			}
			ITargetEffect targetEffect = _effect as ITargetEffect;
			if (targetEffect != null)
			{
				Character character = targetEffect.TargetEntityId.TryGetEntity<Character>();
				if (character != null)
				{
					return !character.GetIsDead();
				}
			}
			return true;
		}

		public static bool CategoryMatchingTriggerPredicate(ITrigger _trigger, IRulesCarrier _entity)
		{
			return _trigger.IsMandatory || _trigger.GetIsCategoryMatching(_entity);
		}

		public static bool TriggerConditionTriggerPredicate(ITrigger _trigger, IRulesCarrier _entity)
		{
			return _trigger.TriggerCondition;
		}

		public static bool MandatoryTriggerPredicate(ITrigger _trigger, IRulesCarrier _entity)
		{
			return _trigger.IsMandatory;
		}

		public static bool ActivityConditionTriggerPredicate(ITrigger _trigger, IRulesCarrier _entity)
		{
			IActivityConditionTrigger activityConditionTrigger = _trigger as IActivityConditionTrigger;
			if (activityConditionTrigger == null)
			{
				return false;
			}
			return activityConditionTrigger.ActivityCondition && _trigger.GetIsCategoryMatching(_entity);
		}

		public static bool CategoryMatchingEffectPredicate(IEffect _effect, IRulesCarrier _entity)
		{
			return _effect.GetIsCategoryMatching(_entity);
		}

		public static bool IsFulfilled(IRulesCarrier _entity, Enum<RuleTypes> _ruleType, Predicate<ITrigger, IRulesCarrier> _predicate, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return _entity.Rules.RulesCollection.GetRule(_ruleType).IsFulfilled(_entity, _predicate, out _availabilityFlags);
		}

		public static bool MeetsActivityConditions(IRulesCarrier _entity, Enum<RuleTypes> _ruleType, out ESkillAvailabilityFlags _availabilityFlags)
		{
			return _entity.Rules.RulesCollection.GetRule(_ruleType).MeetsActivityConditions(_entity, out _availabilityFlags);
		}

		public static void ApplyRules(IRulesCarrier _entity, ETriggerFrequency _frequency, BBList<IEffectInstance> _effectInstances)
		{
			RulesCollection rulesCollection = _entity.Rules.RulesCollection;
			for (int i = 0; i < s_RulesList.Count; i++)
			{
				s_RulesSet[(int)s_RulesList[i]] = false;
			}
			s_RulesList.Clear();
			IRule rule = null;
			int num = 0;
			ETriggerCategoryFlags currentTriggerCategoryFlags = _entity.Rules.CurrentTriggerCategoryFlags;
			for (int j = 0; j <= (int)_frequency; j++)
			{
				num = rulesCollection.GetCount((ETriggerFrequency)j, currentTriggerCategoryFlags);
				for (int k = 0; k < num; k++)
				{
					rule = rulesCollection.GetRule((ETriggerFrequency)j, currentTriggerCategoryFlags, k);
					bool flag = s_RulesSet[(int)rule.Type];
					if (j != 0 && flag)
					{
						continue;
					}
					if (j == 0 && !flag)
					{
						s_RulesSet[(int)rule.Type] = true;
						s_RulesList.Add(rule.Type);
					}
					s_Effects.ClearIfNotEmpty();
					using (IBuffedStatsCollectionHandle stats = rule.StatValueCollection.Apply(_entity as ISuperBuffsStatsCarrier))
					{
						rule.Apply(_entity, s_Effects);
						for (int l = 0; l < s_Effects.Count; l++)
						{
							IEffectInstance effectInstance = s_Effects[l].Apply(_entity, stats);
							if (effectInstance != null)
							{
								_effectInstances.Add(effectInstance);
							}
						}
					}
				}
			}
		}

		public static object LinearHirelingLevelValueProcessor_fixed(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value, StatTypes.EStatType _statType)
		{
			int num = MissionMathHelper.FloatToFixed(_stats.Get_float(_statType));
			return (int)_value + num * HirelingLevel;
		}

		public static object LinearHirelingLevelValueProcessor_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value, StatTypes.EStatType _statType)
		{
			float num = _stats.Get_float(_statType);
			return (float)_value + num * (float)HirelingLevel;
		}

		public static object LinearHirelingLevelValueProcessor1_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_float(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_1);
		}

		public static object LinearHirelingLevelValueProcessor2_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_float(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_2);
		}

		public static object LinearHirelingLevelValueProcessor3_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_float(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_3);
		}

		public static object LinearHirelingLevelValueProcessor4_float(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_float(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_4);
		}

		public static object LinearHirelingLevelValueProcessor_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value, StatTypes.EStatType _statType)
		{
			float num = _stats.Get_float(_statType);
			return (int)_value + (int)(num * (float)HirelingLevel);
		}

		public static object LinearHirelingLevelValueProcessor1_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_int(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_1);
		}

		public static object LinearHirelingLevelValueProcessor2_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_int(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_2);
		}

		public static object LinearHirelingLevelValueProcessor3_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_int(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_3);
		}

		public static object LinearHirelingLevelValueProcessor4_int(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value)
		{
			return LinearHirelingLevelValueProcessor_int(_creator, _stats, _value, StatTypes.EStatType.Rule_HirelingLevelFactor_4);
		}

		public static int LinearHirelingLevelTurnCountProcessor(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount, StatTypes.EStatType _statType)
		{
			float num = _stats.Get_float(_statType);
			return _turnCount + (int)(num * (float)HirelingLevel);
		}

		public static int LinearHirelingLevelTurnCountProcessor1(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount)
		{
			return LinearHirelingLevelTurnCountProcessor(_creator, _stats, _turnCount, StatTypes.EStatType.Rule_HirelingLevelFactor_1);
		}

		public static int LinearHirelingLevelTurnCountProcessor2(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount)
		{
			return LinearHirelingLevelTurnCountProcessor(_creator, _stats, _turnCount, StatTypes.EStatType.Rule_HirelingLevelFactor_2);
		}

		public static int LinearHirelingLevelTurnCountProcessor3(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount)
		{
			return LinearHirelingLevelTurnCountProcessor(_creator, _stats, _turnCount, StatTypes.EStatType.Rule_HirelingLevelFactor_3);
		}

		public static int LinearHirelingLevelTurnCountProcessor4(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount)
		{
			return LinearHirelingLevelTurnCountProcessor(_creator, _stats, _turnCount, StatTypes.EStatType.Rule_HirelingLevelFactor_4);
		}

		public static StatTypes.EStatType GetHirelingScalingFactorStat(IReadonlyStatValueCollection _stats, StatTypes.EStatType _statType)
		{
			for (int i = 81; i <= 84; i++)
			{
				IReadonlyStatCollectionValue readonlyStatCollectionValue = _stats.GetReadonlyStatCollectionValue((StatTypes.EStatType)i);
				for (int j = 0; j < readonlyStatCollectionValue.Count; j++)
				{
					if (readonlyStatCollectionValue.Get_int(j) == (int)_statType)
					{
						return (StatTypes.EStatType)(77 + i - 81);
					}
				}
			}
			return StatTypes.EStatType.Invalid;
		}

		public static float GetScaledRuleStatValue(IReadonlyStatValueCollection _stats, StatTypes.EStatType _statType, int _hirelingLevel)
		{
			float value_number = _stats.GetReadonlyStatValue(_statType).Value_number;
			StatTypes.EStatType hirelingScalingFactorStat = GetHirelingScalingFactorStat(_stats, _statType);
			if (hirelingScalingFactorStat == StatTypes.EStatType.Invalid)
			{
				return value_number;
			}
			float num = _stats.Get_float(hirelingScalingFactorStat);
			return value_number + num * (float)_hirelingLevel;
		}
	}
}
