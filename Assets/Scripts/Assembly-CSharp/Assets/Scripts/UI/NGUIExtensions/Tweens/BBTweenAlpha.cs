using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Alpha")]
	public class BBTweenAlpha : BBTargetedTween
	{
		[Range(0f, 1f)]
		public float from = 1f;

		[Range(0f, 1f)]
		public float to = 1f;

		[Range(0f, 1f)]
		private float m_defaultFrom = 1f;

		[Range(0f, 1f)]
		private float m_defaultTo = 1f;

		private bool m_isCached;

		private UIRect m_rect;

		private Material m_material;

		private SpriteRenderer m_spriteRenderer;

		public float value
		{
			get
			{
				if (!m_isCached)
				{
					UpdateCache();
				}
				if (m_rect != null)
				{
					return m_rect.alpha;
				}
				if (m_spriteRenderer != null)
				{
					return m_spriteRenderer.color.a;
				}
				return (!(m_material != null)) ? 1f : m_material.color.a;
			}
			set
			{
				if (!m_isCached)
				{
					UpdateCache();
				}
				if (m_rect != null)
				{
					m_rect.alpha = value;
				}
				else if (m_spriteRenderer != null)
				{
					Color color = m_spriteRenderer.color;
					color.a = value;
					m_spriteRenderer.color = color;
				}
				else if (m_material != null)
				{
					Color color2 = m_material.color;
					color2.a = value;
					m_material.color = color2;
				}
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

		private void UpdateCache()
		{
			m_isCached = true;
			m_rect = base.Target.GetComponent<UIRect>();
			m_spriteRenderer = base.Target.GetComponent<SpriteRenderer>();
			if (m_rect == null && m_spriteRenderer == null)
			{
				Renderer component = base.Target.GetComponent<Renderer>();
				if (component != null)
				{
					m_material = component.material;
				}
				if (m_material == null)
				{
					m_rect = base.Target.GetComponentInChildren<UIRect>();
				}
			}
		}

		protected override void ClearCache()
		{
			m_isCached = false;
			m_rect = null;
			m_spriteRenderer = null;
			m_material = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		protected override void OnUpdate(float _factor, bool _isFinished)
		{
			value = Mathf.Lerp(from, to, _factor);
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
			from = value;
		}

		public override void SetEndToCurrentValue()
		{
			to = value;
		}

		public override void CopyFrom(BBTargetedTween _template)
		{
			base.CopyFrom(_template);
			BBTweenAlpha bBTweenAlpha = (BBTweenAlpha)_template;
			if (bBTweenAlpha != null)
			{
				from = bBTweenAlpha.from;
				to = bBTweenAlpha.to;
			}
		}
	}
}
