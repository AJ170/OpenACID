using System;
using System.Reflection;
using System.Xml;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.AI.BehaviorLibrary.Components
{
	public abstract class ParameterContainer : IParameterContainer, ICloneable
	{
		public PropertyInfo[] ParameterDefinitions { get; protected set; }

		public void Read(XmlReader _children)
		{
			while (_children.Read())
			{
				if (_children.NodeType != XmlNodeType.Element || !(_children.Name == GetType().Name))
				{
					continue;
				}
				for (int i = 0; i < _children.AttributeCount; i++)
				{
					_children.MoveToNextAttribute();
					string paramName = _children.Name;
					PropertyInfo propertyInfo = Array.Find(ParameterDefinitions, (PropertyInfo _prop) => string.Equals(_prop.Name, paramName));
					if (propertyInfo.PropertyType.IsEnum)
					{
						object value = Enum.Parse(propertyInfo.PropertyType, _children.Value);
						propertyInfo.SetValue(this, value, null);
					}
					else if (propertyInfo.PropertyType.IsEnumStruct())
					{
						object value2 = _children.Value.ToEnum(propertyInfo.PropertyType);
						propertyInfo.SetValue(this, value2, null);
					}
					else
					{
						object value3 = Convert.ChangeType(_children.Value, propertyInfo.PropertyType);
						propertyInfo.SetValue(this, value3, null);
					}
				}
				break;
			}
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}
