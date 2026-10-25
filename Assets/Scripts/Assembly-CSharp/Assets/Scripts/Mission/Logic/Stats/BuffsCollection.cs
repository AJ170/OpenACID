using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI.LogicInterface;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class BuffsCollection
	{
		public delegate void BuffEventHandler(IBuff _buff, bool _removed);

		private readonly BBList<IBuff> m_Buffs = new BBList<IBuff>();

		private readonly ReadOnlyCollection<IBuff> m_BuffAccess;

		private readonly BuffEventHandler m_BuffEventHandler;

		private bool m_DirtySorting;

		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		private static readonly StatValueWrapper_Bool s_StatValueWrapperBool = new StatValueWrapper_Bool();

		private static readonly StatValueWrapper_Int s_StatValueWrapperInt = new StatValueWrapper_Int();

		private static readonly StatValueWrapper_Float s_StatValueWrapperFloat = new StatValueWrapper_Float();

		public ReadOnlyCollection<IBuff> SortedBuffs
		{
			get
			{
				return m_BuffAccess;
			}
		}

		public BuffsCollection(BuffEventHandler _buffEventHandler)
		{
			m_BuffAccess = new ReadOnlyCollection<IBuff>(m_Buffs);
			m_BuffEventHandler = _buffEventHandler;
		}

		public void DoTurnUpdate(int _deltaTurns)
		{
			SortIfNeeded();
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				IBuff buff = m_Buffs[i];
				if (!buff.IsRunning)
				{
					RemoveBuffAtInternal(i);
					InvokeBuffEvent(buff, true);
					i--;
				}
				else if (buff.Type == EBuffType.Frightened || buff.Type == EBuffType.Fleeing)
				{
					LogicMain.MissionResult.IncreaseFrightenedTurnCount();
				}
			}
		}

		private void SortIfNeeded()
		{
			if (m_DirtySorting)
			{
				m_Buffs.Sort((IBuff left, IBuff right) => left.CompareTo(right));
				m_DirtySorting = false;
			}
		}

		public IBuff AddBuff(IBuff _buff)
		{
			AddBuffInternal(_buff);
			InvokeBuffEvent(_buff, false);
			m_DirtySorting = true;
			return _buff;
		}

		public bool RemoveBuff(IBuff _buff)
		{
			bool flag = RemoveBuffInternal(_buff);
			if (flag)
			{
				_buff.Abort();
				InvokeBuffEvent(_buff, true);
			}
			return flag;
		}

		public bool RemoveBuffs(StatTypes.EStatType _statType, int _maxCountToRemove)
		{
			bool result = false;
			int num = 0;
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (num >= _maxCountToRemove)
				{
					break;
				}
				if (m_Buffs[i].StatType == _statType)
				{
					IBuff buff = m_Buffs[i];
					buff.Abort();
					RemoveBuffAtInternal(i);
					i--;
					result = true;
					InvokeBuffEvent(buff, true);
					num++;
				}
			}
			return result;
		}

		public bool RemoveBuffs(EBuffType _buffType, int _maxCountToRemove)
		{
			bool result = false;
			int num = 0;
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (num >= _maxCountToRemove)
				{
					break;
				}
				if (m_Buffs[i].Type == _buffType)
				{
					IBuff buff = m_Buffs[i];
					buff.Abort();
					RemoveBuffAtInternal(i);
					i--;
					result = true;
					InvokeBuffEvent(buff, true);
					num++;
				}
			}
			return result;
		}

		public bool RemoveBuffs(Predicate<IBuff> _predicate, int _maxCountToRemove)
		{
			bool result = false;
			int num = 0;
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (num >= _maxCountToRemove)
				{
					break;
				}
				if (_predicate(m_Buffs[i]))
				{
					IBuff buff = m_Buffs[i];
					buff.Abort();
					RemoveBuffAtInternal(i);
					i--;
					result = true;
					InvokeBuffEvent(buff, true);
					num++;
				}
			}
			return result;
		}

		public virtual bool HasBuffs(StatTypes.EStatType _statType)
		{
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].StatType == _statType)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool HasBuffs(EBuffType _buffType)
		{
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].Type == _buffType)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetBuffs(StatTypes.EStatType _statType, BBList<IBuff> _result)
		{
			bool result = false;
			if (_result != null)
			{
				_result.ClearIfNotEmpty();
				SortIfNeeded();
			}
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].StatType == _statType)
				{
					result = true;
					if (_result == null)
					{
						break;
					}
					_result.Add(m_Buffs[i]);
				}
			}
			return result;
		}

		public virtual bool GetBuffs(EBuffType _buffType, BBList<IBuff> _result)
		{
			bool result = false;
			if (_result != null)
			{
				_result.ClearIfNotEmpty();
				SortIfNeeded();
			}
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].Type == _buffType)
				{
					result = true;
					if (_result == null)
					{
						break;
					}
					_result.Add(m_Buffs[i]);
				}
			}
			return result;
		}

		public int GetLatestBuffDurationEnd(StatTypes.EStatType _statType)
		{
			int num = 0;
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].StatType == _statType)
				{
					if (m_Buffs[i].DurationType == EBuffDurationType.Infinite)
					{
						return int.MaxValue;
					}
					num = Math.Max(num, m_Buffs[i].TurnDuration + m_Buffs[i].StartTurn);
				}
			}
			return num;
		}

		public int GetLatestBuffDurationEnd(EBuffType _buffType)
		{
			int val = 0;
			for (int i = 0; i < m_Buffs.Count; i++)
			{
				if (m_Buffs[i].Type == _buffType)
				{
					if (m_Buffs[i].DurationType == EBuffDurationType.Infinite)
					{
						return int.MaxValue;
					}
					val = Math.Max(val, m_Buffs[i].TurnDuration + m_Buffs[i].StartTurn);
				}
			}
			return 0;
		}

		public int GetBuffed_int(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection)
		{
			int @int = _statValueCollection.Get_int(_statType);
			s_StatValueWrapperInt.Int = @int;
			ComputeBuffs(s_StatValueWrapperInt, _statType);
			return s_StatValueWrapperInt.Int;
		}

		public float GetBuffed_float(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection)
		{
			float @float = _statValueCollection.Get_float(_statType);
			s_StatValueWrapperFloat.Float = @float;
			ComputeBuffs(s_StatValueWrapperFloat, _statType);
			return s_StatValueWrapperFloat.Float;
		}

		public bool GetBuffed_bool(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection)
		{
			bool @bool = _statValueCollection.Get_bool(_statType);
			s_StatValueWrapperBool.Bool = @bool;
			ComputeBuffs(s_StatValueWrapperBool, _statType);
			return s_StatValueWrapperBool.Bool;
		}

		public void ComputeBuffs(IStatValueWrapper _sourceValue, StatTypes.EStatType _statType)
		{
			if (!GetBuffs(_statType, s_BuffList))
			{
				return;
			}
			float payload = 0f;
			int num = int.MinValue;
			for (int i = 0; i < s_BuffList.Count; i++)
			{
				BuffBase buffBase = (BuffBase)s_BuffList[i];
				if (buffBase.Priority != num)
				{
					INumberStatValueWrapper numberStatValueWrapper = _sourceValue as INumberStatValueWrapper;
					payload = ((numberStatValueWrapper == null) ? 0f : numberStatValueWrapper.Number);
					num = buffBase.Priority;
				}
				buffBase.Compute(_sourceValue, payload);
			}
		}

		public virtual void Clear()
		{
			ClearInternal(true);
		}

		protected IBuff GetBuffAt(int _idx)
		{
			return m_Buffs[_idx];
		}

		protected virtual void AddBuffInternal(IBuff _buff)
		{
			m_Buffs.Add(_buff);
		}

		protected virtual bool RemoveBuffInternal(IBuff _buff)
		{
			return m_Buffs.Remove(_buff);
		}

		protected virtual void RemoveBuffAtInternal(int _idx)
		{
			m_Buffs.RemoveAt(_idx);
		}

		protected void ClearInternal(bool _keepInfiniteBuffs)
		{
			List<IBuff> list = m_Buffs.ToList();
			for (int i = 0; i < list.Count; i++)
			{
				IBuff buff = list[i];
				if (!_keepInfiniteBuffs || buff.DurationType == EBuffDurationType.Finite)
				{
					m_Buffs.Remove(buff);
					buff.Abort();
					InvokeBuffEvent(buff, true);
				}
			}
		}

		private void InvokeBuffEvent(IBuff _buff, bool _removed)
		{
			if (m_BuffEventHandler != null)
			{
				m_BuffEventHandler(_buff, _removed);
			}
		}
	}
}
