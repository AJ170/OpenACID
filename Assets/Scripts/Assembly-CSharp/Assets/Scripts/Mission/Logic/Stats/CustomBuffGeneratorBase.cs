using System;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class CustomBuffGeneratorBase : IBuffGenerator
	{
		public delegate int TurnCountProcessorDelegate(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _turnCount);

		public delegate object ValueProcessorDelegate(IEntity _creator, IBuffedStatsCollectionHandle _stats, object _value);

		public Func<IEntity, IBuff, bool> IsActivePredicate { get; set; }

		public int Duration { get; set; }

		public int Interval { get; set; }

		public TurnCountProcessorDelegate DurationProcessor { get; set; }

		public TurnCountProcessorDelegate IntervalProcessor { get; set; }

		public ValueProcessorDelegate ValueProcessor { get; set; }

		public EBuffDurationType DurationType { get; set; }

		public EBuffType BuffType { get; protected set; }

		public StatTypes.EStatType StatType { get; protected set; }

		public StatTypes.EStatType ValueRuleStat { get; private set; }

		protected CustomBuffGeneratorBase(StatTypes.EStatType _valueRuleStat)
		{
			BuffType = EBuffType.Invalid;
			StatType = StatTypes.EStatType.Invalid;
			ValueRuleStat = _valueRuleStat;
		}

		public abstract void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity, bool _doInversion);

		public void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity)
		{
			MakeBuffs(_result, _creator, _targetEntity, true);
		}

		protected int GetProcessedDuration(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _duration)
		{
			return (DurationProcessor != null) ? DurationProcessor(_creator, _stats, _duration) : _duration;
		}

		protected int GetProcessedInterval(IEntity _creator, IBuffedStatsCollectionHandle _stats, int _interval)
		{
			return (IntervalProcessor != null) ? IntervalProcessor(_creator, _stats, _interval) : _interval;
		}
	}
}
