using System;
using AnimationOrTween;
using Assets.Scripts.Audio;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.LoadoutScreen
{
	public class LoadoutScreenNavigation : MonoBehaviour
	{
		private enum EInfoType
		{
			MissionBriefing = 0,
			History = 1
		}

		[SerializeField]
		private GameObject m_InfoButton;

		[SerializeField]
		private GameObject m_Body1;

		[SerializeField]
		private GameObject m_Body2;

		[SerializeField]
		private UIScrollBar m_scrollBar;

		[SerializeField]
		private BBTweenRotation m_tweenRotation;

		[SerializeField]
		private GameObject m_difficultyIndicator;

		[SerializeField]
		private GameObject m_startButton;

		[SerializeField]
		private UITweener m_showHeaderAnimation;

		[SerializeField]
		private UITweener[] ShowAnimationsAlwaysOn;

		[SerializeField]
		private UITweener[] ShowAnimations;

		private bool m_isClosingOldInfo;

		private bool m_startTweenOnUpdate;

		private EInfoType m_infoTypeShown;

		private EInfoType NextInfoTypeShown
		{
			get
			{
				return m_infoTypeShown;
			}
			set
			{
				if (m_infoTypeShown != value)
				{
					m_infoTypeShown = value;
					if (!m_isClosingOldInfo && !m_startTweenOnUpdate)
					{
						m_isClosingOldInfo = true;
						EventDelegate.Add(m_tweenRotation.onFinished, OnTweenFinished, true);
						m_tweenRotation.StartTweenPlayForward();
						AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_small);
					}
				}
			}
		}

		public void Show(Action _onUpdateContent)
		{
			m_showHeaderAnimation.PlayForward();
			for (int i = 0; i < ShowAnimations.Length; i++)
			{
				UITweener uITweener = ShowAnimations[i];
				ResetTween(uITweener);
				uITweener.PlayForward();
			}
			for (int j = 0; j < ShowAnimationsAlwaysOn.Length; j++)
			{
				UITweener uITweener2 = ShowAnimationsAlwaysOn[j];
				ResetTween(uITweener2);
				uITweener2.PlayForward();
			}
			if (_onUpdateContent != null)
			{
				_onUpdateContent();
			}
		}

		public void Hide(Action _onUpdateContent)
		{
			if (NextInfoTypeShown == EInfoType.MissionBriefing)
			{
				for (int i = 0; i < ShowAnimations.Length; i++)
				{
					UITweener uITweener = ShowAnimations[i];
					if (uITweener.direction == Direction.Forward)
					{
						uITweener.Toggle();
					}
					uITweener.ResetToBeginning();
					uITweener.PlayReverse();
				}
			}
			for (int j = 0; j < ShowAnimationsAlwaysOn.Length; j++)
			{
				UITweener uITweener2 = ShowAnimationsAlwaysOn[j];
				if (uITweener2.direction == Direction.Forward)
				{
					uITweener2.Toggle();
				}
				uITweener2.ResetToBeginning();
				uITweener2.PlayReverse();
			}
			_onUpdateContent();
		}

		public void ResetNow()
		{
			m_showHeaderAnimation.ResetToBeginning();
			EventDelegate.Remove(m_tweenRotation.onFinished, OnTweenFinished);
			m_isClosingOldInfo = false;
			m_startTweenOnUpdate = false;
			m_infoTypeShown = EInfoType.MissionBriefing;
			SetShopTypeVisibility(EInfoType.MissionBriefing, true);
			m_tweenRotation.SetTweenToBeginning();
			for (int i = 0; i < ShowAnimations.Length; i++)
			{
				UITweener uITweener = ShowAnimations[i];
				if (uITweener.direction == Direction.Forward)
				{
					uITweener.Toggle();
				}
				uITweener.ResetToBeginning();
			}
			for (int j = 0; j < ShowAnimationsAlwaysOn.Length; j++)
			{
				UITweener uITweener2 = ShowAnimationsAlwaysOn[j];
				if (uITweener2.direction == Direction.Forward)
				{
					uITweener2.Toggle();
				}
				uITweener2.ResetToBeginning();
			}
		}

		private void Awake()
		{
		}

		private void Start()
		{
			EventDelegate.Add(m_InfoButton.AddComponentIfNotExist<UIButton>().onClick, OnClicked);
		}

		private void Update()
		{
			if (m_startTweenOnUpdate)
			{
				m_startTweenOnUpdate = false;
				m_isClosingOldInfo = false;
				SetShopTypeVisibility(NextInfoTypeShown, true);
				m_tweenRotation.StartTweenPlayReverse();
			}
		}

		private void SetShopTypeVisibility(EInfoType _infoType, bool _visible)
		{
			bool active = false;
			bool active2 = false;
			switch (_infoType)
			{
			case EInfoType.MissionBriefing:
				active = _visible;
				break;
			case EInfoType.History:
				active2 = _visible;
				break;
			}
			m_Body1.SetActive(active);
			m_difficultyIndicator.SetActive(active);
			m_startButton.SetActive(active);
			m_Body2.SetActive(active2);
		}

		private static void ResetTween(UITweener _tween)
		{
			if (_tween.direction == Direction.Reverse)
			{
				_tween.Toggle();
			}
			_tween.ResetToBeginning();
		}

		private void OnTweenFinished()
		{
			m_startTweenOnUpdate = true;
			m_scrollBar.value = 0f;
		}

		private void OnClicked()
		{
			NextInfoTypeShown = ((NextInfoTypeShown == EInfoType.MissionBriefing) ? EInfoType.History : EInfoType.MissionBriefing);
		}
	}
}
