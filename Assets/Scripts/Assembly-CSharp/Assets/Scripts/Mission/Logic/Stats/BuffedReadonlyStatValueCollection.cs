using System;
using System.Collections.Generic;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class BuffedReadonlyStatValueCollection
	{
		private class BuffedStatsCollectionHandle : IDisposable, IBuffedStatsCollectionHandle
		{
			private readonly Stack<ISuperBuffStats> m_SuperBuffStatsStack = new Stack<ISuperBuffStats>();

			private readonly BuffedReadonlyStatValueCollection m_Owner;

			public BuffedReadonlyStatValueCollection StatValueCollection
			{
				get
				{
					return m_Owner;
				}
			}

			public BuffedStatsCollectionHandle(BuffedReadonlyStatValueCollection _owner)
			{
				m_Owner = _owner;
			}

			public BuffedStatsCollectionHandle Apply(ISuperBuffStats _superBuffStats)
			{
				m_SuperBuffStatsStack.Push(m_Owner.m_SuperBuffStats);
				m_Owner.m_SuperBuffStats = _superBuffStats;
				return this;
			}

			public void Dispose()
			{
				m_Owner.m_SuperBuffStats = m_SuperBuffStatsStack.Pop();
			}

			public bool Get_bool(StatTypes.EStatType _statType)
			{
				return m_Owner.Get_bool(_statType);
			}

			public float Get_float(StatTypes.EStatType _statType)
			{
				return m_Owner.Get_float(_statType);
			}

			public int Get_int(StatTypes.EStatType _statType)
			{
				return m_Owner.Get_int(_statType);
			}

			public string Get_String(StatTypes.EStatType _statType)
			{
				return m_Owner.Get_String(_statType);
			}

			public bool HasStat(StatTypes.EStatType _statType)
			{
				return m_Owner.HasStat(_statType);
			}
		}

		private readonly IReadonlyStatValueCollection m_StatValueCollection;

		private readonly int m_DefinitionId;

		private readonly BuffedStatsCollectionHandle m_BuffedStatsCollectionHandle;

		private ISuperBuffStats m_SuperBuffStats;

		public BuffedReadonlyStatValueCollection(IReadonlyStatValueCollection _statValueCollection, int _definitionId)
		{
			m_StatValueCollection = _statValueCollection;
			m_DefinitionId = _definitionId;
			m_BuffedStatsCollectionHandle = new BuffedStatsCollectionHandle(this);
		}

		public IBuffedStatsCollectionHandle Apply(ISuperBuffStats _superBuffStats)
		{
			return m_BuffedStatsCollectionHandle.Apply(_superBuffStats);
		}

		public IBuffedStatsCollectionHandle Apply(ISuperBuffsStatsCarrier _superBuffsStatsCarrier)
		{
			ISuperBuffStats superBuffStats2;
			if (_superBuffsStatsCarrier == null)
			{
				ISuperBuffStats superBuffStats = null;
				superBuffStats2 = superBuffStats;
			}
			else
			{
				superBuffStats2 = _superBuffsStatsCarrier.SuperBuffStats;
			}
			return Apply(superBuffStats2);
		}

		public bool Get_bool(StatTypes.EStatType _statType, ISuperBuffStats _superBuffStats)
		{
			using (Apply(_superBuffStats))
			{
				return Get_bool(_statType);
			}
		}

		public bool Get_bool(StatTypes.EStatType _statType, ISuperBuffsStatsCarrier _superBuffsStatsCarrier)
		{
			ISuperBuffStats superBuffStats2;
			if (_superBuffsStatsCarrier == null)
			{
				ISuperBuffStats superBuffStats = null;
				superBuffStats2 = superBuffStats;
			}
			else
			{
				superBuffStats2 = _superBuffsStatsCarrier.SuperBuffStats;
			}
			return Get_bool(_statType, superBuffStats2);
		}

		public float Get_float(StatTypes.EStatType _statType, ISuperBuffStats _superBuffStats)
		{
			using (Apply(_superBuffStats))
			{
				return Get_float(_statType);
			}
		}

		public float Get_float(StatTypes.EStatType _statType, ISuperBuffsStatsCarrier _superBuffsStatsCarrier)
		{
			ISuperBuffStats superBuffStats2;
			if (_superBuffsStatsCarrier == null)
			{
				ISuperBuffStats superBuffStats = null;
				superBuffStats2 = superBuffStats;
			}
			else
			{
				superBuffStats2 = _superBuffsStatsCarrier.SuperBuffStats;
			}
			return Get_float(_statType, superBuffStats2);
		}

		public int Get_int(StatTypes.EStatType _statType, ISuperBuffStats _superBuffStats)
		{
			using (Apply(_superBuffStats))
			{
				return Get_int(_statType);
			}
		}

		public int Get_int(StatTypes.EStatType _statType, ISuperBuffsStatsCarrier _superBuffsStatsCarrier)
		{
			ISuperBuffStats superBuffStats2;
			if (_superBuffsStatsCarrier == null)
			{
				ISuperBuffStats superBuffStats = null;
				superBuffStats2 = superBuffStats;
			}
			else
			{
				superBuffStats2 = _superBuffsStatsCarrier.SuperBuffStats;
			}
			return Get_int(_statType, superBuffStats2);
		}

		public string Get_String(StatTypes.EStatType _statType, ISuperBuffStats _superBuffStats)
		{
			using (Apply(_superBuffStats))
			{
				return Get_String(_statType);
			}
		}

		public string Get_String(StatTypes.EStatType _statType, ISuperBuffsStatsCarrier _superBuffsStatsCarrier)
		{
			ISuperBuffStats superBuffStats2;
			if (_superBuffsStatsCarrier == null)
			{
				ISuperBuffStats superBuffStats = null;
				superBuffStats2 = superBuffStats;
			}
			else
			{
				superBuffStats2 = _superBuffsStatsCarrier.SuperBuffStats;
			}
			return Get_String(_statType, superBuffStats2);
		}

		public bool HasStat(StatTypes.EStatType _type)
		{
			return m_StatValueCollection.HasStat(_type);
		}

		private float Get_float(StatTypes.EStatType _src)
		{
			return (m_SuperBuffStats != null) ? m_SuperBuffStats.GetBuffed_float(_src, m_StatValueCollection, m_DefinitionId) : m_StatValueCollection.Get_float(_src);
		}

		private int Get_int(StatTypes.EStatType _src)
		{
			return (m_SuperBuffStats != null) ? m_SuperBuffStats.GetBuffed_int(_src, m_StatValueCollection, m_DefinitionId) : m_StatValueCollection.Get_int(_src);
		}

		private bool Get_bool(StatTypes.EStatType _src)
		{
			return (m_SuperBuffStats != null) ? m_SuperBuffStats.GetBuffed_bool(_src, m_StatValueCollection, m_DefinitionId) : m_StatValueCollection.Get_bool(_src);
		}

		private EntityId Get_EntityId(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_EntityId(_src);
		}

		private string Get_String(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_String(_src);
		}
	}
}
