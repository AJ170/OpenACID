using System;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI.LogicInterface;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public abstract class BuffBase : ICloneable, IComparable<IBuff>, IBuff
	{
		private readonly EBuffType m_Type;

		private readonly StatTypes.EStatType m_StatType;

		private readonly int m_StartTurn;

		private readonly EBuffDurationType m_DurationType;

		private readonly int m_TurnDuration;

		private readonly int m_TurnInterval;

		private int m_TurnLastExecution;

		private bool m_Aborted;

		public EBuffType Type
		{
			get
			{
				return m_Type;
			}
		}

		public BuffData.IBuffDefinition Definition
		{
			get
			{
				return UbiCore.Utils.Singleton<BuffData>.Instance.GetDefinition(Type);
			}
		}

		public EBuffDurationType DurationType
		{
			get
			{
				return m_DurationType;
			}
		}

		public StatTypes.EStatType StatType
		{
			get
			{
				return m_StatType;
			}
		}

		public int StartTurn
		{
			get
			{
				return m_StartTurn;
			}
		}

		public int TurnDuration
		{
			get
			{
				return m_TurnDuration;
			}
		}

		public int TurnInterval
		{
			get
			{
				return m_TurnInterval;
			}
		}

		public bool IsRunning
		{
			get
			{
				return !m_Aborted && (m_DurationType == EBuffDurationType.Infinite || LogicMain.CurrentTurn - m_StartTurn <= m_TurnDuration);
			}
		}

		public abstract int Priority { get; }

		public Func<IEntity, IBuff, bool> IsActivePredicate { get; set; }

		protected BuffBase(EBuffType _buffType, int _turnDuration, int _turnInterval)
			: this(_buffType, EBuffDurationType.Finite, _turnDuration, _turnInterval)
		{
		}

		protected BuffBase(EBuffType _buffType)
			: this(_buffType, EBuffDurationType.Infinite, 0, 0)
		{
		}

		protected BuffBase(EBuffType _buffType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _turnStartDelay = 0)
			: this(_buffType, StatTypes.EStatType.Invalid, _durationType, _turnDuration, _turnInterval, _turnStartDelay)
		{
			m_StatType = Definition.StatType;
		}

		protected BuffBase(StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _turnStartDelay = 0)
			: this(EBuffType.Invalid, _statType, _durationType, _turnDuration, _turnInterval, _turnStartDelay)
		{
		}

		protected BuffBase(EBuffType _buffType, StatTypes.EStatType _statType, EBuffDurationType _durationType, int _turnDuration, int _turnInterval, int _turnStartDelay = 0)
		{
			m_Type = _buffType;
			m_StatType = _statType;
			m_StartTurn = LogicMain.CurrentTurn + _turnInterval + _turnStartDelay;
			m_DurationType = _durationType;
			m_TurnDuration = _turnDuration;
			m_TurnInterval = _turnInterval;
			m_TurnLastExecution = m_StartTurn;
		}

		public bool GetIsActive(IEntity _carrier)
		{
			return LogicMain.CurrentTurn >= m_StartTurn && IsRunning && (IsActivePredicate == null || IsActivePredicate(_carrier, this));
		}

		public void Abort()
		{
			m_Aborted = true;
		}

		public abstract void Compute(IStatValueWrapper _source, float _payload);

		public bool Apply(IEntity _entity, IStatValueWrapper _wrapper, float _payload)
		{
			int num = LogicMain.CurrentTurn - m_TurnLastExecution;
			if (num >= m_TurnInterval)
			{
				m_TurnLastExecution = LogicMain.CurrentTurn;
				if (GetIsActive(_entity))
				{
					Compute(_wrapper, _payload);
					return true;
				}
			}
			return false;
		}

		public int CompareTo(IBuff other)
		{
			int num = StatType - other.StatType;
			if (num == 0)
			{
				num = Priority - other.Priority;
			}
			return num;
		}

		public abstract object Clone();
	}
}
