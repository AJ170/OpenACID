using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class OverwritingBuff_Bool : OverwritingBuff
	{
		public OverwritingBuff_Bool(EBuffType _buffType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _turnDuration, _turnInterval, new StatValueWrapper_Bool
			{
				Bool = _value
			})
		{
		}

		public OverwritingBuff_Bool(EBuffType _buffType, bool _value)
			: base(_buffType, new StatValueWrapper_Bool
			{
				Bool = _value
			})
		{
		}

		public OverwritingBuff_Bool(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Bool
			{
				Bool = _value
			})
		{
		}

		public OverwritingBuff_Bool(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Bool
			{
				Bool = _value
			})
		{
		}

		public OverwritingBuff_Bool(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Bool
			{
				Bool = _value
			})
		{
		}

		public override object Clone()
		{
			OverwritingBuff_Bool overwritingBuff_Bool = new OverwritingBuff_Bool(Type, DurationType, TurnDuration, TurnInterval, m_Value.Bool);
			overwritingBuff_Bool.IsActivePredicate = IsActivePredicate;
			return overwritingBuff_Bool;
		}
	}
}
