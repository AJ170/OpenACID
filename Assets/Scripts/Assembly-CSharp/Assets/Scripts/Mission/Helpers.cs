using Assets.Scripts.Animation;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace Assets.Scripts.Mission
{
	public static class Helpers
	{
		private static bool IsAncestor(Transform _a, Transform _b)
		{
			while (_b.parent != null)
			{
				if (_a == _b.parent)
				{
					return true;
				}
				_b = _b.parent;
			}
			return false;
		}

		public static GameObject Instantiate(string _resource, bool _castShadow = false)
		{
			Object @object = Resources.Load(_resource);
			GameObject gameObject = Object.Instantiate(@object) as GameObject;
			gameObject.name = @object.name;
			ApplyShadowRecieveAndCast(gameObject, false, _castShadow);
			return gameObject;
		}

		public static T Instantiate<T>(bool _castShadow = false) where T : MonoBehaviour
		{
			GameObject gameObject = new GameObject(typeof(T).ToString());
			T result = gameObject.AddComponent<T>();
			ApplyShadowRecieveAndCast(gameObject, false, _castShadow);
			return result;
		}

		public static GameObject Instantiate(string _resource, string _name, bool _castShadow = false)
		{
			Object original = Resources.Load(_resource);
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			gameObject.name = _name;
			ApplyShadowRecieveAndCast(gameObject, false, _castShadow);
			return gameObject;
		}

		private static void ApplyShadowRecieveAndCast(GameObject _go, bool _recieve, bool _cast)
		{
			Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.receiveShadows = _recieve;
				renderer.shadowCastingMode = (_cast ? ShadowCastingMode.On : ShadowCastingMode.Off);
			}
		}

		public static Quaternion AlignRotationTo(this GameObject _go, Vector3 _normal)
		{
			Vector3 forward = Vector3.Cross(_go.transform.right, _normal);
			return Quaternion.LookRotation(forward, _normal);
		}

		public static void SetTransformAndActivate(GameObject _go, Vector3 _position, Quaternion _rotation)
		{
			NavMeshAgent component = _go.GetComponent<NavMeshAgent>();
			bool enabled = false;
			if (component != null)
			{
				enabled = component.enabled;
				component.enabled = false;
			}
			_go.transform.position = _position;
			_go.transform.rotation = _rotation;
			if (component != null)
			{
				component.enabled = enabled;
			}
			_go.SetActive(true);
		}

		public static float GetDistance(this GameObject _go1, GameObject _go2)
		{
			return (_go1.transform.position - _go2.transform.position).magnitude;
		}

		public static float GetPlaneDistance(this GameObject _go1, GameObject _go2)
		{
			return (_go1.transform.position - _go2.transform.position).LengthXZ();
		}

		public static float GetDistanceFromPlayer(this GameObject _go)
		{
			if (LogicMain.Player == null)
			{
				return float.MaxValue;
			}
			return LogicMain.Player.GetDistance(_go);
		}

		public static float GetPlaneDistanceFromPlayer(this GameObject _go)
		{
			if (LogicMain.Player == null)
			{
				return float.MaxValue;
			}
			return LogicMain.Player.GetPlaneDistance(_go);
		}

		public static bool IsPlayerLookingAt(GameObject _go)
		{
			if (LogicMain.Player == null)
			{
				return false;
			}
			return new Plane(LogicMain.Player.transform.forward, 0f).SameSide(LogicMain.Player.transform.forward, _go.transform.position);
		}

		public static bool IsMovementTask(this AnimationTaskType _type)
		{
			if (_type == AnimationTaskType.PlayerMoveTo || _type == AnimationTaskType.MoveTo)
			{
				return true;
			}
			return false;
		}

		public static bool IsMovementTask(this IAnimationTask _task)
		{
			return _task != null && _task.TaskType.IsMovementTask();
		}

		public static bool IsParkourTask(this IAnimationTask _task)
		{
			return _task is IParkourAnimation;
		}
	}
}
