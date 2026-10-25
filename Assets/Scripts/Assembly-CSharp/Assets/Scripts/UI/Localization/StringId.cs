using System;
using System.Globalization;
using Assets.Scripts.UI.Interfaces;

namespace Assets.Scripts.UI.Localization
{
	public class StringId : IComparable, IValidated
	{
		public static readonly StringId Invalid = new StringId(-1);

		private readonly long m_id = -1L;

		public bool Valid
		{
			get
			{
				return ValidStringId(Value);
			}
		}

		public long Value
		{
			get
			{
				return m_id;
			}
		}

		public StringId(long _id)
		{
			m_id = _id;
		}

		public StringId(int _id)
		{
			m_id = _id;
		}

		public StringId(string _id)
		{
			m_id = Convert.ToInt64(_id);
		}

		public override string ToString()
		{
			return m_id.ToString(CultureInfo.InvariantCulture);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		public static bool TryParse(string _value, out StringId _id)
		{
			long result = -1L;
			if (long.TryParse(_value, out result))
			{
				_id = new StringId(result);
				return true;
			}
			_id = new StringId(-1);
			return false;
		}

		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			StringId stringId = _obj as StringId;
			if (stringId != null)
			{
				return Value.CompareTo(stringId);
			}
			throw new ArgumentException("Right hand side must be, or be convertible to StringId");
		}

		public bool Equals(StringId _other)
		{
			if (object.ReferenceEquals(_other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, _other))
			{
				return true;
			}
			return Value.Equals(_other.Value);
		}

		public override bool Equals(object _obj)
		{
			return Equals(_obj as StringId);
		}

		public static bool ValidStringId(long _stringId)
		{
			return _stringId > 0;
		}

		public static implicit operator long(StringId _id)
		{
			return _id.m_id;
		}

		public static implicit operator StringId(int _id)
		{
			return new StringId(_id);
		}

		public static implicit operator StringId(long _id)
		{
			return new StringId(_id);
		}

		public static bool operator ==(StringId _lhs, StringId _rhs)
		{
			if (object.ReferenceEquals(_lhs, null))
			{
				if (object.ReferenceEquals(_rhs, null))
				{
					return true;
				}
				return false;
			}
			return _lhs.Equals(_rhs);
		}

		public static bool operator !=(StringId _lhs, StringId _rhs)
		{
			return !(_lhs == _rhs);
		}
	}
}
