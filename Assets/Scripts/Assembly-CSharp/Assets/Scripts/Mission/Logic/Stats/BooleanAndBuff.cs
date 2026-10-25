using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class BooleanAndBuff : BooleanBuff
	{
		public override int Priority
		{
			get
			{
				return 5;
			}
		}

		public BooleanAndBuff(EBuffType _buffType, int _turnDuration, bool _value)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _value)
		{
		}

		public BooleanAndBuff(EBuffType _buffType, bool _value)
			: this(_buffType, EBuffDurationType.Infinite, 0, _value)
		{
		}

		public BooleanAndBuff(EBuffType _buffType, int _turnDuration, int _turnInterval, bool _value)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanAndBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, bool _value)
			: this(_buffType, _durationType, _turnDuration, 0, _value)
		{
		}

		public BooleanAndBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanAndBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public BooleanAndBuff(StatTypes.EStatType _statType, bool _value)
			: this(_statType, EBuffDurationType.Infinite, 0, 0, _value)
		{
		}

		private BooleanAndBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, _value)
		{
		}

		public override void Compute(IStatValueWrapper _source, float _payload)
		{
			_source.Bool = _source.Bool && base.Value;
		}

		public override object Clone()
		{
			BooleanAndBuff booleanAndBuff = new BooleanAndBuff(Type, StatType, DurationType, TurnDuration, TurnInterval, base.Value);
			booleanAndBuff.IsActivePredicate = IsActivePredicate;
			return booleanAndBuff;
		}
	}
}
