using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class BooleanOrBuff : BooleanBuff
	{
		public override int Priority
		{
			get
			{
				return 6;
			}
		}

		public BooleanOrBuff(EBuffType _buffType, int _turnDuration, bool _value)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _value)
		{
		}

		public BooleanOrBuff(EBuffType _buffType, bool _value)
			: this(_buffType, EBuffDurationType.Infinite, 0, _value)
		{
		}

		public BooleanOrBuff(EBuffType _buffType, int _turnDuration, int _turnInterval, bool _value)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanOrBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, bool _value)
			: this(_buffType, _durationType, _turnDuration, 0, _value)
		{
		}

		public BooleanOrBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanOrBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanOrBuff(StatTypes.EStatType _statType, bool _value)
			: this(_statType, EBuffDurationType.Infinite, 0, 0, _value)
		{
		}

		private BooleanOrBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public override void Compute(IStatValueWrapper _source, float _payload)
		{
			_source.Bool = _source.Bool || base.Value;
		}

		public override object Clone()
		{
			BooleanOrBuff booleanOrBuff = new BooleanOrBuff(Type, StatType, DurationType, TurnDuration, TurnInterval, base.Value);
			booleanOrBuff.IsActivePredicate = IsActivePredicate;
			return booleanOrBuff;
		}
	}
}
