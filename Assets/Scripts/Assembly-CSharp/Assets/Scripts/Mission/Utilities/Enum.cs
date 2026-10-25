using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Utilities
{
	public struct Enum<T> : IEnum where T : IEnumDefinition, new()
	{
		private class EnumInfos : IEnumInfos
		{
			public ReadOnlyCollection<string> Values
			{
				get
				{
					return Capsule.Names;
				}
			}

			public int Count
			{
				get
				{
					return Capsule.Names.Count;
				}
			}
		}

		private static class Capsule
		{
			public static readonly ReadOnlyCollection<string> Names;

			public static readonly BBList<string> m_Names;

			public static readonly BBDictionary<string, int> m_Indices;

			public static readonly T m_EnumDefinition;

			public static readonly EnumInfos EnumInfos;

			static Capsule()
			{
				m_Names = new BBList<string>();
				m_Indices = new BBDictionary<string, int>();
				m_EnumDefinition = new T();
				EnumInfos = new EnumInfos();
				Names = new ReadOnlyCollection<string>(m_Names);
			}
		}

		public const string InvalidName = "Invalid";

		public static readonly Enum<T> Invalid = default(Enum<T>);

		private readonly int m_Index;

		public bool IsValid
		{
			get
			{
				return m_Index > 0;
			}
		}

		public int Index
		{
			get
			{
				return m_Index - 1;
			}
		}

		public string Name
		{
			get
			{
				return ToString();
			}
		}

		public IEnumInfos Type
		{
			get
			{
				return Capsule.EnumInfos;
			}
		}

		public static ReadOnlyCollection<string> Names
		{
			get
			{
				return Capsule.Names;
			}
		}

		public static int Count
		{
			get
			{
				return Capsule.m_Indices.Count;
			}
		}

		public Enum(int _index)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(_index < Count))
			{
				m_Index = _index + 1;
			}
			else
			{
				m_Index = 0;
			}
		}

		public Enum(string _name)
		{
			m_Index = 0;
			string _result = _name;
			for (int i = 1; Capsule.m_EnumDefinition.GetLookupName(_result, i, ref _result); i++)
			{
				if (Capsule.m_Indices.TryGetValue(_result, out m_Index))
				{
					break;
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj is Enum<T>)
			{
				return (Enum<T>)obj == this;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_Index.GetHashCode();
		}

		public override string ToString()
		{
			return (string)this;
		}

		public IEnum Make(int _index)
		{
			return new Enum<T>(_index);
		}

		public IEnum Make(string _name)
		{
			return new Enum<T>(_name);
		}

		public static Enum<T> GetOrAdd(string _name)
		{
			int value;
			if (!Capsule.m_Indices.TryGetValue(_name, out value))
			{
				value = Capsule.m_Names.Count + 1;
				Capsule.m_Names.Add(_name);
				Capsule.m_Indices[_name] = value;
			}
			return new Enum<T>(value - 1);
		}

		public static bool operator ==(Enum<T> _left, Enum<T> _right)
		{
			return (int)_left == (int)_right;
		}

		public static bool operator !=(Enum<T> _left, Enum<T> _right)
		{
			return !(_left == _right);
		}

		public static explicit operator int(Enum<T> _enum)
		{
			return _enum.Index;
		}

		public static explicit operator Enum<T>(int _index)
		{
			return new Enum<T>(_index);
		}

		public static explicit operator string(Enum<T> _enum)
		{
			if (_enum.IsValid)
			{
				return Capsule.m_Names[(int)_enum];
			}
			return "Invalid";
		}

		public static explicit operator Enum<T>(string _name)
		{
			return new Enum<T>(_name);
		}
	}
}
