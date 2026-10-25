using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class EnumStatValueWrapper<TEnum> : StatValueWrapper_Int, IDataSerializable, IStatValueWrapper, IEnumStatValueWrapper where TEnum : struct
	{
		private static int s_DefaultValue;

		private static bool s_Initialized;

		public Type EnumType
		{
			get
			{
				return typeof(TEnum);
			}
		}

		public EnumStatValueWrapper()
		{
			Int = s_DefaultValue;
		}

		private static int ComputeDefaultValue(Type _valueType)
		{
			Array values = Enum.GetValues(_valueType);
			int num = (int)values.GetValue(0);
			for (int i = 0; i < values.Length; i++)
			{
				int num2 = (int)values.GetValue(i);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		public static void Initialize(Type _enumType)
		{
			s_DefaultValue = ComputeDefaultValue(typeof(TEnum));
			s_Initialized = true;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				Int = Convert.ToInt32(XmlHelpers.Serialize<TEnum>("value", _s));
			}
			else
			{
				XmlHelpers.Serialize("value", _s, (TEnum)Enum.ToObject(typeof(TEnum), Int));
			}
		}

		public override IStatValueWrapper Clone()
		{
			EnumStatValueWrapper<TEnum> enumStatValueWrapper = new EnumStatValueWrapper<TEnum>();
			enumStatValueWrapper.Int = Int;
			return enumStatValueWrapper;
		}
	}
}
