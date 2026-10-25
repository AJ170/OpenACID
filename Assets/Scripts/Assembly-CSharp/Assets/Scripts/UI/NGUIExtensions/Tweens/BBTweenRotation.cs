using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Rotation")]
	public class BBTweenRotation : BBTargetedTween
	{
		public Vector3 from;

		public Vector3 to;

		private Vector3 m_defaultFrom = Vector3.zero;

		private Vector3 m_defaultTo = Vector3.zero;

		private Transform m_cachedTransform;

		public Quaternion value
		{
			get
			{
				return CachedTransform.localRotation;
			}
			set
			{
				CachedTransform.localRotation = value;
			}
		}

		private Transform CachedTransform
		{
			get
			{
				if (m_cachedTransform == null)
				{
					m_cachedTransform = base.Target.transform;
				}
				return m_cachedTransform;
			}
		}

		public override void SwapToAndFromValues()
		{
			Vector3 vector = from;
			from = to;
			to = vector;
			vector = m_defaultFrom;
			m_defaultFrom = m_defaultTo;
			m_defaultTo = vector;
		}

		public override void ValidateTween()
		{
			base.ValidateTween();
			if (m_incrementValueWithFrom)
			{
				from = m_defaultFrom + value.eulerAngles;
				to = m_defaultTo + value.eulerAngles;
			}
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Quaternion.Euler(new Vector3(Mathf.Lerp(from.x, to.x, factor), Mathf.Lerp(from.y, to.y, factor), Mathf.Lerp(from.z, to.z, factor)));
		}

		protected override void ClearCache()
		{
			m_cachedTransform = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		public static BBTweenRotation Begin(GameObject go, float duration, Quaternion rot)
		{
			BBTweenRotation bBTweenRotation = UITweener.Begin<BBTweenRotation>(go, duration);
			bBTweenRotation.from = bBTweenRotation.value.eulerAngles;
			bBTweenRotation.to = rot.eulerAngles;
			if (duration <= 0f)
			{
				bBTweenRotation.Sample(1f, true);
				bBTweenRotation.enabled = false;
			}
			return bBTweenRotation;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			from = value.eulerAngles;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			to = value.eulerAngles;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			value = Quaternion.Euler(from);
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			value = Quaternion.Euler(to);
		}
	}
}
