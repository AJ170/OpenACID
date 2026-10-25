using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.CharacterPooling;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class EntityTracking : FrameUpdateableComponent
	{
		private float m_trackingTime = 1.5f;

		private UnityEngine.Camera m_Camera;

		private Plane[] m_CameraFrustum;

		private bool m_isTrackingTarget;

		public override int Priority
		{
			get
			{
				return 2;
			}
		}

		public EntityTracking(IEntity _entity)
			: base(_entity)
		{
			IsActive = true;
		}

		public override void OnActivate()
		{
			base.OnActivate();
			GameObject gameObject = new GameObject("Entity Tracking Camera");
			m_Camera = gameObject.AddComponent<UnityEngine.Camera>();
			m_Camera.enabled = false;
			m_isTrackingTarget = false;
		}

		public override void OnUnregistered()
		{
			base.OnUnregistered();
			if (m_Camera != null)
			{
				UnityEngine.Object.Destroy(m_Camera.gameObject);
			}
			m_Camera = null;
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			m_Camera.CopyFrom(UnityEngine.Camera.main);
			m_Camera.fieldOfView *= 0.9f;
			m_CameraFrustum = GeometryUtility.CalculateFrustumPlanes(m_Camera);
			base.DoFrameUpdate(_deltaTime);
		}

		private bool IsInLineOfSight(GameObject _instance, Renderer[] _instanceRenderers, Vector3 _eyePos, bool _restrictFOV)
		{
			Bounds? bounds = null;
			Collider component = _instance.GetComponent<Collider>();
			if (component != null)
			{
				bounds = component.bounds;
			}
			else
			{
				foreach (Renderer renderer in _instanceRenderers)
				{
					if (renderer.isVisible && !(renderer.bounds.center == Vector3.zero))
					{
						if (bounds.HasValue)
						{
							Bounds value = bounds.Value;
							value.Encapsulate(renderer.bounds);
							bounds = value;
						}
						else
						{
							bounds = renderer.bounds;
						}
					}
				}
			}
			if (!bounds.HasValue)
			{
				return false;
			}
			Bounds value2 = bounds.Value;
			Vector3 vector = value2.extents * 0.5f;
			int num = 7;
			Vector3 vector2 = UnityEngine.Camera.main.transform.up;
			Quaternion quaternion = Quaternion.AngleAxis(51.42857f, _instance.transform.forward);
			for (int j = 0; j < 7; j++)
			{
				vector2 = quaternion * vector2;
				Vector3 vector3 = new Vector3(Mathf.Clamp(vector2.x, 0f - vector.x, vector.x), Mathf.Clamp(vector2.y, 0f - vector.y, vector.y), Mathf.Clamp(vector2.z, 0f - vector.z, vector.z)) * 0.5f;
				vector3 += value2.center;
				Vector3 direction = vector3 - _eyePos;
				if (direction.sqrMagnitude > 2500f)
				{
					num--;
					continue;
				}
				Debug.DrawLine(_eyePos, vector3, Color.white, 0.05f);
				RaycastHit hitInfo;
				if (Physics.Raycast(_eyePos, direction, out hitInfo, direction.magnitude, Layers.StaticCollidersLayersMaskPreset))
				{
					num--;
				}
			}
			if (num < 4)
			{
				return false;
			}
			bool isVisible = GeometryUtility.TestPlanesAABB((!_restrictFOV) ? AcierCameraController.Instance.LastCameraFrustum : m_CameraFrustum, value2);
			return UpdateTracking(isVisible);
		}

		private bool UpdateTracking(bool _isVisible)
		{
			if (_isVisible)
			{
				m_trackingTime -= Time.deltaTime;
				m_isTrackingTarget = m_trackingTime >= 0f;
				return !m_isTrackingTarget;
			}
			m_isTrackingTarget = false;
			return m_isTrackingTarget;
		}

		private static int RaycastOrder(RaycastHit _left, RaycastHit _right)
		{
			return _left.distance.CompareTo(_right.distance);
		}

		public bool IsCharacterInLineOfSight(EntityId _entityID, bool _restrictFOV, float _trackTime)
		{
			if (!Assets.Scripts.Engine.Helpers.Verify(_entityID.GameEntity is Character))
			{
				return false;
			}
			if (!m_isTrackingTarget)
			{
				m_trackingTime = _trackTime;
				m_isTrackingTarget = true;
			}
			Vector3 position = UnityEngine.Camera.main.transform.position;
			ReadOnlyCollection<DefaultCharacterManager.CacheEntry> activeCacheEntries = ((DefaultCharacterManager)LogicMain.CharacterManager).ActiveCacheEntries;
			for (int i = 0; i < activeCacheEntries.Count; i++)
			{
				DefaultCharacterManager.CacheEntry cacheEntry = activeCacheEntries[i];
				if (cacheEntry.EntityData.Entity.Id == _entityID && cacheEntry.Attached)
				{
					return IsInLineOfSight(cacheEntry.Instance, cacheEntry.Renderers, position, _restrictFOV);
				}
			}
			return false;
		}

		public bool IsInLineOfSight(GameObject _instance, bool _restrictFOV, float _trackTime)
		{
			if (!m_isTrackingTarget)
			{
				m_trackingTime = _trackTime;
				m_isTrackingTarget = true;
			}
			Vector3 position = UnityEngine.Camera.main.transform.position;
			Renderer[] componentsInChildren = _instance.GetComponentsInChildren<Renderer>();
			return IsInLineOfSight(_instance, componentsInChildren, position, _restrictFOV);
		}
	}
}
