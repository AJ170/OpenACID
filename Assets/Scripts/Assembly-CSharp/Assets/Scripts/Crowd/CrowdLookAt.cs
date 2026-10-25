using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdLookAt : MonoBehaviour
	{
		public static bool EnableLookAt = true;

		public static float LookAtDamping = 0.3f;

		public static float LookAtHeight = 1.7f;

		public static float LookAtHeightOffsetFactor = 0.5f;

		public static float LookAtWeightPow = 0.5f;

		public static float LookAtPastDistance = 8f;

		[HideInInspector]
		public Animator Animator;

		public float Weight;

		public Vector3 Target;

		public void OnDrawGizmosSelected()
		{
			if (!(Weight < float.Epsilon))
			{
				Vector3 position = base.transform.position;
			}
		}

		public void LookAtPlayer(Vector3 _currentPosition, Vector3 _facingDirection, float _dt)
		{
			if (LogicMain.PlayerEntity == null)
			{
				return;
			}
			float num = 0f;
			if (Crowd.Instance.IsPlayerNoticeable)
			{
				Vector3 position = LogicMain.PlayerEntity.Position;
				Vector3 vector = position - _currentPosition;
				vector.y *= LookAtHeightOffsetFactor;
				Vector3 normalized = vector.normalized;
				Target = _currentPosition + Vector3.up * LookAtHeight + (vector + normalized * LookAtPastDistance);
				float num2 = Vector3.Dot(normalized, _facingDirection);
				num = ((!(num2 > 0f)) ? 0f : Mathf.Pow(Mathf.Clamp01(num2), LookAtWeightPow));
				if (num > 0f)
				{
					Animator.SetLookAtPosition(Target);
				}
			}
			if (Weight > 0f || num > 0f)
			{
				Weight = Damping.Damp(Weight, num, LookAtDamping, _dt);
				if (num < 0.025f && Weight < 0.025f)
				{
					Weight = 0f;
				}
				Animator.SetLookAtWeight(Weight, 0.25f, 0.85f);
			}
		}

		public void ResetLookAt()
		{
			if (Weight > 0f)
			{
				Weight = 0f;
				Animator.SetLookAtWeight(0f);
			}
		}
	}
}
