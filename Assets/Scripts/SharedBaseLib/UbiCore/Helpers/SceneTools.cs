using SharedBaseLib.System;
using UnityEngine;
using UnityEngine.AI;

namespace UbiCore.Helpers
{
	public class SceneTools
	{
		public static float DefaultNavMeshSearchRadius = 0.5f;

		public static void SnapToNavMesh(Transform _transform, int _allowedMask = -1, float _searchRadius = 10f)
		{
			NavMeshHit hit;
			if (NavMesh.SamplePosition(_transform.position, out hit, _searchRadius, _allowedMask))
			{
				_transform.position = hit.position;
			}
		}

		public static bool PickNavMeshPoint(Vector3 origin, Vector3 direction, out Vector3 point, int walkableMask = -1)
		{
			return PickNavMeshPoint(new Ray(origin, direction), out point, DefaultNavMeshSearchRadius, walkableMask);
		}

		public static bool PickNavMeshPoint(Vector3 origin, Vector3 direction, out Vector3 point, float searchRadius, int walkableMask = -1)
		{
			return PickNavMeshPoint(new Ray(origin, direction), out point, searchRadius, walkableMask);
		}

		public static bool PickNavMeshPoint(Ray ray, out Vector3 point, int walkableMask = -1)
		{
			return PickNavMeshPoint(ray, out point, DefaultNavMeshSearchRadius, walkableMask);
		}

		public static bool PickNavMeshPoint(Ray ray, out Vector3 point, float searchRadius, int walkableMask = -1)
		{
			bool flag = false;
			point = ray.origin;
			RaycastHit[] array = Physics.RaycastAll(ray, float.MaxValue, Layers.StaticCollidersLayersMaskPreset.value);
			float num = float.MaxValue;
			RaycastHit[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit raycastHit = array2[i];
				if (!(raycastHit.distance > num))
				{
					NavMeshHit hit;
					if (NavMesh.SamplePosition(raycastHit.point, out hit, searchRadius, walkableMask))
					{
						num = raycastHit.distance;
						point = hit.position;
						flag = true;
					}
					else if (!flag)
					{
						point = raycastHit.point;
					}
				}
			}
			return flag;
		}

		public static void FindGameObjectAndDestroyImmediate(string _name)
		{
			GameObject gameObject = GameObject.Find(_name);
			if (gameObject != null)
			{
				Object.DestroyImmediate(gameObject);
			}
		}

		public static T[] FindObjectsOfType<T>() where T : Object
		{
			return (T[])Object.FindObjectsOfType(typeof(T));
		}

		public static Bounds GetSceneCollidersBounds()
		{
			Collider[] array = FindObjectsOfType<Collider>();
			Layers.Init();
			if (array.Length != 0)
			{
				Bounds bounds = array[0].bounds;
				for (int i = 1; i < array.Length; i++)
				{
					Collider collider = array[i];
					if (collider.gameObject.layer == Layers.StaticColliderNoShadowLayer || collider.gameObject.layer == Layers.StaticColliderShadowLayer || collider.gameObject.layer == Layers.StaticColliderNoShadowCameraSeeThroughLayer || collider.gameObject.layer == Layers.CameraBlockerLayer)
					{
						bounds.Encapsulate(collider.bounds);
					}
				}
				return bounds;
			}
			return new Bounds(Vector3.zero, Vector3.zero);
		}
	}
}
