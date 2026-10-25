using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IReadonlyStatValueCollection
	{
		IReadonlyStatValueBase GetReadonlyStatValueBase(StatTypes.EStatType _statType);

		IReadonlyStatValue GetReadonlyStatValue(StatTypes.EStatType _statType);

		IReadonlyStatCollectionValue GetReadonlyStatCollectionValue(StatTypes.EStatType _statType);

		float Get_float(StatTypes.EStatType _src);

		int Get_int(StatTypes.EStatType _src);

		EntityId Get_EntityId(StatTypes.EStatType _src);

		bool Get_bool(StatTypes.EStatType _src);

		Vector3 Get_Vector3(StatTypes.EStatType _src);

		string Get_String(StatTypes.EStatType _src);

		bool HasStat(StatTypes.EStatType _type);
	}
}
