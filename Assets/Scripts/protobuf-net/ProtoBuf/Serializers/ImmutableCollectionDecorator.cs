#define DEBUG
using System;
using System.Collections;
using System.Reflection;
using ProtoBuf.Compiler;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class ImmutableCollectionDecorator : ListDecorator
	{
		private readonly MethodInfo builderFactory;

		private readonly MethodInfo add;

		private readonly MethodInfo addRange;

		private readonly MethodInfo finish;

		protected override bool RequireAdd
		{
			get
			{
				return false;
			}
		}

		internal ImmutableCollectionDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull, MethodInfo builderFactory, MethodInfo add, MethodInfo addRange, MethodInfo finish)
			: base(model, declaredType, concreteType, tail, fieldNumber, writePacked, packedWireType, returnList, overwriteList, supportNull)
		{
			this.builderFactory = builderFactory;
			this.add = add;
			this.addRange = addRange;
			this.finish = finish;
		}

		public override object Read(object value, ProtoReader source)
		{
			object obj = builderFactory.Invoke(null, null);
			int field = source.FieldNumber;
			object[] array = new object[1];
			if (base.AppendToCollection && value != null && ((IList)value).Count != 0)
			{
				if (addRange != null)
				{
					array[0] = value;
					addRange.Invoke(obj, array);
				}
				else
				{
					foreach (object item in (IList)value)
					{
						array[0] = item;
						add.Invoke(obj, array);
					}
				}
			}
			if (packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				while (ProtoReader.HasSubValue(packedWireType, source))
				{
					array[0] = Tail.Read(null, source);
					add.Invoke(obj, array);
				}
				ProtoReader.EndSubItem(token, source);
			}
			else
			{
				do
				{
					array[0] = Tail.Read(null, source);
					add.Invoke(obj, array);
				}
				while (source.TryReadFieldHeader(field));
			}
			return finish.Invoke(obj, null);
		}

		protected override void EmitRead(CompilerContext ctx, Local valueFrom)
		{
			using (Local local2 = (base.AppendToCollection ? ctx.GetLocalWithValue(ExpectedType, valueFrom) : null))
			{
				using (Local local = new Local(ctx, builderFactory.ReturnType))
				{
					ctx.EmitCall(builderFactory);
					ctx.StoreValue(local);
					if (base.AppendToCollection)
					{
						CodeLabel label = ctx.DefineLabel();
						if (!ExpectedType.IsValueType)
						{
							ctx.LoadValue(local2);
							ctx.BranchIfFalse(label, false);
						}
						PropertyInfo property = Helpers.GetProperty(ExpectedType, "Length", false);
						if (property == null)
						{
							property = Helpers.GetProperty(ExpectedType, "Count", false);
						}
						ctx.LoadAddress(local2, local2.Type);
						ctx.EmitCall(Helpers.GetGetMethod(property, false, false));
						ctx.BranchIfFalse(label, false);
						Type type = ctx.MapType(typeof(void));
						if (addRange != null)
						{
							ctx.LoadValue(local);
							ctx.LoadValue(local2);
							ctx.EmitCall(addRange);
							if (addRange.ReturnType != null && add.ReturnType != type)
							{
								ctx.DiscardValue();
							}
						}
						else
						{
							MethodInfo moveNext;
							MethodInfo current;
							MethodInfo enumeratorInfo = GetEnumeratorInfo(ctx.Model, out moveNext, out current);
							Helpers.DebugAssert(moveNext != null);
							Helpers.DebugAssert(current != null);
							Helpers.DebugAssert(enumeratorInfo != null);
							Type returnType = enumeratorInfo.ReturnType;
							using (Local local3 = new Local(ctx, returnType))
							{
								ctx.LoadAddress(local2, ExpectedType);
								ctx.EmitCall(enumeratorInfo);
								ctx.StoreValue(local3);
								using (ctx.Using(local3))
								{
									CodeLabel label2 = ctx.DefineLabel();
									CodeLabel label3 = ctx.DefineLabel();
									ctx.Branch(label3, false);
									ctx.MarkLabel(label2);
									ctx.LoadAddress(local, local.Type);
									ctx.LoadAddress(local3, returnType);
									ctx.EmitCall(current);
									ctx.EmitCall(add);
									if (add.ReturnType != null && add.ReturnType != type)
									{
										ctx.DiscardValue();
									}
									ctx.MarkLabel(label3);
									ctx.LoadAddress(local3, returnType);
									ctx.EmitCall(moveNext);
									ctx.BranchIfTrue(label2, false);
								}
							}
						}
						ctx.MarkLabel(label);
					}
					ListDecorator.EmitReadList(ctx, local, Tail, add, packedWireType, false);
					ctx.LoadAddress(local, local.Type);
					ctx.EmitCall(finish);
					if (ExpectedType != finish.ReturnType)
					{
						ctx.Cast(ExpectedType);
					}
				}
			}
		}
	}
}
