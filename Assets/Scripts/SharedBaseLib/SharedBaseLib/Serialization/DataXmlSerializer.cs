using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using SharedBaseLib.Core;
using UnityEngine;

namespace SharedBaseLib.Serialization
{
	public class DataXmlSerializer : DataSerializerBase
	{
		private class ChildIndex
		{
			public int Index { get; set; }
		}

		private static string kItem = "Item";

		private Stream Stream { get; set; }

		private XmlDocument XmlDocument { get; set; }

		private XmlNode XmlNode { get; set; }

		private Stack<XmlNode> XmlNodeStack { get; set; }

		private Dictionary<XmlNode, ChildIndex> XmlNodeCurrentChildIndex { get; set; }

		private bool NewLineOnAttributes { get; set; }

		public DataXmlSerializer(Stream _stream, EMode _mode, bool _newLineOnAttributes = true)
		{
			Stream = _stream;
			base.Mode = _mode;
			XmlDocument = new XmlDocument();
			XmlNodeStack = new Stack<XmlNode>();
			XmlNode = XmlDocument;
			NewLineOnAttributes = _newLineOnAttributes;
			if (base.Mode == EMode.EMode_Read)
			{
				XmlDocument.Load(Stream);
				XmlNodeCurrentChildIndex = new Dictionary<XmlNode, ChildIndex>();
			}
		}

		public override float SerializeFloat(string _name, float _value = 0f)
		{
			float result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<float>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override string SerializeString(string _name, string _value = null)
		{
			string text = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				if (XmlNode.Attributes != null)
				{
					XmlAttribute xmlAttribute = XmlNode.Attributes[_name];
					if (xmlAttribute != null)
					{
						text = xmlAttribute.Value;
						if (text == null)
						{
							text = "";
						}
					}
				}
			}
			else
			{
				WriteAttributeInternal(_name, _value);
			}
			return text;
		}

