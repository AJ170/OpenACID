using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Width")]
	public class BBTweenWidth : BBTargetedTween
	{
		public int from = 100;

		public int to = 100;

		public bool updateTable;

		private int m_defaultFrom = 100;

		private int m_defaultTo = 100;

		private UIWidget m_widget;

		private UITable m_table;

		public UIWidget CachedWidget
		{
			get
			{
				if (m_widget == null)
				{
					m_widget = base.Target.GetComponent<UIWidget>();
				}
				return m_widget;
			}
		}

		public int value
		{
			get
			{
				return CachedWidget.width;
			}
			set
			{
				CachedWidget.width = value;
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
			value = (int)Mathf.Lerp(from, to, factor);
			if (!updateTable)
			{
				return;
			}
			if (m_table == null)
			{
				m_table = NGUITools.FindInParents<UITable>(base.Target);
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
			m_table = null;
			m_widget = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		public static BBTweenWidth Begin(UIWidget widget, float duration, int width)
		{
			BBTweenWidth bBTweenWidth = UITweener.Begin<BBTweenWidth>(widget.gameObject, duration);
			bBTweenWidth.from = widget.width;
			bBTweenWidth.to = width;
			if (duration <= 0f)
			{
				bBTweenWidth.Sample(1f, true);
				bBTweenWidth.enabled = false;
			}
			return bBTweenWidth;
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
