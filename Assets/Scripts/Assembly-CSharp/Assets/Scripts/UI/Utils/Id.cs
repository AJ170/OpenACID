using System;
using System.Globalization;
using Assets.Scripts.UI.Interfaces;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.UI.Utils
{
	public struct Id<T> : IValidated, IEquatable<Id<T>>
	{
		public static readonly Id<T> Invalid = new Id<T>(-1L);

		private long m_id;

		public bool Valid
		{
			get
			{
				return m_id != (long)Invalid;
			}
		}

		public Id(long _id)
		{
			m_id = _id;
		}

		public override string ToString()
		{
			return m_id.ToString(CultureInfo.InvariantCulture);
		}

		public override bool Equals(object _obj)
		{
			if (object.ReferenceEquals(null, _obj))
			{
				return false;
			}
			return _obj is Id<T> && Equals((Id<T>)_obj);
		}

		public bool Equals(Id<T> _other)
		{
			return m_id.Equals(_other.m_id);
		}

		public override int GetHashCode()
		{
			return m_id.GetHashCode();
		}

		public void Serialize(DataSerializerBase _ser)
		{
			m_id = _ser.SerializeInt64("Id", m_id);
		}

		public static implicit operator long(Id<T> _id)
		{
			return _id.m_id;
		}

		public static explicit operator Id<T>(long _value)
		{
			return new Id<T>(_value);
		}

		public static bool operator ==(Id<T> _lhs, Id<T> _rhs)
		{
			return _lhs.m_id == _rhs.m_id;
		}

		public static bool operator !=(Id<T> _lhs, Id<T> _rhs)
		{
			return !(_lhs == _rhs);
		}

		public static Id<T>operator +(Id<T> _id, int _value)
		{
			return new Id<T>(_id.m_id + _value);
		}
	}
}
