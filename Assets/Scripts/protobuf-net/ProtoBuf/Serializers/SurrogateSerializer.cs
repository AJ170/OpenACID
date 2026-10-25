#define DEBUG
using System;
using System.Reflection;
using ProtoBuf.Compiler;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class SurrogateSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		private readonly Type forType;

		private readonly Type declaredType;

		private readonly MethodInfo toTail;

		private readonly MethodInfo fromTail;

		private IProtoTypeSerializer rootTail;

		public bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		public bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		public Type ExpectedType
		{
			get
			{
				return forType;
			}
		}

		bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		void IProtoTypeSerializer.EmitCallback(CompilerContext ctx, Local valueFrom, TypeModel.CallbackType callbackType)
		{
		}

		void IProtoTypeSerializer.EmitCreateInstance(CompilerContext ctx)
		{
			throw new NotSupportedException();
		}

		bool IProtoTypeSerializer.CanCreateInstance()
		{
			return false;
		}

		object IProtoTypeSerializer.CreateInstance(ProtoReader source)
		{
			throw new NotSupportedException();
		}

		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		public SurrogateSerializer(TypeModel model, Type forType, Type declaredType, IProtoTypeSerializer rootTail)
		{
			Helpers.DebugAssert(forType != null, "forType");
			Helpers.DebugAssert(declaredType != null, "declaredType");
			Helpers.DebugAssert(rootTail != null, "rootTail");
			Helpers.DebugAssert(rootTail.RequiresOldValue, "RequiresOldValue");
			Helpers.DebugAssert(!rootTail.ReturnsValue, "ReturnsValue");
			Helpers.DebugAssert(declaredType == rootTail.ExpectedType || Helpers.IsSubclassOf(declaredType, rootTail.ExpectedType));
			this.forType = forType;
			this.declaredType = declaredType;
			this.rootTail = rootTail;
			toTail = GetConversion(model, true);
			fromTail = GetConversion(model, false);
		}

		private static bool HasCast(TypeModel model, Type type, Type from, Type to, out MethodInfo op)
		{
			MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			Type type2 = null;
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.ReturnType != to)
				{
					continue;
				}
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length != 1 || parameters[0].ParameterType != from)
				{
					continue;
				}
				if (type2 == null)
				{
					type2 = model.MapType(typeof(ProtoConverterAttribute), false);
					if (type2 == null)
					{
						break;
					}
				}
				if (methodInfo.IsDefined(type2, true))
				{
					op = methodInfo;
					return true;
				}
			}
			foreach (MethodInfo methodInfo in methods)
			{
				if ((!(methodInfo.Name != "op_Implicit") || !(methodInfo.Name != "op_Explicit")) && methodInfo.ReturnType == to)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == from)
					{
						op = methodInfo;
						return true;
					}
				}
			}
			op = null;
			return false;
		}

		public MethodInfo GetConversion(TypeModel model, bool toTail)
		{
			Type to = (toTail ? declaredType : forType);
			Type from = (toTail ? forType : declaredType);
			MethodInfo op;
			if (HasCast(model, declaredType, from, to, out op) || HasCast(model, forType, from, to, out op))
			{
				return op;
			}
			throw new InvalidOperationException("No suitable conversion operator found for surrogate: " + forType.FullName + " / " + declaredType.FullName);
		}

		public void Write(object value, ProtoWriter writer)
		{
			rootTail.Write(toTail.Invoke(null, new object[1] { value }), writer);
		}

		public object Read(object value, ProtoReader source)
		{
			object[] array = new object[1] { value };
			value = toTail.Invoke(null, array);
			array[0] = rootTail.Read(value, source);
			return fromTail.Invoke(null, array);
		}

		void IProtoSerializer.EmitRead(CompilerContext ctx, Local valueFrom)
		{
			Helpers.DebugAssert(valueFrom != null);
			using (Local local = new Local(ctx, declaredType))
			{
				ctx.LoadValue(valueFrom);
				ctx.EmitCall(toTail);
				ctx.StoreValue(local);
				rootTail.EmitRead(ctx, local);
				ctx.LoadValue(local);
				ctx.EmitCall(fromTail);
				ctx.StoreValue(valueFrom);
			}
		}

		void IProtoSerializer.EmitWrite(CompilerContext ctx, Local valueFrom)
		{
			ctx.LoadValue(valueFrom);
			ctx.EmitCall(toTail);
			rootTail.EmitWrite(ctx, null);
		}
	}
}
