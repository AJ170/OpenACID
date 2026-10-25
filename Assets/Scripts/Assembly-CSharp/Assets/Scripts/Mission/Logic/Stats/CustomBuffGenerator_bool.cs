using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CustomBuffGenerator_bool : CustomBuffGenerator<bool>
	{
		public CustomBuffGenerator_bool(BuffFactory _factory, BuffedReadonlyStatValueCollection _ruleStatValueCollection, StatTypes.EStatType _ruleStatBuffDuration, StatTypes.EStatType _ruleStatBuffInterval, StatTypes.EStatType _ruleStatBuffValue, bool _useSuperStats)
			: base(_ruleStatBuffValue)
		{
			CustomBuffGenerator_bool customBuffGenerator_bool = this;
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
					int duration = ((_ruleStatBuffDuration != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffDuration) : customBuffGenerator_bool.Duration);
					duration = customBuffGenerator_bool.GetProcessedDuration(entity, buffedStatsCollectionHandle, duration);
					int interval = ((_ruleStatBuffInterval != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_int(_ruleStatBuffInterval) : customBuffGenerator_bool.Interval);
					interval = customBuffGenerator_bool.GetProcessedInterval(entity, buffedStatsCollectionHandle, interval);
					bool value = ((_ruleStatBuffValue != StatTypes.EStatType.Invalid) ? buffedStatsCollectionHandle.Get_bool(_ruleStatBuffValue) : customBuffGenerator_bool.Value);
					value = customBuffGenerator_bool.GetProcessedValue(entity, buffedStatsCollectionHandle, value);
					BuffBase buffBase = _factory(entity, customBuffGenerator_bool.BuffType, customBuffGenerator_bool.StatType, customBuffGenerator_bool.DurationType, duration, interval, value, doInversion);
					buffBase.IsActivePredicate = isActivePredicate;
					return buffBase;
				}
			};
		}
	}
}
