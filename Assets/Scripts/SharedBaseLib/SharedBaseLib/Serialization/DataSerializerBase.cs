using System;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

namespace SharedBaseLib.Serialization
{
	public class DataSerializerBase : IDisposable
	{
		protected static string kItemCount = "ItemCount";

		public EMode Mode { get; protected set; }

		public virtual float SerializeFloat(string _name, float _value = 0f)
		{
			return _value;
		}

		public virtual string SerializeString(string _name, string _value = null)
		{
			return _value;
		}

		public virtual bool SerializeBool(string _name, bool _value = false)
		{
			return _value;
		}

		public virtual byte SerializeByte(string _name, byte _value = 0)
		{
			return _value;
		}

		public virtual sbyte SerializeSByte(string _name, sbyte _value = 0)
		{
			return _value;
		}

		public virtual char SerializeChar(string _name, char _value = '\0')
		{
			return _value;
		}

		public virtual decimal SerializeDecimal(string _name, decimal _value = 0m)
		{
			return _value;
		}

		public virtual double SerializeDouble(string _name, double _value = 0.0)
		{
			return _value;
		}

		public virtual short SerializeInt16(string _name, short _value = 0)
		{
			return _value;
		}

		public virtual int SerializeInt32(string _name, int _value = 0)
		{
			return _value;
		}

		public virtual long SerializeInt64(string _name, long _value = 0L)
		{
			return _value;
		}

		public virtual int SerializeInt(string _name, int _value = 0)
		{
			return _value;
		}

		public virtual ushort SerializeUInt16(string _name, ushort _value = 0)
		{
			return _value;
		}

		public virtual uint SerializeUInt32(string _name, uint _value = 0u)
		{
			return _value;
		}

		public virtual ulong SerializeUInt64(string _name, ulong _value = 0uL)
		{
			return _value;
		}

		public virtual IConvertible SerializeGeneric<T>(string _name, T _value = default(T)) where T : IConvertible
		{
			return _value;
		}

		public virtual int SerializeEnum(string _name, Type _type, int _value)
		{
			return _value;
		}

		public virtual Array SerializeEnumArray(string _name, Type _type, Array _value)
		{
			return _value;
		}

		public virtual List<T> SerializeList<T>(string _name, List<T> _value = null, object _customData = null) where T : IDataSerializable, new()
		{
			return _value;
		}

		public virtual List<T> SerializeListGeneric<T>(string _name, List<T> _value = null) where T : IConvertible
		{
			return _value;
		}

		public virtual BBList<T> SerializeList<T>(string _name, BBList<T> _value = null, object _customData = null) where T : IDataSerializable, new()
		{
			return _value;
		}

		public virtual BBList<T> SerializeListGeneric<T>(string _name, BBList<T> _value = null) where T : IConvertible
		{
			return _value;
		}

		public virtual T[] SerializeArray<T>(string _name, T[] _value = null, object _customData = null) where T : IDataSerializable, new()
		{
			return _value;
		}

		public virtual T[] SerializeArrayGeneric<T>(string _name, T[] _value = null) where T : IConvertible
		{
			return _value;
		}

		public virtual short[] SerializeArrayInt16(string _name, short[] _value = null)
		{
			return _value;
		}

		public virtual Vector2 SerializeVector2(string _name, Vector2 _value = default(Vector2))
		{
			return _value;
		}

		public virtual Vector3 SerializeVector3(string _name, Vector3 _value = default(Vector3))
		{
			return _value;
		}

		public virtual Vector4 SerializeVector4(string _name, Vector4 _value = default(Vector4))
		{
			return _value;
		}

		public virtual Rect SerializeRect(string _name, Rect _value = default(Rect))
		{
			return _value;
		}

		public virtual Quaternion SerializeQuaternion(string _name, Quaternion _value = default(Quaternion))
		{
			return _value;
		}

		public virtual Color SerializeColor(string _name, Color _value = default(Color))
		{
			return _value;
		}

		public virtual string SerializeIdentifier(string _name, string _value)
		{
			return _value;
		}

		public virtual Type SerializeType(string _name, Type _value = null)
		{
			return _value;
		}

		public virtual void BeginList()
		{
		}

		public virtual void EndList(int _itemCount)
		{
		}

		public virtual void SerializeBeginNode(string _name)
		{
		}

		public virtual void SerializeEndNode()
		{
		}

		public virtual int SerializeItemCount(int _itemCount = 0)
		{
			return _itemCount;
		}

		public virtual void Dispose()
		{
		}
	}
}
