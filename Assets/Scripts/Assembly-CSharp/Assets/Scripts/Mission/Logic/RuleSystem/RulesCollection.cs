using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using UbiCore.Debug;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class RulesCollection
	{
		public class RuleParamPair
		{
			public RuleData.IRuleDefinition RuleDefinition;

			public IList<float> Parameters;
		}

		private readonly BBList<IRule>[,] m_Rules = new BBList<IRule>[2, 2048];

		private readonly IRule[] m_RulesPerType = new IRule[Enum<RuleTypes>.Count];

		public RulesCollection(BBList<RuleParamPair> _rules)
		{
			for (int i = 0; i < _rules.Count; i++)
			{
				Profiling.Start("Creating Rule", float.MaxValue);
				Profiling.Start("Activator", float.MaxValue);
				IRule rule = (IRule)Activator.CreateInstance(_rules[i].RuleDefinition.InstanceType, _rules[i].RuleDefinition, _rules[i].Parameters);
				Profiling.Stop();
				m_RulesPerType[(int)_rules[i].RuleDefinition.Type] = rule;
				Profiling.Start("Preparing Lists", float.MaxValue);
				for (int j = 0; j < 2048; j++)
				{
					bool flag = true;
					for (int k = 0; k < rule.MandatoryTriggers.Count; k++)
					{
						ITrigger trigger = rule.MandatoryTriggers[k];
						if (!trigger.Categories.Contains((ETriggerCategoryFlags)j))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						continue;
					}
					for (int l = 0; l < rule.Triggers.Count; l++)
					{
						ITrigger trigger2 = rule.Triggers[l];
						if (trigger2.Categories.Contains((ETriggerCategoryFlags)j))
						{
							BBList<IRule> orAddList = GetOrAddList(trigger2.Frequency, (ETriggerCategoryFlags)j);
							if (!orAddList.Contains(rule))
							{
								orAddList.Add(rule);
							}
						}
					}
				}
				Profiling.Stop();
				Profiling.Stop();
			}
			Profiling.Start("Trimming", float.MaxValue);
			for (int m = 0; m < m_RulesPerType.Length; m++)
			{
				IRule rule2 = m_RulesPerType[m];
				if (rule2 == null)
				{
					continue;
				}
				for (int n = 0; n < 2048; n++)
				{
					for (int num = 0; num < rule2.Triggers.Count; num++)
					{
						ITrigger trigger3 = rule2.Triggers[num];
						if (trigger3.Categories.Contains((ETriggerCategoryFlags)n))
						{
							BBList<IRule> list = GetList(trigger3.Frequency, (ETriggerCategoryFlags)n);
							if (list != null)
							{
								list.TrimExcess();
							}
						}
					}
				}
			}
			Profiling.Stop();
		}

		public int GetCount(ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
		{
			BBList<IRule> list = GetList(_frequency, _categoryFlags);
			return (list != null) ? list.Count : 0;
		}

		public IRule GetRule(ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, int _idx)
		{
			return GetList(_frequency, _categoryFlags)[_idx];
		}

		public IRule GetRule(Enum<RuleTypes> _type)
		{
			return m_RulesPerType[(int)_type];
		}

		private BBList<IRule> GetOrAddList(ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
		{
			Profiling.Start("RulesCollection::GetOrAddList", float.MaxValue);
			BBList<IRule> bBList = GetList(_frequency, _categoryFlags);
			if (bBList == null)
			{
				bBList = (m_Rules[(int)_frequency, (int)_categoryFlags] = new BBList<IRule>());
			}
			Profiling.Stop();
			return bBList;
		}

		private BBList<IRule> GetList(ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
		{
			return m_Rules[(int)_frequency, (int)_categoryFlags];
		}
	}
}
