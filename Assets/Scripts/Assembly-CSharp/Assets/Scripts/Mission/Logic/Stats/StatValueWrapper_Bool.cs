using System;
using System.Globalization;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_Bool : StatValueWrapperBase<StatValueWrapper_Bool>
	{
		public override bool Bool { get; set; }

		public override Type ValueType
		{
			get
			{
				return typeof(bool);
			}
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return Bool.Equals(Casted(obj).Bool);
		}

		public override int GetHashCode()
		{
			return Bool.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_Bool statValueWrapper_Bool = new StatValueWrapper_Bool();
			statValueWrapper_Bool.Bool = Bool;
			return statValueWrapper_Bool;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			Bool = _wrapper.Bool;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				Bool = XmlHelpers.Serialize("value", _s, false);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, Bool);
			}
		}

		public override string ToString()
		{
			return Bool.ToString(CultureInfo.InvariantCulture);
		}
	}
}
