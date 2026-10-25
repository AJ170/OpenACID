using System;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIOnScreenTimerBehaviour : UIBaseBehaviour
	{
		public UILabel MessageLabel;

		public UITweenHelper TweenHelper;

		public BBTweenAlpha TimeRunningOutTween;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.OnScreenTimer;
			}
		}

		public override bool FullyVisible
		{
			get
			{
				UITweenHelper component = base.gameObject.GetComponent<UITweenHelper>();
				if (component != null)
				{
					return base.gameObject.activeInHierarchy && (component.Tweener.value == component.Tweener.to || TimeRunningOutTween.enabled);
				}
				return base.gameObject.activeInHierarchy;
			}
		}

		public void ClearTimer()
		{
			TimeRunningOutTween.SetTweenToBeginning();
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void SetDisplayTime(int _timeInSeconds)
		{
			if (_timeInSeconds < 0)
			{
				return;
			}
			TimeSpan timeSpan = TimeSpan.FromSeconds(_timeInSeconds);
			string empty = string.Empty;
			empty = ((timeSpan.Minutes <= 0) ? string.Format("{0}", _timeInSeconds) : string.Format("{0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds));
			MessageLabel.text = empty;
			if (!TweenHelper.IsTweening && base.gameObject.activeInHierarchy)
			{
				if (_timeInSeconds < 10 && !TimeRunningOutTween.enabled)
				{
					TimeRunningOutTween.StartTweenPlayForward();
				}
				else if (_timeInSeconds >= 10 && TimeRunningOutTween.enabled)
				{
					TimeRunningOutTween.SetTweenToBeginning();
				}
			}
		}
	}
}
