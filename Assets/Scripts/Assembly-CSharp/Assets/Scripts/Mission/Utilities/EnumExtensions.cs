using System;
using AcierProtocol;

namespace Assets.Scripts.Mission.Utilities
{
	public static class EnumExtensions
	{
		public static int ParseFlagSetString(this string _str, Type _type, char _separator = ',')
		{
			int num = 0;
			string[] array = _str.Split(_separator);
			for (int i = 0; i < array.Length; i++)
			{
				num |= (int)Enum.Parse(_type, array[i]);
			}
			return num;
		}

		public static Enum<T> ToEnum<T>(this string _name) where T : IEnumDefinition, new()
		{
			return new Enum<T>(_name);
		}

		public static IEnum ToEnum(this string _name, Type _type)
		{
			return (IEnum)Activator.CreateInstance(_type, _name);
		}

		public static bool IsEnumStruct(this Type _type)
		{
			return typeof(IEnum).IsAssignableFrom(_type);
		}

		public static int FixEnum<T>(int _targetKinds)
		{
			int num = _targetKinds;
			if (num == -1)
			{
				num = 0;
				foreach (T value in Enum.GetValues(typeof(T)))
				{
					num |= Convert.ToInt32(value);
				}
			}
			if (num < -1)
			{
				num = ~num + 1;
			}
			return num;
		}

		public static EEra Fix(this EEra _era)
		{
			if (_era == EEra.EEra_None)
			{
				return EEra.Common;
			}
			return _era;
		}
	}
}
