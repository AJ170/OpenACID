using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public class TriggerSet
	{
		private readonly BBList<ITrigger>[] m_TriggersByType = new BBList<ITrigger>[42];

		private readonly BBList<ITrigger> m_Triggers = new BBList<ITrigger>(10);

		private readonly ReadOnlyCollection<ITrigger> m_TriggersAccess;

		public ReadOnlyCollection<ITrigger> Triggers
		{
			get
			{
				return m_TriggersAccess;
			}
		}

		public TriggerSet()
		{
			m_TriggersAccess = new ReadOnlyCollection<ITrigger>(m_Triggers);
			for (int i = 0; i < m_TriggersByType.Length; i++)
			{
				m_TriggersByType[i] = new BBList<ITrigger>(5);
			}
		}

		public T AddTrigger<T>(T _trigger) where T : ITrigger
		{
			m_Triggers.Add(_trigger);
			BBList<ITrigger> triggersByType = GetTriggersByType(_trigger.Type);
			if (!triggersByType.Contains(_trigger))
			{
				triggersByType.Add(_trigger);
			}
			return _trigger;
		}

		public bool RemoveTrigger(ITrigger _trigger)
		{
			BBList<ITrigger> triggersByType = GetTriggersByType(_trigger.Type);
			if (!triggersByType.Remove(_trigger))
			{
				return false;
			}
			return true;
		}

		public BBList<ITrigger> GetTriggersByType(ETriggerType _type)
		{
			return m_TriggersByType[(int)_type];
		}

		public bool HasTriggersFromType(ETriggerType _type)
		{
			return GetTriggersByType(_type).Count > 0;
		}

		public bool HasTrigger(ITrigger _trigger)
		{
			BBList<ITrigger> triggersByType = GetTriggersByType(_trigger.Type);
			return triggersByType.Contains(_trigger);
		}

		public bool HasTriggers(IList<ITrigger> _triggers)
		{
			for (int i = 0; i < _triggers.Count; i++)
			{
				ITrigger trigger = _triggers[i];
				if (!HasTrigger(trigger))
				{
					return false;
				}
			}
			return true;
		}

		public void Clear()
		{
			for (int i = 0; i < m_Triggers.Count; i++)
			{
				BBList<ITrigger> triggersByType = GetTriggersByType(m_Triggers[i].Type);
				triggersByType.ClearIfNotEmpty();
			}
			m_Triggers.ClearIfNotEmpty();
		}
	}
}
