using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class StalkingZoneComponent : Component
	{
		private readonly float m_ColliderRadiusScale = 1f;

		private BBDictionary<EntityId, uint> m_CurrentCollisions = new BBDictionary<EntityId, uint>();

		private BBList<SphereCollider> m_Colliders = new BBList<SphereCollider>();

		private Vector3 m_center;

		public override int Priority
		{
			get
			{
				return 27;
			}
		}

		public Vector3 Center
		{
			get
			{
				return m_center;
			}
		}

		public StalkingZoneComponent(IEntity _entity)
			: base(_entity)
		{
		}

		private void InitializeColliders()
		{
			GameObject gameObject = base.GameObjectEntity.GameObject;
			int childCount = gameObject.transform.childCount;
			m_center = Vector3.zero;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				SphereCollider component = child.gameObject.GetComponent<SphereCollider>();
				component.isTrigger = true;
				m_Colliders.Add(component);
				child.gameObject.AddComponent<CollisionHandler>();
				CollisionHandler component2 = child.gameObject.GetComponent<CollisionHandler>();
				component2.StalkingZoneComponent = this;
				m_center += child.position;
			}
			m_center /= (float)childCount;
		}

		private void UninitializeColliders()
		{
			GameObject gameObject = base.GameObjectEntity.GameObject;
			int childCount = gameObject.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				SphereCollider component = child.gameObject.GetComponent<SphereCollider>();
				component.isTrigger = false;
				CollisionHandler component2 = child.gameObject.GetComponent<CollisionHandler>();
				component2.StalkingZoneComponent = null;
				UnityEngine.Object.Destroy(component2);
			}
			m_Colliders.ClearIfNotEmpty();
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			InitializeColliders();
		}

		public override void Detach()
		{
			base.Detach();
			UninitializeColliders();
		}

		public void OnCollisionEnter(EntityId _id)
		{
			uint value = 0u;
			if (m_CurrentCollisions.TryGetValue(_id, out value))
			{
				value = (m_CurrentCollisions[_id] = value + 1);
			}
			else
			{
				m_CurrentCollisions[_id] = 1u;
			}
		}

		public void OnCollisionExit(EntityId _id)
		{
			uint value = 0u;
			if (m_CurrentCollisions.TryGetValue(_id, out value))
			{
				if (value > 1)
				{
					value = (m_CurrentCollisions[_id] = value - 1);
				}
				else
				{
					m_CurrentCollisions.Remove(_id);
				}
			}
		}

		public bool IsInStalkingZone(GameEntity _entity)
		{
			bool flag = false;
			if (m_CurrentCollisions.ContainsKey(_entity.Id) && m_CurrentCollisions[_entity.Id] != 0)
			{
				Vector3 position = _entity.Position;
				position.y = 0f;
				int num = 0;
				while (!flag && num < m_Colliders.Count)
				{
					SphereCollider sphereCollider = m_Colliders[num];
					Vector3 vector = sphereCollider.gameObject.transform.position + sphereCollider.center;
					vector.y = 0f;
					Vector3 vector2 = vector - position;
					float num2 = sphereCollider.radius * m_ColliderRadiusScale;
					flag = vector2.sqrMagnitude < num2 * num2;
					num++;
				}
			}
			return flag;
		}

		public bool IsPositionInStalkingZone(Vector3 _position)
		{
			bool flag = false;
			int num = 0;
			while (!flag && num < m_Colliders.Count)
			{
				SphereCollider sphereCollider = m_Colliders[num];
				Vector3 vector = sphereCollider.gameObject.transform.position + sphereCollider.center;
				float num2 = sphereCollider.radius * m_ColliderRadiusScale * (sphereCollider.radius * m_ColliderRadiusScale);
				float sqrMagnitude = (vector - _position).sqrMagnitude;
				flag = sqrMagnitude <= num2;
				num++;
			}
			return flag;
		}

		public Vector3 ChooseClosestColliderPositionInStalkingZone(Vector3 _reference)
		{
			float num = float.PositiveInfinity;
			Vector3 result = Entity.Cast<StalkingZone>().Position;
			for (int i = 0; i < m_Colliders.Count; i++)
			{
				Vector3 vector = m_Colliders[i].gameObject.transform.position + m_Colliders[i].center;
				Vector3 vector2 = vector - _reference;
				vector2.y = 0f;
				float sqrMagnitude = vector2.sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result = vector;
				}
			}
			return result;
		}
	}
}
