using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CustomBuffGenerator_int : CustomBuffGenerator<int>
	{
		public CustomBuffGenerator_int(BuffFactory _factory, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats)
			: base(_ruleStatBuffValue)
		{
			CustomBuffGenerator_int customBuffGenerator_int = this;
			m_Generator = delegate(IEntity entity, Func<IEntity, IBuff, bool> isActivePredicate, bool doInversion)
			{
				BuffedReadonlyStatValueCollection buffedReadonlyStatValueCollection = _ruleStatValueCollection;
				object superBuffsStatsCarrier2;
				if (_useSuperStats)
				{
					ISuperBuffsStatsCarrier superBuffsStatsCarrier = entity as ISuperBuffsStatsCarrier;
					superBuffsStatsCarrier2 = superBuffsStatsCarrier;
				}
				else
				{
					superBuffsStatsCarrier2 = null;
				}
				using (IBuffedStatsCollectionHandle buffedStatsCollectionHandle = buffedReadonlyStatValueCollection.Apply((ISuperBuffsStatsCarrier)superBuffsStatsCarrier2))
				{
					int duration = ((_ruleStatBuffDuration != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffDuration) : customBuffGenerator_int.Duration);
					duration = customBuffGenerator_int.GetProcessedDuration(entity, buffedStatsCollectionHandle, duration);
					int interval = ((_ruleStatBuffInterval != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffInterval) : customBuffGenerator_int.Interval);
					interval = customBuffGenerator_int.GetProcessedInterval(entity, buffedStatsCollectionHandle, interval);
					int value = ((_ruleStatBuffValue != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffValue) : customBuffGenerator_int.Value);
					value = customBuffGenerator_int.GetProcessedValue(entity, buffedStatsCollectionHandle, value);
					BuffBase buffBase = _factory(entity, customBuffGenerator_int.BuffType, customBuffGenerator_int.StatType, customBuffGenerator_int.DurationType, duration, interval, value, doInversion);
					buffBase.IsActivePredicate = isActivePredicate;
					return buffBase;
				}
			};
		}
	}
}
