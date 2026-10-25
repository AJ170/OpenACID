using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Types;
using Mission.Helpers;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class RuleDataExtensions
	{
		public static StringMapper GetStringMapperForRules(this RuleData.IRuleDefinition _def, StringId _ruleKey, int _hirelingLevel)
		{
			StringMapper stringMapper = new StringMapper();
			List<string> parametersInString = LocalizationManager.Instance.GetParametersInString(_ruleKey);
			string value = "_unit";
			for (int i = 0; i < parametersInString.Count; i++)
			{
				string text = parametersInString[i];
				bool flag = text.EndsWith(value);
				if (flag)
				{
					text = text.Substring(0, text.LastIndexOf(value));
				}
				StatTypes.EStatType _result;
				if (!EnumHelpers.TryParse<StatTypes.EStatType>(text, out _result))
				{
					continue;
				}
				string key = parametersInString[i];
				IStatValue statValue = _def.GetReadonlyStatValueBase(_result) as IStatValue;
				if (statValue != null)
				{
					if (flag)
					{
						stringMapper.Add(key, LocalizationManager.Instance.Get(UbiCore.Utils.Singleton<UIDataMapper>.Instance.UnitDescriptions[(int)statValue.Unit].NameShortId));
					}
					else
					{
						stringMapper.Add(key, new UIRule(statValue, _def, _hirelingLevel));
					}
				}
			}
			return stringMapper;
		}
	}
}
