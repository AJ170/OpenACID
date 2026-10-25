using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween position")]
	public class BBTweenPosition : BBTargetedTween
	{
		public enum ETweenPosition
		{
			AllAxis = 0,
			YAxisOnly = 1,
			XAxisOnly = 2
		}

		public Vector3 from;

		public Vector3 to;

		[SerializeField]
		private ETweenPosition m_tweenType;

		[HideInInspector]
		public bool worldSpace;

		private Vector3 m_defaultFrom = Vector3.zero;

		private Vector3 m_defaultTo = Vector3.zero;

		private Transform m_cachedTransform;

		private UIRect m_rect;

		public ETweenPosition TweenType
		{
			get
			{
				return m_tweenType;
			}
			set
			{
				m_tweenType = value;
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

		private UIRect Rect
		{
			get
			{
				if (m_rect == null)
				{
					m_rect = base.Target.GetComponent<UIRect>();
				}
				return m_rect;
			}
		}

		public Vector3 value
		{
			get
			{
				return (!worldSpace) ? CachedTransform.localPosition : CachedTransform.position;
			}
			set
			{
				if (Rect == null || !Rect.isAnchored || worldSpace)
				{
					if (worldSpace)
					{
						if (m_tweenType == ETweenPosition.XAxisOnly)
						{
							value.y = CachedTransform.position.y;
						}
						else if (m_tweenType == ETweenPosition.YAxisOnly)
						{
							value.x = CachedTransform.position.x;
						}
						CachedTransform.position = value;
					}
					else
					{
						if (m_tweenType == ETweenPosition.XAxisOnly)
						{
							value.y = CachedTransform.localPosition.y;
						}
						else if (m_tweenType == ETweenPosition.YAxisOnly)
						{
							value.x = CachedTransform.localPosition.x;
						}
						CachedTransform.localPosition = value;
					}
				}
				else
				{
					value -= CachedTransform.localPosition;
					if (m_tweenType == ETweenPosition.XAxisOnly)
					{
						value.y = 0f;
					}
					else if (m_tweenType == ETweenPosition.YAxisOnly)
					{
						value.x = 0f;
					}
					NGUIMath.MoveRect(Rect, value.x, value.y);
				}
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
			if (base.IsTweenValidationAllowed)
			{
				base.ValidateTween();
				if (m_incrementValueWithFrom)
				{
					from = m_defaultFrom + CachedTransform.localPosition;
					to = m_defaultTo + CachedTransform.localPosition;
				}
			}
		}

		public override void CopyFrom(BBTargetedTween _template)
		{
			base.CopyFrom(_template);
			BBTweenPosition bBTweenPosition = (BBTweenPosition)_template;
			if (bBTweenPosition != null)
			{
				m_tweenType = bBTweenPosition.m_tweenType;
				from = bBTweenPosition.from;
				to = bBTweenPosition.to;
				m_defaultFrom = bBTweenPosition.m_defaultFrom;
				m_defaultTo = bBTweenPosition.m_defaultTo;
			}
		}

		protected override void ClearCache()
		{
			m_rect = null;
			m_cachedTransform = null;
		}

		protected override void Init()
		{
			if (!m_isInitialized)
			{
				base.Init();
				m_defaultTo = to;
				m_defaultFrom = from;
			}
		}

		protected override void OnUpdate(float _factor, bool _isFinished)
		{
			value = from * (1f - _factor) + to * _factor;
		}

		public static BBTweenPosition Begin(GameObject _go, float _duration, Vector3 _pos)
		{
			BBTweenPosition bBTweenPosition = UITweener.Begin<BBTweenPosition>(_go, _duration);
			bBTweenPosition.from = bBTweenPosition.value;
			bBTweenPosition.to = _pos;
			if (_duration <= 0f)
			{
				bBTweenPosition.Sample(1f, true);
				bBTweenPosition.enabled = false;
			}
			return bBTweenPosition;
		}

		public static BBTweenPosition Begin(GameObject _go, float _duration, Vector3 _pos, bool _worldSpace)
		{
			BBTweenPosition bBTweenPosition = UITweener.Begin<BBTweenPosition>(_go, _duration);
			bBTweenPosition.worldSpace = _worldSpace;
			bBTweenPosition.from = bBTweenPosition.value;
			bBTweenPosition.to = _pos;
			if (_duration <= 0f)
			{
				bBTweenPosition.Sample(1f, true);
				bBTweenPosition.enabled = false;
			}
			return bBTweenPosition;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			from = value;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			to = value;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			value = from;
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			value = to;
		}
	}
}