		public override bool SerializeBool(string _name, bool _value = false)
		{
			bool result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<bool>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override byte SerializeByte(string _name, byte _value = 0)
		{
			byte result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<byte>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override sbyte SerializeSByte(string _name, sbyte _value = 0)
		{
			sbyte result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<sbyte>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override char SerializeChar(string _name, char _value = '\0')
		{
			char result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<char>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override decimal SerializeDecimal(string _name, decimal _value = 0m)
		{
			decimal result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<decimal>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override double SerializeDouble(string _name, double _value = 0.0)
		{
			double result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<double>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override short SerializeInt16(string _name, short _value = 0)
		{
			short result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<short>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override int SerializeInt32(string _name, int _value = 0)
		{
			int result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<int>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override long SerializeInt64(string _name, long _value = 0L)
		{
			long result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<long>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override int SerializeInt(string _name, int _value = 0)
		{
			int result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<int>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override ushort SerializeUInt16(string _name, ushort _value = 0)
		{
			ushort result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<ushort>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override uint SerializeUInt32(string _name, uint _value = 0u)
		{
			uint result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<uint>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override ulong SerializeUInt64(string _name, ulong _value = 0uL)
		{
			ulong result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = ReadAttributeInternal<ulong>(_name);
			}
			else
			{
				WriteAttributeInternal(_name, _value.ToString(CultureInfo.InvariantCulture));
			}
			return result;
		}

		public override IConvertible SerializeGeneric<T>(string _name, T _value = default(T))
		{
			return SerializeString(_name, (_value != null) ? _value.ToString(CultureInfo.InvariantCulture) : "");
		}

		public override int SerializeEnum(string _name, Type _type, int _value)
		{
			int num = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				string text = ReadAttributeInternal<string>(_name);
				if (!string.IsNullOrEmpty(text))
				{
					num = 0;
					string[] array = text.Split(',');
					for (int i = 0; i < array.Length; i++)
					{
						num |= (int)Enum.Parse(_type, array[i]);
					}
				}
			}
			else
			{
				string empty = string.Empty;
				if (Enum.IsDefined(_type, _value))
				{
					empty = Enum.ToObject(_type, _value).ToString();
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					bool flag = true;
					foreach (int value in Enum.GetValues(_type))
					{
						if (value != 0 && (_value & value) == value)
						{
							if (!flag)
							{
								stringBuilder.Append(',');
							}
							flag = false;
							stringBuilder.Append(Enum.ToObject(_type, value).ToString());
						}
					}
					empty = stringBuilder.ToString();
				}
				WriteAttributeInternal(_name, empty);
			}
			return num;
		}

		public override Array SerializeEnumArray(string _name, Type _type, Array _value)
		{
			Array array = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = Array.CreateInstance(_type, num);
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					int value = SerializeEnum(kItem, _type, 0);
					array.SetValue(Enum.ToObject(_type, value), i);
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeBeginNode(kItem);
					WriteAttributeInternal(kItem, _value.GetValue(j).ToString());
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return array;
		}

		public override List<T> SerializeList<T>(string _name, List<T> _value = null, object _customData = null)
		{
			List<T> list = _value;
			SerializeBeginNode(_name);
			Type typeFromHandle = typeof(T);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				list = new List<T>(num);
				for (int i = 0; i < num; i++)
				{
					T item = new T();
					item.Serialize(typeFromHandle.Name, this, _customData);
					list.Add(item);
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					_value[j].Serialize(typeFromHandle.Name, this, _customData);
				}
			}
			SerializeEndNode();
			return list;
		}

		public override List<T> SerializeListGeneric<T>(string _name, List<T> _value = null)
		{
			List<T> list = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				list = new List<T>(num);
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					list.Add((T)SerializeGeneric<T>(kItem));
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					SerializeBeginNode(kItem);
					SerializeGeneric(kItem, _value[j]);
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return list;
		}

		public override BBList<T> SerializeList<T>(string _name, BBList<T> _value = null, object _customData = null)
		{
			BBList<T> bBList = _value;
			SerializeBeginNode(_name);
			Type typeFromHandle = typeof(T);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				bBList = new BBList<T>(num);
				for (int i = 0; i < num; i++)
				{
					T item = new T();
					item.Serialize(typeFromHandle.Name, this, _customData);
					bBList.Add(item);
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					_value[j].Serialize(typeFromHandle.Name, this, _customData);
				}
			}
			SerializeEndNode();
			return bBList;
		}

		public override BBList<T> SerializeListGeneric<T>(string _name, BBList<T> _value = null)
		{
			BBList<T> bBList = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				bBList = new BBList<T>(num);
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					bBList.Add((T)SerializeGeneric<T>(kItem));
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					SerializeBeginNode(kItem);
					SerializeGeneric(kItem, _value[j]);
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return bBList;
		}

		public override T[] SerializeArray<T>(string _name, T[] _value = null, object _customData = null)
		{
			T[] array = _value;
			SerializeBeginNode(_name);
			Type typeFromHandle = typeof(T);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new T[num];
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					T val = new T();
					val.Serialize(typeFromHandle.Name, this, _customData);
					array[i] = val;
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeBeginNode(kItem);
					_value[j].Serialize(typeFromHandle.Name, this, _customData);
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return array;
		}

		public override T[] SerializeArrayGeneric<T>(string _name, T[] _value = null)
		{
			T[] array = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new T[num];
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					array[i] = (T)SerializeGeneric<T>(kItem);
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeBeginNode(kItem);
					SerializeGeneric(kItem, _value[j]);
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return array;
		}

		public override short[] SerializeArrayInt16(string _name, short[] _value = null)
		{
			short[] array = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new short[num];
				for (int i = 0; i < num; i++)
				{
					SerializeBeginNode(kItem);
					array[i] = SerializeInt16(kItem, array[i]);
					SerializeEndNode();
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeBeginNode(kItem);
					SerializeInt16(kItem, _value[j]);
					SerializeEndNode();
				}
			}
			SerializeEndNode();
			return array;
		}

		public override Vector2 SerializeVector2(string _name, Vector2 _value = default(Vector2))
		{
			Vector2 result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = SerializeFloat("x");
				result.y = SerializeFloat("y");
			}
			else
			{
				SerializeFloat("x", _value.x);
				SerializeFloat("y", _value.y);
			}
			SerializeEndNode();
			return result;
		}

		public override Vector3 SerializeVector3(string _name, Vector3 _value = default(Vector3))
		{
			Vector3 result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = SerializeFloat("x");
				result.y = SerializeFloat("y");
				result.z = SerializeFloat("z");
			}
			else
			{
				SerializeFloat("x", _value.x);
				SerializeFloat("y", _value.y);
				SerializeFloat("z", _value.z);
			}
			SerializeEndNode();
			return result;
		}

		public override Vector4 SerializeVector4(string _name, Vector4 _value = default(Vector4))
		{
			Vector4 result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = SerializeFloat("x");
				result.y = SerializeFloat("y");
				result.z = SerializeFloat("z");
				result.w = SerializeFloat("w");
			}
			else
			{
				SerializeFloat("x", _value.x);
				SerializeFloat("y", _value.y);
				SerializeFloat("z", _value.z);
				SerializeFloat("w", _value.w);
			}
			SerializeEndNode();
			return result;
		}

		public override Rect SerializeRect(string _name, Rect _value = default(Rect))
		{
			Rect result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = SerializeFloat("x");
				result.y = SerializeFloat("y");
				result.width = SerializeFloat("width");
				result.height = SerializeFloat("height");
			}
			else
			{
				SerializeFloat("x", _value.x);
				SerializeFloat("y", _value.y);
				SerializeFloat("width", _value.width);
				SerializeFloat("height", _value.height);
			}
			SerializeEndNode();
			return result;
		}

		public override Quaternion SerializeQuaternion(string _name, Quaternion _value = default(Quaternion))
		{
			Quaternion result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = SerializeFloat("x");
				result.y = SerializeFloat("y");
				result.z = SerializeFloat("z");
				result.w = SerializeFloat("w");
			}
			else
			{
				SerializeFloat("x", _value.x);
				SerializeFloat("y", _value.y);
				SerializeFloat("z", _value.z);
				SerializeFloat("w", _value.w);
			}
			SerializeEndNode();
			return result;
		}

		public override Color SerializeColor(string _name, Color _value = default(Color))
		{
			Color result = _value;
			SerializeBeginNode(_name);
			if (base.Mode == EMode.EMode_Read)
			{
				result.r = SerializeFloat("r");
				result.g = SerializeFloat("g");
				result.b = SerializeFloat("b");
				result.a = SerializeFloat("a");
			}
			else
			{
				SerializeFloat("r", _value.r);
				SerializeFloat("g", _value.g);
				SerializeFloat("b", _value.b);
				SerializeFloat("a", _value.a);
			}
			SerializeEndNode();
			return result;
		}

		public override Type SerializeType(string _name, Type _value = null)
		{
			Type type = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				if (XmlNode.Attributes != null)
				{
					XmlAttribute xmlAttribute = XmlNode.Attributes[_name];
					if (xmlAttribute != null)
					{
						string value = xmlAttribute.Value;
						if (value != null)
						{
							type = Type.GetType(value);
							if (type == null)
							{
								type = null;
							}
						}
					}
				}
			}
			else
			{
				WriteAttributeInternal(_name, _value.AssemblyQualifiedName);
			}
			return type;
		}

		public override string SerializeIdentifier(string _name, string _value)
		{
			string result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = XmlNode.Attributes[_name].Value;
			}
			else
			{
				WriteAttributeInternal(_name, _value);
			}
			return result;
		}

