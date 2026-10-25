using System;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class StatValueBase<T> : IReadonlyStatValueBase, IStatValueBase, IAcierDataSerializable where T : class, IStatValueWrapper, new()
	{
		protected static readonly T s_DefaultValue = new T();

		private readonly StatTypes.EStatType m_statType;

		private static readonly Type s_EnumValueType = ((s_DefaultValue is IEnumStatValueWrapper) ? ((IEnumStatValueWrapper)s_DefaultValue).EnumType : null);

		public static Type ValueTypeStatic
		{
			get
			{
				T val = s_DefaultValue;
				return val.ValueType;
			}
		}

		public Type ValueType
		{
			get
			{
				return ValueTypeStatic;
			}
		}

		public StatTypes.EStatType StatType
		{
			get
			{
				return m_statType;
			}
		}

		public StatTypes.EStatUnit Unit
		{
			get
			{
				return StatTypes.GetStatUnit(StatType);
			}
		}

		public StatTypes.EStatCategory Category
		{
			get
			{
				return StatTypes.GetStatCategory(StatType);
			}
		}

		public bool IsImmutable
		{
			get
			{
				return Category == StatTypes.EStatCategory.Immutable;
			}
		}

		public bool IsMutable
		{
			get
			{
				return Category == StatTypes.EStatCategory.Mutable;
			}
		}

		public bool IsComputed
		{
			get
			{
				return Category == StatTypes.EStatCategory.Computed;
			}
		}

		public bool IsScaled
		{
			get
			{
				return StatTypes.GetIsScaled(StatType);
			}
		}

		public Type EnumValueType
		{
			get
			{
				return s_EnumValueType;
			}
		}

		public abstract bool HasDefaultValue { get; }

		public bool IsWritingAllowed
		{
			get
			{
				return true;
			}
		}

		public virtual StatValueCollection StatValueCollection
		{
			get
			{
				return null;
			}
		}

		protected StatValueBase(StatTypes.EStatType _statType, StatValueCollection _statValueCollection)
		{
			m_statType = _statType;
			bool flag = StatType != _statType;
		}

		public abstract void ApplyBuff(IBuff _buff, IEntity _entity, float _payload);

		public abstract IReadonlyStatValueBase Clone(StatValueCollection _newStatValueCollection);

		public abstract void CopyFrom(IReadonlyStatValueBase _other);

		public abstract bool HasEqualValue(IReadonlyStatValueBase _other);

		public override bool Equals(object obj)
		{
			IReadonlyStatValueBase readonlyStatValueBase = obj as IReadonlyStatValueBase;
			if (readonlyStatValueBase == null)
			{
				return false;
			}
			return HasEqualValue(readonlyStatValueBase);
		}

		public static bool operator ==(StatValueBase<T> _this, object _other)
		{
			if (object.ReferenceEquals(_this, _other))
			{
				return true;
			}
			if (object.ReferenceEquals(_this, null))
			{
				return false;
			}
			return _this.Equals(_other);
		}

		public static bool operator !=(StatValueBase<T> _this, object _other)
		{
			return !(_this == _other);
		}
	}
}
