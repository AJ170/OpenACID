using System;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class NumberBuff : BuffBase, ICloneable, IComparable<IBuff>, IBuff, INumberBuff
	{
		private readonly float m_magnitude;

		public float Magnitude
		{
			get
			{
				return m_magnitude;
			}
		}

		protected NumberBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude, int _turnStartDelay = 0)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, _turnStartDelay)
		{
			m_magnitude = _magnitude;
		}

		protected NumberBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude, int _turnStartDelay = 0)
			: base(_statType, _durationType, _turnDuration, _turnInterval, _turnStartDelay)
		{
			m_magnitude = _magnitude;
		}

		protected NumberBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude, int _turnStartDelay = 0)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, _turnStartDelay)
		{
			m_magnitude = _magnitude;
		}
	}
}
