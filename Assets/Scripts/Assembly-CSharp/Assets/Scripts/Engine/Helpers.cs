using System;
using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UbiCore.Logging;
using UnityEngine;

namespace Assets.Scripts.Engine
{
	public static class Helpers
	{
		public static T ForceGetComponent<T>(this GameObject _go) where T : Component
		{
			return _go.GetComponent<T>().ForceSet();
		}

		public static T ForceGetComponent<T>(this GameObject _go, string _msg) where T : Component
		{
			return _go.GetComponent<T>().ForceSet(_msg);
		}

		public static T ForceGetComponent<T>(this Component _component) where T : Component
		{
			return _component.GetComponent<T>().ForceSet();
		}

		public static T ForceGetComponent<T>(this Component _component, string _msg) where T : Component
		{
			return _component.GetComponent<T>().ForceSet(_msg);
		}

		public static Transform FindChildCached(this Transform _transform, string _name, ref Dictionary<int, Transform> _cache)
		{
			int hashCode = _name.GetHashCode();
			if (_cache.ContainsKey(hashCode))
			{
				return _cache[hashCode];
			}
			int hashCode2 = _transform.name.GetHashCode();
			if (!_cache.ContainsKey(hashCode2))
			{
				_cache.Add(hashCode2, _transform);
			}
			if (hashCode == hashCode2)
			{
				return _transform;
			}
			for (int i = 0; i < _transform.childCount; i++)
			{
				Transform child = _transform.GetChild(i);
				Transform transform = child.FindChildCached(_name, ref _cache);
				if (transform != null)
				{
					return transform;
				}
			}
			return null;
		}

		public static T GetComponentLimitedRecursive<T>(this GameObject _gameObject, int _maxLevel = 3, int _level = 0) where T : MonoBehaviour
		{
			if (_level > _maxLevel)
			{
				return (T)null;
			}
			T component = _gameObject.GetComponent<T>();
			if ((UnityEngine.Object)component != (UnityEngine.Object)null)
			{
				return component;
			}
			Transform transform = _gameObject.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				component = child.gameObject.GetComponentLimitedRecursive<T>(_maxLevel, _level + 1);
				if ((UnityEngine.Object)component != (UnityEngine.Object)null)
				{
					return component;
				}
			}
			return (T)null;
		}

		public static bool DoIfIs<T>(this object _obj, Action<T> _action) where T : class
		{
			T val = _obj as T;
			if (val == null)
			{
				return false;
			}
			_action(val);
			return true;
		}

		public static TResult DoIfIs<T, TResult>(this object _obj, Func<T, TResult> _action) where T : class
		{
			T val = _obj as T;
			if (val == null)
			{
				return default(TResult);
			}
			return _action(val);
		}

		public static TResult DoIfIs<T, TResult>(this object _obj, Func<T, TResult> _action, TResult _defaultResult) where T : class
		{
			T val = _obj as T;
			if (val == null)
			{
				return _defaultResult;
			}
			return _action(val);
		}

		public static bool DoIfSet<T>(this T _obj, Action<T> _action) where T : class
		{
			if (_obj == null)
			{
				return false;
			}
			_action(_obj);
			return true;
		}

		public static TResult DoIfSet<T, TResult>(this T _obj, Func<T, TResult> _action) where T : class
		{
			if (_obj == null)
			{
				return default(TResult);
			}
			return _action(_obj);
		}

		public static TResult DoIfSet<T, TResult>(this T _obj, Func<T, TResult> _action, TResult _defaultResult) where T : class
		{
			if (_obj == null)
			{
				return _defaultResult;
			}
			return _action(_obj);
		}

		public static T Cast<T>(this object _obj)
		{
			return (T)_obj;
		}

		public static T As<T>(this object _obj) where T : class
		{
			return _obj as T;
		}

		public static T ForceSet<T>(this T _obj) where T : class
		{
			return _obj;
		}

		public static T ForceSet<T>(this T _obj, string _msg) where T : class
		{
			return _obj;
		}

		public static void ClearIfNotEmpty(this IList _this)
		{
			if (_this != null && _this.Count > 0)
			{
				_this.Clear();
			}
		}

		public static void ClearIfNotEmpty<T>(this BBList<T> _this)
		{
			if (_this != null && _this.Count > 0)
			{
				_this.Clear();
			}
		}

		public static bool Verify(bool _condition)
		{
			if (!_condition)
			{
				Verify(false, "Verify");
			}
			return _condition;
		}

		public static bool Verify(bool _condition, string _str)
		{
			if (!_condition)
			{
				Verify(false, ChannelLogger.EChannel.Default, _str);
			}
			return _condition;
		}

		public static bool Verify(bool _condition, ChannelLogger.EChannel _channel, string _str, params object[] _params)
		{
			return _condition;
		}

		public static bool Verify(bool _condition, string _str, params object[] _params)
		{
			return _condition;
		}

		public static bool VerifyOrExit(bool _condition)
		{
			bool flag = Verify(_condition);
			if (!flag)
			{
			}
			return flag;
		}

		public static bool VerifyOrExit(bool _condition, string _str)
		{
			bool flag = Verify(_condition, _str);
			if (!flag)
			{
			}
			return flag;
		}

		public static bool VerifyOrExit(bool _condition, ChannelLogger.EChannel _channel, string _str, params object[] _params)
		{
			bool flag = Verify(_condition, _channel, _str, _params);
			if (!flag)
			{
			}
			return flag;
		}

		public static T NotNullOrExit<T>(T _object) where T : class
		{
			VerifyOrExit(_object != null);
			return _object;
		}

		public static T NotNullOrExit<T>(T _object, string _str) where T : class
		{
			VerifyOrExit(_object != null, _str);
			return _object;
		}

		public static T NotNullOrExit<T>(T _object, string _str, ChannelLogger.EChannel _channel, params object[] _params) where T : class
		{
			VerifyOrExit(_object != null, _channel, _str, _params);
			return _object;
		}

		public static int GetBbHashCode(this string _str)
		{
			if (string.IsNullOrEmpty(_str))
			{
				return 0;
			}
			return (int)StringCRC32.ComputeLowerCase(_str);
		}

		public static string GetBuildType()
		{
			string text = "?";
			return "Standalone - Android";
		}
	}
}
