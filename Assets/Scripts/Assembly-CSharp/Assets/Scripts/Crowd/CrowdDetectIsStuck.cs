using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdDetectIsStuck : MonoBehaviour
	{
		public static float Radius = 0.1f;

		public static float Timeout = 4f;

		public float LastMoveTime;

		public Vector3 LastPosition;

		public bool IsStuck;

		public void Reset(Vector3 _position)
		{
			LastMoveTime = LogicMain.CurrentTime;
			LastPosition = _position;
			IsStuck = false;
		}

		public bool UpdateIsStuck(Vector3 _position)
		{
			IsStuck = false;
			if ((_position - LastPosition).sqrMagnitude > Radius * Radius)
			{
				Reset(_position);
			}
			else
			{
				float num = LogicMain.CurrentTime - LastMoveTime;
				if (num > Timeout)
				{
					IsStuck = true;
				}
			}
			return IsStuck;
		}
	}
}
