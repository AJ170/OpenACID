using System.Collections;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial
{
	public abstract class TutorialFlowBase : UIScreenChild, ITutorialFlowable
	{
		private class EffectEntry
		{
			public GameObject Effect { get; set; }

			public bool DelayedActivated { get; set; }
		}

		public delegate void BlockerTappedDelegate();

		private bool m_isAppQuitting;

		private List<EffectEntry> m_effects = new List<EffectEntry>();

		private bool m_showEffects = true;

		protected BBList<FullscreenBlocker> m_blockers = new BBList<FullscreenBlocker>();

		protected FullscreenBlocker m_highestBlocker;

		private List<UIEventTrigger> m_triggers = new List<UIEventTrigger>();

		private bool m_onSetupDone;

		public abstract ETutorialCategory Category { get; }

		public abstract STutorialFlowId Index { get; }

		public virtual string Name
		{
			get
			{
				return typeof(TutorialFlowBase).ToString();
			}
		}

		public bool IsActive { get; protected set; }

		public static event BlockerTappedDelegate BlockerTapped;

		private static void OnBlockerTapped()
		{
			BlockerTappedDelegate blockerTapped = TutorialFlowBase.BlockerTapped;
			if (blockerTapped != null)
			{
				blockerTapped();
			}
		}

		protected override void OnScreenInit()
		{
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(Category) != null)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.RegisterToCategory(this);
			}
			base.gameObject.SetActive(false);
		}

		protected override void OnScreenDestroy()
		{
			if (!m_isAppQuitting)
			{
				if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager != null && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(Category) != null)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.UnRegisterFromCategory(this);
				}
				if (m_screen != null)
				{
					EventDelegate.Remove(m_screen.onSetup, OnSetup);
				}
			}
		}

		protected virtual void OnApplicationQuit()
		{
			m_isAppQuitting = true;
		}

		public virtual void Activate()
		{
			IsActive = true;
			base.gameObject.SetActive(true);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenPopped, OnScreenPushPop);
			EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenPushed, OnScreenPushPop);
			CollectBlockers();
			InitializeBlockers();
			EScreenID screenId = GetScreenId();
			UIScreenManager instance = Singleton<UIScreenManager>.Instance;
			if (screenId != EScreenID.Invalid && screenId != instance.CurrentScreenId)
			{
				instance.ChangeScreen(screenId, new UITweenHandle(null, null));
			}
			if (screenId == instance.CurrentScreenId)
			{
				UIScreen currentScreen = instance.CurrentScreen;
				if (currentScreen != null && currentScreen.IsEntered)
				{
					OnSetup();
				}
				else if (m_screen != null)
				{
					EventDelegate.Add(m_screen.onSetup, OnSetup);
				}
			}
			else if (m_screen != null)
			{
				EventDelegate.Add(m_screen.onSetup, OnSetup);
			}
			ActivateBackButton(false);
		}

		public void ClearEffects()
		{
			if (m_effects == null)
			{
				return;
			}
			for (int i = 0; i < m_effects.Count; i++)
			{
				if (m_effects[i].Effect != null)
				{
					m_effects[i].Effect.SetActive(false);
					Object.Destroy(m_effects[i].Effect);
					m_effects[i].Effect = null;
				}
			}
			m_effects.ClearIfNotEmpty();
		}

		public virtual void Deactivate()
		{
			if (IsActive)
			{
				IsActive = false;
				if (base.gameObject != null)
				{
					base.gameObject.SetActive(false);
				}
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnScreenPushPop);
				EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPushed, OnScreenPushPop);
				ClearEffects();
				for (int i = 0; i < m_triggers.Count; i++)
				{
					EventDelegate.Remove(m_triggers[i].onPress, OnBlockerTap);
				}
				m_triggers.Clear();
				m_blockers.Clear();
				m_highestBlocker = null;
				ActivateBackButton(true);
			}
		}

		private void OnDestroy()
		{
			OnDestroyBase();
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPopped, OnScreenPushPop);
			EventDelegate.Remove(Singleton<UIScreenManager>.Instance.onScreenPushed, OnScreenPushPop);
			ClearEffects();
			if (m_triggers != null)
			{
				for (int i = 0; i < m_triggers.Count; i++)
				{
					EventDelegate.Remove(m_triggers[i].onPress, OnBlockerTap);
				}
				m_triggers.Clear();
			}
			m_blockers.Clear();
			m_highestBlocker = null;
		}

		protected virtual void OnDestroyBase()
		{
		}

		protected void OnSetup()
		{
			if (!m_onSetupDone)
			{
				if (m_screen != null)
				{
					EventDelegate.Remove(m_screen.onSetup, OnSetup);
				}
				OnSetupInternal();
				m_onSetupDone = true;
			}
		}

		protected virtual void OnSetupInternal()
		{
		}

		public abstract bool ConditionAllowShow();

		protected void ActivateBackButton(bool _activate)
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.BackButton != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.BackButton.gameObject.SetActive(_activate);
			}
		}

		protected void CancelTutorial()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishTutorial();
		}

		protected void UnblockOptionsButton(FullscreenBlocker _blocker)
		{
			UIWidget optionsButton = Singleton<UIScreenManager>.Instance.SceneData.OptionsButton;
			if (Singleton<UIScreenManager>.Instance.SceneData != null && optionsButton != null)
			{
				UnblockWidget(optionsButton, _blocker);
			}
		}

		protected void UnblockWidget(UIWidget _widget, FullscreenBlocker _blocker, UIWidget[] _excludedWidgets = null)
		{
			UIWidget[] widgets = _excludedWidgets ?? _widget.GetComponentsInChildren<UIWidget>(true);
			_blocker.AddNonBlocked(widgets);
		}

		protected void UnblockWidget(UIToggle _widget, FullscreenBlocker _blocker)
		{
			UIWidget[] componentsInChildren = _widget.gameObject.GetComponentsInChildren<UIWidget>(true);
			_blocker.AddNonBlocked(componentsInChildren);
		}

		private void OnScreenPushPop()
		{
			m_showEffects = Singleton<UIScreenManager>.Instance.CurrentScreenId == GetScreenId();
			UpdateEffectsVisibility();
		}

		private void UpdateEffectsVisibility()
		{
			for (int i = 0; i < m_effects.Count; i++)
			{
				m_effects[i].Effect.SetActive(m_showEffects && m_effects[i].DelayedActivated);
			}
		}

		private EffectEntry AddEffect(GameObject _effect)
		{
			EffectEntry effectEntry = new EffectEntry();
			effectEntry.Effect = _effect;
			effectEntry.DelayedActivated = false;
			m_effects.Add(effectEntry);
			return effectEntry;
		}

		protected GameObject CreateButtonHighlight(GameObject _parent, Vector3 _offset)
		{
			GameObject gameObject = CreateButtonHighlightPlain(_parent, _offset, GetHightlightLayer(), GetHightlightSortingOrder());
			EffectEntry entry = AddEffect(gameObject);
			StartCoroutine(ActivateDelayed(entry, 1.5f));
			return gameObject;
		}

		public static GameObject CreateButtonHighlightPlain(GameObject _parent, Vector3 _offset, int _highlightLayer, int _highlightSortingOrder)
		{
			Object original = UnityEngine.Resources.Load("UI/Misc/ButtonHighlight");
			GameObject gameObject = Object.Instantiate(original) as GameObject;
			if (_highlightLayer > 0)
			{
				NGUITools.SetLayer(gameObject, _highlightLayer);
			}
			if (_highlightSortingOrder > 0)
			{
				ChangeRenderQueue[] componentsInChildren = gameObject.GetComponentsInChildren<ChangeRenderQueue>(true);
				if (componentsInChildren != null)
				{
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].sortingOrder = _highlightSortingOrder;
					}
				}
			}
			gameObject.transform.parent = _parent.transform;
			Vector3 vector = Vector3.zero;
			UIWidget component = _parent.GetComponent<UIWidget>();
			if (component != null)
			{
				vector = UIUtil.GetWidgetCenter(component);
			}
			gameObject.transform.localPosition = vector + _offset;
			gameObject.SetActive(false);
			return gameObject;
		}

		public virtual EScreenID GetScreenId()
		{
			return EScreenID.Invalid;
		}

		private IEnumerator ActivateDelayed(EffectEntry _entry, float _delaySeconds)
		{
			yield return StartCoroutine(Timer.WaitForSecondsUnscaled(_delaySeconds));
			if (_entry != null && _entry.Effect != null)
			{
				_entry.DelayedActivated = true;
				UpdateEffectsVisibility();
			}
		}

		protected void CollectBlockers()
		{
			GameObject rootGameObject = base.gameObject.GetRootGameObject();
			FullscreenBlocker[] componentsInChildren = rootGameObject.GetComponentsInChildren<FullscreenBlocker>(true);
			foreach (FullscreenBlocker fullscreenBlocker in componentsInChildren)
			{
				m_blockers.Add(fullscreenBlocker);
				if (m_highestBlocker == null || fullscreenBlocker.FinalDepthIncrement > m_highestBlocker.FinalDepthIncrement)
				{
					m_highestBlocker = fullscreenBlocker;
				}
			}
		}

		protected void InitializeBlockers()
		{
			for (int i = 0; i < m_blockers.Count; i++)
			{
				UIEventTrigger uIEventTrigger = m_blockers[i].gameObject.AddComponentIfNotExist<UIEventTrigger>();
				EventDelegate.Add(uIEventTrigger.onPress, OnBlockerTap);
				m_triggers.Add(uIEventTrigger);
			}
		}

		public void OnBlockerTap()
		{
			OnBlockerTapped();
		}

		public void ActivateMenuBarBlockers(bool _active)
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.EnableBarBlockers(_active);
			}
		}

		protected virtual int GetHightlightLayer()
		{
			return -1;
		}

		protected virtual int GetHightlightSortingOrder()
		{
			return -1;
		}
	}
}
