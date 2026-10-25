using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface ISuperBuffStats : IInstanceCarrier, IInstanceStatValueCollection, IEntityStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		int GetBuffed_int(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1);

		float GetBuffed_float(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1);

		bool GetBuffed_bool(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1);

		EHidingSpotFlags GetBuffed_EHidingSpotFlags(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1);

		void AddSuperBuff(IBuff _buff, int _definitionId = -1);

		bool RemoveSuperBuff(IBuff _buff, int _definitionId = -1);
	}
}
