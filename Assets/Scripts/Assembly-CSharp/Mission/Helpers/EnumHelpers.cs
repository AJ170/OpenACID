using System;
using System.Collections.Generic;

namespace Mission.Helpers
{
	public static class EnumHelpers
	{
		public static T IndexToValue<T>(int _index) where T : struct, IConvertible, IComparable
		{
			Array values = Enum.GetValues(typeof(T));
			return (T)values.GetValue(_index);
		}

		public static int ValueToIndex<T>(T _value) where T : struct, IConvertible, IComparable
		{
			Array values = Enum.GetValues(typeof(T));
			for (int i = 0; i < values.Length; i++)
			{
				if (((T)values.GetValue(i)).CompareTo(_value) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		public static TEnum Parse<TEnum>(string _value) where TEnum : struct
		{
			return (TEnum)Enum.Parse(typeof(TEnum), _value);
		}

		public static bool TryParse<T>(string _enumString, out T _result)
		{
			if (Enum.IsDefined(typeof(T), _enumString))
			{
				_result = (T)Enum.Parse(typeof(T), _enumString, true);
				return true;
			}
			_result = default(T);
			return false;
		}

		public static T FromMask<T>(this IEnumerable<T> _enumList) where T : struct, IConvertible, IComparable, IFormattable
		{
			int num = 0;
			foreach (T _enum in _enumList)
			{
				num |= (int)(object)_enum;
			}
			return (T)(object)num;
		}

		public static T[] ToFlagList<T>(this T _enum) where T : struct, IConvertible, IComparable, IFormattable
		{
			T[] array = (T[])Enum.GetValues(typeof(T));
			int num = (int)(object)_enum;
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if ((num & (int)(object)array[i]) != 0)
				{
					num2++;
				}
			}
			T[] array2 = new T[num2];
			int num3 = 0;
			foreach (object obj in array)
			{
				int num4 = (int)obj;
				if ((num & num4) != 0)
				{
					array2[num3++] = (T)obj;
				}
			}
			return array2;
		}
	}
}
