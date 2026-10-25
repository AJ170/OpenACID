using System;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class AdditiveNumberBuff : NumberBuff, ICloneable, IComparable<IBuff>, IBuff, INumberBuff, IAdditiveNumberBuff
	{
		public override int Priority
		{
			get
			{
				return 3;
			}
		}

		public AdditiveNumberBuff(EBuffType _buffType, int _turnDuration, float _magnitude)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _magnitude)
		{
		}

		public AdditiveNumberBuff(EBuffType _buffType, float _magnitude)
			: this(_buffType, EBuffDurationType.Infinite, 0, _magnitude)
		{
		}

		public AdditiveNumberBuff(EBuffType _buffType, int _turnDuration, int _turnInterval, float _magnitude)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public AdditiveNumberBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, float _magnitude)
			: this(_buffType, _durationType, _turnDuration, 0, _magnitude)
		{
		}

		public AdditiveNumberBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public AdditiveNumberBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude)
			: base(_statType, _durationType, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public AdditiveNumberBuff(StatTypes.EStatType _statType, float _magnitude)
			: this(_statType, EBuffDurationType.Infinite, 0, 0, _magnitude)
		{
		}

		private AdditiveNumberBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public override void Compute(IStatValueWrapper _source, float _payload)
		{
			float num = _source.Number + Magnitude;
			if (float.IsPositiveInfinity(num))
			{
				num = float.MaxValue;
			}
			else if (float.IsNegativeInfinity(num))
			{
				num = float.MinValue;
			}
			_source.Number = num;
		}

		public override object Clone()
		{
			AdditiveNumberBuff additiveNumberBuff = new AdditiveNumberBuff(Type, StatType, DurationType, TurnDuration, TurnInterval, Magnitude);
			additiveNumberBuff.IsActivePredicate = IsActivePredicate;
			return additiveNumberBuff;
		}
	}
}
