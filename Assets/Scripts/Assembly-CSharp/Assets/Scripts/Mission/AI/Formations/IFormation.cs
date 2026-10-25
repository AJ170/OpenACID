using Assets.Scripts.Mission.Logic;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Formations
{
	public interface IFormation
	{
		int MaxCount { get; set; }

		int MemberCount { get; }

		BBList<MemberInfo> Entities { get; }

		EntityId EntityAtHidingSpot { get; }

		MemberInfo EntityAtHidingSpotInfo { get; }

		void Init(EntityId _parentId, float _maxExtend, float _maxDeviation, bool _fixedLeaderSlot);

		bool Add(EntityId _entityId);

		bool Add(EntityId _entityId, out bool _newlyAdded);

		void Remove(EntityId _entityId);

		void Remove(EntityId _entityId, out bool _newlyRemoved, bool _lowPrio);

		bool GetEntityIndex(EntityId _entityId, out int _entityIndex);

		bool AddAtHidingSpot(EntityId _id);

		bool RemoveFromHidingSpot(EntityId _id);

		bool GetTargetWorldPosition(EntityId _entityId, out Vector3 _position);

		bool Contains(EntityId _entityId);

		void GetTargetWorldPosition(int _entityIndex, bool _sample, out Vector3 _position);

		Vector3 CalcCurrentMemberCenter();

		void HighlightAllMembers(bool _on);

		void ReassignPositionIndices();

		void DemandMemberFrameUpdate(bool _demand);

		bool GetMemberInfo(EntityId _id, out MemberInfo _memberInfo);
	}
}
