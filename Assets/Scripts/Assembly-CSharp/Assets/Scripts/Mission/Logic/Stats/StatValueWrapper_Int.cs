using System;
using System.Globalization;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_Int : StatValueWrapperBase<StatValueWrapper_Int>, IDataSerializable, IStatValueWrapper, INumberStatValueWrapper, IComparable<StatValueWrapper_Int>
	{
		public override int Int { get; set; }

		public override float Number
		{
			get
			{
				return Int;
			}
			set
			{
				Int = (int)value;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(int);
			}
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return Int.Equals(Casted(obj).Int);
		}

		public override int GetHashCode()
		{
			return Int.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_Int statValueWrapper_Int = new StatValueWrapper_Int();
			statValueWrapper_Int.Int = Int;
			return statValueWrapper_Int;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			Int = _wrapper.Int;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				Int = XmlHelpers.Serialize("value", _s, 0);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, Int);
			}
		}

		public int CompareTo(StatValueWrapper_Int _other)
		{
			return Int.CompareTo(_other.Int);
		}

		public override string ToString()
		{
			return Int.ToString(CultureInfo.InvariantCulture);
		}
	}
}
