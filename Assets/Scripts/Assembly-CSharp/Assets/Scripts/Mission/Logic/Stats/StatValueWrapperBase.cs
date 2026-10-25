using System;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class StatValueWrapperBase<TWrapper> : IDataSerializable, IStatValueWrapper where TWrapper : class, IStatValueWrapper, new()
	{
		public virtual int Int
		{
			get
			{
				HandleInvalidSet("Int");
				return 0;
			}
			set
			{
				HandleInvalidSet("Int");
			}
		}

		public virtual float Float
		{
			get
			{
				HandleInvalidSet("Float");
				return 0f;
			}
			set
			{
				HandleInvalidSet("Float");
			}
		}

		public virtual bool Bool
		{
			get
			{
				HandleInvalidSet("Bool");
				return false;
			}
			set
			{
				HandleInvalidSet("Bool");
			}
		}

		public virtual string String
		{
			get
			{
				HandleInvalidSet("String");
				return string.Empty;
			}
			set
			{
				HandleInvalidSet("String");
			}
		}

		public virtual EntityId EntityId
		{
			get
			{
				HandleInvalidSet("EntityId");
				return EntityId.InvalidId;
			}
			set
			{
				HandleInvalidSet("EntityId");
			}
		}

		public virtual Vector3 Vector3
		{
			get
			{
				HandleInvalidSet("Vector3");
				return Vector3.zero;
			}
			set
			{
				HandleInvalidSet("Vector3");
			}
		}

		public virtual float Number
		{
			get
			{
				HandleInvalidSet("Number");
				return 0f;
			}
			set
			{
				HandleInvalidSet("Number");
			}
		}

		public abstract Type ValueType { get; }

		public TWrapper Casted(object _obj)
		{
			return (TWrapper)_obj;
		}

		public abstract IStatValueWrapper Clone();

		public abstract void CopyFrom(IStatValueWrapper _wrapper);

		public abstract void Serialize(string _name, DataSerializerBase _s, object _customData = null);

		public override bool Equals(object obj)
		{
			if (!(obj is StatValueWrapperBase<TWrapper>))
			{
				return false;
			}
			return true;
		}

		private void HandleInvalidGet(string _type)
		{
		}

		private void HandleInvalidSet(string _type)
		{
		}
	}
}
