using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using SharedBaseLib.Core;
using UnityEngine;

namespace SharedBaseLib.Serialization
{
	public class DataBinarySerializer : DataSerializerBase
	{
		private Stream Stream { get; set; }

		private BinaryWriter BinaryWriter { get; set; }

		private BinaryReader BinaryReader { get; set; }

		private Stack<long> ListBegin { get; set; }

		public DataBinarySerializer(Stream _stream, EMode _mode)
		{
			Stream = _stream;
			base.Mode = _mode;
			if (base.Mode == EMode.EMode_Read)
			{
				Stream.Position = 0L;
				BinaryReader = new BinaryReader(Stream);
			}
			else
			{
				ListBegin = new Stack<long>();
				BinaryWriter = new BinaryWriter(Stream);
			}
		}

		public override float SerializeFloat(string _name, float _value = 0f)
		{
			float result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override string SerializeString(string _name, string _value = null)
		{
			string result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadString();
			}
			else
			{
				BinaryWriter.Write((_value == null) ? "" : _value);
			}
			return result;
		}

		public override bool SerializeBool(string _name, bool _value = false)
		{
			bool result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadBoolean();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override byte SerializeByte(string _name, byte _value = 0)
		{
			byte result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadByte();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override sbyte SerializeSByte(string _name, sbyte _value = 0)
		{
			sbyte result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadSByte();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override char SerializeChar(string _name, char _value = '\0')
		{
			char result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadChar();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override decimal SerializeDecimal(string _name, decimal _value = 0m)
		{
			decimal result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadDecimal();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override double SerializeDouble(string _name, double _value = 0.0)
		{
			double result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadDouble();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override short SerializeInt16(string _name, short _value = 0)
		{
			short result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadInt16();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override int SerializeInt32(string _name, int _value = 0)
		{
			int result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadInt32();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override long SerializeInt64(string _name, long _value = 0L)
		{
			long result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadInt64();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override int SerializeInt(string _name, int _value = 0)
		{
			int result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadInt32();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override ushort SerializeUInt16(string _name, ushort _value = 0)
		{
			ushort result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadUInt16();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override uint SerializeUInt32(string _name, uint _value = 0u)
		{
			uint result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadUInt32();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override ulong SerializeUInt64(string _name, ulong _value = 0uL)
		{
			ulong result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = BinaryReader.ReadUInt64();
			}
			else
			{
				BinaryWriter.Write(_value);
			}
			return result;
		}

		public override IConvertible SerializeGeneric<T>(string _name, T _value = default(T))
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(bool))
			{
				return SerializeBool("", _value.ToBoolean(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(byte))
			{
				return SerializeByte("", _value.ToByte(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(sbyte))
			{
				return SerializeSByte("", _value.ToSByte(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(char))
			{
				return SerializeChar("", _value.ToChar(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(decimal))
			{
				return SerializeDecimal("", _value.ToDecimal(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(double))
			{
				return SerializeDouble("", _value.ToDouble(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(float))
			{
				return SerializeFloat("", _value.ToSingle(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(int))
			{
				return SerializeInt32("", _value.ToInt32(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(uint))
			{
				return SerializeUInt32("", _value.ToUInt32(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(long))
			{
				return SerializeInt64("", _value.ToInt64(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(ulong))
			{
				return SerializeUInt64("", _value.ToUInt64(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(short))
			{
				return SerializeInt16("", _value.ToInt16(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(ushort))
			{
				return SerializeUInt16("", _value.ToUInt16(CultureInfo.InvariantCulture));
			}
			if (typeFromHandle == typeof(string))
			{
				return SerializeString("", (_value != null) ? _value.ToString(CultureInfo.InvariantCulture) : null);
			}
			throw new ArgumentException(string.Format("DataBinarySerializer.SerializeGeneric<T>: Type {0} is not valid!", typeFromHandle.Name));
		}

		public override int SerializeEnum(string _name, Type _type, int _value)
		{
			int num = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				string text = SerializeString(_name);
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
				SerializeString(_name, empty);
			}
			return num;
		}

		public override Array SerializeEnumArray(string _name, Type _type, Array _value)
		{
			Array array = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = Array.CreateInstance(_type, num);
				for (int i = 0; i < num; i++)
				{
					int value = SerializeEnum("", _type, 0);
					array.SetValue(Enum.ToObject(_type, value), i);
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeString("", _value.GetValue(j).ToString());
				}
			}
			return array;
		}

		public override List<T> SerializeList<T>(string _name, List<T> _value = null, object _customData = null)
		{
			List<T> list = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				list = new List<T>(num);
				for (int i = 0; i < num; i++)
				{
					T item = new T();
					item.Serialize("", this, _customData);
					list.Add(item);
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					_value[j].Serialize("", this, _customData);
				}
			}
			return list;
		}

		public override List<T> SerializeListGeneric<T>(string _name, List<T> _value = null)
		{
			List<T> list = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				list = new List<T>(num);
				for (int i = 0; i < num; i++)
				{
					list.Add((T)SerializeGeneric<T>(""));
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					SerializeGeneric("", _value[j]);
				}
			}
			return list;
		}

		public override BBList<T> SerializeList<T>(string _name, BBList<T> _value = null, object _customData = null)
		{
			BBList<T> bBList = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				bBList = new BBList<T>(num);
				for (int i = 0; i < num; i++)
				{
					T item = new T();
					item.Serialize("", this, _customData);
					bBList.Add(item);
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					_value[j].Serialize("", this, _customData);
				}
			}
			return bBList;
		}

		public override BBList<T> SerializeListGeneric<T>(string _name, BBList<T> _value = null)
		{
			BBList<T> bBList = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				bBList = new BBList<T>(num);
				for (int i = 0; i < num; i++)
				{
					bBList.Add((T)SerializeGeneric<T>(""));
				}
			}
			else
			{
				SerializeItemCount(_value.Count);
				for (int j = 0; j < _value.Count; j++)
				{
					SerializeGeneric("", _value[j]);
				}
			}
			return bBList;
		}

		public override T[] SerializeArray<T>(string _name, T[] _value = null, object _customData = null)
		{
			T[] array = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new T[num];
				for (int i = 0; i < num; i++)
				{
					T val = new T();
					val.Serialize("", this, _customData);
					array[i] = val;
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					_value[j].Serialize("", this, _customData);
				}
			}
			return array;
		}

		public override T[] SerializeArrayGeneric<T>(string _name, T[] _value = null)
		{
			T[] array = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new T[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = (T)SerializeGeneric<T>("");
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeGeneric("", _value[j]);
				}
			}
			return array;
		}

		public override short[] SerializeArrayInt16(string _name, short[] _value = null)
		{
			short[] array = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				int num = SerializeItemCount();
				array = new short[num];
				for (int i = 0; i < num; i++)
				{
					array[i] = SerializeInt16("item", array[i]);
				}
			}
			else
			{
				SerializeItemCount(_value.Length);
				for (int j = 0; j < _value.Length; j++)
				{
					SerializeInt16("", array[j]);
				}
			}
			return array;
		}

		public override Vector2 SerializeVector2(string _name, Vector2 _value = default(Vector2))
		{
			Vector2 result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = BinaryReader.ReadSingle();
				result.y = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.x);
				BinaryWriter.Write(_value.y);
			}
			return result;
		}

		public override Vector3 SerializeVector3(string _name, Vector3 _value = default(Vector3))
		{
			Vector3 result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = BinaryReader.ReadSingle();
				result.y = BinaryReader.ReadSingle();
				result.z = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.x);
				BinaryWriter.Write(_value.y);
				BinaryWriter.Write(_value.z);
			}
			return result;
		}

		public override Vector4 SerializeVector4(string _name, Vector4 _value = default(Vector4))
		{
			Vector4 result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = BinaryReader.ReadSingle();
				result.y = BinaryReader.ReadSingle();
				result.z = BinaryReader.ReadSingle();
				result.w = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.x);
				BinaryWriter.Write(_value.y);
				BinaryWriter.Write(_value.z);
				BinaryWriter.Write(_value.w);
			}
			return result;
		}

		public override Rect SerializeRect(string _name, Rect _value = default(Rect))
		{
			Rect result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = BinaryReader.ReadSingle();
				result.y = BinaryReader.ReadSingle();
				result.width = BinaryReader.ReadSingle();
				result.height = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.x);
				BinaryWriter.Write(_value.y);
				BinaryWriter.Write(_value.width);
				BinaryWriter.Write(_value.height);
			}
			return result;
		}

		public override Quaternion SerializeQuaternion(string _name, Quaternion _value = default(Quaternion))
		{
			Quaternion result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.x = BinaryReader.ReadSingle();
				result.y = BinaryReader.ReadSingle();
				result.z = BinaryReader.ReadSingle();
				result.w = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.x);
				BinaryWriter.Write(_value.y);
				BinaryWriter.Write(_value.z);
				BinaryWriter.Write(_value.w);
			}
			return result;
		}

		public override Color SerializeColor(string _name, Color _value = default(Color))
		{
			Color result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result.a = BinaryReader.ReadSingle();
				result.b = BinaryReader.ReadSingle();
				result.g = BinaryReader.ReadSingle();
				result.r = BinaryReader.ReadSingle();
			}
			else
			{
				BinaryWriter.Write(_value.a);
				BinaryWriter.Write(_value.b);
				BinaryWriter.Write(_value.g);
				BinaryWriter.Write(_value.r);
			}
			return result;
		}

		public override Type SerializeType(string _name, Type _value = null)
		{
			Type result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				result = Type.GetType(BinaryReader.ReadString());
			}
			else
			{
				BinaryWriter.Write(_value.AssemblyQualifiedName);
			}
			return result;
		}

		public override string SerializeIdentifier(string _name, string _value)
		{
			string result = _value;
			if (base.Mode == EMode.EMode_Read)
			{
				char[] array = new char[_value.Length];
				for (int i = 0; i < _value.Length; i++)
				{
					array[i] = BinaryReader.ReadChar();
				}
				result = new string(array);
			}
			else
			{
				char[] array2 = _value.ToCharArray();
				for (int j = 0; j < _value.Length; j++)
				{
					BinaryWriter.Write(array2[j]);
				}
			}
			return result;
		}

		public override void BeginList()
		{
			if (base.Mode == EMode.EMode_Write)
			{
				ListBegin.Push(Stream.Position);
				SerializeInt32("", -1);
			}
		}

		public override void EndList(int _itemCount)
		{
			if (base.Mode == EMode.EMode_Write)
			{
				long position = Stream.Position;
				long offset = ListBegin.Pop();
				Stream.Seek(offset, SeekOrigin.Begin);
				SerializeItemCount(_itemCount);
				Stream.Seek(position, SeekOrigin.Begin);
			}
		}

		public override int SerializeItemCount(int _itemCount = 0)
		{
			return SerializeInt32(DataSerializerBase.kItemCount, _itemCount);
		}

		public override void SerializeBeginNode(string _name)
		{
		}

		public override void SerializeEndNode()
		{
		}

		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~DataBinarySerializer()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BinaryWriter != null)
				{
					BinaryWriter.Close();
					BinaryWriter = null;
				}
				if (BinaryReader != null)
				{
					BinaryReader.Close();
					BinaryReader = null;
				}
			}
		}
	}
}
