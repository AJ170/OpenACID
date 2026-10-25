using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatEnumMapValueScalable_Float<TKey> : StatCollectionValue<StatValueWrapperScalable_Float>, IDataSerializable, IReadonlyStatCollectionValue, IStatCollectionValue, IScalableStatCollectionValue, IStatEnumMapValue, IReadonlyStatValueBase, IStatValueBase, IManagedStatValueBase, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable where TKey : struct
	{
		public Type KeyType
		{
			get
			{
				return typeof(TKey);
			}
		}

		public StatEnumMapValueScalable_Float(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
			: base(_statType, _statValueCollection)
		{
		}

		public override int Get_int(int _idx)
		{
			if (IsScaled)
			{
				StatValueWrapperScalable_Float statValueWrapperScalable_Float = m_Collection[_idx];
				return (int)((float)statValueWrapperScalable_Float.Int + StatValueCollection.IncrementFactor * statValueWrapperScalable_Float.LevelIncrement);
			}
			return base.Get_int(_idx);
		}

		public override float Get_float(int _idx)
		{
			if (IsScaled)
			{
				StatValueWrapperScalable_Float statValueWrapperScalable_Float = m_Collection[_idx];
				return statValueWrapperScalable_Float.Float + StatValueCollection.IncrementFactor * statValueWrapperScalable_Float.LevelIncrement;
			}
			return base.Get_float(_idx);
		}

		public override int[] GetValuesCopy_int()
		{
			int[] array = new int[m_Collection.Count];
			for (int i = 0; i < m_Collection.Count; i++)
			{
				array[i] = Get_int(i);
			}
			return array;
		}

		public override float[] GetValuesCopy_float()
		{
			float[] array = new float[m_Collection.Count];
			for (int i = 0; i < m_Collection.Count; i++)
			{
				array[i] = Get_float(i);
			}
			return array;
		}

		public void SetLevelIncrement(int _idx, float _levelIncrement)
		{
			((IScalableNumberStatValueWrapper)m_Collection[_idx]).LevelIncrement = _levelIncrement;
		}

		public float GetLevelIncrement(int _idx)
		{
			return ((IScalableNumberStatValueWrapper)m_Collection[_idx]).LevelIncrement;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				m_Collection.Clear();
				BBSimpleList<float> list = XmlHelpers.SerializeEnumMap<TKey, float>("Value", _s);
				m_Collection.AddRange(list.Select((float floatValue) => new StatValueWrapperScalable_Float
				{
					Float = floatValue
				}));
				list = XmlHelpers.SerializeEnumMap<TKey, float>("LevelIncrement", _s);
				for (int i = 0; i < m_Collection.Count; i++)
				{
					m_Collection[i].LevelIncrement = list[i];
				}
			}
			else
			{
				XmlHelpers.SerializeEnumMap<TKey, float>("Value", _s, m_Collection.Select((StatValueWrapperScalable_Float wrapper) => wrapper.Float));
				XmlHelpers.SerializeEnumMap<TKey, float>("LevelIncrement", _s, m_Collection.Select((StatValueWrapperScalable_Float wrapper) => wrapper.LevelIncrement));
			}
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatEnumMapValueScalable_Float<TKey> statEnumMapValueScalable_Float = new StatEnumMapValueScalable_Float<TKey>(StatType, _newStatValueCollection);
			statEnumMapValueScalable_Float.CopyFrom(this);
			return statEnumMapValueScalable_Float;
		}
	}
}
