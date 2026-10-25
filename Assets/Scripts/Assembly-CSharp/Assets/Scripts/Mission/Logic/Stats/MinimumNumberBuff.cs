using System;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class MinimumNumberBuff : NumberBuff
	{
		public override int Priority
		{
			get
			{
				return 1;
			}
		}

		public MinimumNumberBuff(EBuffType _buffType, int _turnDuration, float _magnitude)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _magnitude)
		{
		}

		public MinimumNumberBuff(EBuffType _buffType, float _magnitude)
			: this(_buffType, EBuffDurationType.Infinite, 0, _magnitude)
		{
		}

		public MinimumNumberBuff(EBuffType _buffType, int _turnDuration, int _turnInterval, float _magnitude)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public MinimumNumberBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, float _magnitude)
			: this(_buffType, _durationType, _turnDuration, 0, _magnitude)
		{
		}

		public MinimumNumberBuff(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude, int _turnStartDelay = 0)
			: base(_buffType, _durationType, _turnDuration, _turnInterval, _magnitude, _turnStartDelay)
		{
		}

		public MinimumNumberBuff(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude, int _turnStartDelay = 0)
			: base(_statType, _durationType, _turnDuration, _turnInterval, _magnitude, _turnStartDelay)
		{
		}

		public MinimumNumberBuff(StatTypes.EStatType _statType, float _magnitude)
			: this(_statType, EBuffDurationType.Infinite, 0, 0, _magnitude)
		{
		}

		private MinimumNumberBuff(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, float _magnitude)
			: base(_buffType, _statType, _durationType, _turnDuration, _turnInterval, _magnitude)
		{
		}

		public override void Compute(IStatValueWrapper _source, float _payload)
		{
			_source.Number = Math.Min(_source.Number, Magnitude);
		}

		public override object Clone()
		{
			MinimumNumberBuff minimumNumberBuff = new MinimumNumberBuff(Type, StatType, DurationType, TurnDuration, TurnInterval, Magnitude);
			minimumNumberBuff.IsActivePredicate = IsActivePredicate;
			return minimumNumberBuff;
		}
	}
}
