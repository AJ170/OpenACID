using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Texture Desaturation")]
	public class BBTweenTextureDesaturation : BBTargetedTween
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

		private UIAchievementTexture m_texture;

		public float value
		{
			get
			{
				if (!m_isCached)
				{
					UpdateCache();
				}
				return (!(m_texture != null)) ? 1f : m_texture.Desaturation;
			}
			set
			{
				if (!m_isCached)
				{
					UpdateCache();
				}
				if (m_texture != null)
				{
					m_texture.Desaturation = value;
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
			m_texture = base.Target.GetComponent<UIAchievementTexture>();
		}

		protected override void ClearCache()
		{
			m_isCached = false;
			m_texture = null;
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

		public static BBTweenAlpha Begin(GameObject _go, float _duration, float _value)
		{
			BBTweenAlpha bBTweenAlpha = UITweener.Begin<BBTweenAlpha>(_go, _duration);
			bBTweenAlpha.from = bBTweenAlpha.value;
			bBTweenAlpha.to = _value;
			if (_duration <= 0f)
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
