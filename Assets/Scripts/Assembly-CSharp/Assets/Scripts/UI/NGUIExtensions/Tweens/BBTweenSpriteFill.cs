using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Sprite Fill")]
	public class BBTweenSpriteFill : BBTargetedTween
	{
		[Range(0f, 1f)]
		public float from;

		[Range(0f, 1f)]
		public float to = 1f;

		[Range(0f, 1f)]
		private float m_defaultFrom = 1f;

		[Range(0f, 1f)]
		private float m_defaultTo = 1f;

		private UISprite m_sprite;

		public float value
		{
			get
			{
				return (!(Sprite != null)) ? 0f : Sprite.fillAmount;
			}
			set
			{
				if (Sprite != null)
				{
					Sprite.fillAmount = value;
				}
			}
		}

		private UISprite Sprite
		{
			get
			{
				if (m_sprite == null)
				{
					m_sprite = base.Target.GetComponent<UISprite>();
				}
				return m_sprite;
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

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Mathf.Lerp(from, to, factor);
		}

		protected override void ClearCache()
		{
			m_sprite = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		public static BBTweenSpriteFill Begin(GameObject go, float duration, float progress)
		{
			BBTweenSpriteFill bBTweenSpriteFill = UITweener.Begin<BBTweenSpriteFill>(go, duration);
			bBTweenSpriteFill.from = bBTweenSpriteFill.value;
			bBTweenSpriteFill.to = progress;
			if (duration <= 0f)
			{
				bBTweenSpriteFill.Sample(1f, true);
				bBTweenSpriteFill.enabled = false;
			}
			return bBTweenSpriteFill;
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
