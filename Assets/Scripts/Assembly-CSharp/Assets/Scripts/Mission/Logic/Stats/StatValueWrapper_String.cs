using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_String : StatValueWrapperBase<StatValueWrapper_String>
	{
		public override string String { get; set; }

		public override Type ValueType
		{
			get
			{
				return typeof(string);
			}
		}

		public StatValueWrapper_String()
		{
			String = string.Empty;
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return String.Equals(Casted(obj).String);
		}

		public override int GetHashCode()
		{
			return String.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_String statValueWrapper_String = new StatValueWrapper_String();
			statValueWrapper_String.String = String;
			return statValueWrapper_String;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			String = _wrapper.String;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				String = XmlHelpers.Serialize<string>("value", _s);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, String);
			}
		}

		public override string ToString()
		{
			return String;
		}
	}
}