		public override void SerializeBeginNode(string _name)
		{
			if (base.Mode == EMode.EMode_Read)
			{
				int currentXmlNodeChildIndex = GetCurrentXmlNodeChildIndex(XmlNode);
				XmlNode xmlNode = XmlNode;
				XmlNodeStack.Push(xmlNode);
				string name = xmlNode.Name;
				if (currentXmlNodeChildIndex >= 0)
				{
					XmlNode = xmlNode.ChildNodes[currentXmlNodeChildIndex];
				}
				else
				{
					XmlNode = xmlNode.NextSibling;
				}
			}
			else
			{
				XmlNode xmlNode2 = XmlNode;
				XmlNodeStack.Push(xmlNode2);
				XmlNode = XmlDocument.CreateElement(_name);
				xmlNode2.AppendChild(XmlNode);
			}
		}

		public override void SerializeEndNode()
		{
			if (base.Mode == EMode.EMode_Read)
			{
				XmlNode = XmlNodeStack.Pop();
				IncrementXmlNodeChildIndex(XmlNode);
			}
			else
			{
				XmlNode = XmlNodeStack.Pop();
			}
		}

		private T ReadAttributeInternal<T>(string _name) where T : IConvertible
		{
			T result = default(T);
			if (base.Mode == EMode.EMode_Read && XmlNode.Attributes != null)
			{
				XmlAttribute xmlAttribute = XmlNode.Attributes[_name];
				if (xmlAttribute != null)
				{
					string value = xmlAttribute.Value;
					if (value != null)
					{
						TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
						if (converter != null)
						{
							return (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, value);
						}
					}
				}
			}
			return result;
		}

