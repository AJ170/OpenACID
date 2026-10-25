using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class SuperBuffs
	{
		private readonly BBDictionary<int, BuffsCollection> m_BuffsCollections = new BBDictionary<int, BuffsCollection>();

		private static readonly StatValueWrapper_Bool s_StatValueWrapperBool = new StatValueWrapper_Bool();

		private static readonly StatValueWrapper_Int s_StatValueWrapperInt = new StatValueWrapper_Int();

		private static readonly StatValueWrapper_Float s_StatValueWrapperFloat = new StatValueWrapper_Float();

		public int GetBuffed_int(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			int @int = _statValueCollection.Get_int(_statType);
			s_StatValueWrapperInt.Int = @int;
			GetValue(s_StatValueWrapperInt, _statType, _definitionId);
			return s_StatValueWrapperInt.Int;
		}

		public float GetBuffed_float(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			float @float = _statValueCollection.Get_float(_statType);
			s_StatValueWrapperFloat.Float = @float;
			GetValue(s_StatValueWrapperFloat, _statType, _definitionId);
			return s_StatValueWrapperFloat.Float;
		}

		public bool GetBuffed_bool(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			bool @bool = _statValueCollection.Get_bool(_statType);
			s_StatValueWrapperBool.Bool = @bool;
			GetValue(s_StatValueWrapperBool, _statType, _definitionId);
			return s_StatValueWrapperBool.Bool;
		}

		public void AddBuff(IBuff _buff, int _definitionId = -1)
		{
			BuffsCollection value;
			if (!m_BuffsCollections.TryGetValue(_definitionId, out value))
			{
				value = (m_BuffsCollections[_definitionId] = new CachedBuffsCollection(null));
			}
			value.AddBuff(_buff);
		}

		public bool RemoveBuff(IBuff _buff, int _definitionId = -1)
		{
			BuffsCollection value;
			if (!m_BuffsCollections.TryGetValue(_definitionId, out value))
			{
				return false;
			}
			return value.RemoveBuff(_buff);
		}

		public void DoTurnUpdate(int _deltaTurns)
		{
			foreach (BuffsCollection value in m_BuffsCollections.Values)
			{
				value.DoTurnUpdate(_deltaTurns);
			}
		}

		private void GetValue(IStatValueWrapper _srcValue, StatTypes.EStatType _statType, int _definitionId)
		{
			BuffsCollection value;
			if (m_BuffsCollections.TryGetValue(-1, out value))
			{
				value.ComputeBuffs(_srcValue, _statType);
			}
			if (_definitionId >= 0 && m_BuffsCollections.TryGetValue(_definitionId, out value))
			{
				value.ComputeBuffs(_srcValue, _statType);
			}
		}
	}
}
