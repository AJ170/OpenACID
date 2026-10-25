using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Database;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public abstract class HideoutManager<T> : HotspotManager<T> where T : InteractableObject
	{
		protected EntityId m_currentHideout = EntityId.InvalidId;

		public T Current
		{
			get
			{
				return m_currentHideout.TryGetEntity<T>();
			}
			set
			{
				m_currentHideout = ((value != null) ? value.Id : EntityId.InvalidId);
				IsHideoutActive = m_currentHideout != EntityId.InvalidId;
			}
		}

		protected bool IsHideoutActive { get; set; }

		public bool LeaveHideoutAfterKill { get; protected set; }

		protected HideoutManager(string _rootNodeName)
			: base(_rootNodeName)
		{
			m_currentHideout = EntityId.InvalidId;
		}

		protected override bool Interact(AssassinCharacter _assassin, T _entity, NavMeshAgent _agent)
		{
			bool result = false;
			InteractionLocators.Locator _locator;
			NavMeshPath _path;
			if (m_currentHideout == EntityId.InvalidId && _assassin.AssassinInteraction.CanInteractWithInteractableObject(_entity.Id) && _agent != null && _entity.InteractionLocators.ChooseClosestLocator(_agent.transform.position, _assassin.GetAutoPassableNavMeshLayerMask(), out _locator, out _path))
			{
				result = _assassin.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(_entity.Id, EInteractionReason.InteractWithObject, _locator);
			}
			return result;
		}

		public void OnHideoutExited(IStateMachine stateMachine, bool canceled)
		{
			Current = (T)null;
		}

		public bool OnKillFromHideout(EntityId _id)
		{
			bool flag = false;
			if (IsHideoutActive && IsIdlingInHideout() && PlayerHelper.IsHidingInObject)
			{
				flag = OnKill(_id);
				if (LeaveHideoutAfterKill)
				{
					IsHideoutActive = !flag;
				}
			}
			return flag;
		}

		public override bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			bool flag = base.LeaveInteraction(_assassin, _destination);
			if (flag && IsHideoutActive && IsIdlingInHideout())
			{
				flag = OnExitHideout(_assassin, _destination);
				IsHideoutActive = !flag;
			}
			return flag;
		}

		protected GameObject CreatePrefabIfNotExists(GameObject _parent, GameID _gameID)
		{
			ItemDef item = Main.Instance.GameDB.GetItem<ItemDef>(_gameID);
			GameObject gameObject = _parent.FindDirectChildByName(item.Name);
			if (gameObject != null)
			{
				return gameObject;
			}
			UnityEngine.Object original = item.ItemPrefab.AccessObject<UnityEngine.Object>();
			gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
			gameObject.name = item.Name;
			gameObject.transform.parent = _parent.transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			return gameObject;
		}

		protected abstract bool OnExitHideout(AssassinCharacter _assassin, Vector3 _destination);

		protected abstract bool OnKill(EntityId _id);

		protected abstract bool IsIdlingInHideout();
	}
}
