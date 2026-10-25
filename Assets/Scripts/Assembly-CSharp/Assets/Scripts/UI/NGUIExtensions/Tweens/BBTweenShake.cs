using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Shake")]
	public class BBTweenShake : BBTargetedTween
	{
		[Range(0f, 1f)]
		public float from = 1f;

		[Range(0f, 1f)]
		public float to = 1f;

		public float ShakePositionFactor = 180f;

		public float ShakeScaleFactor = 0.2f;

		public float ShakeRotaionFactor;

		private Vector3 m_originalPos = Vector3.zero;

		private Quaternion m_originalRotation;

		private float m_shakeIntensity;

		private Transform m_cachedTransform;

		public float Value
		{
			get
			{
				return m_shakeIntensity;
			}
			set
			{
				m_shakeIntensity = value;
				if (ShakePositionFactor > 0f)
				{
					Vector3 localPosition = m_originalPos + Random.insideUnitSphere * m_shakeIntensity * ShakePositionFactor;
					localPosition.z = 0f;
					CachedTransform.localPosition = localPosition;
				}
				if (ShakeScaleFactor > 0f)
				{
					float num = 1f - Random.Range(0f - m_shakeIntensity, m_shakeIntensity) * ShakeScaleFactor;
					CachedTransform.localScale = new Vector3(num, num, num);
				}
				if (ShakeRotaionFactor > 0f)
				{
					CachedTransform.localRotation = new Quaternion(0f, 0f, m_originalRotation.z + Random.Range(0f - m_shakeIntensity, m_shakeIntensity) * ShakeRotaionFactor, m_originalRotation.w + Random.Range(0f - m_shakeIntensity, m_shakeIntensity) * ShakeRotaionFactor);
				}
			}
		}

		private Transform CachedTransform
		{
			get
			{
				if (m_cachedTransform == null)
				{
					m_cachedTransform = base.Target.transform;
					m_originalPos = m_cachedTransform.localPosition;
					m_originalRotation = m_cachedTransform.localRotation;
				}
				return m_cachedTransform;
			}
		}

		public override void SwapToAndFromValues()
		{
			float num = from;
			from = to;
			to = num;
		}

		public override void ValidateTween()
		{
			base.ValidateTween();
		}

		protected override void ClearCache()
		{
			m_cachedTransform = null;
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			Value = Mathf.Lerp(from, to, factor);
		}

		public static BBTweenAlpha Begin(GameObject go, float duration, float alpha)
		{
			BBTweenAlpha bBTweenAlpha = UITweener.Begin<BBTweenAlpha>(go, duration);
			bBTweenAlpha.from = bBTweenAlpha.value;
			bBTweenAlpha.to = alpha;
			if (duration <= 0f)
			{
				bBTweenAlpha.Sample(1f, true);
				bBTweenAlpha.enabled = false;
			}
			return bBTweenAlpha;
		}

		public override void SetStartToCurrentValue()
		{
			from = Value;
		}

		public override void SetEndToCurrentValue()
		{
			to = Value;
		}
	}
}
