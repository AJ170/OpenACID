using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.NewsFeedScreen.Data;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.TweenHandling;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen
{
	public class UINewsFeedDetails : UIScreenChild
	{
		[SerializeField]
		private UISprite m_waitingIndicator;

		[SerializeField]
		private GameObject m_newsFeedItemTemplet;

		[SerializeField]
		private UIPanel m_newsFeedScrollPanel;

		[SerializeField]
		private UITable m_newsFeedItemTable;

		[SerializeField]
		private TweenHandler m_tweenHandler;

		[SerializeField]
		private UIExtendedButton m_changeScreenButton;

		[SerializeField]
		private BBTweenPosition m_tweenItemRef;

		[SerializeField]
		private UINewsFeedShopBanners m_newsFeedBanners;

		[SerializeField]
		private UIRect m_shopBannerArea;

		[SerializeField]
		private UIRect m_newsFeedArea;

		private bool m_hasSeenAnimus;

		private int m_waitingForResizeCounter;

		private List<UINewsFeedItemDetails> m_newsFeedItems = new List<UINewsFeedItemDetails>();

		private bool m_initDone;

		private bool m_updateNow;

		public static void StartAnimusAndGoToScreen(EScreenID _targetScreen)
		{
			UIScreen screen = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.NewsFeedScreen);
			UINewsFeedDetails componentInChildren = screen.GetComponentInChildren<UINewsFeedDetails>();
			if (componentInChildren != null)
			{
				componentInChildren.GoToScreen(_targetScreen);
			}
		}

		public void GoToScreen(EScreenID _targetScreen)
		{
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Show();
			if (!m_hasSeenAnimus)
			{
				m_hasSeenAnimus = true;
				UINotificationPopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
				subSystemByType.IsShowingNotificationPaused = false;
				UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.OnEnterAnimusAtStartup();
				if (Main.Instance.IsNetworkEnabled && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialFinished && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.SetFirstAvailableStep() && Singleton<UIScreenManager>.Instance.CurrentScreen.ScreenId != m_screen.ScreenId)
				{
					return;
				}
			}
			Singleton<UIScreenManager>.Instance.ChangeScreen(_targetScreen, new UITweenHandle(null, null));
		}

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Add(m_changeScreenButton.onClick, OnChangeScreenClick);
		}

		protected override void OnScreenDestroy()
		{
			if ((bool)m_screen)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
				EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			}
			EventDelegate.Remove(m_changeScreenButton.onClick, OnChangeScreenClick);
		}

		protected virtual void Start()
		{
			m_initDone = true;
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_newsFeedItems.Count; i++)
			{
				NGUITools.Destroy(m_newsFeedItems[i]);
			}
			m_newsFeedItems.Clear();
			m_newsFeedItems = null;
		}

		private void Update()
		{
			if (m_updateNow)
			{
				m_updateNow = false;
				OnFeedUpdated();
			}
		}

		private void UpdateFeed()
		{
			Singleton<StateManager>.Instance.NewsFeedManager.FetchCachedData();
		}

		private void OnScreenActivate()
		{
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Exclude();
			m_waitingIndicator.gameObject.SetActive(true);
			NewsFeedManager newsFeedManager = Singleton<StateManager>.Instance.NewsFeedManager;
			newsFeedManager.OnFeedUpdated = (NewsFeedManager.OnFeedUpdatedEvent)Delegate.Combine(newsFeedManager.OnFeedUpdated, new NewsFeedManager.OnFeedUpdatedEvent(OnFeedUpdated));
			Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner).OnChanged += new SimpleDelegate.CallbackFunction(OnShopBannerEventChanged);
			Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges).OnChanged += new SimpleDelegate.CallbackFunction(OnProgressionEventChanged);
			m_tweenHandler.ResetToBeginning();
			UpdateFeed();
		}

		private void OnScreenDeActivate()
		{
			NewsFeedManager newsFeedManager = Singleton<StateManager>.Instance.NewsFeedManager;
			newsFeedManager.OnFeedUpdated = (NewsFeedManager.OnFeedUpdatedEvent)Delegate.Remove(newsFeedManager.OnFeedUpdated, new NewsFeedManager.OnFeedUpdatedEvent(OnFeedUpdated));
			Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Shop_Ad_Banner).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopBannerEventChanged);
			Singleton<StateManager>.Instance.EventManager.GetEventManger(EEventType.Event_Progression_Challenges).OnChanged -= new SimpleDelegate.CallbackFunction(OnProgressionEventChanged);
			Singleton<StateManager>.Instance.NewsFeedManager.Abandon();
			UITable newsFeedItemTable = m_newsFeedItemTable;
			newsFeedItemTable.onReposition = (UITable.OnReposition)Delegate.Remove(newsFeedItemTable.onReposition, new UITable.OnReposition(OnTableReposition));
		}

		private void OnChangeScreenClick()
		{
			if (Singleton<UIScreenManager>.Instance.CurrentScreen.ScreenId == m_screen.ScreenId)
			{
				GoToScreen(EScreenID.Mission);
			}
		}

		[ContextMenu("OnFeedUpdated")]
		private void OnFeedUpdated()
		{
			if (Application.isPlaying && !m_initDone && NGUITools.GetActive(this))
			{
				m_updateNow = true;
				return;
			}
			m_newsFeedScrollPanel.alpha = 0f;
			if (m_newsFeedBanners.HasBannersToShow)
			{
				m_newsFeedArea.leftAnchor.Set(m_shopBannerArea.transform, 0f, 550f);
			}
			else
			{
				m_newsFeedArea.leftAnchor.Set(base.transform.parent, 0f, 0f);
			}
			m_shopBannerArea.gameObject.SetActive(m_newsFeedBanners.HasBannersToShow);
			m_waitingIndicator.gameObject.SetActive(false);
			BBList<NewsFeedDataBase> newsFeedEntries = Singleton<StateManager>.Instance.NewsFeedManager.NewsFeedEntries;
			for (int i = m_newsFeedItems.Count; i < newsFeedEntries.Count; i++)
			{
				GameObject gameObject = NGUITools.AddChild(m_newsFeedItemTable.gameObject, m_newsFeedItemTemplet);
				UINewsFeedItemDetails component = gameObject.GetComponent<UINewsFeedItemDetails>();
				m_newsFeedItems.Add(component);
			}
			m_waitingForResizeCounter = 0;
			m_newsFeedArea.ForceUpdate(true);
			for (int j = 0; j < m_newsFeedItems.Count; j++)
			{
				UINewsFeedItemDetails uINewsFeedItemDetails = m_newsFeedItems[j];
				if (j < newsFeedEntries.Count)
				{
					uINewsFeedItemDetails.gameObject.SetActive(true);
					m_waitingForResizeCounter++;
					EventDelegate.Add(uINewsFeedItemDetails.OnResized, OnResized, true);
					uINewsFeedItemDetails.UpdateAnchorTarget(m_newsFeedArea);
					uINewsFeedItemDetails.UpdateData(newsFeedEntries[j]);
				}
				else
				{
					uINewsFeedItemDetails.gameObject.SetActive(false);
				}
			}
		}

		private void OnResized()
		{
			m_waitingForResizeCounter--;
			if (m_waitingForResizeCounter <= 0)
			{
				UITable newsFeedItemTable = m_newsFeedItemTable;
				newsFeedItemTable.onReposition = (UITable.OnReposition)Delegate.Combine(newsFeedItemTable.onReposition, new UITable.OnReposition(OnTableReposition));
				m_newsFeedItemTable.repositionNow = true;
			}
		}

		private void OnTableReposition()
		{
			UITable newsFeedItemTable = m_newsFeedItemTable;
			newsFeedItemTable.onReposition = (UITable.OnReposition)Delegate.Remove(newsFeedItemTable.onReposition, new UITable.OnReposition(OnTableReposition));
			m_tweenHandler.RemoveAllTweendReferences();
			m_tweenHandler.StartTimeDelay = 1f;
			float _tweenDistance = 0f;
			List<Transform> children = m_newsFeedItemTable.Children;
			for (int i = 0; i < children.Count; i++)
			{
				UINewsFeedItemDetails component = children[i].GetComponent<UINewsFeedItemDetails>();
				component.UpdateTween(m_tweenItemRef, i, m_tweenHandler, out _tweenDistance);
			}
			m_newsFeedBanners.UpdateFeedBanners(_tweenDistance, m_tweenHandler);
			m_newsFeedScrollPanel.alpha = 1f;
			m_newsFeedScrollPanel.GetComponent<UIScrollView>().ResetPosition();
			m_tweenHandler.StartTweeningForward();
		}

		private void OnShopBannerEventChanged(params object[] _params)
		{
			OnFeedUpdated();
		}

		private void OnProgressionEventChanged(params object[] _params)
		{
			OnFeedUpdated();
		}
	}
}
