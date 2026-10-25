using System;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Slerp")]
	public class BBTweenSlerp : BBTargetedTween
	{
		[SerializeField]
		private Vector3 m_from;

		[SerializeField]
		private Vector3 m_to;

		[SerializeField]
		private Vector3 m_curveOffset;

		[HideInInspector]
		public bool worldSpace;

		private Vector3 m_defaultFrom = Vector3.zero;

		private Vector3 m_defaultTo = Vector3.zero;

		private Transform m_cachedTransform;

		private UIRect m_rect;

		public Vector3 From
		{
			get
			{
				return m_from;
			}
			set
			{
				m_from = value;
			}
		}

		public Vector3 To
		{
			get
			{
				return m_to;
			}
			set
			{
				m_to = value;
			}
		}

		public Vector3 CurveOffset
		{
			get
			{
				return m_curveOffset;
			}
			set
			{
				m_curveOffset = value;
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
						CachedTransform.position = value;
					}
					else
					{
						CachedTransform.localPosition = value;
					}
				}
				else
				{
					value -= CachedTransform.localPosition;
					NGUIMath.MoveRect(Rect, value.x, value.y);
				}
			}
		}

		public override void SwapToAndFromValues()
		{
			Vector3 from = m_from;
			m_from = m_to;
			m_to = from;
			from = m_defaultFrom;
			m_defaultFrom = m_defaultTo;
			m_defaultTo = from;
		}

		public override void ValidateTween()
		{
			base.ValidateTween();
			if (m_incrementValueWithFrom)
			{
				m_from = m_defaultFrom + CachedTransform.localPosition;
				m_to = m_defaultTo + CachedTransform.localPosition;
			}
		}

		public override void CopyFrom(BBTargetedTween _template)
		{
			base.CopyFrom(_template);
			BBTweenSlerp bBTweenSlerp = (BBTweenSlerp)_template;
			if (bBTweenSlerp != null)
			{
				From = bBTweenSlerp.From;
				To = bBTweenSlerp.To;
				CurveOffset = bBTweenSlerp.CurveOffset;
				m_defaultFrom = bBTweenSlerp.m_defaultFrom;
				m_defaultTo = bBTweenSlerp.m_defaultTo;
			}
		}

		protected override void OnUpdate(float _factor, bool _isFinished)
		{
			Vector3 vector = To - From;
			Vector3 from = From;
			from.x += vector.x * _factor + Mathf.Sin(_factor * (float)Math.PI) * CurveOffset.x;
			from.y += vector.y * _factor + Mathf.Sin(_factor * (float)Math.PI) * CurveOffset.y;
			from.z += vector.z * _factor;
			value = from;
		}

		protected override void ClearCache()
		{
			m_rect = null;
			m_cachedTransform = null;
		}

		private void Awake()
		{
			m_defaultTo = To;
			m_defaultFrom = From;
		}

		public static BBTweenSlerp Begin(GameObject go, float _duration, Vector3 _pos, Vector3 _curveFactor)
		{
			BBTweenSlerp bBTweenSlerp = UITweener.Begin<BBTweenSlerp>(go, _duration);
			bBTweenSlerp.From = bBTweenSlerp.value;
			bBTweenSlerp.To = _pos;
			bBTweenSlerp.CurveOffset = _curveFactor;
			if (_duration <= 0f)
			{
				bBTweenSlerp.Sample(1f, true);
				bBTweenSlerp.enabled = false;
			}
			return bBTweenSlerp;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			Debug.Log(value);
			From = value;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			To = value;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			value = From;
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			value = To;
		}
	}
}
