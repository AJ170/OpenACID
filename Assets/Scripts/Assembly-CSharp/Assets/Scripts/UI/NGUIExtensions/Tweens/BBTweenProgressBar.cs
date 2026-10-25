using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Progress bar")]
	public class BBTweenProgressBar : BBTargetedTween
	{
		[Range(0f, 1f)]
		public float from;

		[Range(0f, 1f)]
		public float to = 1f;

		[Range(0f, 1f)]
		private float m_defaultFrom = 1f;

		[Range(0f, 1f)]
		private float m_defaultTo = 1f;

		private UIProgressBar m_progressBar;

		public float value
		{
			get
			{
				return (!(ProgressBar != null)) ? 0f : ProgressBar.value;
			}
			set
			{
				if (ProgressBar != null)
				{
					ProgressBar.value = value;
				}
			}
		}

		private UIProgressBar ProgressBar
		{
			get
			{
				if (m_progressBar == null)
				{
					m_progressBar = base.Target.GetComponent<UIProgressBar>();
				}
				return m_progressBar;
			}
		}

		public override void SwapToAndFromValues()
		{
			float num = from;
			from = to;
			to = num;
			num = m_defaultFrom;
			m_defaultFrom = m_defaultTo;
			m_defaultTo = num;
		}

		public override void ValidateTween()
		{
			base.ValidateTween();
			if (m_incrementValueWithFrom)
			{
				from = Mathf.Clamp01(m_defaultFrom + value);
				to = Mathf.Clamp01(m_defaultTo + value);
			}
		}

		protected override void ClearCache()
		{
			m_progressBar = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Mathf.Lerp(from, to, factor);
		}

		public static BBTweenProgressBar Begin(GameObject go, float duration, float progress)
		{
			BBTweenProgressBar bBTweenProgressBar = UITweener.Begin<BBTweenProgressBar>(go, duration);
			bBTweenProgressBar.from = bBTweenProgressBar.value;
			bBTweenProgressBar.to = progress;
			if (duration <= 0f)
			{
				bBTweenProgressBar.Sample(1f, true);
				bBTweenProgressBar.enabled = false;
			}
			return bBTweenProgressBar;
		}

		public override void SetStartToCurrentValue()
		{
			from = value;
		}

		public override void SetEndToCurrentValue()
		{
			to = value;
		}
	}
}
