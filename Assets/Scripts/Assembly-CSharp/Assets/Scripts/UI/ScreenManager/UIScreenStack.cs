using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Helpers;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIScreenStack
	{
		private class ScreenGateKeeper
		{
			public enum EStages
			{
				ScreenDeactivated = 0,
				OutTweenFinished = 1
			}

			private readonly bool[] m_flags = new bool[Enum.GetNames(typeof(EStages)).Length];

			public UIScreen Screen { get; set; }

			public bool Finished
			{
				get
				{
					for (int i = 0; i < m_flags.Length; i++)
					{
						if (!m_flags[i])
						{
							return false;
						}
					}
					return true;
				}
			}

			public ScreenGateKeeper()
			{
				Clear();
			}

			public void Clear()
			{
				Screen = null;
				for (int i = 0; i < m_flags.Length; i++)
				{
					m_flags[i] = false;
				}
			}

			public void Set(EStages _stage)
			{
				m_flags[(int)_stage] = true;
			}
		}

		public delegate void OnScreenChangeDelegate(EScreenID _from, EScreenID _to);

		public readonly List<EventDelegate> onScreenChanged = new List<EventDelegate>();

		public readonly List<EventDelegate> onScreenPopped = new List<EventDelegate>();

		public readonly List<EventDelegate> onScreenPushed = new List<EventDelegate>();

		private readonly Stack<UIScreen> m_screenStack = new Stack<UIScreen>(6);

		private readonly ScreenGateKeeper m_gateKeeper = new ScreenGateKeeper();

		private Coroutine m_PrepareRoutine;

		public bool IsScreenStacksModal { get; set; }

		public int Depth
		{
			get
			{
				return m_screenStack.Count;
			}
		}

		public UIScreen CurrentScreen
		{
			get
			{
				return (m_screenStack.Count <= 0) ? null : m_screenStack.Peek();
			}
		}

		public UIScreen PreviousScreen { get; private set; }

		public EScreenID LastValidScreenId { get; private set; }

		public event OnScreenChangeDelegate OnScreenChange;

		public void PushScreen(UIScreen _screen, UITweener _tween)
		{
			if (IsScreenStacksModal)
			{
			}
			UIScreen previousScreen = PreviousScreen;
			if (previousScreen != null)
			{
				previousScreen.LooseFocus(previousScreen, _screen);
			}
			if (_screen == m_gateKeeper.Screen)
			{
				if (PreviousScreen != null && PreviousScreen.OutTweener != null)
				{
					EventDelegate.Remove(PreviousScreen.OutTweener.onFinished, OnScreenTweenFinished);
				}
				m_gateKeeper.Clear();
			}
			if (_tween == null)
			{
				_tween = _screen.InTweener;
			}
			m_screenStack.Push(_screen);
			if (m_PrepareRoutine != null)
			{
				Main.Instance.StopCoroutine(m_PrepareRoutine);
			}
			m_PrepareRoutine = Main.Instance.StartCoroutine(Co_PrepareAndActivate(_screen, _tween));
		}

		private IEnumerator Co_PrepareAndActivate(UIScreen _screen, UITweener _tween)
		{
			if (_screen.onPrepareActivate.Count > 0)
			{
				_screen.PrepareActivate();
				yield return new WaitForEndOfFrame();
			}
			m_PrepareRoutine = null;
			ActivateScreen(_screen, _tween);
		}

		public UIScreen PopScreen(UITweener _tween = null)
		{
			PreviousScreen = m_screenStack.Pop();
			if (PreviousScreen != null)
			{
				PreviousScreen.LooseFocus(PreviousScreen, CurrentScreen);
				UpdateLastValidScreenId();
				if (_tween == null)
				{
					_tween = PreviousScreen.OutTweener;
				}
			}
			UIScreen uIScreen = ((m_screenStack.Count <= 0) ? null : m_screenStack.Peek());
			if (uIScreen != null)
			{
				uIScreen.GainFocus();
			}
			m_gateKeeper.Clear();
			m_gateKeeper.Screen = PreviousScreen;
			if (PreviousScreen != null)
			{
				EventDelegate.Add(PreviousScreen.onScreenDeActivated, OnScreenDeactivated, true);
				PreviousScreen.DeActivate();
			}
			if (_tween != null)
			{
				_tween.RewindAndPlay(OnScreenTweenFinished, true);
			}
			else
			{
				m_gateKeeper.Set(ScreenGateKeeper.EStages.OutTweenFinished);
			}
			ManageEndScreenState();
			EventDelegate.Execute(onScreenPopped);
			return PreviousScreen;
		}

		public bool ChangeScreen(EScreenID _screenID, UITweenHandle _tween)
		{
			if (m_screenStack.Count > 0 && (m_screenStack.Peek() == null || m_screenStack.Peek().ScreenId == _screenID))
			{
				return false;
			}
			if (this.OnScreenChange != null)
			{
				this.OnScreenChange((m_screenStack.Count <= 0) ? EScreenID.Invalid : m_screenStack.Peek().ScreenId, _screenID);
			}
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(_screenID);
			UITweener @in = _tween.InTween ?? screen.InTweener;
			UITweener @out = _tween.OutTween ?? ((m_screenStack.Count <= 0) ? null : m_screenStack.Peek().OutTweener);
			return ChangeScreen(screen, @in, @out);
		}

		public void PurgeStack()
		{
			if (!m_gateKeeper.Finished && m_gateKeeper.Screen != null)
			{
				m_gateKeeper.Screen.Exit();
				m_gateKeeper.Screen.Active = false;
			}
			m_screenStack.Clear();
			onScreenChanged.Clear();
			m_gateKeeper.Clear();
			PreviousScreen = null;
		}

		private void UpdateLastValidScreenId()
		{
			if (PreviousScreen != null && !OptionsScreenHelper.IsOptionsScreen(PreviousScreen.ScreenId))
			{
				LastValidScreenId = PreviousScreen.ScreenId;
			}
		}

		private void ClearStack()
		{
			while (m_screenStack.Count > 0)
			{
				UIScreen uIScreen = m_screenStack.Pop();
				uIScreen.Exit();
			}
		}

		private bool ChangeScreen(UIScreen _screen, UITweener _in, UITweener _out)
		{
			PreviousScreen = ((m_screenStack.Count <= 0) ? null : m_screenStack.Pop());
			UpdateLastValidScreenId();
			ClearStack();
			if (PreviousScreen != null)
			{
				m_gateKeeper.Clear();
				m_gateKeeper.Screen = PreviousScreen;
				EventDelegate.Add(PreviousScreen.onScreenDeActivated, OnScreenDeactivated, true);
				EventDelegate.Add(PreviousScreen.onScreenDeActivated, delegate
				{
					PushScreen(_screen, _in);
				}, true);
				_out = _out ?? PreviousScreen.OutTweener;
				if (_out != null && _out.gameObject.activeInHierarchy)
				{
					EventDelegate.Add(_out.onFinished, delegate
					{
						_out.ResetToBeginning();
					}, true);
					_out.RewindAndPlay(OnScreenTweenFinished, true);
				}
				else
				{
					m_gateKeeper.Set(ScreenGateKeeper.EStages.OutTweenFinished);
				}
				PreviousScreen.DeActivate();
			}
			else
			{
				PushScreen(_screen, _in);
			}
			return true;
		}

		private void ActivateScreen(UIScreen _screen, UITweener _tween = null)
		{
			_screen.GainFocus();
			_screen.Active = true;
			_screen.Activate();
			EventDelegate.Execute(onScreenChanged);
			if ((bool)_tween)
			{
				_tween.RewindAndPlay(_screen.Enter, true);
			}
			else
			{
				_screen.Enter();
			}
			EventDelegate.Execute(onScreenPushed);
		}

		private void ManageEndScreenState()
		{
			if (m_gateKeeper.Finished && m_gateKeeper.Screen != null)
			{
				m_gateKeeper.Screen.Exit();
				m_gateKeeper.Screen.Active = false;
			}
		}

		private void OnScreenTweenFinished()
		{
			m_gateKeeper.Set(ScreenGateKeeper.EStages.OutTweenFinished);
			ManageEndScreenState();
		}

		private void OnScreenDeactivated()
		{
			m_gateKeeper.Set(ScreenGateKeeper.EStages.ScreenDeactivated);
			ManageEndScreenState();
		}
	}
}
