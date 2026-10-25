using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Scale")]
	public class BBTweenScale : BBTargetedTween
	{
		public Vector3 from = Vector3.one;

		public Vector3 to = Vector3.one;

		public bool updateTable;

		private Vector3 m_defaultFrom = Vector3.zero;

		private Vector3 m_defaultTo = Vector3.zero;

		private UITable mTable;

		private Transform m_cachedTransform;

		public Vector3 value
		{
			get
			{
				return CachedTransform.localScale;
			}
			set
			{
				CachedTransform.localScale = value;
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
				from = m_defaultFrom + value;
				to = m_defaultTo + value;
			}
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Vector3.Lerp(from, to, factor);
			if (!updateTable)
			{
				return;
			}
			if (mTable == null)
			{
				mTable = NGUITools.FindInParents<UITable>(base.Target.gameObject);
				if (mTable == null)
				{
					updateTable = false;
					return;
				}
			}
			mTable.repositionNow = true;
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

		public static BBTweenScale Begin(GameObject go, float duration, Vector3 scale)
		{
			BBTweenScale bBTweenScale = UITweener.Begin<BBTweenScale>(go, duration);
			bBTweenScale.from = bBTweenScale.value;
			bBTweenScale.to = scale;
			if (duration <= 0f)
			{
				bBTweenScale.Sample(1f, true);
				bBTweenScale.enabled = false;
			}
			return bBTweenScale;
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
