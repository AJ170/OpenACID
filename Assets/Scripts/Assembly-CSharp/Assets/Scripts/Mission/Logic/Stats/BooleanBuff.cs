using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class BooleanBuff : BuffBase
	{
		private readonly bool m_value;

		public bool Value
		{
			get
			{
				return m_value;
			}
		}

		protected BooleanBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval)
		{
			m_value = _value;
		}

		protected BooleanBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval)
		{
			m_value = _value;
		}

		protected BooleanBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, bool _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval)
		{
			m_value = _value;
		}
	}
}
