using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class CustomBuffGenerator<TSource> : CustomBuffGeneratorBase where TSource : struct
	{
		public delegate BuffBase BuffFactory(IEntity _creator, EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _duration, int _interval, TSource _value, bool _doInversion);

		protected Func<IEntity, Func<IEntity, IBuff, bool>, bool, IBuff> m_Generator;

		public TSource Value { get; set; }

		protected CustomBuffGenerator(StatTypes.EStatType _valueRuleStat)
			: base(_valueRuleStat)
		{
		}

		public override void MakeBuffs(BBList<IBuff> _result, IEntity _creator, IEntity _targetEntity, bool _doInversion)
		{
			this.ForceList(ref _result);
			_result.ClearIfNotEmpty();
			_result.Add(m_Generator(_creator, IsActivePredicate, _doInversion));
		}

		protected TSource GetProcessedValue(IEntity _creator, IBuffedStatsCollectionHandle _stats, TSource _value)
		{
			return (base.ValueProcessor != null) ? ((TSource)base.ValueProcessor(_creator, _stats, _value)) : _value;
		}
	}
}
