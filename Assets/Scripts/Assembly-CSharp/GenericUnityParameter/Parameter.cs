using System;
using UnityEngine;

namespace GenericUnityParameter
{
	[Serializable]
	public class Parameter
	{
		[SerializeField]
		private DataType m_Type;

		[SerializeField]
		private Vector4 m_Value;

		public DataType Type
		{
			get
			{
				return m_Type;
			}
			set
			{
				m_Type = value;
			}
		}

		public bool BoolValue
		{
			get
			{
				return m_Value.x != 0f;
			}
			set
			{
				m_Value.x = (value ? 1 : 0);
			}
		}

		public int IntValue
		{
			get
			{
				return Mathf.FloorToInt(m_Value.x);
			}
			set
			{
				m_Value.x = value;
			}
		}

		public float FloatValue
		{
			get
			{
				return m_Value.x;
			}
			set
			{
				m_Value.x = value;
			}
		}

		public Vector4 Vector4Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		public Color ColorValue
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		public Parameter()
		{
			Type = DataType.Vector4;
		}

		public Parameter(DataType _type)
		{
			Type = _type;
		}

		public override string ToString()
		{
			switch (m_Type)
			{
			case DataType.Bool:
				return BoolValue.ToString();
			case DataType.Int:
				return IntValue.ToString();
			case DataType.Float:
				return FloatValue.ToString();
			case DataType.Vector4:
				return Vector4Value.ToString();
			case DataType.Color:
				return ColorValue.ToString();
			default:
				return "Unknown Type";
			}
		}
	}
}
