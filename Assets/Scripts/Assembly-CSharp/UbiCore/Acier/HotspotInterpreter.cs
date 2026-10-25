using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Acier
{
	public abstract class HotspotInterpreter : IconInterpreter
	{
		public static float AutoSelectRadius = 5f;

		private Plane m_dragPlane = new Plane(Vector3.up, 0f);

		private RaycastHit m_raycastHit = default(RaycastHit);

		private Vector2 m_controllerDirection;

		private bool m_controllerMoving;

		private bool m_inputLock;

		public override bool OnSecondTap(TapEvent _event)
		{
			return OnTap(_event);
		}

		public override bool OnTap(TapEvent _event)
		{
			bool flag = base.OnTap(_event);
			if (!flag)
			{
				flag = EvaluateSelectFromHotspot(_event.Position, false);
				if (!flag)
				{
					flag = EvaluateKillFromHotspot(_event.Position, false);
					if (flag)
					{
					}
				}
			}
			return flag;
		}

		public override bool OnDoubleTap(TapEvent _event)
		{
			bool flag = base.OnDoubleTap(_event);
			if (!flag)
			{
				flag = EvaluateSelectFromHotspot(_event.Position, true);
				if (!flag)
				{
					flag = EvaluateKillFromHotspot(_event.Position, true);
					if (!flag)
					{
						flag = EvaluateLeaveHotspot(_event.Position, false);
					}
				}
			}
			return flag;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			m_controllerDirection = gamepad.Direction;
			if (m_controllerDirection.sqrMagnitude > 0f)
			{
				m_controllerMoving = true;
				Vector3 forward = Camera.main.transform.forward;
				forward.y = 0f;
				forward.Normalize();
				Vector3 vector = Quaternion.LookRotation(forward) * new Vector3(m_controllerDirection.x, 0f, m_controllerDirection.y).normalized;
				float magnitude = m_controllerDirection.magnitude;
				Vector3 destination = IconInterpreter.NavMeshAgent.transform.position + vector * magnitude;
				if (OnLeaveHotspot(destination))
				{
					return;
				}
			}
			else if (m_controllerMoving)
			{
				m_controllerMoving = false;
			}
			if (AcierInputHandler.Instance.FingerCount > 1)
			{
				m_inputLock = true;
			}
		}

		public override IconMaterial GetDefaultActionIcon()
		{
			IconMaterial defaultActionIcon = base.GetDefaultActionIcon();
			if (defaultActionIcon != 0)
			{
				return defaultActionIcon;
			}
			return IconMaterial.Invalid;
		}

		public override bool OnTriggerDefaultAction()
		{
			if (base.OnTriggerDefaultAction())
			{
				return true;
			}
			return false;
		}

		private bool EvaluateSelectFromHotspot(Vector2 _normalizedPosition, bool _simulate)
		{
			if (RaycastForInteractableObject(_normalizedPosition, ref m_raycastHit, GetHideoutColliders()))
			{
				EnemyCharacter enemyCharacter = m_raycastHit.collider.gameObject.TryGetEntity() as EnemyCharacter;
				if (enemyCharacter != null && !enemyCharacter.GetIsDead())
				{
					if (!_simulate)
					{
						OnSetSelectedEnemy(enemyCharacter);
					}
					return true;
				}
			}
			return false;
		}

		private bool EvaluateLeaveHotspot(Vector2 _position, bool _simulate)
		{
			bool result = false;
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			Vector3 position = navMeshAgent.transform.position;
			m_dragPlane.distance = 0f - position.y;
			Ray ray = Camera.main.ScreenPointToRay(_position);
			Vector3 _hitPoint;
			if (m_dragPlane.RayCast(ray, out _hitPoint) && !_simulate)
			{
				result = OnLeaveHotspot(_hitPoint);
			}
			return result;
		}

		private bool EvaluateKillFromHotspot(Vector2 _normalizedPosition, bool _simulate)
		{
			bool result = false;
			if (RaycastForInteractableObject(_normalizedPosition, ref m_raycastHit, GetHideoutColliders()))
			{
				GameObjectEntity gameObjectEntity = m_raycastHit.collider.gameObject.TryGetEntity();
				if (gameObjectEntity is Character && !_simulate)
				{
					OnKillFromHotspot(gameObjectEntity.Id);
					result = !((Character)gameObjectEntity).GetIsDead();
				}
			}
			return result;
		}

		public override bool OnDrag(DragEvent _event)
		{
			if (base.OnDrag(_event))
			{
				return true;
			}
			if (_event.Phase == InputEvent.InputPhase.Started)
			{
				m_inputLock = false;
			}
			if (m_inputLock)
			{
				return false;
			}
			Vector3 forward = Camera.main.transform.forward;
			forward.y = 0f;
			forward.Normalize();
			Quaternion quaternion = Quaternion.LookRotation(forward);
			Vector3 vector = new Vector3(_event.StickInput.x, 0f, _event.StickInput.y);
			Vector3 vector2 = quaternion * vector.normalized;
			float magnitude = _event.StickInput.magnitude;
			if (magnitude < float.Epsilon)
			{
				return false;
			}
			Vector3 destination = IconInterpreter.NavMeshAgent.transform.position + vector2 * magnitude;
			return OnLeaveHotspot(destination);
		}

		public override bool OnSelect(SelectEvent _event)
		{
			if (base.OnSelect(_event))
			{
				return true;
			}
			if (_event.Phase == InputEvent.InputPhase.Started)
			{
				m_inputLock = false;
			}
			if (m_inputLock)
			{
				return false;
			}
			NavMeshAgent navMeshAgent = IconInterpreter.NavMeshAgent;
			Vector3 position = navMeshAgent.transform.position;
			Ray ray = Camera.main.ScreenPointToRay(_event.Position);
			Vector3 destination = ray.origin + ray.direction * ((position.y - ray.origin.y) / (0f - Mathf.Abs(ray.direction.y)));
			return OnLeaveHotspot(destination);
		}

		private Collider[] GetHideoutColliders()
		{
			return UbiCore.Utils.Singleton<HotspotService>.Instance.GetCurrentHideout().UnityComponentCache.GameObject.GetComponentsInChildren<Collider>();
		}

		protected abstract bool OnKillFromHotspot(EntityId _id);

		protected abstract bool OnLeaveHotspot(Vector3 _destination);

		protected abstract void OnSetSelectedEnemy(EnemyCharacter _enemy);

		protected abstract EnemyCharacter GetSelectedEnemy();
	}
}
