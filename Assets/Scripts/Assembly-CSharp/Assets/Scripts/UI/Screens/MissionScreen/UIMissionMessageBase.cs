using System.Collections;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public class UIMissionMessageBase : MonoBehaviour
	{
		public enum EAnimationState
		{
			Opening = 0,
			Open = 1,
			Closing = 2,
			Closed = 3
		}

		private const string c_openAnimationName = "MissionQuestOpen";

		private const string c_closeAnimationName = "MissionQuestClose";

		[SerializeField]
		private GameObject m_mainContainer;

		[SerializeField]
		private UIExtendedButton m_openCloseButton;

		[SerializeField]
		private BoxCollider m_openBoxCollider;

		[SerializeField]
		private UISwapTweenValuesHelper m_swapTweenValuesHelper;

		[SerializeField]
		private UnityEngine.Animation m_openCloseAnimation;

		protected EAnimationState m_currentState = EAnimationState.Closed;

		private bool m_isActivated;

		public bool IsCloseDisabled { get; set; }

		public bool IsActivated
		{
			get
			{
				return m_isActivated;
			}
			protected set
			{
				m_isActivated = value;
				m_mainContainer.SetActive(m_isActivated);
				if (!m_isActivated)
				{
					SetSelectedState(false, true);
				}
			}
		}

		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		protected virtual void Awake()
		{
			EventDelegate.Add(m_openCloseButton.onClick, OnToggleOpenCloseState);
			m_swapTweenValuesHelper.SwapValues();
			InstantChanageAnimationState(EAnimationState.Closed);
		}

		protected virtual void OnDestroy()
		{
			EventDelegate.Remove(m_openCloseButton.onClick, OnToggleOpenCloseState);
		}

		protected void OnToggleOpenCloseState()
		{
			if (m_currentState == EAnimationState.Open)
			{
				ChangeQuestMenuOpenState(false);
			}
			else if (m_currentState == EAnimationState.Closed)
			{
				ChangeQuestMenuOpenState(true);
			}
		}

		protected void OnToggleOpenState()
		{
			ChangeQuestMenuOpenState(true);
		}

		public void ChangeQuestMenuOpenState(bool _isOpenMenu)
		{
			if (IsActivated && ((_isOpenMenu && m_currentState == EAnimationState.Closed) || (!_isOpenMenu && !IsCloseDisabled && m_currentState == EAnimationState.Open)))
			{
				EAnimationState newState = (_isOpenMenu ? EAnimationState.Open : EAnimationState.Closed);
				base.gameObject.SetActive(true);
				StartCoroutine(StartChangeAnimationState(newState));
			}
		}

		protected virtual void SetSelectedState(bool _isSelected, bool _forceUpdate = false)
		{
		}

		protected IEnumerator StartChangeAnimationState(EAnimationState _newState)
		{
			string animationName2 = string.Empty;
			SetSelectedState(false);
			m_openBoxCollider.enabled = false;
			if (_newState == EAnimationState.Open)
			{
				animationName2 = "MissionQuestOpen";
				m_currentState = EAnimationState.Opening;
				m_swapTweenValuesHelper.SwapValues();
			}
			else
			{
				animationName2 = "MissionQuestClose";
				m_currentState = EAnimationState.Closing;
				m_swapTweenValuesHelper.SwapValues();
			}
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			m_openCloseAnimation.SampleAnimation(0f, animationName2);
			yield return wait;
			m_openCloseAnimation.Play(animationName2);
			while (m_openCloseAnimation.isPlaying)
			{
				bool flag = _newState == EAnimationState.Closed;
				m_openBoxCollider.enabled = flag;
				yield return flag;
			}
			m_currentState = _newState;
		}

		protected void InstantChanageAnimationState(EAnimationState _newState)
		{
			StopAllCoroutines();
			string empty = string.Empty;
			SetSelectedState(false);
			m_openBoxCollider.enabled = false;
			if (_newState == EAnimationState.Open)
			{
				empty = "MissionQuestOpen";
				if (m_currentState != 0)
				{
					m_swapTweenValuesHelper.SwapValues();
				}
				m_currentState = EAnimationState.Opening;
			}
			else
			{
				empty = "MissionQuestClose";
				if (m_currentState != EAnimationState.Closing)
				{
					m_swapTweenValuesHelper.SwapValues();
				}
				m_currentState = EAnimationState.Closing;
			}
			m_openCloseAnimation.SampleAnimation(1f, empty);
			m_openBoxCollider.enabled = _newState == EAnimationState.Closed;
			m_currentState = _newState;
		}

		protected virtual void OnEnable()
		{
			if (m_currentState == EAnimationState.Opening)
			{
				InstantChanageAnimationState(EAnimationState.Open);
			}
			else if (m_currentState == EAnimationState.Closing)
			{
				InstantChanageAnimationState(EAnimationState.Closed);
			}
		}
	}
}
