using Assets.Scripts.Mission.UI.LogicInterface;
using UbiCore;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class InverseMultiplicativeNumberBuffGenerator_float : CustomBuffGenerator_float
	{
		public InverseMultiplicativeNumberBuffGenerator_float(EBuffType _buffType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.BuffType = _buffType;
		}

		public InverseMultiplicativeNumberBuffGenerator_float(StatTypes.EStatType _statType, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats = true)
			: base(Generator, _ruleStatValueCollection, _ruleStatBuffDuration, _ruleStatBuffInterval, _ruleStatBuffValue, _useSuperStats)
		{
			base.StatType = _statType;
		}

		private static BuffBase Generator(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, float _value, bool _doInversion)
		{
			float num = ((!_doInversion) ? _value : _value.Clamp(0f, 1f));
			return (_buffType != EBuffType.Invalid) ? new MultiplicativeNumberBuff(_buffType, _durationType, _duration, _interval, (!_doInversion) ? num : (1f - num)) : new MultiplicativeNumberBuff(_statType, _durationType, _duration, _interval, (!_doInversion) ? num : (1f - num));
		}
	}
}
