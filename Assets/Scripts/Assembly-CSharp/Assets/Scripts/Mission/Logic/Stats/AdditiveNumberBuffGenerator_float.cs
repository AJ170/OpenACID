using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class AdditiveNumberBuffGenerator_float : CustomBuffGenerator_float
	{
		public AdditiveNumberBuffGenerator_float(EBuffType _buffType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.BuffType = _buffType;
		}

		public AdditiveNumberBuffGenerator_float(StatTypes.EStatType _statType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.StatType = _statType;
		}

		private static BuffBase Generator(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			return (_buffType != EBuffType.Invalid) ? new AdditiveNumberBuff(_buffType, _durationType, _duration, _interval, _value) : new AdditiveNumberBuff(_statType, _durationType, _duration, _interval, _value);
		}
	}
}
