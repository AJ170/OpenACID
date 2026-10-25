using System;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueWrapper_Vector3 : StatValueWrapperBase<StatValueWrapper_Vector3>
	{
		public override Vector3 Vector3 { get; set; }

		public override Type ValueType
		{
			get
			{
				return typeof(Vector3);
			}
		}

		public override bool Equals(object obj)
		{
			if (!base.Equals(obj))
			{
				return false;
			}
			return Vector3.Equals(Casted(obj).Vector3);
		}

		public override int GetHashCode()
		{
			return Vector3.GetHashCode();
		}

		public override IStatValueWrapper Clone()
		{
			StatValueWrapper_Vector3 statValueWrapper_Vector = new StatValueWrapper_Vector3();
			statValueWrapper_Vector.Vector3 = Vector3;
			return statValueWrapper_Vector;
		}

		public override void CopyFrom(IStatValueWrapper _wrapper)
		{
			Vector3 = _wrapper.Vector3;
		}

		public override void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			if (_s.Mode == EMode.EMode_Read)
			{
				Vector3 = XmlHelpers.Serialize<Vector3>("value", _s);
			}
			else
			{
				XmlHelpers.Serialize("value", _s, Vector3);
			}
		}

		public override string ToString()
		{
			return Vector3.ToString();
		}
	}
}
