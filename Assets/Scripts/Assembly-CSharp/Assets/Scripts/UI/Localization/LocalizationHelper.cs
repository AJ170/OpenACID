using System;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Localization
{
	public class LocalizationHelper
	{
		private static float[] m_thresholds = new float[6] { 10000f, 100000f, 1000000f, 10000000f, 100000000f, 1E+09f };

		public static string ParseStringToLocaKey(string _value, StringMapper _mapper = null)
		{
			StringId _id;
			if (StringId.TryParse(_value, out _id))
			{
				return LocalizationManager.Instance.Get(_id, _mapper);
			}
			return _value;
		}

		public static StringMapper GetStringMapperForGeneratedMissions(RegionDefinition _def, RegionDefinition.AppearanceOption _appearanceOption = null)
		{
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("Gen_Mission_Location", LocalizationManager.Instance.Get(_def.GetRegionNameShortLoca(_appearanceOption).Key));
			stringMapper.Add("Gen_Mission_Date", LocalizationManager.Instance.Get(_def.GetRegionDateLoca(_appearanceOption).Key));
			stringMapper.Add("Gen_Mission_Era", _def.Age.DisplayName());
			stringMapper.Add("Gen_Mission_Mood", _def.Mood.DisplayName());
			stringMapper.Add("Genmis_Enemy_Target", LocalizationManager.Instance.Get(3561));
			return stringMapper;
		}

		public static string AbbreviateNumbers(long _number)
		{
			return AbbreviateNumbers(_number, LocalizationManager.Instance.CurrentLanguage);
		}

		private static string AbbreviateNumbers(long _number, ELanguages _language)
		{
			if ((float)_number < m_thresholds[0])
			{
				return _number.ToString();
			}
			float num = 0f;
			int num2 = 0;
			AbbreviatedNumberMapping value = null;
			if (!UbiCore.Utils.Singleton<UIDataMapper>.Instance.AbbreviatedNumberMapping.TryGetValue(_language, out value))
			{
				value = UbiCore.Utils.Singleton<UIDataMapper>.Instance.AbbreviatedNumberMapping[ELanguages.en_GB];
			}
			int[] divider = value.Divider;
			int[] locaKey = value.LocaKey;
			for (int num3 = m_thresholds.Length - 1; num3 >= 0; num3--)
			{
				if ((float)_number >= m_thresholds[num3])
				{
					num2 = locaKey[num3];
					num = divider[num3];
					break;
				}
			}
			double value2 = Math.Floor((float)_number / num * 100f) / 100.0;
			StringMapper stringMapper = new StringMapper();
			stringMapper.Add("amount", LocalizationManager.Instance.GetDecimal(value2, "t"));
			StringMapper mapper = stringMapper;
			return LocalizationManager.Instance.Get(num2, mapper);
		}
	}
}
