using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UbiCore.Debug;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class StatValueCollection : IReadonlyStatValueCollection, IStatValueCollection, IAcierDataSerializable, IAcierDataTemplateSerializable
	{
		public enum EAccessMode
		{
			Initialization = 0,
			WriteMutable = 1,
			WriteComputed = 2,
			WriteComputedExplicit = 3,
			Count = 4
		}

		public enum EType
		{
			Definition = 0,
			Instance = 1
		}

		public delegate void StatValueChangedEventHandler(IReadonlyStatValue _statValue, IStatValueWrapper _previousValueWrapper);

		private static string kStatValueCollection;

		private static string kStatValuesEnumType;

		private static string kType;

		private static string kElement;

		private EAccessMode m_AccessMode = EAccessMode.WriteMutable;

		private readonly IStatsHelper[] m_StatsHelpers;

		private readonly IManagedStatValueBase[] m_StatValues;

		private readonly EType m_Type;

		private readonly IReadonlyStatValueCollection m_Owner;

		private readonly int m_MinStatIndex = 387;

		private readonly int m_MaxStatIndex;

		private readonly int m_ArraySize;

		private static readonly bool[][] s_IsWritingAllowedCache;

		public EAccessMode AccessMode
		{
			get
			{
				return m_AccessMode;
			}
			set
			{
				m_AccessMode = value;
			}
		}

		public EType Type
		{
			get
			{
				return m_Type;
			}
		}

		public IReadonlyStatValueCollection Owner
		{
			get
			{
				return m_Owner;
			}
		}

		public float IncrementFactor
		{
			get
			{
				IInstanceStatValueCollection instanceStatValueCollection = m_Owner as IInstanceStatValueCollection;
				if (instanceStatValueCollection != null)
				{
					return instanceStatValueCollection.IncrementFactor;
				}
				return 0f;
			}
		}

		public event StatValueChangedEventHandler StatValueChanged
		{
			add
			{
				this.m_StatValueChanged = (StatValueChangedEventHandler)Delegate.Combine(this.m_StatValueChanged, value);
			}
			remove
			{
				this.m_StatValueChanged = (StatValueChangedEventHandler)Delegate.Remove(this.m_StatValueChanged, value);
			}
		}

		private event StatValueChangedEventHandler m_StatValueChanged;

		public StatValueCollection()
		{
		}

		public StatValueCollection(IStatsHelper[] _helpers, EType _type, IReadonlyStatValueCollection _owner)
		{
			Profiling.Start("StatValueCollection(IStatsHelper[],EType,IStatValueCollection)", float.MaxValue);
			m_StatsHelpers = _helpers;
			m_Type = _type;
			m_Owner = _owner;
			for (int i = 0; i < m_StatsHelpers.Length; i++)
			{
				if (!m_StatsHelpers[i].ImmutableRange.IsEmpty)
				{
					m_MinStatIndex = Math.Min(m_MinStatIndex, (int)(m_StatsHelpers[i].ImmutableRange.Start + 1));
					m_MaxStatIndex = Math.Max(m_MaxStatIndex, (int)(m_StatsHelpers[i].ImmutableRange.End - 1));
				}
				if (!m_StatsHelpers[i].MutableRange.IsEmpty)
				{
					m_MinStatIndex = Math.Min(m_MinStatIndex, (int)(m_StatsHelpers[i].MutableRange.Start + 1));
					m_MaxStatIndex = Math.Max(m_MaxStatIndex, (int)(m_StatsHelpers[i].MutableRange.End - 1));
				}
				if (!m_StatsHelpers[i].ComputedRange.IsEmpty)
				{
					m_MinStatIndex = Math.Min(m_MinStatIndex, (int)(m_StatsHelpers[i].ComputedRange.Start + 1));
					m_MaxStatIndex = Math.Max(m_MaxStatIndex, (int)(m_StatsHelpers[i].ComputedRange.End - 1));
				}
			}
			m_ArraySize = m_MaxStatIndex - m_MinStatIndex + 1;
			m_StatValues = new IManagedStatValueBase[m_ArraySize];
			m_AccessMode = EAccessMode.Initialization;
			for (int j = 0; j < StatTypes.StatValueTemplates.Count; j++)
			{
				StatTypes.EStatType eStatType = (StatTypes.EStatType)j;
				if (ComputeHasStat(eStatType) && (_type != EType.Instance || !StatTypes.IsImmutable(eStatType) || StatTypes.GetIsScaled(eStatType)))
				{
					m_StatValues[IndexOf(eStatType)] = (IManagedStatValueBase)StatTypes.StatValueTemplates[j].Clone(this);
				}
			}
			m_AccessMode = EAccessMode.WriteMutable;
			Profiling.Stop();
		}

		public StatValueCollection(IStatsHelper[] _helpers, IReadonlyStatValueCollection _owner, IReadonlyStatValueCollection _template)
			: this(_helpers, EType.Definition, _owner)
		{
			m_AccessMode = EAccessMode.Initialization;
			if (_template != null)
			{
				Assign(_template);
			}
			m_AccessMode = EAccessMode.WriteMutable;
		}

		static StatValueCollection()
		{
			kStatValueCollection = "StatValueCollection";
			kStatValuesEnumType = "StatType";
			kType = "Type";
			kElement = "Element";
			s_IsWritingAllowedCache = new bool[4][];
			for (int i = 0; i < 4; i++)
			{
				s_IsWritingAllowedCache[i] = new bool[387];
				EAccessMode eAccessMode = (EAccessMode)i;
				for (int j = 0; j < StatTypes.StatValueTemplates.Count; j++)
				{
					if (StatTypes.StatValueTemplates[j] != null)
					{
						bool flag = false;
						if (StatTypes.IsMutable((StatTypes.EStatType)j))
						{
							flag = eAccessMode == EAccessMode.WriteMutable || eAccessMode == EAccessMode.Initialization;
						}
						else if (StatTypes.IsImmutable((StatTypes.EStatType)j))
						{
							flag = eAccessMode == EAccessMode.Initialization;
						}
						else if (StatTypes.IsComputed((StatTypes.EStatType)j))
						{
							flag = ((StatTypes.GetStatUpdateType((StatTypes.EStatType)j) != StatTypes.EStatUpdateType.Explicit) ? (eAccessMode == EAccessMode.WriteComputed || eAccessMode == EAccessMode.Initialization) : (eAccessMode == EAccessMode.WriteComputedExplicit || eAccessMode == EAccessMode.Initialization));
						}
						s_IsWritingAllowedCache[i][j] = flag;
					}
				}
			}
		}

		public void Copy(StatTypes.EStatType _src, StatTypes.EStatType _dst)
		{
			GetStatValue(_dst).CopyFrom(GetReadonlyStatValue(_src));
		}

		public void Set_float(StatTypes.EStatType _dst, float _value)
		{
			GetStatValue(_dst).Value_float = _value;
		}

		public void Set_int(StatTypes.EStatType _dst, int _value)
		{
			GetStatValue(_dst).Value_int = _value;
		}

		public void Set_EntityId(StatTypes.EStatType _dst, EntityId _value)
		{
			GetStatValue(_dst).Value_EntityId = _value;
		}

		public void Set_bool(StatTypes.EStatType _dst, bool _value)
		{
			GetStatValue(_dst).Value_bool = _value;
		}

		public void Set_string(StatTypes.EStatType _dst, string _value)
		{
			GetStatValue(_dst).Value_string = _value;
		}

		public void Set_Vector3(StatTypes.EStatType _dst, Vector3 _value)
		{
			GetStatValue(_dst).Value_Vector3 = _value;
		}

		public float Get_float(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_float;
		}

		public int Get_int(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_int;
		}

		public EntityId Get_EntityId(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_EntityId;
		}

		public bool Get_bool(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_bool;
		}

		public Vector3 Get_Vector3(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_Vector3;
		}

		public string Get_String(StatTypes.EStatType _src)
		{
			return GetReadonlyStatValue(_src).Value_string;
		}

		public float AddFloat(StatTypes.EStatType _dst, StatTypes.EStatType _src)
		{
			return GetStatValue(_dst).Value_float += GetReadonlyStatValue(_src).Value_float;
		}

		public float AddFloat(StatTypes.EStatType _dst, float _value)
		{
			return GetStatValue(_dst).Value_float += _value;
		}

		public int AddInt(StatTypes.EStatType _dst, StatTypes.EStatType _src)
		{
			return GetStatValue(_dst).Value_int += GetReadonlyStatValue(_src).Value_int;
		}

		public int AddInt(StatTypes.EStatType _dst, int _value)
		{
			return GetStatValue(_dst).Value_int += _value;
		}

		public IReadonlyStatValueBase GetReadonlyStatValueBase(StatTypes.EStatType _statType)
		{
			return GetStatAt(_statType);
		}

		public IReadonlyStatValue GetReadonlyStatValue(StatTypes.EStatType _statType)
		{
			IManagedStatValueBase statAt = GetStatAt(_statType);
			return (IReadonlyStatValue)statAt;
		}

		public IStatValueBase GetStatValueBase(StatTypes.EStatType _statType)
		{
			return GetStatAt(_statType);
		}

		public IStatValue GetStatValue(StatTypes.EStatType _statType)
		{
			IManagedStatValueBase statAt = GetStatAt(_statType);
			if (!Assets.Scripts.Engine.Helpers.Verify(IsWritingAllowed(_statType)))
			{
				return null;
			}
			return (IStatValue)statAt;
		}

		public IReadonlyStatCollectionValue GetReadonlyStatCollectionValue(StatTypes.EStatType _statType)
		{
			IManagedStatValueBase statAt = GetStatAt(_statType);
			return (IReadonlyStatCollectionValue)statAt;
		}

		public IStatCollectionValue GetStatCollectionValue(StatTypes.EStatType _statType)
		{
			IManagedStatValueBase statAt = GetStatAt(_statType);
			if (!Assets.Scripts.Engine.Helpers.Verify(IsWritingAllowed(_statType)))
			{
				return null;
			}
			return (IStatCollectionValue)statAt;
		}

		public bool HasStat(StatTypes.EStatType _type)
		{
			return GetStatAt(_type) != null;
		}

		public bool IsWritingAllowed(StatTypes.EStatType _type)
		{
			return m_AccessMode == EAccessMode.Initialization || (s_IsWritingAllowedCache[(int)m_AccessMode][(int)_type] && (LogicMain.IsInTurnUpdate || StatTypes.GetStatUpdateType(_type) != StatTypes.EStatUpdateType.Turnwise));
		}

		public void Reset()
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.Initialization))
			{
				return;
			}
			for (int i = 0; i < m_StatValues.Length; i++)
			{
				if (m_StatValues[i] != null)
				{
					m_StatValues[i].Reset();
				}
			}
		}

		public void Assign(IReadonlyStatValueCollection _source)
		{
			Profiling.Start("StatValueCollection::Assign", float.MaxValue);
			if (Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.Initialization))
			{
				IReadonlyStatValueBase readonlyStatValueBase = null;
				for (int i = 0; i < m_StatValues.Length; i++)
				{
					StatTypes.EStatType eStatType = (StatTypes.EStatType)(i + m_MinStatIndex);
					if (!_source.HasStat(eStatType) || !ComputeHasStat(eStatType))
					{
						continue;
					}
					readonlyStatValueBase = _source.GetReadonlyStatValueBase(eStatType);
					if (Type == EType.Instance && readonlyStatValueBase.IsImmutable && !readonlyStatValueBase.IsScaled)
					{
						m_StatValues[i] = (IManagedStatValueBase)readonlyStatValueBase;
						if (eStatType != StatTypes.EStatType.Character_Conflict_BaseHealth)
						{
						}
					}
					else
					{
						m_StatValues[i].CopyFrom(readonlyStatValueBase);
					}
				}
			}
			Profiling.Stop();
		}

		public void Serialize(DataSerializerBase _s, StatValueCollection _template)
		{
			if (_s.Mode == EMode.EMode_Write)
			{
				BBList<IManagedStatValueBase> bBList = new BBList<IManagedStatValueBase>();
				for (int i = 0; i < m_StatValues.Length; i++)
				{
					IManagedStatValueBase managedStatValueBase = m_StatValues[i];
					if (managedStatValueBase != null && (managedStatValueBase.Category == StatTypes.EStatCategory.Immutable || managedStatValueBase.Category == StatTypes.EStatCategory.Mutable))
					{
						bool forcedExport = StatTypes.GetForcedExport(managedStatValueBase.StatType);
						IManagedStatValueBase managedStatValueBase2 = null;
						if (_template != null && !forcedExport)
						{
							managedStatValueBase2 = _template.GetStatAt(managedStatValueBase.StatType);
						}
						if ((managedStatValueBase2 == null && !managedStatValueBase.HasDefaultValue) || (managedStatValueBase2 != null && !managedStatValueBase.HasEqualValue(managedStatValueBase2)) || forcedExport)
						{
							bBList.Add(managedStatValueBase);
						}
					}
				}
				_s.SerializeBeginNode(kStatValueCollection);
				_s.SerializeItemCount(bBList.Count);
				for (int j = 0; j < bBList.Count; j++)
				{
					_s.SerializeBeginNode(kElement);
					_s.SerializeEnum(kStatValuesEnumType, bBList[j].StatType.GetType(), (int)bBList[j].StatType);
					_s.SerializeString(kType, bBList[j].GetType().Name);
					bBList[j].Serialize("StatValue", _s);
					_s.SerializeEndNode();
				}
				_s.SerializeEndNode();
			}
			else
			{
				m_AccessMode = EAccessMode.Initialization;
				_s.SerializeBeginNode(kStatValueCollection);
				int num = _s.SerializeItemCount();
				for (int k = 0; k < num; k++)
				{
					_s.SerializeBeginNode(kElement);
					StatTypes.EStatType eStatType = (StatTypes.EStatType)_s.SerializeEnum(kStatValuesEnumType, StatTypes.EStatType.__Start_Immutable_Rule.GetType(), 0);
					_s.SerializeString(kType);
					IManagedStatValueBase managedStatValueBase3 = (IManagedStatValueBase)StatTypes.StatValueTemplates[(int)eStatType].Clone(this);
					m_StatValues[IndexOf(eStatType)] = managedStatValueBase3;
					managedStatValueBase3.Serialize("StatValue", _s);
					_s.SerializeEndNode();
				}
				_s.SerializeEndNode();
				m_AccessMode = EAccessMode.WriteMutable;
			}
		}

		public void OnValueChanged<T>(IReadonlyStatValue _statValue, T _previousValueWrapper) where T : class, IStatValueWrapper, new()
		{
			if (Type == EType.Instance && (AccessMode == EAccessMode.WriteMutable || AccessMode == EAccessMode.Initialization) && StatTypes.GetFireEventOnChange(_statValue.StatType) && !_statValue.HasEqualValue(_previousValueWrapper))
			{
				StatValueChangedEventHandler statValueChanged = this.m_StatValueChanged;
				if (statValueChanged != null)
				{
					statValueChanged(_statValue, _previousValueWrapper);
				}
				Messenger.Instance.Broadcast(new StatValueChangedEvent(m_Owner.Cast<EntityStats>().Entity, _statValue, _previousValueWrapper));
			}
		}

		public void UpdateComputedFrame(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, float _timeDelta)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.WriteComputed || AccessMode == EAccessMode.Initialization))
			{
				for (int i = 0; i < m_StatsHelpers.Length; i++)
				{
					m_StatsHelpers[i].UpdateComputedFrame(_stats, _instanceCarrier, _timeDelta);
				}
			}
		}

		public void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.WriteComputed || AccessMode == EAccessMode.Initialization))
			{
				for (int i = 0; i < m_StatsHelpers.Length; i++)
				{
					m_StatsHelpers[i].UpdateComputedTurn(_stats, _instanceCarrier, _deltaTurns);
				}
			}
		}

		public void UpdateComputedExplicit(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.WriteComputedExplicit || AccessMode == EAccessMode.Initialization))
			{
				for (int i = 0; i < m_StatsHelpers.Length; i++)
				{
					m_StatsHelpers[i].UpdateComputedExplicit(_stats, _instanceCarrier);
				}
			}
		}

		public void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(AccessMode == EAccessMode.WriteMutable))
			{
				AccessMode = EAccessMode.Initialization;
				for (int i = 0; i < m_StatsHelpers.Length; i++)
				{
					m_StatsHelpers[i].InitializeMutable(_stats, _instanceCarrier);
				}
				AccessMode = EAccessMode.WriteMutable;
			}
		}

		private bool ComputeHasStat(StatTypes.EStatType _type)
		{
			for (int i = 0; i < m_StatsHelpers.Length; i++)
			{
				if (m_StatsHelpers[i].Contains(_type))
				{
					return true;
				}
			}
			return false;
		}

		private IManagedStatValueBase GetStatAt(int _index)
		{
			if (_index < 0 || _index >= m_ArraySize)
			{
				return null;
			}
			return m_StatValues[_index];
		}

		private IManagedStatValueBase GetStatAt(StatTypes.EStatType _statType)
		{
			return GetStatAt(IndexOf(_statType));
		}

		private int IndexOf(StatTypes.EStatType _statType)
		{
			return (int)(_statType - m_MinStatIndex);
		}
	}
}
