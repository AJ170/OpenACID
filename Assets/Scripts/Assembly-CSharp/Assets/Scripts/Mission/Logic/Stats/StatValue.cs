using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValue<T> : StatValueBase<T>, IDataSerializable, IReadonlyStatValueBase, IStatValueWrapperContainer, IStatValueBase, IManagedStatValueBase, IReadonlyStatValue, IStatValue, IAcierDataSerializable, IAcierDataTemplateSerializable, IAcierDataDeSerializable where T : class, IStatValueWrapper, new()
	{
		protected readonly T m_ValueWrapper;

		protected static readonly T s_TmpNewValueWrapper = new T();

		protected static readonly T s_TmpOldValueWrapper = new T();

		public IStatValueWrapper ValueWrapper
		{
			get
			{
				return m_ValueWrapper;
			}
		}

		public virtual int Value_int
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Int;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.Int = value;
				SetValue();
			}
		}

		public virtual float Value_float
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Float;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.Float = value;
				SetValue();
			}
		}

		public virtual float Value_number
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Number;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.Number = value;
				SetValue();
			}
		}

		public bool Value_bool
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Bool;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.Bool = value;
				SetValue();
			}
		}

		public string Value_string
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.String;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.String = value;
				SetValue();
			}
		}

		public EntityId Value_EntityId
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.EntityId;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.EntityId = value;
				SetValue();
			}
		}

		public Vector3 Value_Vector3
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Vector3;
			}
			set
			{
				T val = s_TmpNewValueWrapper;
				val.Vector3 = value;
				SetValue();
			}
		}

		public override bool HasDefaultValue
		{
			get
			{
				T valueWrapper = m_ValueWrapper;
				return valueWrapper.Equals(StatValueBase<T>.s_DefaultValue);
			}
		}

		public StatValue(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
			: base(_statType, _statValueCollection)
		{
			T val = StatValueBase<T>.s_DefaultValue;
			m_ValueWrapper = (T)val.Clone();
			bool _changed = false;
			FixValue(m_ValueWrapper, out _changed);
		}

		public override string ToString()
		{
			if (ValueType == typeof(float))
			{
				return Value_float.ToString();
			}
			if (ValueType == typeof(int))
			{
				return Value_int.ToString();
			}
			T valueWrapper = m_ValueWrapper;
			return valueWrapper.ToString();
		}

		public override bool HasEqualValue(IReadonlyStatValueBase _other)
		{
			StatValue<T> statValue = _other as StatValue<T>;
			if (statValue == null)
			{
				return false;
			}
			T valueWrapper = m_ValueWrapper;
			return valueWrapper.Equals(statValue.m_ValueWrapper);
		}

		public float GetPayload()
		{
			INumberStatValueWrapper numberStatValueWrapper = ValueWrapper as INumberStatValueWrapper;
			if (numberStatValueWrapper != null)
			{
				return numberStatValueWrapper.Number;
			}
			return 0f;
		}

		public bool HasEqualValue(IStatValueWrapper _statValueWrapper)
		{
			T valueWrapper = m_ValueWrapper;
			return valueWrapper.Equals(_statValueWrapper);
		}

		public override void CopyFrom(IReadonlyStatValueBase _other)
		{
			IScalableStat scalableStat = _other as IScalableStat;
			if (scalableStat != null && _other.IsScaled)
			{
				T val = s_TmpNewValueWrapper;
				val.Number = ((IReadonlyStatValue)scalableStat).Value_number;
			}
			else
			{
				IStatValueWrapperContainer statValueWrapperContainer = (IStatValueWrapperContainer)_other;
				T val2 = s_TmpNewValueWrapper;
				val2.CopyFrom(statValueWrapperContainer.ValueWrapper);
			}
			SetValue();
		}

		public void CopyTo(IStatValueWrapper _wrapper)
		{
			_wrapper.CopyFrom(m_ValueWrapper);
		}

		public void CopyFrom(IStatValueWrapper _wrapper)
		{
			T valueWrapper = m_ValueWrapper;
			valueWrapper.CopyFrom(_wrapper);
		}

		public void Reset()
		{
			T valueWrapper = m_ValueWrapper;
			valueWrapper.CopyFrom(StatValueBase<T>.s_DefaultValue);
		}

		public override void ApplyBuff(IBuff _buff, IEntity _entity, float _payload)
		{
			if (IsWritingAllowed)
			{
				_buff.Apply(_entity, m_ValueWrapper, _payload);
			}
		}

		public override IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection)
		{
			StatValue<T> statValue = new StatValue<T>(StatType, _newStatValueCollection);
			statValue.CopyFrom(this);
			return statValue;
		}

		public virtual void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			_s.SerializeBeginNode("Value");
			T valueWrapper = m_ValueWrapper;
			valueWrapper.Serialize(_name, _s, _customData);
			_s.SerializeEndNode();
		}

		protected virtual void FixValue(T _newValueWrapper, out bool _changed)
		{
			_changed = false;
		}

		protected void SetValue()
		{
			if (!IsWritingAllowed)
			{
				return;
			}
			T val = s_TmpOldValueWrapper;
			val.CopyFrom(m_ValueWrapper);
			T val2 = s_TmpNewValueWrapper;
			if (val2.Equals(s_TmpOldValueWrapper))
			{
				return;
			}
			bool _changed = false;
			StatValueCollection statValueCollection = StatValueCollection;
			if (statValueCollection != null && statValueCollection.AccessMode != 0)
			{
				FixValue(s_TmpNewValueWrapper, out _changed);
			}
			T valueWrapper = m_ValueWrapper;
			valueWrapper.CopyFrom(s_TmpNewValueWrapper);
			if (statValueCollection == null)
			{
				return;
			}
			if (_changed)
			{
				T valueWrapper2 = m_ValueWrapper;
				if (valueWrapper2.Equals(s_TmpOldValueWrapper))
				{
					return;
				}
			}
			statValueCollection.OnValueChanged(this, s_TmpOldValueWrapper);
		}
	}
}