		private void WriteAttributeInternal(string _name, string _value)
		{
			if (base.Mode == EMode.EMode_Write)
			{
				XmlAttribute xmlAttribute = XmlDocument.CreateAttribute(_name);
				xmlAttribute.Value = _value;
				XmlNode.Attributes.Append(xmlAttribute);
			}
		}

		private int GetCurrentXmlNodeChildIndex(XmlNode _node)
		{
			ChildIndex value = null;
			if (!XmlNodeCurrentChildIndex.TryGetValue(_node, out value))
			{
				value = new ChildIndex();
				value.Index = ((_node.ChildNodes.Count <= 0) ? (-1) : 0);
				XmlNodeCurrentChildIndex[_node] = value;
			}
			return value.Index;
		}

		private void IncrementXmlNodeChildIndex(XmlNode _node)
		{
			ChildIndex value = null;
			if (!XmlNodeCurrentChildIndex.TryGetValue(_node, out value))
			{
				value = new ChildIndex();
				value.Index = ((_node.ChildNodes.Count <= 0) ? (-1) : 0);
				XmlNodeCurrentChildIndex[_node] = value;
			}
			else if (value.Index >= 0)
			{
				value.Index++;
			}
		}

		public override void BeginList()
		{
		}

		public override void EndList(int _itemCount)
		{
		}

		public override int SerializeItemCount(int _itemCount = 0)
		{
			int result = SerializeInt32(DataSerializerBase.kItemCount, _itemCount);
			if (base.Mode == EMode.EMode_Read)
			{
				int count = XmlNode.ChildNodes.Count;
			}
			return result;
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DataXmlSerializer()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (base.Mode == EMode.EMode_Write && XmlDocument != null)
			{
				XmlWriterSettings settings = new XmlWriterSettings
				{
					NewLineOnAttributes = NewLineOnAttributes,
					CloseOutput = true,
					Indent = true,
					NewLineChars = "\r\n",
					OmitXmlDeclaration = true
				};
				using (XmlWriter xmlWriter = XmlWriter.Create(Stream, settings))
				{
					XmlDocument.Save(xmlWriter);
				}
			}
			if (disposing)
			{
				XmlDocument = null;
			}
		}
	}
}
