using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Height")]
	public class BBTweenHeight : BBTargetedTween
	{
		public int from = 100;

		public int to = 100;

		public bool updateTable;

		private int m_defaultFrom = 100;

		private int m_defaultTo = 100;

		private UIWidget m_cachedWidget;

		private UITable m_table;

		public UIWidget CachedWidget
		{
			get
			{
				if (m_cachedWidget == null)
				{
					m_cachedWidget = base.Target.GetComponent<UIWidget>();
				}
				return m_cachedWidget;
			}
		}

		public int value
		{
			get
			{
				return (CachedWidget != null) ? CachedWidget.height : 0;
			}
			set
			{
				CachedWidget.height = value;
			}
		}

		public override void SwapToAndFromValues()
		{
			int num = from;
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
				from = m_defaultFrom + value;
				to = m_defaultTo + value;
			}
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Mathf.RoundToInt((float)from * (1f - factor) + (float)to * factor);
			if (!updateTable)
			{
				return;
			}
			if (m_table == null)
			{
				m_table = NGUITools.FindInParents<UITable>(base.Target.gameObject);
				if (m_table == null)
				{
					updateTable = false;
					return;
				}
			}
			m_table.repositionNow = true;
		}

		protected override void ClearCache()
		{
			m_cachedWidget = null;
			m_table = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		public static BBTweenHeight Begin(UIWidget widget, float duration, int height)
		{
			BBTweenHeight bBTweenHeight = UITweener.Begin<BBTweenHeight>(widget.gameObject, duration);
			bBTweenHeight.from = widget.height;
			bBTweenHeight.to = height;
			if (duration <= 0f)
			{
				bBTweenHeight.Sample(1f, true);
				bBTweenHeight.enabled = false;
			}
			return bBTweenHeight;
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
