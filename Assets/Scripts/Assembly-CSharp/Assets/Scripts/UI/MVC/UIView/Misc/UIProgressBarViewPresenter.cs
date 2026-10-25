using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UIProgressBarViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIProgressBar m_progressBar;

		[CheckAssigned]
		[SerializeField]
		private UIAlignmentBox m_alignmentBox;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_currentValueText;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_maxValueText;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_currentValueIcon;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_maxValueIcon;

		[SerializeField]
		[CheckAssigned]
		private BBTweenProgressBar m_progressBarTween;

		[SerializeField]
		[CheckAssigned]
		private BBTweenTickingCounterText m_tickingCounterTween;

		public bool ShowIcons
		{
			set
			{
				m_currentValueIcon.gameObject.SetActive(value);
				m_maxValueIcon.gameObject.SetActive(value);
			}
		}

		public UIProgressBar ProgressBar
		{
			get
			{
				return m_progressBar;
			}
		}

		public BBTweenProgressBar ProgressBarTween
		{
			get
			{
				return m_progressBarTween;
			}
		}

		public BBTweenTickingCounterText TickingCounterTween
		{
			get
			{
				return m_tickingCounterTween;
			}
		}

		public void SetupTweener(int _currentValue, int _animationEndValue, int _startValue, int _maxValue)
		{
			int num = Mathf.Min(_maxValue, _animationEndValue);
			TickingCounterTween.to = num;
			TickingCounterTween.from = _currentValue;
			TickingCounterTween.StringSufix = "/";
			TickingCounterTween.SetTweenToBeginning();
			m_maxValueText.SetNumber(_maxValue);
			int num2 = _currentValue - _startValue;
			int num3 = _maxValue - _startValue;
			int num4 = num - _startValue;
			ProgressBarTween.from = (float)num2 / (float)num3;
			ProgressBarTween.to = (float)num4 / (float)num3;
			ProgressBarTween.SetTweenToBeginning();
			Reposition();
		}

		public void Reposition()
		{
			m_alignmentBox.RepositionNow = true;
		}
	}
}
