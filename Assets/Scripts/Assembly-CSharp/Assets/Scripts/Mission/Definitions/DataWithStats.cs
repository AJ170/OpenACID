using AssetBundleData;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Mission.Definitions
{
	public static class DataWithStats
	{
		public interface IDefinitionWithStats : Data.IDefinition, IReadonlyStatValueCollection
		{
			IReadonlyStatValueCollection StatValueCollection { get; }
		}

		public abstract class DefinitionWithStats : Data.Definition, Data.IDefinition, IDefinitionWithStats, IReadonlyStatValueCollection, IStatValueCollection
		{
			protected readonly StatValueCollection m_StatValueCollection;

			public IReadonlyStatValueCollection StatValueCollection
			{
				get
				{
					return m_StatValueCollection;
				}
			}

			private DefinitionWithStats DefinitionWithStatsTemplate
			{
				get
				{
					return (DefinitionWithStats)base.TemplateDefinition;
				}
			}

			protected DefinitionWithStats(IStatsHelper[] _statsHelpers, AssetBundleInfo _assetBundleInfo, bool _addToDefinitionManager)
				: base(_assetBundleInfo, _addToDefinitionManager)
			{
				IReadonlyStatValueCollection template;
				if (DefinitionWithStatsTemplate == null)
				{
					IReadonlyStatValueCollection readonlyStatValueCollection = null;
					template = readonlyStatValueCollection;
				}
				else
				{
					template = DefinitionWithStatsTemplate.StatValueCollection;
				}
				m_StatValueCollection = new StatValueCollection(_statsHelpers, this, template);
			}

			public override void Serialize(DataSerializerBase _s)
			{
				base.Serialize(_s);
				m_StatValueCollection.Serialize(_s, (DefinitionWithStatsTemplate != null) ? DefinitionWithStatsTemplate.m_StatValueCollection : null);
			}

			public void Copy(StatTypes.EStatType _src, StatTypes.EStatType _dst)
			{
				m_StatValueCollection.Copy(_src, _dst);
			}

			public void Set_float(StatTypes.EStatType _dst, float _value)
			{
				m_StatValueCollection.Set_float(_dst, _value);
			}

			public void Set_int(StatTypes.EStatType _dst, int _value)
			{
				m_StatValueCollection.Set_int(_dst, _value);
			}

			public void Set_EntityId(StatTypes.EStatType _dst, EntityId _value)
			{
				m_StatValueCollection.Set_EntityId(_dst, _value);
			}

			public void Set_bool(StatTypes.EStatType _dst, bool _value)
			{
				m_StatValueCollection.Set_bool(_dst, _value);
			}

			public void Set_string(StatTypes.EStatType _dst, string _value)
			{
				m_StatValueCollection.Set_string(_dst, _value);
			}

			public void Set_Vector3(StatTypes.EStatType _dst, Vector3 _value)
			{
				m_StatValueCollection.Set_Vector3(_dst, _value);
			}

			public float Get_float(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_float(_src);
			}

			public int Get_int(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_int(_src);
			}

			public EntityId Get_EntityId(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_EntityId(_src);
			}

			public bool Get_bool(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_bool(_src);
			}

			public Vector3 Get_Vector3(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_Vector3(_src);
			}

			public string Get_String(StatTypes.EStatType _src)
			{
				return m_StatValueCollection.Get_String(_src);
			}

			public float AddFloat(StatTypes.EStatType _dst, StatTypes.EStatType _src)
			{
				return m_StatValueCollection.AddFloat(_dst, _src);
			}

			public float AddFloat(StatTypes.EStatType _dst, float _value)
			{
				return m_StatValueCollection.AddFloat(_dst, _value);
			}

			public int AddInt(StatTypes.EStatType _dst, StatTypes.EStatType _src)
			{
				return m_StatValueCollection.AddInt(_dst, _src);
			}

			public int AddInt(StatTypes.EStatType _dst, int _value)
			{
				return m_StatValueCollection.AddInt(_dst, _value);
			}

			public IReadonlyStatValueBase GetReadonlyStatValueBase(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetReadonlyStatValueBase(_statType);
			}

			public IReadonlyStatValue GetReadonlyStatValue(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetReadonlyStatValue(_statType);
			}

			public IReadonlyStatCollectionValue GetReadonlyStatCollectionValue(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetReadonlyStatCollectionValue(_statType);
			}

			public IStatValueBase GetStatValueBase(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetStatValueBase(_statType);
			}

			public IStatValue GetStatValue(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetStatValue(_statType);
			}

			public IStatCollectionValue GetStatCollectionValue(StatTypes.EStatType _statType)
			{
				return m_StatValueCollection.GetStatCollectionValue(_statType);
			}

			public bool HasStat(StatTypes.EStatType _type)
			{
				return m_StatValueCollection.HasStat(_type);
			}

			public bool IsWritingAllowed(StatTypes.EStatType _type)
			{
				return m_StatValueCollection.IsWritingAllowed(_type);
			}

			protected void InitializeMutables()
			{
				m_StatValueCollection.InitializeMutable(this, null);
			}
		}
	}
}
