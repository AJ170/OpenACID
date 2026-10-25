using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CustomBuffGenerator_float : CustomBuffGenerator<float>
	{
		public CustomBuffGenerator_float(BuffFactory _factory, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats)
			: base(_ruleStatBuffValue)
		{
			CustomBuffGenerator_float customBuffGenerator_float = this;
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
					int duration = ((_ruleStatBuffDuration != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffDuration) : customBuffGenerator_float.Duration);
					duration = customBuffGenerator_float.GetProcessedDuration(entity, buffedStatsCollectionHandle, duration);
					int interval = ((_ruleStatBuffInterval != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffInterval) : customBuffGenerator_float.Interval);
					interval = customBuffGenerator_float.GetProcessedInterval(entity, buffedStatsCollectionHandle, interval);
					float value = ((_ruleStatBuffValue != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_float(_ruleStatBuffValue) : customBuffGenerator_float.Value);
					value = customBuffGenerator_float.GetProcessedValue(entity, buffedStatsCollectionHandle, value);
					BuffBase buffBase = _factory(entity, customBuffGenerator_float.BuffType, customBuffGenerator_float.StatType, customBuffGenerator_float.DurationType, duration, interval, value, doInversion);
					buffBase.IsActivePredicate = isActivePredicate;
					return buffBase;
				}
			};
		}
	}
}
