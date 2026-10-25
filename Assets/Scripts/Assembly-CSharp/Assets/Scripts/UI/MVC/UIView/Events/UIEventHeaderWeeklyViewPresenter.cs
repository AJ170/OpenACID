using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Events
{
	public class UIEventHeaderWeeklyViewPresenter : UIEventHeaderViewPresenter
	{
		[CheckAssigned]
		[SerializeField]
		private UIWidget m_descriptionContainer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_rewardAnchor;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_noRewardInfo;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_eventScoreTarget;

		[SerializeField]
		[CheckAssigned]
		private BBTweenTickingCounterText m_eventScoreTween;

		[CheckAssigned]
		[SerializeField]
		private UnityEngine.Animation m_eventScoreAnimation;

		public GameObject EventScoreTarget
		{
			get
			{
				return m_eventScoreTarget;
			}
		}

		public int CurrentEventScore
		{
			get
			{
				return m_eventScoreTween.value;
			}
		}

		public int EventScoreValueLeftToTween
		{
			get
			{
				return (int)m_eventScoreTween.to - m_eventScoreTween.value;
			}
		}

		public UnityEngine.Animation EventScoreAnimation
		{
			get
			{
				return m_eventScoreAnimation;
			}
		}

		public void SetScore(long _currentScore, long _newScore)
		{
			m_eventScoreTween.from = _currentScore;
			m_eventScoreTween.to = _newScore;
			m_eventScoreTween.SetTweenToBeginning();
		}

		public void ShowRewardInfo(bool _visibility)
		{
			m_noRewardInfo.gameObject.SetActive(_visibility);
		}

		public void SetRewardAsAnchorTarget(bool _anchorToRewardTarget)
		{
			UIRect uIRect = ((!_anchorToRewardTarget) ? base.ViewRoot : m_rewardAnchor);
			int num = ((!_anchorToRewardTarget) ? 1 : 0);
			m_descriptionContainer.rightAnchor.Set(uIRect.transform, num, -20f);
			m_descriptionContainer.ResetAnchors();
			m_descriptionContainer.UpdateAnchors();
		}
	}
}
