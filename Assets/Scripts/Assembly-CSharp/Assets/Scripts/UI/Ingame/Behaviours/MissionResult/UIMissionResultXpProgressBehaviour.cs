using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.MissionResult
{
	public class UIMissionResultXpProgressBehaviour : MonoBehaviour
	{
		[SerializeField]
		private BBTweenAnimation m_progressAnimation;

		[SerializeField]
		private UIExtendedLabel m_assassinLevel;

		[SerializeField]
		private LevelUpEffectManager m_levelUpEffect;

		[SerializeField]
		private UIProgressBar m_missionStartXpProgressBar;

		[SerializeField]
		private BBTweenProgressBar m_newXpProgressBar;

		[SerializeField]
		private BBTweenTickingCounterText m_xpTextCounter;

		[SerializeField]
		private ParticleSystem m_progressBarParticles;

		[SerializeField]
		private UINotificationPopupBehaviour m_notificationPopupBehaviour;

		[SerializeField]
		private Texture m_skillPointTexture;

		private int m_displayedAssassinLevel;

		private int m_newAssassinLevels;

		private bool m_hasAssassinMaxLevel;

		private long m_maxXpForLevel;

		private long m_remainingXpToAnimate;

		private IAudioHandle m_experienceTickAudio;

		private IAudioHandle m_levelUpAudio;

		public bool IsLevelingUp { get; private set; }

		private void Awake()
		{
			m_experienceTickAudio = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_missionreward_score_loop, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			m_experienceTickAudio.Load();
			m_levelUpAudio = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_missionreward_level_up, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			m_levelUpAudio.Load();
		}

		public void SetMissionData(long _currentAssassinXp, int _assassinsLevel, int _newAssassinLevels, long _maxXpForLevel, bool _hasAssassinMaxLevel, long _totalXpRecived)
		{
			m_displayedAssassinLevel = _assassinsLevel;
			m_newAssassinLevels = _newAssassinLevels;
			m_hasAssassinMaxLevel = _hasAssassinMaxLevel;
			m_maxXpForLevel = _maxXpForLevel;
			m_missionStartXpProgressBar.gameObject.SetActive(true);
			m_missionStartXpProgressBar.value = (float)_currentAssassinXp / (float)m_maxXpForLevel;
			m_missionStartXpProgressBar.ForceUpdate();
			m_assassinLevel.text = m_displayedAssassinLevel.ToString();
			SetupXpProgressAnimation(_currentAssassinXp, _totalXpRecived);
			EventDelegate.Add(m_progressAnimation.OnAnimationsCompleted, OnAnimationsCompleted);
			EventDelegate.Add(m_progressAnimation.OnAnimationsBegin, OnBeforeAnimationBegin);
		}

		public void UpdateScreen()
		{
			m_notificationPopupBehaviour.UpdateNotification();
			if (m_progressBarParticles.isPlaying)
			{
				m_progressBarParticles.playbackSpeed = 1f / UbiCore.Utils.Singleton<Timer>.Instance.timeScale;
			}
		}

		private void OnDestroy()
		{
			if (m_experienceTickAudio != null)
			{
				m_experienceTickAudio.Stop();
				m_experienceTickAudio.Unload();
				m_experienceTickAudio = null;
			}
			if (m_levelUpAudio != null)
			{
				m_levelUpAudio.Stop();
				m_levelUpAudio.Unload();
				m_levelUpAudio = null;
			}
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_progressAnimation.OnAnimationsCompleted, OnAnimationsCompleted);
			EventDelegate.Remove(m_progressAnimation.OnAnimationsBegin, OnBeforeAnimationBegin);
			StopSound();
		}

		private void StopSound()
		{
			if (m_experienceTickAudio != null)
			{
				m_experienceTickAudio.Stop();
			}
		}

		private void IncrementAssassinsLevel()
		{
			if (!m_hasAssassinMaxLevel)
			{
				XpPerLevelTable xpPerLevelTable = Singleton<StateManager>.Instance.TemplateManager.XpPerLevelTable;
				m_displayedAssassinLevel++;
				m_hasAssassinMaxLevel = xpPerLevelTable.IsMaxLevel(m_displayedAssassinLevel);
				m_maxXpForLevel = xpPerLevelTable.GetMaxXpToNextLevel(m_displayedAssassinLevel);
			}
		}

		private void ShowSkillPointsPopup()
		{
			int skillpointForLevel = Singleton<StateManager>.Instance.TemplateManager.LevelSkillPointMapping.GetSkillpointForLevel(m_displayedAssassinLevel);
			m_assassinLevel.text = m_displayedAssassinLevel.ToString();
			if (EntitlementExtensions.IsSkillTrainingUnlocked())
			{
				string headerText = LocalizationManager.Instance.Get(1721, new StringMapper { { "Skill_Points_Gained", skillpointForLevel } });
				string infoText = LocalizationManager.Instance.Get(710);
				m_notificationPopupBehaviour.ShowNotification(headerText, infoText, UINotificationPopupBehaviour.ETextureType.NormalTexture, 0, m_skillPointTexture, 2);
			}
		}

		private void SetupXpProgressAnimation(long _startExp, long _newExp)
		{
			m_remainingXpToAnimate = 0L;
			long num = _startExp + _newExp;
			if (m_hasAssassinMaxLevel)
			{
				m_missionStartXpProgressBar.gameObject.SetActive(true);
				m_missionStartXpProgressBar.value = 1f;
				m_missionStartXpProgressBar.ForceUpdate();
				m_xpTextCounter.from = m_maxXpForLevel;
				m_xpTextCounter.to = m_maxXpForLevel;
				m_xpTextCounter.StringSufix = " / " + LocalizationManager.Instance.GetInteger(m_maxXpForLevel);
				m_xpTextCounter.SetTweenToEnd();
				m_newXpProgressBar.gameObject.SetActive(false);
				m_newXpProgressBar.from = 1f;
				m_newXpProgressBar.to = 1f;
			}
			else
			{
				if (num > m_maxXpForLevel)
				{
					m_remainingXpToAnimate = num - m_maxXpForLevel;
					num = m_maxXpForLevel;
				}
				m_newXpProgressBar.from = (float)_startExp / (float)m_maxXpForLevel;
				m_newXpProgressBar.to = (float)num / (float)m_maxXpForLevel;
				m_newXpProgressBar.SetTweenToEnd();
				m_xpTextCounter.from = _startExp;
				m_xpTextCounter.to = num;
				m_xpTextCounter.StringSufix = " / " + LocalizationManager.Instance.GetInteger(m_maxXpForLevel);
				m_xpTextCounter.SetTweenToEnd();
			}
			m_progressAnimation.SpeedBoost = Math.Max(0.01f, m_newXpProgressBar.to - m_newXpProgressBar.from);
		}

		private void OnBeforeAnimationBegin()
		{
			if (!m_hasAssassinMaxLevel)
			{
				m_experienceTickAudio.Play(true);
				m_progressBarParticles.gameObject.SetActive(true);
				m_progressBarParticles.Play();
			}
			else
			{
				m_progressAnimation.DontStartAnimation();
			}
		}

		private void OnAnimationsCompleted()
		{
			m_experienceTickAudio.Stop();
			m_progressBarParticles.Stop();
			if (m_progressAnimation.IsFinishingAnimationInstantly)
			{
				FinishLvlUpInstantly(m_progressAnimation.IsDeactivating);
			}
			else if (m_newAssassinLevels > 0)
			{
				m_newAssassinLevels--;
				IncrementAssassinsLevel();
				StartCoroutine("AnimateLevelUp");
			}
			else
			{
				m_progressAnimation.ReactivateParentAnimation();
			}
		}

		private IEnumerator AnimateLevelUp()
		{
			IsLevelingUp = true;
			m_levelUpEffect.gameObject.SetActive(true);
			m_levelUpEffect.PlayEffects();
			m_levelUpAudio.Play();
			ShowSkillPointsPopup();
			while (m_levelUpEffect.IsPlayingEfect())
			{
				yield return new WaitForEndOfFrame();
			}
			m_missionStartXpProgressBar.value = 0f;
			m_missionStartXpProgressBar.gameObject.SetActive(false);
			bool hasXpToAnimate = m_remainingXpToAnimate > 0;
			SetupXpProgressAnimation(0L, m_remainingXpToAnimate);
			if (hasXpToAnimate)
			{
				m_progressAnimation.StartAnimation(true);
			}
			else
			{
				m_progressAnimation.ReactivateParentAnimation();
			}
			IsLevelingUp = false;
		}

		private void FinishLvlUpInstantly(bool _addRemainingXp)
		{
			StopCoroutine("AnimateLevelUp");
			m_progressBarParticles.Stop();
			m_levelUpEffect.StopEffects();
			m_levelUpEffect.gameObject.SetActive(false);
			StopSound();
			if (!_addRemainingXp)
			{
				return;
			}
			EventDelegate.Remove(m_progressAnimation.OnAnimationsCompleted, OnAnimationsCompleted);
			EventDelegate.Remove(m_progressAnimation.OnAnimationsBegin, OnBeforeAnimationBegin);
			if (m_remainingXpToAnimate <= 0)
			{
				return;
			}
			m_missionStartXpProgressBar.value = 0f;
			while (m_remainingXpToAnimate > 0)
			{
				if (!IsLevelingUp)
				{
					IncrementAssassinsLevel();
					m_missionStartXpProgressBar.gameObject.SetActive(false);
				}
				IsLevelingUp = false;
				SetupXpProgressAnimation(0L, m_remainingXpToAnimate);
			}
			m_assassinLevel.text = m_displayedAssassinLevel.ToString();
		}
	}
}
