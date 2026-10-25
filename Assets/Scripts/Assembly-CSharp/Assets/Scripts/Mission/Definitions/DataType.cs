using System;
using Database;

namespace Assets.Scripts.Mission.Definitions
{
	public abstract class DataType<TType, TDefinition> : IDataType, IComparable<TType> where TType : DataType<TType, TDefinition>, IDataType where TDefinition : class, Data.IDefinition
	{
		private readonly GameID m_gameID;

		public GameID GameID
		{
			get
			{
				return m_gameID;
			}
		}

		public TDefinition Definition
		{
			get
			{
				return DataManager.GetDefinition((TType)this);
			}
		}

		public bool IsValid
		{
			get
			{
				return GameID.IsValid(m_gameID) && DataManager.IsValid((TType)this);
			}
		}

		protected abstract IDataManager<TType, TDefinition> DataManager { get; }

		protected DataType(GameID _gameID)
		{
			m_gameID = _gameID;
		}

		public override int GetHashCode()
		{
			return m_gameID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj is DataType<TType, TDefinition>)
			{
				DataType<TType, TDefinition> dataType = (DataType<TType, TDefinition>)obj;
				return m_gameID == dataType.m_gameID;
			}
			return base.Equals(obj);
		}

		public int CompareTo(TType other)
		{
			return ((IDataType)other).GameID.CompareTo(m_gameID);
		}

		public static explicit operator GameID(DataType<TType, TDefinition> _type)
		{
			return _type.m_gameID;
		}

		public static bool operator ==(DataType<TType, TDefinition> _left, DataType<TType, TDefinition> _right)
		{
			bool flag = object.Equals(null, _left);
			bool flag2 = object.Equals(null, _right);
			if (flag && flag2)
			{
				return true;
			}
			if (flag || flag2)
			{
				return false;
			}
			return _left.m_gameID == _right.m_gameID;
		}

		public static bool operator !=(DataType<TType, TDefinition> _left, DataType<TType, TDefinition> _right)
		{
			return !(_left == _right);
		}
	}
}
