using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class OverwritingBuff_Float : OverwritingBuff
	{
		public OverwritingBuff_Float(EBuffType _buffType, int _turnDuration, int _turnInterval, float _value)
			: base(_buffType, _turnDuration, _turnInterval, new StatValueWrapper_Float
			{
				Float = _value
			})
		{
		}

		public OverwritingBuff_Float(EBuffType _buffType, float _value)
			: base(_buffType, new StatValueWrapper_Float
			{
				Float = _value
			})
		{
		}

		public OverwritingBuff_Float(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Float
			{
				Float = _value
			})
		{
		}

		public OverwritingBuff_Float(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Float
			{
				Float = _value
			})
		{
		}

		public OverwritingBuff_Float(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, new StatValueWrapper_Float
			{
				Float = _value
			})
		{
		}

		public override object Clone()
		{
			OverwritingBuff_Float overwritingBuff_Float = new OverwritingBuff_Float(Type, DurationType, TurnDuration, TurnInterval, m_Value.Float);
			overwritingBuff_Float.IsActivePredicate = IsActivePredicate;
			return overwritingBuff_Float;
		}
	}
}
