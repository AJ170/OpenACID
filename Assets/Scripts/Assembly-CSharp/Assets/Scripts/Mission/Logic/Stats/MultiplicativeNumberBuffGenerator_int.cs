using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class MultiplicativeNumberBuffGenerator_int : CustomBuffGenerator_int
	{
		public MultiplicativeNumberBuffGenerator_int(EBuffType _buffType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.BuffType = _buffType;
		}

		public MultiplicativeNumberBuffGenerator_int(StatTypes.EStatType _statType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.StatType = _statType;
		}

		private static BuffBase Generator(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, int _value, bool _doInversion)
		{
			return (_buffType != EBuffType.Invalid) ? new MultiplicativeNumberBuff(_buffType, _durationType, _duration, _interval, _value) : new MultiplicativeNumberBuff(_statType, _durationType, _duration, _interval, _value);
		}
	}
}
