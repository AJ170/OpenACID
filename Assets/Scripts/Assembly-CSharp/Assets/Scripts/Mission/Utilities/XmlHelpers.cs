using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using Assets.Scripts.Mission.Logic;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Utilities
{
	public static class XmlHelpers
	{
		public static T Serialize<T>(string _name, DataSerializerBase _s, [Optional] T _value)
		{
			T result = _value;
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(Vector3))
			{
				result = (T)(object)_s.SerializeVector3(_name, (Vector3)(object)_value);
			}
			else if (typeFromHandle == typeof(EntityId))
			{
				result = (T)(object)SerializeEntityId(_name, _s, (EntityId)(object)_value);
			}
			else if (typeFromHandle == typeof(Type))
			{
				result = (T)(object)_s.SerializeType(_name, (Type)(object)_value);
			}
			else if (typeFromHandle.IsEnum)
			{
				result = (T)(object)_s.SerializeEnum(_name, typeFromHandle, (int)(object)_value);
			}
			else if (typeFromHandle == typeof(string))
			{
				if (_s.Mode == EMode.EMode_Write)
				{
					_s.SerializeString(_name, (string)(object)_value);
				}
				else
				{
					result = (T)(object)_s.SerializeString(_name);
				}
			}
			else
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
				if (_s.Mode == EMode.EMode_Write)
				{
					string value = converter.ConvertToString(null, CultureInfo.InvariantCulture, _value);
					_s.SerializeString(_name, value);
				}
				else
				{
					string text = _s.SerializeString(_name);
					result = (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, text);
				}
			}
			return result;
		}

		public static EntityId SerializeEntityId(string _name, DataSerializerBase _s, EntityId _value = null)
		{
			EntityId result = _value;
			_s.SerializeBeginNode(_name);
			if (_s.Mode == EMode.EMode_Read)
			{
				uint slot = _s.SerializeUInt32("slot");
				byte usageCounter = _s.SerializeByte("usageCounter", 0);
				result = LogicMain.EntityManager.GetEntityId(slot, usageCounter);
			}
			else
			{
				_s.SerializeUInt32("slot", _value.Slot);
				_s.SerializeByte("usageCounter", _value.UsageCounter);
			}
			_s.SerializeEndNode();
			return result;
		}

		public static BBSimpleList<TValue> SerializeEnumMap<TKey, TValue>(string _name, DataSerializerBase _s, BBSimpleList<TValue> _values = null, BBSimpleList<TValue> _templateValues = null) where TKey : struct
		{
			_s.SerializeBeginNode(_name);
			TValue[] array = ((_values == null) ? null : _values.ToArray());
			Type typeFromHandle = typeof(TKey);
			Array values = Enum.GetValues(typeFromHandle);
			if (_s.Mode == EMode.EMode_Read)
			{
				BBDictionary<string, TValue> bBDictionary = SerializeDictionary<string, TValue>(_s);
				int num = -1;
				if (bBDictionary != null)
				{
					foreach (KeyValuePair<string, TValue> item in bBDictionary)
					{
						int val = (int)Enum.Parse(typeFromHandle, item.Key);
						num = Math.Max(num, val);
					}
				}
				array = new TValue[Math.Max(num + 1, (_templateValues != null) ? _templateValues.Count : 0)];
				if (_templateValues != null)
				{
					for (int i = 0; i < _templateValues.Count; i++)
					{
						array[i] = _templateValues[i];
					}
				}
				foreach (KeyValuePair<string, TValue> item2 in bBDictionary)
				{
					try
					{
						array[(int)Enum.Parse(typeFromHandle, item2.Key)] = item2.Value;
					}
					catch (Exception)
					{
						string key = item2.Key;
						throw;
					}
				}
			}
			else
			{
				BBDictionary<string, TValue> bBDictionary2 = new BBDictionary<string, TValue>(_values.Count);
				for (int j = 0; j < _values.Count; j++)
				{
					if (_templateValues == null || _templateValues.Count <= j || !object.Equals(_values[j], _templateValues[j]))
					{
						bBDictionary2[values.GetValue(j).ToString()] = _values[j];
					}
				}
				SerializeDictionary(_s, bBDictionary2);
			}
			_s.SerializeEndNode();
			return new BBSimpleList<TValue>(array);
		}

		public static BBDictionary<TKey, TValue> SerializeDictionary<TKey, TValue>(DataSerializerBase _s, BBDictionary<TKey, TValue> _dictionary = null)
		{
			BBDictionary<TKey, TValue> bBDictionary = _dictionary;
			_s.SerializeBeginNode("Pairs");
			if (_s.Mode == EMode.EMode_Read)
			{
				int num = _s.SerializeItemCount();
				bBDictionary = new BBDictionary<TKey, TValue>(num);
				for (int i = 0; i < num; i++)
				{
					_s.SerializeBeginNode("Pair");
					TKey key = Serialize<TKey>("key", _s);
					TValue value = Serialize<TValue>("value", _s);
					bBDictionary[key] = value;
					_s.SerializeEndNode();
				}
			}
			else
			{
				_s.SerializeItemCount(_dictionary.Count);
				foreach (KeyValuePair<TKey, TValue> item in _dictionary)
				{
					_s.SerializeBeginNode("Pair");
					Serialize("key", _s, item.Key);
					Serialize("value", _s, item.Value);
					_s.SerializeEndNode();
				}
			}
			_s.SerializeEndNode();
			return bBDictionary;
		}
	}
}
