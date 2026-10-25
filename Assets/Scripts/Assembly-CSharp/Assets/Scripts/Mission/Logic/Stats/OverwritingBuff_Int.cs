using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class OverwritingBuff_Int : OverwritingBuff
	{
		public OverwritingBuff_Int(EBuffType _buffType, int _turnDuration, int _turnInterval, int _value)
			: base(_buffType, _turnDuration, _turnInterval, new StatValueWrapper_Int
			{
				Int = _value
			})
		{
		}

		public OverwritingBuff_Int(EBuffType _buffType, int _value)
			: base(_buffType, new StatValueWrapper_Int
			{
				Int = _value
			})
		{
		}

		public OverwritingBuff_Int(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Int
			{
				Int = _value
			})
		{
		}

		public OverwritingBuff_Int(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Int
			{
				Int = _value
			})
		{
		}

		public OverwritingBuff_Int(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Int
			{
				Int = _value
			})
		{
		}

		public override object Clone()
		{
			OverwritingBuff_Int overwritingBuff_Int = new OverwritingBuff_Int(Type, DurationType, TurnDuration, TurnInterval, m_Value.Int);
			overwritingBuff_Int.IsActivePredicate = IsActivePredicate;
			return overwritingBuff_Int;
		}
	}
}
