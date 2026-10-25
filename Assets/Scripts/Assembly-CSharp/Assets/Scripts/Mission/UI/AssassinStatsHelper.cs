using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.UI
{
	public static class AssassinStatsHelper
	{
		private class AssassinStatsMappingEntry
		{
			public StatTypes.EStatType BaseStatType;

			public StatTypes.EStatType ActualStatType;
		}

		private static readonly BBDictionary<int, AssassinStatsMappingEntry> s_AssassinStatsMapping;

		static AssassinStatsHelper()
		{
			s_AssassinStatsMapping = new BBDictionary<int, AssassinStatsMappingEntry>
			{
				{
					0,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Character_Conflict_BaseHealth,
						ActualStatType = StatTypes.EStatType.Character_Conflict_ActualMaxHealth
					}
				},
				{
					1,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Character_Conflict_BaseStrength,
						ActualStatType = StatTypes.EStatType.Character_Conflict_ActualStrength
					}
				},
				{
					2,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Character_Conflict_BaseEvasion,
						ActualStatType = StatTypes.EStatType.Character_Conflict_ActualEvasion
					}
				},
				{
					3,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Character_Conflict_BasePrecision,
						ActualStatType = StatTypes.EStatType.Character_Conflict_ActualPrecision
					}
				},
				{
					4,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Player_BaseIncognito,
						ActualStatType = StatTypes.EStatType.Player_ActualIncognito
					}
				},
				{
					5,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Character_Conflict_BaseArmourRating,
						ActualStatType = StatTypes.EStatType.Character_Conflict_ActualArmourRating
					}
				},
				{
					16,
					new AssassinStatsMappingEntry
					{
						BaseStatType = StatTypes.EStatType.Player_BaseFearsomeFactor,
						ActualStatType = StatTypes.EStatType.Player_BaseFearsomeFactor
					}
				}
			};
		}

		public static bool IsPrimaryStat(StatTypes.EStatType _stat)
		{
			foreach (KeyValuePair<int, AssassinStatsMappingEntry> item in s_AssassinStatsMapping)
			{
				if ((item.Value.BaseStatType == _stat || item.Value.ActualStatType == _stat) && item.Key < 6)
				{
					return true;
				}
			}
			return false;
		}

		public static float GetStat(AvatarData.IAvatarDefinition _avatarDefinition, EAssassinStat _statType, bool _ignoreBuffs, out bool _isBuffed)
		{
			_isBuffed = false;
			switch (_statType)
			{
			case EAssassinStat.Health:
			case EAssassinStat.Strength:
			case EAssassinStat.Evasion:
			case EAssassinStat.Precision:
			case EAssassinStat.Incognito:
			case EAssassinStat.FearsomeFactor:
				return ApplyBuffs(_avatarDefinition, _statType, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.ArmorRating:
				return (!_ignoreBuffs) ? ApplyBuffs(_avatarDefinition, _statType, _ignoreBuffs, ref _isBuffed) : 0f;
			case EAssassinStat.Damage:
			{
				float num = ((!_ignoreBuffs) ? ApplyBuffs(_avatarDefinition, StatTypes.EStatType.Character_Conflict_BaseWeaponDamage, StatTypes.EStatType.Character_Conflict_ActualWeaponDamage, _ignoreBuffs, ref _isBuffed) : 6f);
				float stat = GetStat(_avatarDefinition, EAssassinStat.Strength, _ignoreBuffs, out _isBuffed);
				float num2 = ApplyBuffs(_avatarDefinition, 1f, StatTypes.EStatType.Character_Conflict_ActualDamageMultiplier, _ignoreBuffs, ref _isBuffed);
				return num2 * (num + DerivedStatsHelper.StrengthDamage(stat, _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageOverride)));
			}
			case EAssassinStat.GunpowderBonus:
				return ApplyBuffs(_avatarDefinition, 0f, StatTypes.EStatType.Character_Conflict_GunpowderDamageBonusFactor, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.Dodge:
				return 1f - ApplyBuffs(_avatarDefinition, 1f - DerivedStatsHelper.DodgeChance(GetStat(_avatarDefinition, EAssassinStat.Evasion, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_DodgeChanceOverride)), StatTypes.EStatType.Character_Conflict_DodgeChance, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.CriticalChances:
				return 1f - ApplyBuffs(_avatarDefinition, 1f - DerivedStatsHelper.CriticalChance(GetStat(_avatarDefinition, EAssassinStat.Precision, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_CriticalChanceOverride)), StatTypes.EStatType.Character_Conflict_CriticalChance, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.CriticalModifier:
				return ApplyBuffs(_avatarDefinition, StatTypes.EStatType.Character_Conflict_BaseCriticalMultiplier, StatTypes.EStatType.Character_Conflict_ActualCriticalMultiplier, _ignoreBuffs, ref _isBuffed) - 1f;
			case EAssassinStat.StealthModifier:
				return ApplyBuffs(_avatarDefinition, DerivedStatsHelper.StealthModifier(GetStat(_avatarDefinition, EAssassinStat.Incognito, _ignoreBuffs, out _isBuffed)), StatTypes.EStatType.Player_StealthModifier, _ignoreBuffs, ref _isBuffed) / 100f;
			case EAssassinStat.StealthDamage:
				return ApplyBuffs(_avatarDefinition, 1f, StatTypes.EStatType.Character_Conflict_InitAnonymousDamageMultiplier, _ignoreBuffs, ref _isBuffed) - 1f;
			case EAssassinStat.ArmorBlock:
			{
				float baseStatValue = DerivedStatsHelper.ArmourDamageReduction(GetStat(_avatarDefinition, EAssassinStat.ArmorRating, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_ArmourDamageReductionOverride));
				return ApplyBuffs(_avatarDefinition, baseStatValue, StatTypes.EStatType.Character_Conflict_ArmourDamageReduction, _ignoreBuffs, ref _isBuffed);
			}
			case EAssassinStat.Parry:
				return DerivedStatsHelper.StrengthDamageReduction(GetStat(_avatarDefinition, EAssassinStat.Strength, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_StrengthDamageReductionOverride)) + ApplyBuffs(_avatarDefinition, StatTypes.EStatType.Character_Conflict_BaseWeaponDeflect, StatTypes.EStatType.Character_Conflict_ActualWeaponDeflect, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.Feinting:
				return ApplyBuffs(_avatarDefinition, DerivedStatsHelper.Feint(GetStat(_avatarDefinition, EAssassinStat.Evasion, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_FeintOverride)), StatTypes.EStatType.Character_Conflict_Feint, _ignoreBuffs, ref _isBuffed);
			case EAssassinStat.ArmorPiercing:
				return ApplyBuffs(_avatarDefinition, DerivedStatsHelper.ArmourPiercingFlat(GetStat(_avatarDefinition, EAssassinStat.Precision, _ignoreBuffs, out _isBuffed), _avatarDefinition.Get_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFlatOverride)), StatTypes.EStatType.Character_Conflict_ArmourPiercingFlat, _ignoreBuffs, ref _isBuffed);
			default:
				return 0f;
			}
		}

		public static bool IsFactor(EAssassinStat _assassinStat)
		{
			switch (_assassinStat)
			{
			case EAssassinStat.CriticalChances:
			case EAssassinStat.CriticalModifier:
			case EAssassinStat.GunpowderBonus:
			case EAssassinStat.StealthDamage:
			case EAssassinStat.StealthModifier:
			case EAssassinStat.Dodge:
			case EAssassinStat.FearsomeFactor:
				return true;
			default:
				return false;
			}
		}

		private static float ApplyBuffs(AvatarData.IAvatarDefinition _avatarDefinition, EAssassinStat _assassinStat, bool _ignore, ref bool _isBuffed)
		{
			AssassinStatsMappingEntry assassinStatsMappingEntry = s_AssassinStatsMapping[(int)_assassinStat];
			return ApplyBuffs(_avatarDefinition, assassinStatsMappingEntry.BaseStatType, assassinStatsMappingEntry.ActualStatType, _ignore, ref _isBuffed);
		}

		private static float ApplyBuffs(AvatarData.IAvatarDefinition _avatarDefinition, StatTypes.EStatType _baseStatType, StatTypes.EStatType _buffedStatType, bool _ignore, ref bool _isBuffed)
		{
			return ApplyBuffs(_avatarDefinition, _avatarDefinition.Get_float(_baseStatType), _buffedStatType, _ignore, ref _isBuffed);
		}

		private static float ApplyBuffs(AvatarData.IAvatarDefinition _avatarDefinition, float _baseStatValue, StatTypes.EStatType _buffedStatType, bool _ignore, ref bool _isBuffed)
		{
			float num = _baseStatValue;
			if (!_ignore)
			{
				BBList<SkillData.SkillPreview> bBList = new BBList<SkillData.SkillPreview>();
				IReadonlyStatCollectionValue readonlyStatCollectionValue = _avatarDefinition.StatValueCollection.GetReadonlyStatCollectionValue(StatTypes.EStatType.Character_Skills);
				for (int i = 0; i < readonlyStatCollectionValue.Count; i++)
				{
					string text = readonlyStatCollectionValue.Get_string(i);
					if (!string.IsNullOrEmpty(text))
					{
						SkillData.SkillPreview skillPreview = UbiCore.Utils.Singleton<SkillData>.Instance.GetSkillPreview(text, true);
						if (skillPreview != null && skillPreview.StatType == _buffedStatType)
						{
							bBList.Add(skillPreview);
						}
					}
				}
				bBList.Sort((SkillData.SkillPreview l, SkillData.SkillPreview r) => l.BonusType.CompareTo(r.BonusType));
				int j;
				for (j = 0; j < bBList.Count && bBList[j].BonusType != SkillData.SkillPreview.EBonusType.Multiplicative; j++)
				{
					if (bBList[j].BonusType == SkillData.SkillPreview.EBonusType.Additive)
					{
						num += bBList[j].Magnitude;
						_isBuffed = true;
					}
				}
				float num2 = num;
				for (; j < bBList.Count; j++)
				{
					if (bBList[j].BonusType == SkillData.SkillPreview.EBonusType.Multiplicative)
					{
						num = num - num2 + num2 * bBList[j].Magnitude;
						_isBuffed = true;
					}
				}
			}
			return num;
		}
	}
}
