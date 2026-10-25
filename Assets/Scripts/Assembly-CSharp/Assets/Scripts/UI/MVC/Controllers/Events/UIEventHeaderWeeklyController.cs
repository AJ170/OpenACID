using System;
using System.Collections;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.Models.Events;
using Assets.Scripts.UI.MVC.Models.Misc;
using Assets.Scripts.UI.MVC.UIView.Events;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Events
{
	public class UIEventHeaderWeeklyController : UITabButtonHeaderController
	{
		[CheckAssigned]
		[SerializeField]
		private UIEventHeaderWeeklyViewPresenter m_headerView;

		[CheckAssigned]
		[SerializeField]
		private UIRewardItemController m_rewardItemController;

		[CheckAssigned]
		[SerializeField]
		private UIProgressBarController m_progressionBarController;

		private UIRewardItemController m_currentSelected;

		private UIEventModel m_eventModel;

		private EventProgressionChallenges.EventProgression m_eventProgression;

		private int m_rewardLevel;

		private int m_scoreToStillBeAnimated;

		private int m_totalScoreAfterAnimation;

		private readonly UIProgressBarModel m_progressBarModel = new UIProgressBarModel();

		private bool m_isAnimatingEventScore;

		private UITabButtonModel m_modelAfterAnimationIsComplete;

		private bool IswaitingForRewardAnimation
		{
			get
			{
				return m_isAnimatingEventScore || UIEventScreenChild.IsWaitingForItemClaim;
			}
		}

		public override void DeselectRewardItem()
		{
			if (m_currentSelected != null)
			{
				m_currentSelected.IsHighlighted = false;
				m_currentSelected.Close();
			}
		}

		public override void Initialize(UITabButtonModel _model)
		{
			m_isAnimatingEventScore = false;
			m_modelAfterAnimationIsComplete = null;
			m_rewardLevel = -1;
			UpdateController(_model);
			base.IsInitialized = true;
		}

		public override void UpdateController(UITabButtonModel _model)
		{
			if (IswaitingForRewardAnimation)
			{
				m_modelAfterAnimationIsComplete = _model;
				return;
			}
			m_eventModel = _model as UIEventModel;
			if (m_eventModel != null && m_eventModel.EventProgressionChallenges != null)
			{
				m_currentSelected = null;
				m_eventProgression = m_eventModel.EventProgressionChallenges.EventProgressionData;
				m_scoreToStillBeAnimated = 0;
				long eventScore = Singleton<StateManager>.Instance.ProfileManager.Profile.GetEventScore();
				SetupScoreAnimation((int)eventScore);
				string value = LocalizationManager.Instance.Get(m_eventProgression.MainHeaderTitleKey);
				m_headerView.HeaderDescription = LocalizationManager.Instance.Get(7854, new StringMapper { { "event_name", value } });
				UIItemUtil.UpdateUITexture(m_eventProgression.LayoutTheme.ScreenImageGuiId, m_headerView.EventTexture);
			}
		}

		public override void Show()
		{
			base.Show();
			m_headerView.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			m_headerView.gameObject.SetActive(false);
		}

		public void OnEventScoreItemClaimed(UIRewardItemController _rewardItemController)
		{
			m_isAnimatingEventScore = true;
			int eventScore = _rewardItemController.CustomItemCount;
			_rewardItemController.ClaimReward(true, m_headerView.EventScoreTarget, delegate
			{
				IncreaseCurrentScore(eventScore);
			});
		}

		protected override void OnAwakeUnity()
		{
			base.OnAwakeUnity();
			UIRewardItemController rewardItemController = m_rewardItemController;
			rewardItemController.OnClicked = (Action<UIRewardItemController>)Delegate.Combine(rewardItemController.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
		}

		protected override void OnDestroyUnity()
		{
			base.OnDestroyUnity();
			m_eventProgression = null;
			if (m_rewardItemController != null)
			{
				UIRewardItemController rewardItemController = m_rewardItemController;
				rewardItemController.OnClicked = (Action<UIRewardItemController>)Delegate.Remove(rewardItemController.OnClicked, new Action<UIRewardItemController>(OnRewardItemClicked));
			}
		}

		private void OnRewardItemClicked(UIRewardItemController _controller)
		{
			if (m_currentSelected != null && m_currentSelected != _controller)
			{
				m_currentSelected.IsHighlighted = false;
			}
			m_currentSelected = _controller;
			Action onItemClicked = base.OnItemClicked;
			if (onItemClicked != null)
			{
				onItemClicked();
			}
		}

		private void IncreaseCurrentScore(int _addScore)
		{
			if (base.gameObject.activeInHierarchy)
			{
				StopCoroutine("AnimateReciveScoreAndReward");
				m_headerView.EventScoreAnimation.Stop();
				int currentEventScore = m_headerView.CurrentEventScore;
				m_scoreToStillBeAnimated += m_headerView.EventScoreValueLeftToTween;
				m_scoreToStillBeAnimated += _addScore;
				SetupScoreAnimation(currentEventScore);
				StartCoroutine("AnimateReciveScoreAndReward");
			}
		}

		private IEnumerator AnimateReciveScoreAndReward()
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			UnityEngine.Animation scoreAnimation = m_headerView.EventScoreAnimation;
			scoreAnimation.Play();
			m_progressionBarController.PlayProgressSound();
			yield return wait;
			while (scoreAnimation.isPlaying)
			{
				yield return wait;
			}
			m_progressionBarController.StopProgressSound();
			if (m_rewardItemController.IsActive && m_progressBarModel.AnimationEndValue == m_progressBarModel.MaxValue)
			{
				m_rewardItemController.ClaimReward(true);
			}
			bool restartAnimation = m_scoreToStillBeAnimated > 0;
			SetupScoreAnimation(m_totalScoreAfterAnimation);
			yield return new WaitForSeconds(2f);
			if (restartAnimation)
			{
				StartCoroutine("AnimateReciveScoreAndReward");
				yield break;
			}
			m_isAnimatingEventScore = false;
			UpdateController(m_modelAfterAnimationIsComplete);
		}

		private void SetupScoreAnimation(int _currentScore)
		{
			int _startScoreForLevel = 0;
			int _maxScoreForLevel = 0;
			int rewardLevel = m_rewardLevel;
			bool rewardLevelAndScoreFromTotalScore = GetRewardLevelAndScoreFromTotalScore(_currentScore, ref m_rewardLevel, ref _startScoreForLevel, ref _maxScoreForLevel);
			int num = 0;
			if (!rewardLevelAndScoreFromTotalScore)
			{
				int b = _maxScoreForLevel - _currentScore;
				num = Mathf.Min(m_scoreToStillBeAnimated, b);
				SetupRewardProgressBar(num, _currentScore, _startScoreForLevel, _maxScoreForLevel);
				m_headerView.ShowRewardInfo(false);
				m_progressionBarController.Show();
			}
			else
			{
				num = m_scoreToStillBeAnimated;
				m_headerView.ShowRewardInfo(true);
				m_progressionBarController.Hide();
			}
			m_scoreToStillBeAnimated -= num;
			m_totalScoreAfterAnimation = _currentScore + num;
			m_headerView.SetScore(_currentScore, m_totalScoreAfterAnimation);
			if (rewardLevel != m_rewardLevel)
			{
				UpdateCurrentDisplayedRewardItem();
			}
			m_headerView.SetRewardAsAnchorTarget(m_rewardItemController.IsActive);
		}

		private bool GetRewardLevelAndScoreFromTotalScore(int _currentScore, ref int _rewardLevel, ref int _startScoreForLevel, ref int _maxScoreForLevel)
		{
			bool flag = true;
			int num = 0;
			EventProgressionChallenges.EventProgression.EventProgressionReward[] mainRewards = m_eventProgression.MainRewards;
			for (int i = 0; i < mainRewards.Length; i++)
			{
				int eventScoreMarker = mainRewards[i].EventScoreMarker;
				int num2 = eventScoreMarker - _currentScore;
				if (num2 > 0)
				{
					_rewardLevel = i;
					flag = false;
					_maxScoreForLevel = eventScoreMarker;
					break;
				}
				num = eventScoreMarker;
			}
			_startScoreForLevel = num;
			if (flag)
			{
				int num3 = mainRewards.Length - 1;
				_rewardLevel = num3 + 1;
				_startScoreForLevel = mainRewards[num3].EventScoreMarker;
				_maxScoreForLevel = mainRewards[num3].EventScoreMarker;
			}
			return flag;
		}

		private void SetupRewardProgressBar(int _scoreToAnimate, int _currentScore, int _startScoreForLevel, int _maxScoreForLevel)
		{
			m_progressBarModel.AnimationEndValue = _scoreToAnimate + _currentScore;
			m_progressBarModel.CurrentValue = _currentScore;
			m_progressBarModel.StartValue = _startScoreForLevel;
			m_progressBarModel.MaxValue = _maxScoreForLevel;
			m_progressBarModel.ShowIcons = true;
			m_progressionBarController.UpdateController(m_progressBarModel, true);
		}

		private void UpdateCurrentDisplayedRewardItem()
		{
			EventProgressionChallenges.EventProgression.EventProgressionReward[] mainRewards = m_eventProgression.MainRewards;
			if (mainRewards.Length > m_rewardLevel && mainRewards[m_rewardLevel] != null)
			{
				m_rewardItemController.Initialize(mainRewards[m_rewardLevel]);
			}
			else
			{
				m_rewardItemController.Hide();
			}
		}
	}
}
