using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CharacterConflictStatsHelper : StatsHelper<CharacterConflictStatsHelper>
	{
		public CharacterConflictStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_CharacterConflict, StatTypes.EStatType.__End_Immutable_CharacterConflict), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_CharacterConflict, StatTypes.EStatType.__End_Mutable_CharacterConflict), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_CharacterConflict, StatTypes.EStatType.__End_Computed_CharacterConflict))
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseHealth, StatTypes.EStatType.Character_Conflict_Health);
			_stats.Set_bool(StatTypes.EStatType.Character_Conflict_AboutToDie, false);
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_BleedingDamage, 0f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_BleedingDamageRelative, 0f);
			_stats.Set_bool(StatTypes.EStatType.Character_Conflict_WasPickpocketed, false);
		}

		public override void UpdateComputedExplicit(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			Character source = _stats.Get_EntityId(StatTypes.EStatType.Character_Conflict_CurrentSourceCharacter).TryGetEntity<Character>();
			Character target = _stats.Get_EntityId(StatTypes.EStatType.Character_Conflict_CurrentTargetCharacter).TryGetEntity<Character>();
			EApplyDamageFlags flags = (EApplyDamageFlags)_stats.Get_int(StatTypes.EStatType.Character_Conflict_CurrentDamageFlags);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseHealth, StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
			UpdateComputedExplicitDerived(_stats, _instanceCarrier, source, target, flags);
			UpdateComputedExplicitPrimary(_stats, _instanceCarrier, source, target, flags);
		}

		private static void UpdateComputedExplicitDerived(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, Character _source, Character _target, EApplyDamageFlags _flags)
		{
			float piercingSurprise = 0f;
			if (CombatHelper.IsSet(_flags, EApplyDamageFlags.ApplySkills) && _target != null && _stats.Get_int(StatTypes.EStatType.Character_Conflict_PiercingStrikeCount) > 0)
			{
				piercingSurprise = _stats.Get_float(StatTypes.EStatType.Character_Conflict_PiercingSurpriseArmourFactor);
			}
			float overrideValue = _stats.Get_float(StatTypes.EStatType.Character_Conflict_ArmourDamageReductionOverride);
			float overrideValue2 = _stats.Get_float(StatTypes.EStatType.Character_Conflict_DodgeChanceOverride);
			float overrideValue3 = _stats.Get_float(StatTypes.EStatType.Character_Conflict_CriticalChanceOverride);
			float overrideValue4 = _stats.Get_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFlatOverride);
			float overrideValue5 = _stats.Get_float(StatTypes.EStatType.Character_Conflict_FeintOverride);
			float evasion = _stats.Get_float(StatTypes.EStatType.Character_Conflict_ActualEvasion);
			float precision = _stats.Get_float(StatTypes.EStatType.Character_Conflict_ActualPrecision);
			float targetArmour = _stats.Get_float(StatTypes.EStatType.Character_Conflict_ActualArmourRating);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_AttackChance, 1f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_DodgeChance, 1f - DerivedStatsHelper.DodgeChance(evasion, overrideValue2));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_CriticalChance, 1f - DerivedStatsHelper.CriticalChance(precision, overrideValue3));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_CriticalDamageBonus, 0f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_GunpowderDamageBonusFactor, 1f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_ArmourDamageReduction, DerivedStatsHelper.ArmourDamageReduction(targetArmour, overrideValue));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFlat, DerivedStatsHelper.ArmourPiercingFlat(precision, overrideValue4));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_ArmourPiercingFactor, DerivedStatsHelper.ArmourPiercingFactor(piercingSurprise));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_Feint, DerivedStatsHelper.Feint(evasion, overrideValue5));
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_ActualDamageMultiplier, 1f);
		}

		private static void UpdateComputedExplicitPrimary(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, Character _source, Character _target, EApplyDamageFlags _flags)
		{
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseWeaponDamage, StatTypes.EStatType.Character_Conflict_ActualWeaponDamage);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseStrength, StatTypes.EStatType.Character_Conflict_ActualStrength);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseEvasion, StatTypes.EStatType.Character_Conflict_ActualEvasion);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseArmourRating, StatTypes.EStatType.Character_Conflict_ActualArmourRating);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseCriticalMultiplier, StatTypes.EStatType.Character_Conflict_ActualCriticalMultiplier);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseCourage, StatTypes.EStatType.Character_Conflict_ActualCourage);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseRangedWeaponDamage, StatTypes.EStatType.Character_Conflict_ActualRangedlWeaponDamage);
			_stats.Copy(StatTypes.EStatType.Character_Conflict_BaseWeaponDeflect, StatTypes.EStatType.Character_Conflict_ActualWeaponDeflect);
			float num = _stats.Get_float(StatTypes.EStatType.Character_Conflict_BasePrecision);
			if (_target != null && _target.Buffs.HasBuffs(EBuffType.Shredded))
			{
				num *= _stats.Get_float(StatTypes.EStatType.Character_Conflict_PrecisionFactorShredded);
			}
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_ActualPrecision, num);
			_stats.Set_int(StatTypes.EStatType.Character_Conflict_PiercingStrikeCount, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Conflict_FastFingersStrikeCount, 0);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_PiercingSurpriseArmourFactor, 0f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_PrecisionFactorShredded, 1f);
			_stats.Set_bool(StatTypes.EStatType.Character_Conflict_InitAnonymousBonus, false);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_InitAnonymousDamageMultiplier, 1f);
			_stats.Set_float(StatTypes.EStatType.Character_Conflict_SkillDamageBonusFactor, 1f);
		}
	}
}
