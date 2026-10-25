using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIRotateObject : MonoBehaviour
	{
		public UIGestureRecogniser Recognizer;

		public GameObject Object { get; set; }

		private void OnEnable()
		{
			Recognizer.OnDragCallback += OnDragCallback;
		}

		private void OnDisable()
		{
			Recognizer.OnDragCallback -= OnDragCallback;
		}

		private void OnDragCallback(Vector2 _delta)
		{
			if (!(Object == null))
			{
				float angle = _delta.x * -90f;
				Object.transform.RotateAround(Object.transform.position, Vector3.up, angle);
			}
		}
	}
}
