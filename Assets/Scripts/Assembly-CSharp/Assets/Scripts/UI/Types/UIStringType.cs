using System;
using Assets.Scripts.UI.Interfaces;

namespace Assets.Scripts.UI.Types
{
	public abstract class UIStringType : IComparable, IValidated
	{
		public string Value { get; private set; }

		public virtual bool Valid
		{
			get
			{
				return !string.IsNullOrEmpty(Value);
			}
		}

		protected UIStringType(string _value)
		{
			Value = _value;
		}

		protected UIStringType()
		{
			Value = string.Empty;
		}

		public override string ToString()
		{
			return Value ?? "NULL";
		}

		public int CompareTo(object _obj)
		{
			if (_obj == null)
			{
				return 1;
			}
			UIStringType uIStringType = _obj as UIStringType;
			if (uIStringType != null)
			{
				return string.Compare(Value, uIStringType.Value, StringComparison.Ordinal);
			}
			throw new ArgumentException("Right hand side must be UIStringType");
		}

		public bool Equals(UIStringType _other)
		{
			if (object.ReferenceEquals(_other, null))
			{
				return false;
			}
			if (object.ReferenceEquals(this, _other))
			{
				return true;
			}
			return string.Equals(Value, _other.Value);
		}

		public override bool Equals(object _obj)
		{
			return Equals(_obj as UIStringType);
		}

		public override int GetHashCode()
		{
			return (Value != null) ? Value.GetHashCode() : 0;
		}

		public static implicit operator string(UIStringType _type)
		{
			return (!(_type != null)) ? string.Empty : _type.ToString();
		}

		public static bool operator ==(UIStringType _lhs, UIStringType _rhs)
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

		public static bool operator !=(UIStringType _lhs, UIStringType _rhs)
		{
			return !(_lhs == _rhs);
		}
	}
}
