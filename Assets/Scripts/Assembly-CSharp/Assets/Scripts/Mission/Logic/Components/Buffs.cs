using System;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using UbiCore.Debug;
using UbiCore.Messenger;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class Buffs : TurnUpdateableComponent
	{
		public delegate void BuffEventHandler(BuffEvent _event);

		private readonly BuffsCollection m_Buffs;

		public sealed override int Priority
		{
			get
			{
				return 34;
			}
		}

		public ReadOnlyCollection<IBuff> SortedBuffs
		{
			get
			{
				return m_Buffs.SortedBuffs;
			}
		}

		private event BuffEventHandler m_OnBuffEvent;

		public event BuffEventHandler OnBuffEvent
		{
			add
			{
				this.m_OnBuffEvent = (BuffEventHandler)Delegate.Combine(this.m_OnBuffEvent, value);
			}
			remove
			{
				this.m_OnBuffEvent = (BuffEventHandler)Delegate.Remove(this.m_OnBuffEvent, value);
			}
		}

		public Buffs(IEntity _entity)
			: base(_entity)
		{
			m_Buffs = new BuffsCollection(InvokeBuffEvent);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("Buffs::DoTurnUpdate()", float.MaxValue);
			m_Buffs.DoTurnUpdate(_deltaTurns);
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public IBuff AddBuff(IBuff _buff)
		{
			m_Buffs.AddBuff(_buff);
			BuffData.IBuffDefinition definition = _buff.Type.GetDefinition();
			if (definition != null && definition.TriggerCharacterConditionChange)
			{
				Entity.Cast<IBuffsCarrier>().TriggerCharacterConditionChange();
			}
			if (_buff.StatType != StatTypes.EStatType.Invalid && StatTypes.GetStatUpdateType(_buff.StatType) == StatTypes.EStatUpdateType.Explicit)
			{
				Entity.Cast<IEntityStatsCarrier>().EntityStats.ScheduleExplicitStatsUpdate();
			}
			if (Entity is EnemyCharacter && _buff.Type == EBuffType.Shredded)
			{
				LogicMain.MissionResult.IncreaseEnemiesShreddedCount();
			}
			return _buff;
		}

		public bool RemoveBuff(IBuff _buff)
		{
			return m_Buffs.RemoveBuff(_buff);
		}

		public bool RemoveBuffs(StatTypes.EStatType _statType, int _maxCountToRemove)
		{
			return m_Buffs.RemoveBuffs(_statType, _maxCountToRemove);
		}

		public bool RemoveBuffs(EBuffType _buffType, int _maxCountToRemove)
		{
			return m_Buffs.RemoveBuffs(_buffType, _maxCountToRemove);
		}

		public bool RemoveBuffs(Predicate<IBuff> _predicate, int _maxCountToRemove)
		{
			return m_Buffs.RemoveBuffs(_predicate, _maxCountToRemove);
		}

		public bool HasBuffs(StatTypes.EStatType _statType)
		{
			return m_Buffs.HasBuffs(_statType);
		}

		public bool HasBuffs(EBuffType _buffType)
		{
			return m_Buffs.HasBuffs(_buffType);
		}

		public int GetLatestBuffDurationEnd(EBuffType _buffType)
		{
			return m_Buffs.GetLatestBuffDurationEnd(_buffType);
		}

		public int GetLatestBuffDurationEnd(StatTypes.EStatType _statType)
		{
			return m_Buffs.GetLatestBuffDurationEnd(_statType);
		}

		private void Clear()
		{
			m_Buffs.Clear();
		}

		private void InvokeBuffEvent(IBuff _buff, bool _removed)
		{
			BuffEvent buffEvent = new BuffEvent(_buff, Entity.Id, _removed);
			Messenger.Instance.Broadcast(buffEvent);
			BuffEventHandler onBuffEvent = this.m_OnBuffEvent;
			if (onBuffEvent != null)
			{
				onBuffEvent(buffEvent);
			}
		}
	}
}
