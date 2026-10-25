using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public interface IStatValueCollection : IReadonlyStatValueCollection
	{
		IStatValueBase GetStatValueBase(StatTypes.EStatType _statType);

		IStatValue GetStatValue(StatTypes.EStatType _statType);

		IStatCollectionValue GetStatCollectionValue(StatTypes.EStatType _statType);

		void Copy(StatTypes.EStatType _src, StatTypes.EStatType _dst);

		void Set_float(StatTypes.EStatType _dst, float _value);

		void Set_int(StatTypes.EStatType _dst, int _value);

		void Set_EntityId(StatTypes.EStatType _dst, EntityId _value);

		void Set_bool(StatTypes.EStatType _dst, bool _value);

		void Set_string(StatTypes.EStatType _dst, string _string);

		void Set_Vector3(StatTypes.EStatType _dst, Vector3 _value);

		float AddFloat(StatTypes.EStatType _dst, StatTypes.EStatType _src);

		float AddFloat(StatTypes.EStatType _dst, float _value);

		int AddInt(StatTypes.EStatType _dst, StatTypes.EStatType _src);

		int AddInt(StatTypes.EStatType _dst, int _value);

		bool IsWritingAllowed(StatTypes.EStatType _type);
	}
}
