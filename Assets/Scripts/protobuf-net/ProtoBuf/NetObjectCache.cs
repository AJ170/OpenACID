#define DEBUG
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	internal sealed class NetObjectCache
	{
		private class ReferenceHashtable : Hashtable
		{
			protected override int GetHash(object key)
			{
				return RuntimeHelpers.GetHashCode(key);
			}

			protected override bool KeyEquals(object item, object key)
			{
				return item == key;
			}
		}

		internal const int Root = 0;

		private MutableList underlyingList;

		private object rootObject;

		private int trapStartIndex;

		private ReferenceHashtable objectKeys;

		private Hashtable stringKeys;

		private MutableList List
		{
			get
			{
				if (underlyingList == null)
				{
					underlyingList = new MutableList();
				}
				return underlyingList;
			}
		}

		internal object GetKeyedObject(int key)
		{
			if (key-- == 0)
			{
				if (rootObject == null)
				{
					throw new ProtoException("No root object assigned");
				}
				return rootObject;
			}
			BasicList list = List;
			if (key < 0 || key >= list.Count)
			{
				Helpers.DebugWriteLine("Missing key: " + key);
				throw new ProtoException("Internal error; a missing key occurred");
			}
			object obj = list[key];
			if (obj == null)
			{
				throw new ProtoException("A deferred key does not have a value yet");
			}
			return obj;
		}

		internal void SetKeyedObject(int key, object value)
		{
			if (key-- == 0)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (rootObject != null && rootObject != value)
				{
					throw new ProtoException("The root object cannot be reassigned");
				}
				rootObject = value;
				return;
			}
			MutableList list = List;
			if (key < list.Count)
			{
				object obj = list[key];
				if (obj == null)
				{
					list[key] = value;
				}
				else if (!object.ReferenceEquals(obj, value))
				{
					throw new ProtoException("Reference-tracked objects cannot change reference");
				}
			}
			else if (key != list.Add(value))
			{
				throw new ProtoException("Internal error; a key mismatch occurred");
			}
		}

		internal int AddObjectKey(object value, out bool existing)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value == rootObject)
			{
				existing = true;
				return 0;
			}
			string text = value as string;
			BasicList list = List;
			int num;
			if (text == null)
			{
				if (objectKeys == null)
				{
					objectKeys = new ReferenceHashtable();
					num = -1;
				}
				else
				{
					object obj = objectKeys[value];
					num = ((obj == null) ? (-1) : ((int)obj));
				}
			}
			else if (stringKeys == null)
			{
				stringKeys = new Hashtable();
				num = -1;
			}
			else
			{
				object obj = stringKeys[text];
				num = ((obj == null) ? (-1) : ((int)obj));
			}
			if (!(existing = num >= 0))
			{
				num = list.Add(value);
				if (text == null)
				{
					objectKeys.Add(value, num);
				}
				else
				{
					stringKeys.Add(text, num);
				}
			}
			return num + 1;
		}

		internal void RegisterTrappedObject(object value)
		{
			if (rootObject == null)
			{
				rootObject = value;
			}
			else
			{
				if (underlyingList == null)
				{
					return;
				}
				for (int i = trapStartIndex; i < underlyingList.Count; i++)
				{
					trapStartIndex = i + 1;
					if (underlyingList[i] == null)
					{
						underlyingList[i] = value;
						break;
					}
				}
			}
		}

		internal void Clear()
		{
			trapStartIndex = 0;
			rootObject = null;
			if (underlyingList != null)
			{
				underlyingList.Clear();
			}
			if (stringKeys != null)
			{
				stringKeys.Clear();
			}
			if (objectKeys != null)
			{
				objectKeys.Clear();
			}
		}
	}
}
