using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Formations
{
	public abstract class AbstractFormation : IFormation
	{
		private const int LeaderSlot = 1;

		private int m_maxCount = 4;

		protected EntityId m_parentId = EntityId.InvalidId;

		protected Vector3[] m_positions;

		protected BBList<MemberInfo> m_entities;

		protected int[] m_entityIndexToSlot;

		private MemberInfo m_entityAtHindingSpot = new MemberInfo();

		private int[] m_slotToEntityIndex;

		private bool m_fixedLeaderSlot;

		private int m_demandFrameUpdateCount;

		public int MaxCount
		{
			get
			{
				return m_maxCount;
			}
			set
			{
				if (value != m_maxCount)
				{
					m_maxCount = value;
					ResetAndCalculateLocalPositions();
				}
			}
		}

		public int MemberCount
		{
			get
			{
				return m_entities.Count;
			}
		}

		public bool Full
		{
			get
			{
				return MemberCount >= MaxCount;
			}
		}

		public float MaxExtend { get; private set; }

		public float MaxDeviation { get; private set; }

		public BBList<MemberInfo> Entities
		{
			get
			{
				return m_entities;
			}
		}

		public EntityId EntityAtHidingSpot
		{
			get
			{
				return m_entityAtHindingSpot.m_id;
			}
			protected set
			{
				m_entityAtHindingSpot.Init(value);
			}
		}

		public MemberInfo EntityAtHidingSpotInfo
		{
			get
			{
				return m_entityAtHindingSpot;
			}
		}

		public void Init(EntityId _parentId, float _maxExtend, float _maxDeviation, bool _fixedLeaderSlot)
		{
			m_parentId = _parentId;
			MaxExtend = _maxExtend;
			MaxDeviation = _maxDeviation;
			m_fixedLeaderSlot = _fixedLeaderSlot;
			ResetAndCalculateLocalPositions();
		}

		public bool Add(EntityId _entityId)
		{
			bool _newlyAdded;
			return Add(_entityId, out _newlyAdded);
		}

		public bool Add(EntityId _entityId, out bool _newlyAdded)
		{
			_newlyAdded = false;
			bool flag = Contains(_entityId);
			MemberInfo _memberInfo = null;
			if (!flag && !Full)
			{
				_memberInfo = new MemberInfo();
				_memberInfo.Init(_entityId);
				m_entities.Add(_memberInfo);
				if (m_demandFrameUpdateCount > 0)
				{
					Assets.Scripts.Mission.Motion.WithoutAgent.Helper.DemandNavAgentSurrogateFrameUpdate(_entityId, true);
				}
				flag = true;
				_newlyAdded = true;
			}
			if (flag)
			{
				if (_memberInfo == null)
				{
					GetMemberInfo(_entityId, out _memberInfo);
				}
				_memberInfo.UpdateAddTurn();
			}
			return flag;
		}

		public void Remove(EntityId _entityId)
		{
			bool _newlyRemoved;
			Remove(_entityId, out _newlyRemoved, false);
		}

		public void Remove(EntityId _entityId, out bool _newlyRemoved, bool _lowPrio)
		{
			_newlyRemoved = false;
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				MemberInfo memberInfo = m_entities[i];
				if (memberInfo.m_id != _entityId)
				{
					continue;
				}
				bool flag = memberInfo.AddTurn == LogicMain.CurrentTurn;
				if (!flag || !_lowPrio)
				{
					if (flag)
					{
					}
					m_entities.RemoveAt(i);
					_newlyRemoved = true;
				}
				break;
			}
			if (_newlyRemoved && m_demandFrameUpdateCount > 0)
			{
				Assets.Scripts.Mission.Motion.WithoutAgent.Helper.DemandNavAgentSurrogateFrameUpdate(_entityId, false);
			}
		}

		public bool AddAtHidingSpot(EntityId _id)
		{
			if (EntityAtHidingSpot != EntityId.InvalidId)
			{
				return false;
			}
			EntityAtHidingSpot = _id;
			return true;
		}

		public bool RemoveFromHidingSpot(EntityId _id)
		{
			if (EntityAtHidingSpot == _id)
			{
				EntityAtHidingSpot = EntityId.InvalidId;
				return true;
			}
			return false;
		}

		public bool GetTargetWorldPosition(EntityId _entityId, out Vector3 _position)
		{
			_position = m_parentId.GameEntity.Position;
			if (!_entityId.IsValid)
			{
				return false;
			}
			if (_entityId == EntityAtHidingSpot)
			{
				GetHidingSpotTargetWorldPosition(out _position);
				return true;
			}
			int _entityIndex;
			if (!GetEntityIndex(_entityId, out _entityIndex))
			{
				return false;
			}
			GetTargetWorldPosition(_entityIndex, _entityId.GameEntity.IsAttached, out _position);
			return true;
		}

		public void GetHidingSpotTargetWorldPosition(out Vector3 _position)
		{
			_position = m_parentId.GameEntity.Position;
		}

		public Vector3 CalcCurrentMemberCenter()
		{
			int count = m_entities.Count;
			if (count == 0)
			{
				return m_parentId.GameEntity.Position;
			}
			Vector3 zero = Vector3.zero;
			for (int i = 0; i < count; i++)
			{
				EntityId id = m_entities[i].m_id;
				if (id != EntityId.InvalidId)
				{
					zero += id.GameEntity.Position;
				}
			}
			return zero / count;
		}

		public bool Contains(EntityId _entityId)
		{
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				MemberInfo memberInfo = m_entities[i];
				if (memberInfo.m_id == _entityId)
				{
					return true;
				}
			}
			return false;
		}

		public bool GetEntityIndex(EntityId _entityId, out int _entityIndex)
		{
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				MemberInfo memberInfo = m_entities[i];
				if (memberInfo.m_id == _entityId)
				{
					_entityIndex = i;
					return true;
				}
			}
			_entityIndex = -1;
			return false;
		}

		public void DemandMemberFrameUpdate(bool _demand)
		{
			bool flag = m_demandFrameUpdateCount > 0;
			m_demandFrameUpdateCount = ((!_demand) ? (m_demandFrameUpdateCount - 1) : (m_demandFrameUpdateCount + 1));
			bool flag2 = m_demandFrameUpdateCount > 0;
			if (flag != flag2)
			{
				int count = m_entities.Count;
				for (int i = 0; i < count; i++)
				{
					EntityId id = m_entities[i].m_id;
					Assets.Scripts.Mission.Motion.WithoutAgent.Helper.DemandNavAgentSurrogateFrameUpdate(id, _demand);
				}
			}
		}

		protected void ResetAndCalculateLocalPositions()
		{
			if (m_entities == null)
			{
				m_entities = new BBList<MemberInfo>(MaxCount);
			}
			m_positions = new Vector3[MaxCount];
			m_slotToEntityIndex = new int[MaxCount];
			m_entityIndexToSlot = new int[MaxCount];
			for (int i = 0; i < MaxCount; i++)
			{
				m_entityIndexToSlot[i] = i;
			}
			CalculateLocalPositions();
		}

		protected abstract void CalculateLocalPositions();

		public void GetTargetWorldPosition(int _entityIndex, bool _sample, out Vector3 _position)
		{
			if (MemberCount == 1)
			{
				_position = m_parentId.GameEntity.Position;
			}
			else
			{
				int slot = m_entityIndexToSlot[_entityIndex];
				GetSlotWorldPosition(slot, out _position);
			}
			NavMeshHit _hit;
			if (_sample && NavigationHelper.CascadingSamplePosition(_position, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				_position = _hit.position;
			}
		}

		public void HighlightAllMembers(bool _on)
		{
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				EntityId id = m_entities[i].m_id;
				if (id != EntityId.InvalidId)
				{
					if (_on)
					{
						id.GameEntity.Highlight.SetMode(Highlight.EHighlightLayer.Hiding, Highlight.ESelectionMode.Hiding);
					}
					else
					{
						id.GameEntity.Highlight.RemoveMode(Highlight.EHighlightLayer.Hiding);
					}
				}
			}
		}

		public void ReassignPositionIndices()
		{
			int memberCount = MemberCount;
			if (memberCount < 2)
			{
				return;
			}
			for (int i = 0; i < MaxCount; i++)
			{
				m_slotToEntityIndex[i] = -1;
			}
			for (int j = 0; j < MemberCount; j++)
			{
				EntityId id = m_entities[j].m_id;
				Vector3 position = id.GameEntity.Position;
				float num = float.MaxValue;
				int num2 = -1;
				if (j == 0 && m_fixedLeaderSlot)
				{
					num2 = 1;
				}
				else
				{
					for (int k = 0; k < MaxCount; k++)
					{
						if (m_slotToEntityIndex[k] < 0)
						{
							Vector3 _position;
							GetSlotWorldPosition(k, out _position);
							float magnitude = (_position - position).magnitude;
							if (magnitude < num)
							{
								num = magnitude;
								num2 = k;
							}
						}
					}
				}
				m_slotToEntityIndex[num2] = j;
			}
			for (int l = 0; l < MaxCount; l++)
			{
				int num3 = m_slotToEntityIndex[l];
				if (num3 >= 0)
				{
					m_entityIndexToSlot[num3] = l;
				}
			}
		}

		public bool GetMemberInfo(EntityId _id, out MemberInfo _memberInfo)
		{
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				_memberInfo = m_entities[i];
				if (_memberInfo.m_id == _id)
				{
					return true;
				}
			}
			_memberInfo = new MemberInfo();
			_memberInfo.Init(EntityId.InvalidId);
			return false;
		}

		private void GetSlotWorldPosition(int _slot, out Vector3 _position)
		{
			Vector3 vector = m_parentId.GameEntity.Rotation * m_positions[_slot];
			_position = m_parentId.GameEntity.Position + vector;
		}
	}
}
