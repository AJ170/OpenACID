using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.RuleSystem.Effects;
using Assets.Scripts.Mission.Logic.RuleSystem.Triggers;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Rules
{
	public abstract class PassiveStatModifierRuleBase<T> : RuleBase<T> where T : Character, IRulesCarrier, ISkillsCarrier
	{
		public CustomBuffGeneratorBase BuffGenerator { get; private set; }

		protected PassiveStatModifierRuleBase(RuleData.IRuleDefinition _ruleDefinition, IList<float> _parameters)
			: base(_ruleDefinition, _parameters)
		{
			SkillData.ISkillDefinition definition = UbiCore.Utils.Singleton<SkillData>.Instance.GetDefinition(Definition.SkillType);
			ETriggerFrequency frequency = definition.Frequency;
			ETriggerCategoryFlags categoryFlags = definition.Categories.Include(ETriggerCategoryFlags.NoBirth);
			AddMandatoryTrigger(new BirthTrigger<T>(this, frequency, categoryFlags));
			AddMandatoryTrigger(new SkillRankTrigger<T, ISkillPayload>(this, frequency, categoryFlags, ETargetSource.RuleOwner));
		}

		protected TGenerator AddBuff<TGenerator>(TGenerator _buffGenerator) where TGenerator : CustomBuffGeneratorBase
		{
			AddEffect(new BuffEffect<T>(this, _buffGenerator, ETargetSource.RuleOwner));
			BuffGenerator = _buffGenerator;
			return _buffGenerator;
		}

		protected TGenerator AddSuperBuff<TGenerator>(TGenerator _buffGenerator, int _ruleDefinitionId) where TGenerator : CustomBuffGeneratorBase
		{
			AddEffect(new SuperBuffEffect<T>(this, ETargetSource.RuleOwner, _buffGenerator, _ruleDefinitionId));
			BuffGenerator = _buffGenerator;
			return _buffGenerator;
		}

		protected TGenerator AddSuperBuff<TGenerator>(TGenerator _buffGenerator, Enum<RuleTypes> _ruleType) where TGenerator : CustomBuffGeneratorBase
		{
			AddEffect(new SuperBuffEffect<T>(this, ETargetSource.RuleOwner, _buffGenerator, _ruleType.GetDefinition().Id));
			BuffGenerator = _buffGenerator;
			return _buffGenerator;
		}

		protected TGenerator AddSuperBuff<TGenerator>(TGenerator _buffGenerator, Enum<SkillTypes> _skillType) where TGenerator : CustomBuffGeneratorBase
		{
			if (!_skillType.IsValid)
			{
				AddSuperBuff(_buffGenerator, -1);
			}
			else
			{
				Enum<SkillTypes> baseSkillType = _skillType.GetDefinition().BaseSkillType;
				int maxRank = _skillType.GetDefinition().MaxRank;
				for (int i = 0; i <= maxRank; i++)
				{
					Enum<SkillTypes> skillType = (Enum<SkillTypes>)((int)baseSkillType + i);
					AddSuperBuff(_buffGenerator, skillType.GetDefinition().RuleDefinition.Id);
				}
			}
			return _buffGenerator;
		}

		protected TGenerator AddSuperBuff<TGenerator>(TGenerator _buffGenerator) where TGenerator : CustomBuffGeneratorBase
		{
			return AddSuperBuff(_buffGenerator, -1);
		}

		protected CustomBuffGenerator<int> AddOverwritingSuperBuff_int(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<int> result;
			AddSuperBuff(result = new OverwritingBuffGenerator_int(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<int> AddOverwritingSuperBuff_int(StatTypes.EStatType _statType)
		{
			return AddAdditiveSuperBuff_int(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddOverwritingSuperBuff_float(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new OverwritingBuffGenerator_float(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddOverwritingSuperBuff_float(StatTypes.EStatType _statType)
		{
			return AddAdditiveSuperBuff_float(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<int> AddAdditiveSuperBuff_int(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<int> result;
			AddSuperBuff(result = new AdditiveNumberBuffGenerator_int(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<int> AddAdditiveSuperBuff_int(StatTypes.EStatType _statType)
		{
			return AddAdditiveSuperBuff_int(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddAdditiveSuperBuff_float(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new AdditiveNumberBuffGenerator_float(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddAdditiveSuperBuff_float(StatTypes.EStatType _statType)
		{
			return AddAdditiveSuperBuff_float(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<int> AddMultiplicativeSuperBuff_int(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<int> result;
			AddSuperBuff(result = new MultiplicativeNumberBuffGenerator_int(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<int> AddMultiplicativeSuperBuff_int(StatTypes.EStatType _statType)
		{
			return AddMultiplicativeSuperBuff_int(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddMultiplicativeSuperBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _sourceStatType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new MultiplicativeNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _sourceStatType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddMultiplicativeSuperBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _sourceStatType)
		{
			return AddMultiplicativeSuperBuff_float(_targetStatType, _sourceStatType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddMultiplicativeSuperBuff_float(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new MultiplicativeNumberBuffGenerator_float(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddMultiplicativeSuperBuff_float(StatTypes.EStatType _statType)
		{
			return AddMultiplicativeSuperBuff_float(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<int> AddMaximumSuperBuff_int(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<int> result;
			AddSuperBuff(result = new MaximumNumberBuffGenerator_int(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<int> AddMaximumSuperBuff_int(StatTypes.EStatType _statType)
		{
			return AddMaximumSuperBuff_int(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddMaximumSuperBuff_float(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new MaximumNumberBuffGenerator_float(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddMaximumSuperBuff_float(StatTypes.EStatType _statType)
		{
			return AddMaximumSuperBuff_float(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<int> AddMinimumSuperBuff_int(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<int> result;
			AddSuperBuff(result = new MinimumNumberBuffGenerator_int(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<int> AddMinimumSuperBuff_int(StatTypes.EStatType _statType)
		{
			return AddMinimumSuperBuff_int(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<float> AddMinimumSuperBuff_float(StatTypes.EStatType _statType, Enum<SkillTypes> _skillType)
		{
			CustomBuffGenerator<float> result;
			AddSuperBuff(result = new MinimumNumberBuffGenerator_float(_statType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _statType, false)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			}, _skillType);
			return result;
		}

		protected CustomBuffGenerator<float> AddMinimumSuperBuff_float(StatTypes.EStatType _statType)
		{
			return AddMinimumSuperBuff_float(_statType, Enum<SkillTypes>.Invalid);
		}

		protected CustomBuffGenerator<int> AddOverwritingBuff_int(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new OverwritingBuffGenerator_int(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddOverwritingBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new OverwritingBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<int> AddAdditiveBuff_int(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new AdditiveNumberBuffGenerator_int(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddAdditiveBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new AdditiveNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddAdditiveBuff_float(EBuffType _buffType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new AdditiveNumberBuffGenerator_float(_buffType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<int> AddMultiplicativeBuff_int(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultiplicativeNumberBuffGenerator_int(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<int> AddMultiplicativeBuff_int(EBuffType _buffType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultiplicativeNumberBuffGenerator_int(_buffType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMultiplicativeBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultiplicativeNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMultiplicativeBuff_float(EBuffType _buffType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultiplicativeNumberBuffGenerator_float(_buffType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddInverseMultiplicativeBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new InverseMultiplicativeNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddInverseMultiplicativeBuff_float(EBuffType _buffType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new InverseMultiplicativeNumberBuffGenerator_float(_buffType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMultInverseMultiplicativeBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultInverseMultiplicativeNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMultInverseMultiplicativeBuff_float(EBuffType _buffType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MultInverseMultiplicativeNumberBuffGenerator_float(_buffType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<int> AddMaximumBuff_int(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MaximumNumberBuffGenerator_int(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMaximumBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MaximumNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<int> AddMinimumBuff_int(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MinimumNumberBuffGenerator_int(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_int(_creator, _stats, _value, base.Parameters)
			});
		}

		protected CustomBuffGenerator<float> AddMinimumBuff_float(StatTypes.EStatType _targetStatType, StatTypes.EStatType _ruleStatType)
		{
			return AddBuff(new MinimumNumberBuffGenerator_float(_targetStatType, StatValueCollection, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, _ruleStatType)
			{
				DurationType = EBuffDurationType.Infinite,
				ValueProcessor = (IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value) => PassiveStatModifierRuleTools.LinearSkillParameterValueProcessor_float(_creator, _stats, _value, base.Parameters)
			});
		}
	}
}
