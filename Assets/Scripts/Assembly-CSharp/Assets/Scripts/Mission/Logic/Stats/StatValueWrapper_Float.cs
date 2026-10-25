using System;
using System.Globalization;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_Float : StatValueWrapperBase<StatValueWrapper_Float>, IDataSerializable, IStatValueWrapper, INumberStatValueWrapper, IComparable<StatValueWrapper_Float>
	{
		public override float Float { get; set; }

		public override float Number
		{
			get
			{
				return Float;
			}
			set
			{
				Float = value;
			}
		}

		public override Type ValueType
		{
			get
			{
				return typeof(float);
			}
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return Float.Equals(Casted(obj).Float);
		}

		public override int GetHashCode()
		{
			return Float.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_Float statValueWrapper_Float = new StatValueWrapper_Float();
			statValueWrapper_Float.Float = Float;
			return statValueWrapper_Float;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			Float = _wrapper.Float;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				Float = XmlHelpers.Serialize("value", _s, 0f);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, Float);
			}
		}

		public int CompareTo(StatValueWrapper_Float _other)
		{
			return Float.CompareTo(_other.Float);
		}

		public override string ToString()
		{
			return Float.ToString(CultureInfo.InvariantCulture);
		}
	}
}
