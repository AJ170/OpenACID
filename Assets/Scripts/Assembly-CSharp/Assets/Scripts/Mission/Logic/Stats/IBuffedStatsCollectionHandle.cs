using System;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IBuffedStatsCollectionHandle : IDisposable
	{
		BuffedReadonlyStatValueCollection StatValueCollection { get; }

		bool Get_bool(StatTypes.EStatType _statType);

		float Get_float(StatTypes.EStatType _statType);

		int Get_int(StatTypes.EStatType _statType);

		string Get_String(StatTypes.EStatType _statType);

		bool HasStat(StatTypes.EStatType _statType);
	}
}
