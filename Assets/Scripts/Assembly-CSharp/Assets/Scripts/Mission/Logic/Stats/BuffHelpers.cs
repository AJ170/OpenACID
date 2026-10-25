using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public static class BuffHelpers
	{
		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public static IBuffGenerator MakeIncognitoBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_float(MakeIncognitoBuff, _ruleStats, StatTypes.EStatType.Rule_IncognitoBonusDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_IncognitoBonus, true);
		}

		public static IBuffGenerator MakeArmoredBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_float(MakeArmoredBuff, _ruleStats, StatTypes.EStatType.Rule_ArmourRatingBonusDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_ArmourRatingFactor, true);
		}

		public static IBuffGenerator MakeSilencingBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_bool(MakeSilencingBuff, _ruleStats, StatTypes.EStatType.Rule_SilencedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, true);
		}

		public static IBuffGenerator MakeStunningBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_int(MakeStunningBuff, _ruleStats, StatTypes.EStatType.Rule_StunDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, true);
		}

		public static IBuffGenerator MakeSweetMusicBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_int(MakeSweetMusicBuff, _ruleStats, StatTypes.EStatType.Rule_SweetMusicDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, true);
		}

		public static IBuffGenerator MakeSmokeBombedBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats, CustomBuffGeneratorBase.TurnCountProcessorDelegate _durationProcessor)
		{
			CustomBuffGenerator_int customBuffGenerator_int = new CustomBuffGenerator_int(MakeSmokeBombedBuff, _ruleStats, StatTypes.EStatType.Rule_StunDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, true);
			customBuffGenerator_int.DurationProcessor = _durationProcessor;
			return customBuffGenerator_int;
		}

		public static IBuffGenerator MakeBleedingBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats, CustomBuffGeneratorBase.ValueProcessorDelegate _valueProcessor, CustomBuffGeneratorBase.TurnCountProcessorDelegate _durationProcessor, StatTypes.EStatType _bleedAmountStatType = StatTypes.EStatType.Rule_BleedAmount)
		{
			CustomBuffGenerator_float customBuffGenerator_float = new CustomBuffGenerator_float(MakeBleedingBuff, _ruleStats, StatTypes.EStatType.Rule_BleedDuration, StatTypes.EStatType.Rule_BleedInterval, _bleedAmountStatType, true);
			customBuffGenerator_float.ValueProcessor = _valueProcessor;
			customBuffGenerator_float.DurationProcessor = _durationProcessor;
			return customBuffGenerator_float;
		}

		public static IBuffGenerator MakeBleedingRelativeBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats, CustomBuffGeneratorBase.ValueProcessorDelegate _valueProcessor, CustomBuffGeneratorBase.TurnCountProcessorDelegate _durationProcessor)
		{
			CustomBuffGenerator_float customBuffGenerator_float = new CustomBuffGenerator_float(MakeBleedingRelativeBuff, _ruleStats, StatTypes.EStatType.Rule_BleedDuration, StatTypes.EStatType.Rule_BleedInterval, StatTypes.EStatType.Rule_BleedAmountRelative, true);
			customBuffGenerator_float.ValueProcessor = _valueProcessor;
			customBuffGenerator_float.DurationProcessor = _durationProcessor;
			return customBuffGenerator_float;
		}

		public static IBuffGenerator MakeDazedBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats, CustomBuffGeneratorBase.TurnCountProcessorDelegate _durationProcessor)
		{
			MaximumNumberBuffGenerator_float maximumNumberBuffGenerator_float = new MaximumNumberBuffGenerator_float(EBuffType.Dazed, _ruleStats, StatTypes.EStatType.Rule_FrightenedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_FrightenedAttackDelayFactor);
			maximumNumberBuffGenerator_float.DurationProcessor = _durationProcessor;
			return maximumNumberBuffGenerator_float;
		}

		public static IBuffGenerator MakeFrightenedBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			IBuffGenerator[] buffGenerators = new IBuffGenerator[2]
			{
				new MaximumNumberBuffGenerator_float(EBuffType.Frightened, _ruleStats, StatTypes.EStatType.Rule_FrightenedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_FrightenedAttackDelayFactor),
				new MaximumNumberBuffGenerator_float(StatTypes.EStatType.Enemy_FearsomeFactor, _ruleStats, StatTypes.EStatType.Rule_FrightenedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Rule_FrightenedFearsomeFactor)
			};
			return new WrappingBuffGenerator(buffGenerators);
		}

		public static IBuffGenerator MakeConfusedBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats)
		{
			return new CustomBuffGenerator_int(MakeConfusedBuff, _ruleStats, StatTypes.EStatType.Rule_ConfusedDuration, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, true);
		}

		public static IBuffGenerator MakeConfusedBuffGenerator(BuffedReadonlyStatValueCollection _ruleStats, int _duration)
		{
			CustomBuffGenerator_int customBuffGenerator_int = new CustomBuffGenerator_int(MakeConfusedBuff, _ruleStats, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, StatTypes.EStatType.Invalid, false);
			customBuffGenerator_int.Duration = _duration;
			return customBuffGenerator_int;
		}

		public static void ApplyBuffGenerator(IBuffGenerator _generator, IEntity _creator, IBuffsCarrier _target)
		{
			_generator.MakeBuffs(s_BuffList, _creator, _target);
			for (int i = 0; i < s_BuffList.Count; i++)
			{
				_target.Buffs.AddBuff(s_BuffList[i]);
			}
		}

		public static bool CancelBuffOnEnterCombat(IBuff _buff)
		{
			return _buff.Type != EBuffType.Invalid && _buff.Definition.CancelOnEnterCombat;
		}

		public static bool CancelBuffOnDeath(IBuff _buff)
		{
			return _buff.Type != EBuffType.Invalid && _buff.Definition.CancelOnDeath;
		}

		private static BuffBase MakeIncognitoBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.Incognito, _duration, _value);
		}

		private static BuffBase MakeArmoredBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			return new MultiplicativeNumberBuff(EBuffType.Armored, _duration, _value);
		}

		private static BuffBase MakeSilencingBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, bool _value, bool _doInversion)
		{
			return new BooleanOrBuff(EBuffType.Silenced, _duration, true);
		}

		private static BuffBase MakeStunningBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, int _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.Stunned, _duration, 1f);
		}

		private static BuffBase MakeSweetMusicBuff(IEntity _creator, EBuffType _bufftype, StatTypes.EStatType _stattype, EBuffDurationType _durationtype, int _duration, int _interval, int _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.SweetMusic, _duration, 1f);
		}

		private static BuffBase MakeSmokeBombedBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, int _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.SmokeBombed, _duration, 1f);
		}

		private static BuffBase MakeBleedingBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.Bleeding, EBuffDurationType.Finite, _duration, _interval, _value);
		}

		private static BuffBase MakeBleedingRelativeBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.BleedingRelative, EBuffDurationType.Finite, _duration, _interval, _value);
		}

		private static BuffBase MakeConfusedBuff(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, int _value, bool _doInversion)
		{
			return new AdditiveNumberBuff(EBuffType.Confused, _duration, 1f);
		}
	}
}
