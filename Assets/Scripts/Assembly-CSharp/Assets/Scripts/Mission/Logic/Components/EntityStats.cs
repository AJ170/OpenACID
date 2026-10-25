using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Debug;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class EntityStats : TurnAndFrameUpdateableComponent, IInstanceCarrier, IInstanceStatValueCollection, IEntityStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public static readonly IStatsHelper[] StatsHelpers = new IStatsHelper[1] { StatsHelper<EntityStatsHelper>.Instance };

		private readonly StatValueCollection m_StatValueCollection;

		public virtual float IncrementFactor
		{
			get
			{
				return 0f;
			}
		}

		public override int Priority
		{
			get
			{
				return 35;
			}
		}

		public bool ExplicitStatsAreDirty { get; private set; }

		protected EntityStats(IEntity _entity, IStatsHelper[] _helpers)
			: base(_entity)
		{
			m_StatValueCollection = new StatValueCollection(_helpers, StatValueCollection.EType.Instance, this);
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.Initialization;
			m_StatValueCollection.Assign(Entity.Definition.EntityDefinition.StatValueCollection);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
		}

		public override void OnActivate()
		{
			m_StatValueCollection.InitializeMutable(this, this);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.Initialization;
			m_StatValueCollection.UpdateComputedFrame(this, this, TimeSinceFrameUpdate);
			ApplyBuffs(StatTypes.EStatUpdateType.Framewise);
			m_StatValueCollection.UpdateComputedTurn(this, this, TurnsSinceTurnUpdate);
			ApplyBuffs(StatTypes.EStatUpdateType.Turnwise);
			m_StatValueCollection.UpdateComputedExplicit(this, this);
			ApplyBuffs(StatTypes.EStatUpdateType.Explicit);
			m_StatValueCollection.UpdateComputedExplicit(this, this);
			ApplyBuffs(StatTypes.EStatUpdateType.Explicit);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
		}

		public void AddListener(StatValueCollection.StatValueChangedEventHandler _listener)
		{
			m_StatValueCollection.StatValueChanged += _listener;
		}

		public void RemoveListener(StatValueCollection.StatValueChangedEventHandler _listener)
		{
			m_StatValueCollection.StatValueChanged -= _listener;
		}

		public void Copy(StatTypes.EStatType _src, StatTypes.EStatType _dst)
		{
			m_StatValueCollection.Copy(_src, _dst);
		}

		public void Set_float(StatTypes.EStatType _dst, float _value)
		{
			m_StatValueCollection.Set_float(_dst, _value);
		}

		public void Set_int(StatTypes.EStatType _dst, int _value)
		{
			m_StatValueCollection.Set_int(_dst, _value);
		}

		public void Set_EntityId(StatTypes.EStatType _dst, EntityId _value)
		{
			m_StatValueCollection.Set_EntityId(_dst, _value);
		}

		public void Set_bool(StatTypes.EStatType _dst, bool _value)
		{
			m_StatValueCollection.Set_bool(_dst, _value);
		}

		public void Set_string(StatTypes.EStatType _dst, string _value)
		{
			m_StatValueCollection.Set_string(_dst, _value);
		}

		public void Set_Vector3(StatTypes.EStatType _dst, Vector3 _value)
		{
			m_StatValueCollection.Set_Vector3(_dst, _value);
		}

		public float Get_float(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_float(_src);
		}

		public int Get_int(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_int(_src);
		}

		public EntityId Get_EntityId(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_EntityId(_src);
		}

		public bool Get_bool(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_bool(_src);
		}

		public Vector3 Get_Vector3(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_Vector3(_src);
		}

		public string Get_String(StatTypes.EStatType _src)
		{
			return m_StatValueCollection.Get_String(_src);
		}

		public float AddFloat(StatTypes.EStatType _dst, StatTypes.EStatType _src)
		{
			return m_StatValueCollection.AddFloat(_dst, _src);
		}

		public float AddFloat(StatTypes.EStatType _dst, float _value)
		{
			return m_StatValueCollection.AddFloat(_dst, _value);
		}

		public int AddInt(StatTypes.EStatType _dst, StatTypes.EStatType _src)
		{
			return m_StatValueCollection.AddInt(_dst, _src);
		}

		public int AddInt(StatTypes.EStatType _dst, int _value)
		{
			return m_StatValueCollection.AddInt(_dst, _value);
		}

		public IReadonlyStatValueBase GetReadonlyStatValueBase(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetReadonlyStatValueBase(_statType);
		}

		public IReadonlyStatValue GetReadonlyStatValue(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetReadonlyStatValue(_statType);
		}

		public IReadonlyStatCollectionValue GetReadonlyStatCollectionValue(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetReadonlyStatCollectionValue(_statType);
		}

		public IStatValueBase GetStatValueBase(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetStatValueBase(_statType);
		}

		public IStatValue GetStatValue(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetStatValue(_statType);
		}

		public IStatCollectionValue GetStatCollectionValue(StatTypes.EStatType _statType)
		{
			return m_StatValueCollection.GetStatCollectionValue(_statType);
		}

		public bool HasStat(StatTypes.EStatType _type)
		{
			return m_StatValueCollection.HasStat(_type);
		}

		public bool IsWritingAllowed(StatTypes.EStatType _type)
		{
			return m_StatValueCollection.IsWritingAllowed(_type);
		}

		public void ScheduleExplicitStatsUpdate()
		{
			ExplicitStatsAreDirty = true;
		}

		public sealed override void DoFrameUpdate(float _timeDelta)
		{
			Profiling.Start("EntityStats::DoFrameUpdate()", float.MaxValue);
			UpdateComputedStatsFrame(_timeDelta);
			if (ExplicitStatsAreDirty)
			{
				UpdateComputedStatsExplicit();
			}
			base.DoFrameUpdate(_timeDelta);
			Profiling.Stop();
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			Profiling.Start("EntityStats::DoTurnUpdate()", float.MaxValue);
			UpdateComputedStatsTurn(_deltaTurns);
			base.DoTurnUpdate(_deltaTurns);
			Profiling.Stop();
		}

		public void UpdateComputedStatsExplicit()
		{
			ExplicitStatsAreDirty = false;
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteComputedExplicit;
			m_StatValueCollection.UpdateComputedExplicit(this, this);
			ApplyBuffs(StatTypes.EStatUpdateType.Explicit);
			m_StatValueCollection.UpdateComputedExplicit(this, this);
			ApplyBuffs(StatTypes.EStatUpdateType.Explicit);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
		}

		private void UpdateComputedStatsFrame(float _timeDelta)
		{
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteComputed;
			m_StatValueCollection.UpdateComputedFrame(this, this, _timeDelta);
			ApplyBuffs(StatTypes.EStatUpdateType.Framewise);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
		}

		private void UpdateComputedStatsTurn(int _deltaTurns)
		{
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteComputed;
			m_StatValueCollection.UpdateComputedTurn(this, this, _deltaTurns);
			ApplyBuffs(StatTypes.EStatUpdateType.Turnwise);
			m_StatValueCollection.AccessMode = StatValueCollection.EAccessMode.WriteMutable;
		}

		private void ApplyBuffs(StatTypes.EStatUpdateType _updateType)
		{
			IBuffsCarrier buffsCarrier = Entity as IBuffsCarrier;
			if (buffsCarrier == null)
			{
				return;
			}
			IBuff buff = null;
			IStatValueBase statValueBase = null;
			StatTypes.EStatType eStatType = StatTypes.EStatType.Invalid;
			int num = int.MinValue;
			float payload = 0f;
			for (int i = 0; i < buffsCarrier.Buffs.SortedBuffs.Count; i++)
			{
				buff = buffsCarrier.Buffs.SortedBuffs[i];
				if (!buff.IsRunning)
				{
					continue;
				}
				StatTypes.EStatUpdateType statUpdateType = StatTypes.GetStatUpdateType(buff.StatType);
				if (statUpdateType == _updateType)
				{
					statValueBase = m_StatValueCollection.GetStatValueBase(buff.StatType);
					IStatValue statValue = statValueBase as IStatValue;
					if (statValue == null)
					{
						payload = 0f;
					}
					else if (buff.StatType != eStatType || buff.Priority != num)
					{
						payload = statValue.GetPayload();
						eStatType = buff.StatType;
						num = buff.Priority;
					}
					statValueBase.ApplyBuff(buff, Entity, payload);
				}
			}
		}
	}
}
