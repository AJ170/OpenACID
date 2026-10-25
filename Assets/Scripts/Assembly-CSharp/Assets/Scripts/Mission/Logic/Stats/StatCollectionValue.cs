using System.Text;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatCollectionValue<T> : StatValueBase<T>, IDataSerializable, IReadonlyStatCollectionValue, IStatCollectionValue, IReadonlyStatValueBase, IStatValueBase, IManagedStatValueBase, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable where T : class, IStatValueWrapper, new()
	{
		protected BBSimpleList<T> m_Collection = new BBSimpleList<T>();

		public override bool HasDefaultValue
		{
			get
			{
				return Count == 0;
			}
		}

		public int Count
		{
			get
			{
				return m_Collection.Count;
			}
		}

		public StatCollectionValue(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
			: base(_statType, _statValueCollection)
		{
		}

		public override void ApplyBuff(IBuff _buff, IEntity _entity, float _payload)
		{
			if (IsWritingAllowed)
			{
				for (int i = 0; i < m_Collection.Count; i++)
				{
					T wrapper = m_Collection[i];
					_buff.Apply(_entity, wrapper, _payload);
				}
			}
		}

		public override bool HasEqualValue(IReadonlyStatValueBase _other)
		{
			StatCollectionValue<T> statCollectionValue = _other as StatCollectionValue<T>;
			if (statCollectionValue == null)
			{
				return false;
			}
			return m_Collection.SequenceEqual(statCollectionValue.m_Collection);
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatCollectionValue<T> statCollectionValue = new StatCollectionValue<T>(StatType, _newStatValueCollection);
			for (int i = 0; i < m_Collection.Count; i++)
			{
				T val = m_Collection[i];
				statCollectionValue.m_Collection.Add((T)val.Clone());
			}
			return statCollectionValue;
		}

		public bool Contains_int(int _value)
		{
			return m_Collection.Any((T el) => el.Int == _value);
		}

		public bool Contains_float(float _value)
		{
			return m_Collection.Any((T el) => el.Float == _value);
		}

		public bool Contains_bool(bool _value)
		{
			return m_Collection.Any((T el) => el.Bool == _value);
		}

		public bool Contains_string(string _value)
		{
			return m_Collection.Any((T el) => el.String == _value);
		}

		public void Clear()
		{
			if (!IsWritingAllowed)
			{
			}
			m_Collection.Clear();
		}

		public IStatCollectionValue Add_int(int _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			m_Collection.Add(new T
			{
				Int = _value
			});
			return this;
		}

		public IStatCollectionValue Add_float(float _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			m_Collection.Add(new T
			{
				Float = _value
			});
			return this;
		}

		public IStatCollectionValue Add_bool(bool _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			m_Collection.Add(new T
			{
				Bool = _value
			});
			return this;
		}

		public IStatCollectionValue Add_string(string _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			m_Collection.Add(new T
			{
				String = _value
			});
			return this;
		}

		public IStatCollectionValue Remove_int(int _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			int num = m_Collection.FindIndex((T el) => el.Int == _value);
			if (num >= 0)
			{
				m_Collection.RemoveAt(num);
			}
			return this;
		}

		public IStatCollectionValue Remove_float(float _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			int num = m_Collection.FindIndex((T el) => el.Float == _value);
			if (num >= 0)
			{
				m_Collection.RemoveAt(num);
			}
			return this;
		}

		public IStatCollectionValue Remove_bool(bool _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			int num = m_Collection.FindIndex((T el) => el.Bool == _value);
			if (num >= 0)
			{
				m_Collection.RemoveAt(num);
			}
			return this;
		}

		public IStatCollectionValue Remove_string(string _value)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			int num = m_Collection.FindIndex((T el) => el.String == _value);
			if (num >= 0)
			{
				m_Collection.RemoveAt(num);
			}
			return this;
		}

		public virtual int Get_int(int _idx)
		{
			T val = m_Collection[_idx];
			return val.Int;
		}

		public virtual float Get_float(int _idx)
		{
			T val = m_Collection[_idx];
			return val.Float;
		}

		public bool Get_bool(int _idx)
		{
			T val = m_Collection[_idx];
			return val.Bool;
		}

		public string Get_string(int _idx)
		{
			T val = m_Collection[_idx];
			return val.String;
		}

		public IStatCollectionValue Set_int(int _value, int _idx)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			T val = m_Collection[_idx];
			val.Int = _value;
			return this;
		}

		public IStatCollectionValue Set_float(float _value, int _idx)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			T val = m_Collection[_idx];
			val.Float = _value;
			return this;
		}

		public IStatCollectionValue Set_bool(bool _value, int _idx)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			T val = m_Collection[_idx];
			val.Bool = _value;
			return this;
		}

		public IStatCollectionValue Set_string(string _value, int _idx)
		{
			if (!IsWritingAllowed)
			{
				return this;
			}
			T val = m_Collection[_idx];
			val.String = _value;
			return this;
		}

		public void Reset()
		{
			if (IsWritingAllowed)
			{
				m_Collection.Clear();
			}
		}

		public virtual int[] GetValuesCopy_int()
		{
			return m_Collection.Select((T el) => el.Int).ToArray();
		}

		public virtual float[] GetValuesCopy_float()
		{
			return m_Collection.Select((T el) => el.Float).ToArray();
		}

		public bool[] GetValuesCopy_bool()
		{
			return m_Collection.Select((T el) => el.Bool).ToArray();
		}

		public string[] GetValuesCopy_string()
		{
			return m_Collection.Select((T el) => el.String).ToArray();
		}

		public virtual void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			_s.SerializeBeginNode("CollectionValues");
			int num = _s.SerializeItemCount(m_Collection.Count);
			if (_s.Mode == EMode.EMode_Read)
			{
				m_Collection.Clear();
				for (int i = 0; i < num; i++)
				{
					_s.SerializeBeginNode("Value");
					T val = StatValueBase<T>.s_DefaultValue;
					T value = (T)val.Clone();
					m_Collection.Add(value);
					value.Serialize(_name, _s, _customData);
					_s.SerializeEndNode();
				}
			}
			else
			{
				for (int j = 0; j < num; j++)
				{
					_s.SerializeBeginNode("Value");
					T val2 = m_Collection[j];
					val2.Serialize(_name, _s, _customData);
					_s.SerializeEndNode();
				}
			}
			_s.SerializeEndNode();
		}

		public override void CopyFrom(IReadonlyStatValueBase _other)
		{
			if (!IsWritingAllowed)
			{
			}
			Clear();
			IReadonlyStatCollectionValue readonlyStatCollectionValue = (IReadonlyStatCollectionValue)_other;
			for (int i = 0; i < readonlyStatCollectionValue.Count; i++)
			{
				T val = StatValueBase<T>.s_DefaultValue;
				T val2 = (T)val.Clone();
				m_Collection.Add(val2);
				readonlyStatCollectionValue.CopyTo(val2, i);
			}
		}

		public void CopyTo(IStatValueWrapper _wrapper, int _idx)
		{
			_wrapper.CopyFrom(m_Collection[_idx]);
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{ ");
			for (int i = 0; i < m_Collection.Count; i++)
			{
				T val = m_Collection[i];
				stringBuilder.Append(val.ToString());
				stringBuilder.Append("; ");
			}
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}
	}
}
