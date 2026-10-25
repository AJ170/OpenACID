using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UITweenedScreenComponent : MonoBehaviour
	{
		public enum ETriggerPhase
		{
			None = 0,
			OnScreenEnter = 1,
			OnScreenExit = 2,
			OnScreenActivate = 3,
			OnScreenDeActivate = 4,
			ManuallyStart = 5
		}

		public enum ETransitionMode
		{
			None = 0,
			Position = 1,
			Alpha = 2,
			Scale = 3,
			Special = 4
		}

		public enum EStartingSide
		{
			Left = 0,
			Right = 1
		}

		[SerializeField]
		private ETransitionMode m_TransitionMode = ETransitionMode.Position;

		[SerializeField]
		private int m_PlaceInLine;

		[SerializeField]
		private ETriggerPhase m_phase;

		[SerializeField]
		private UITweener m_tweener;

		[SerializeField]
		private UIScreen m_screenParent;

		[SerializeField]
		private UIScreen[] m_screens;

		[SerializeField]
		private bool m_anyScreen = true;

		[SerializeField]
		private EStartingSide m_startingSide;

		private bool m_playing;

		public List<EventDelegate> onPlay = new List<EventDelegate>();

		public List<EventDelegate> onFinish = new List<EventDelegate>();

		public ETransitionMode TranititionMode
		{
			get
			{
				return m_TransitionMode;
			}
		}

		public int PlaceinLine
		{
			get
			{
				return m_PlaceInLine;
			}
			set
			{
				m_PlaceInLine = value;
			}
		}

		public ETriggerPhase Phase
		{
			get
			{
				return m_phase;
			}
		}

		public UITweener Tweener
		{
			get
			{
				return m_tweener;
			}
			set
			{
				m_tweener = value;
				SetupListeners();
			}
		}

		public EStartingSide StartingSide
		{
			get
			{
				return m_startingSide;
			}
			set
			{
				m_startingSide = value;
			}
		}

		public bool CanDeactivate
		{
			get
			{
				return !m_playing;
			}
		}

		public void SetToBegining()
		{
			if (m_tweener != null)
			{
				m_tweener.ResetToBeginning();
			}
		}

		public void ForcePlayTween()
		{
			PlayTween(m_tweener);
		}

		public void SetScreenParent(UIScreen _parent)
		{
			if (m_screenParent != null)
			{
				EventDelegate.Remove(m_screenParent.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screenParent.onDeActivate, OnScreenDeActivate);
				EventDelegate.Remove(m_screenParent.onEnter, OnScreenEnter);
				EventDelegate.Remove(m_screenParent.onExit, OnScreenExit);
			}
			m_screenParent = _parent;
		}

		private void Start()
		{
			SetupListeners();
		}

		private void SetupListeners()
		{
			if ((bool)m_tweener)
			{
				EventDelegate.Remove(m_tweener.onFinished, OnFinish);
				EventDelegate.Add(m_tweener.onFinished, OnFinish);
			}
		}

		private void OnEnable()
		{
			if ((bool)m_tweener)
			{
				m_tweener.enabled = false;
			}
			if (m_screenParent != null)
			{
				m_screenParent = NGUITools.FindInParents<UIScreen>(base.gameObject);
			}
			if (m_screenParent != null)
			{
				EventDelegate.Add(m_screenParent.onActivate, OnScreenActivate);
				EventDelegate.Add(m_screenParent.onDeActivate, OnScreenDeActivate);
				EventDelegate.Add(m_screenParent.onEnter, OnScreenEnter);
				EventDelegate.Add(m_screenParent.onExit, OnScreenExit);
			}
		}

		private void OnDisable()
		{
			m_playing = false;
			if (m_screenParent != null)
			{
				EventDelegate.Remove(m_screenParent.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screenParent.onDeActivate, OnScreenDeActivate);
				EventDelegate.Remove(m_screenParent.onEnter, OnScreenEnter);
				EventDelegate.Remove(m_screenParent.onExit, OnScreenExit);
			}
		}

		private void OnDestroy()
		{
			onPlay.Clear();
			onFinish.Clear();
		}

		private void OnScreenEnter()
		{
			if (base.enabled && m_phase == ETriggerPhase.OnScreenEnter && CanTrigger(ETriggerPhase.OnScreenEnter))
			{
				PlayTween(m_tweener);
			}
		}

		private void OnScreenExit()
		{
			if (base.enabled && m_phase == ETriggerPhase.OnScreenExit && CanTrigger(ETriggerPhase.OnScreenExit))
			{
				PlayTween(m_tweener);
			}
		}

		private void OnScreenActivate()
		{
			if (base.enabled && m_phase == ETriggerPhase.OnScreenActivate && CanTrigger(ETriggerPhase.OnScreenActivate))
			{
				PlayTween(m_tweener);
			}
		}

		private void OnScreenDeActivate()
		{
			if (base.enabled && m_phase == ETriggerPhase.OnScreenDeActivate && CanTrigger(ETriggerPhase.OnScreenDeActivate))
			{
				PlayTween(m_tweener);
			}
		}

		private void PlayTween(UITweener _tween)
		{
			if ((bool)_tween)
			{
				_tween.ResetToBeginning();
				m_playing = true;
				Main.Instance.StartCoroutine(StartTransitionAtNextFrame(_tween));
			}
		}

		private IEnumerator StartTransitionAtNextFrame(UITweener _tween)
		{
			yield return new WaitForEndOfFrame();
			EventDelegate.Execute(onPlay);
			_tween.PlayForward();
		}

		private void OnFinish()
		{
			m_playing = false;
			EventDelegate.Execute(onFinish);
		}

		private bool CanTrigger(ETriggerPhase _phase)
		{
			UIScreen screen = ((_phase != ETriggerPhase.OnScreenActivate && _phase != ETriggerPhase.OnScreenEnter) ? Singleton<UIScreenManager>.Instance.CurrentScreen : Singleton<UIScreenManager>.Instance.PreviousScreen);
			return m_anyScreen || (m_screens != null && Contains(m_screens, screen));
		}

		private static bool Contains(UIScreen[] _array, UIScreen _screen)
		{
			for (int i = 0; i < _array.Length; i++)
			{
				if (_array[i] == _screen)
				{
					return true;
				}
			}
			return false;
		}
	}
}
