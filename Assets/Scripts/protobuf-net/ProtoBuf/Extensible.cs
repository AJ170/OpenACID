using System;
using System.Collections;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	public abstract class Extensible : IExtensible
	{
		private IExtension extensionObject;

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return GetExtensionObject(createIfMissing);
		}

		protected virtual IExtension GetExtensionObject(bool createIfMissing)
		{
			return GetExtensionObject(ref extensionObject, createIfMissing);
		}

		public static IExtension GetExtensionObject(ref IExtension extensionObject, bool createIfMissing)
		{
			if (createIfMissing && extensionObject == null)
			{
				extensionObject = new BufferExtension();
			}
			return extensionObject;
		}

		public static bool TryGetValue(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool allowDefinedTag, out object value)
		{
			value = null;
			bool result = false;
			foreach (object extendedValue in ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, true, allowDefinedTag))
			{
				value = extendedValue;
				result = true;
			}
			return result;
		}

		public static IEnumerable GetValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format)
		{
			return ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, false, false);
		}

		public static void AppendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
		}
	}
}
