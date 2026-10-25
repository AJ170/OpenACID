using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class OverwritingBuff : BuffBase
	{
		protected readonly IStatValueWrapper m_Value;

		public override int Priority
		{
			get
			{
				return 0;
			}
		}

		protected OverwritingBuff(EBuffType _buffType, int _turnDuration, int _turnInterval, IStatValueWrapper _value)
			: base(_buffType, _turnDuration, _turnInterval)
		{
			m_Value = _value;
		}

		protected OverwritingBuff(EBuffType _buffType, IStatValueWrapper _value)
			: base(_buffType)
		{
			m_Value = _value;
		}

		protected OverwritingBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, IStatValueWrapper _value)
			: base(_buffType, _durationType, _turnDuration, _turnInterval)
		{
			m_Value = _value;
		}

		protected OverwritingBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, IStatValueWrapper _value)
			: base(_statType, _durationType, _turnDuration, _turnInterval)
		{
			m_Value = _value;
		}

		protected OverwritingBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, IStatValueWrapper _value)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval)
		{
			m_Value = _value;
		}

		public override void Compute(IStatValueWrapper _source, float _payload)
		{
			_source.CopyFrom(m_Value);
		}
	}
}
