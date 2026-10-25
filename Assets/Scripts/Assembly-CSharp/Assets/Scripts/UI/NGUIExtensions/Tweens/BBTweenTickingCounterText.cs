using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Ticker Text")]
	public class BBTweenTickingCounterText : BBTargetedTween
	{
		public long from;

		public long to;

		public bool UpdateTable;

		public UIAlignmentBox AlignmentBox;

		private long m_defaultFrom;

		private long m_defaultTo;

		private int m_currentValue;

		private UILabel m_numberText;

		public string StringSufix { get; set; }

		public int value
		{
			get
			{
				return m_currentValue;
			}
			set
			{
				m_currentValue = value;
				if (NumberTextLabel != null)
				{
					NumberTextLabel.text = LocalizationManager.Instance.GetInteger(m_currentValue) + StringSufix;
				}
			}
		}

		private UILabel NumberTextLabel
		{
			get
			{
				if (m_numberText == null)
				{
					m_numberText = base.Target.GetComponent<UILabel>();
				}
				return m_numberText;
			}
		}

		public BBTweenTickingCounterText()
		{
			StringSufix = string.Empty;
		}

		public override void SwapToAndFromValues()
		{
			long num = from;
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
			if (UpdateTable && AlignmentBox != null)
			{
				AlignmentBox.RepositionNow = true;
			}
		}

		protected override void ClearCache()
		{
			m_numberText = null;
		}

		private void Awake()
		{
			m_defaultTo = to;
			m_defaultFrom = from;
		}

		public static BBTweenTickingCounterText Begin(GameObject go, float duration, int _number)
		{
			BBTweenTickingCounterText bBTweenTickingCounterText = UITweener.Begin<BBTweenTickingCounterText>(go, duration);
			bBTweenTickingCounterText.from = bBTweenTickingCounterText.value;
			bBTweenTickingCounterText.to = _number;
			if (duration <= 0f)
			{
				bBTweenTickingCounterText.Sample(1f, true);
				bBTweenTickingCounterText.enabled = false;
			}
			return bBTweenTickingCounterText;
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
