using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIScreenManager : Singleton<UIScreenManager>, IManager
	{
		public enum EAtlasType
		{
			Shared = 0,
			Animus = 1,
			InGame = 2
		}

		private const string s_PopupPath = "UI/Animus/Screens/UIPopup";

		private const string s_PopupBarePath = "UI/Animus/Screens/UIPopupBare";

		private const string s_LoadingOverlayPath = "UI/Animus/Screens/LoadingOverlay";

		private UIAtlas[] m_atlases;

		private bool m_assassinContainerLastState;

		private readonly UIScreenStack m_modalScreenStack = new UIScreenStack();

		private readonly UIScreenStack m_baseScreenStack = new UIScreenStack();

		private readonly BBDictionary<int, UIScreen> m_screens = new BBDictionary<int, UIScreen>(32);

		private EScreenID m_startScreenId = EScreenID.Mission;

		private UIScreenHistoryDetails m_lastScreenHistory = new UIScreenHistoryDetails();

		public readonly List<EventDelegate> onScreenChanged = new List<EventDelegate>();

		public readonly List<EventDelegate> onScreenPopped = new List<EventDelegate>();

		public readonly List<EventDelegate> onScreenPushed = new List<EventDelegate>();

		public readonly List<EventDelegate> onModalScreenChanged = new List<EventDelegate>();

		public readonly List<EventDelegate> onModalScreenPopped = new List<EventDelegate>();

		public readonly List<EventDelegate> onScreenManagerInitialized = new List<EventDelegate>();

		public bool IsInitialized { get; private set; }

		public bool OverrideLockedScreen { get; set; }

		public UIScreen CurrentModalScreen
		{
			get
			{
				if (m_modalScreenStack.Depth > 0)
				{
					return m_modalScreenStack.CurrentScreen;
				}
				return null;
			}
		}

		public UIScreen PreviousModalScreen
		{
			get
			{
				return m_modalScreenStack.PreviousScreen;
			}
		}

		public UIScreen CurrentScreen
		{
			get
			{
				return m_baseScreenStack.CurrentScreen;
			}
		}

		public UIScreen PreviousScreen
		{
			get
			{
				return m_baseScreenStack.PreviousScreen;
			}
		}

		public EScreenID LastValidScreenId
		{
			get
			{
				return m_baseScreenStack.LastValidScreenId;
			}
		}

		public EScreenID CurrentScreenId
		{
			get
			{
				return (!(CurrentScreen != null)) ? EScreenID.Invalid : CurrentScreen.ScreenId;
			}
		}

		public bool HasStartedTutorial { get; set; }

		public UIScreenHistoryDetails LastScreenHistory
		{
			get
			{
				return m_lastScreenHistory;
			}
		}

		public UISceneData SceneData { get; set; }

		public event UIScreenStack.OnScreenChangeDelegate OnScreenChange;

		public UIAtlas GetAtlasByType(EAtlasType _type)
		{
			string atlasName = UIDataMapper.SHARED_ATLAS_NAME;
			switch (_type)
			{
			case EAtlasType.Animus:
				atlasName = UIDataMapper.ANIMUS_ATLAS_NAME;
				break;
			case EAtlasType.Shared:
				atlasName = UIDataMapper.SHARED_ATLAS_NAME;
				break;
			case EAtlasType.InGame:
				atlasName = UIDataMapper.INGAME_ATLAS_NAME;
				break;
			}
			UIAtlas uIAtlas = null;
			if (m_atlases != null)
			{
				uIAtlas = Array.Find(m_atlases, (UIAtlas atlas) => string.Compare(atlas.name, atlasName, StringComparison.InvariantCultureIgnoreCase) == 0);
			}
			if (uIAtlas == null)
			{
				m_atlases = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
				if (m_atlases != null)
				{
					uIAtlas = Array.Find(m_atlases, (UIAtlas atlas) => string.Compare(atlas.name, atlasName, StringComparison.InvariantCultureIgnoreCase) == 0);
				}
			}
			return uIAtlas;
		}

		public void ShowPopup(UIScreen _screen, UITweener _tween)
		{
			if (m_modalScreenStack.Depth == 0 && m_baseScreenStack.Depth > 0)
			{
				m_baseScreenStack.CurrentScreen.LooseFocus(m_baseScreenStack.CurrentScreen, _screen);
			}
			m_modalScreenStack.PushScreen(_screen, _tween);
		}

		public void HidePopupIfPopupTypeIsOpend(UIPopup.EContentType _type)
		{
			if (m_modalScreenStack.Depth != 0)
			{
				UIScreen currentScreen = m_modalScreenStack.CurrentScreen;
				if (currentScreen != null && currentScreen.ScreenId == EScreenID.UIPopUp && (currentScreen as UIPopup).CurrentContentType == _type)
				{
					HideCurrentPopupScreen();
				}
			}
		}

		public void HideCurrentPopupScreen(UITweener _tween = null)
		{
			if (m_modalScreenStack.Depth != 0)
			{
				m_modalScreenStack.PopScreen(_tween);
				if (m_modalScreenStack.Depth == 0 && m_baseScreenStack.Depth > 0)
				{
					m_baseScreenStack.CurrentScreen.GainFocus();
				}
			}
		}

		public void ClearStack(UITweener _tween = null)
		{
			if (m_modalScreenStack.Depth > 0)
			{
				m_modalScreenStack.PurgeStack();
			}
			if (m_baseScreenStack.Depth > 0)
			{
				m_baseScreenStack.PopScreen(_tween);
				m_baseScreenStack.PurgeStack();
			}
		}

		public void GoToPreviousScreen()
		{
			if (!(CurrentScreen == null))
			{
				ChangeScreen(CurrentScreen.m_previousScreen, new UITweenHandle(null, null));
			}
		}

		public void PushScreen(UIScreen _screen, UITweener _tween)
		{
			EScreenID eScreenID = ((!(m_modalScreenStack.CurrentScreen != null)) ? EScreenID.Invalid : m_modalScreenStack.CurrentScreen.ScreenId);
			if (SceneData != null && SceneData.AssassinContainer != null)
			{
				m_assassinContainerLastState = SceneData.AssassinContainer.IsVisible;
			}
			m_baseScreenStack.PushScreen(_screen, _tween);
		}

		public UIScreen PopScreen(UITweener _tween = null)
		{
			UIScreen result = m_baseScreenStack.PopScreen(_tween);
			if (SceneData != null && SceneData.AssassinContainer != null)
			{
				SceneData.AssassinContainer.SetVisibility(m_assassinContainerLastState);
			}
			return result;
		}

		public void ChangeScreen(EScreenID _screenID, UITweenHandle _tween)
		{
			if (OverrideLockedScreen || IsScreenUnlocked(_screenID))
			{
				OverrideLockedScreen = false;
				if (m_baseScreenStack.ChangeScreen(_screenID, _tween))
				{
					LastScreenHistory.OnScreenChange(_screenID);
				}
			}
		}

		public bool IsScreenUnlocked(EScreenID _screenID)
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.HasCompletedAssassinCreation)
			{
				return true;
			}
			UIButtonHeader.EHeaderType buttonHeaderTypeByScreen = UIButtonHeaderUtil.GetButtonHeaderTypeByScreen(_screenID);
			return UIButtonHeaderUtil.IsButtonHeaderTypeUnlocked(buttonHeaderTypeByScreen);
		}

		public UIScreen GetScreen(EScreenID _id)
		{
			UIScreen value = null;
			m_screens.TryGetValue((int)_id, out value);
			return value;
		}

		public void RegisterScreen(UIScreen _screen, bool _enabled = false)
		{
			if (!m_screens.ContainsKey((int)_screen.ScreenId))
			{
				_screen.Active = _enabled;
				m_screens[(int)_screen.ScreenId] = _screen;
			}
		}

		public void Initialize()
		{
			UpdateScreenList();
			Transform transform = UbiCore.Utils.Singleton<UiMain>.Instance.UISharedRoot.transform;
			if (GetScreen(EScreenID.UIPopUp) == null)
			{
				CreateAndRegisterScreen("UI/Animus/Screens/UIPopup", "__popup", transform);
			}
			if (GetScreen(EScreenID.UIPopUpBare) == null)
			{
				CreateAndRegisterScreen("UI/Animus/Screens/UIPopupBare", "__popupbare", transform);
			}
			if (GetScreen(EScreenID.LoadingOverlay) == null)
			{
				CreateAndRegisterScreen("UI/Animus/Screens/LoadingOverlay", "__LoadingOverlay", transform);
			}
			m_baseScreenStack.IsScreenStacksModal = false;
			m_modalScreenStack.IsScreenStacksModal = true;
			EventDelegate.Add(m_modalScreenStack.onScreenChanged, OnModalScreenChanged);
			EventDelegate.Add(m_modalScreenStack.onScreenPopped, OnModalScreenPopped);
			EventDelegate.Add(m_baseScreenStack.onScreenChanged, OnScreenChanged);
			EventDelegate.Add(m_baseScreenStack.onScreenPopped, OnScreenPopped);
			EventDelegate.Add(m_baseScreenStack.onScreenPushed, OnScreenPushed);
			m_baseScreenStack.OnScreenChange += OnScreenChangeCallback;
			List<int> list = new List<int>(m_screens.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				m_screens[list[i]].Init();
			}
			m_atlases = Resources.FindObjectsOfTypeAll(typeof(UIAtlas)) as UIAtlas[];
			IsInitialized = true;
			EventDelegate.Execute(onScreenManagerInitialized);
		}

		public void PurgeManager()
		{
			m_baseScreenStack.PurgeStack();
			m_modalScreenStack.PurgeStack();
			m_screens.Clear();
			onScreenChanged.Clear();
			onModalScreenChanged.Clear();
			onModalScreenPopped.Clear();
			onScreenManagerInitialized.Clear();
			FixScreenAfterMission();
			SceneData = null;
			m_atlases = null;
			IsInitialized = false;
			HasStartedTutorial = false;
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.PurgeAnimusScreen();
			}
			m_baseScreenStack.OnScreenChange -= this.OnScreenChange;
		}

		public void ActivateStartupScreen()
		{
			List<int> list = new List<int>(m_screens.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				m_screens[list[i]].Active = false;
			}
			EScreenID screenID = m_startScreenId;
			UIScreenHistoryDetails.EUIScreenGroups groupIdByScreenId = UIScreenHistoryDetails.GetGroupIdByScreenId(m_startScreenId, Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType);
			UIScreenHistoryDetails.ScreenHistory lastScreenByScreenGroup = LastScreenHistory.GetLastScreenByScreenGroup(groupIdByScreenId);
			if (lastScreenByScreenGroup != null && lastScreenByScreenGroup.ScreenId != EScreenID.Invalid)
			{
				screenID = lastScreenByScreenGroup.ScreenId;
			}
			if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.HasCompletedAssassinCreation && UbiCore.Utils.Singleton<UiMain>.Instance.ShouldShowAnimusStartupScreens)
			{
				screenID = EScreenID.NewsFeedScreen;
			}
			else if (Main.Instance.IsNetworkEnabled && !UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsTutorialFinished && UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.SetFirstAvailableStep())
			{
				return;
			}
			ChangeScreen(screenID, new UITweenHandle());
		}

		public void ValidateMissionListOnError()
		{
			if (!Singleton<StateManager>.Instance.MissionSelectionManager.IsSelectedMissionCampaign)
			{
				UIScreenHistoryDetails.ScreenHistory lastScreenByScreenGroup = m_lastScreenHistory.GetLastScreenByScreenGroup(UIScreenHistoryDetails.EUIScreenGroups.ContractMission);
				if (Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType != lastScreenByScreenGroup.MenuType)
				{
					Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = lastScreenByScreenGroup.MenuType;
				}
				if (CurrentScreenId != lastScreenByScreenGroup.ScreenId)
				{
					ChangeScreen(lastScreenByScreenGroup.ScreenId, new UITweenHandle());
				}
			}
		}

		private void OnScreenChangeCallback(EScreenID _from, EScreenID _to)
		{
			if (this.OnScreenChange != null)
			{
				this.OnScreenChange(_from, _to);
			}
		}

		private void OnModalScreenChanged()
		{
			EventDelegate.Execute(onModalScreenChanged);
		}

		private void OnModalScreenPopped()
		{
			EventDelegate.Execute(onModalScreenPopped);
		}

		private void OnScreenChanged()
		{
			EventDelegate.Execute(onScreenChanged);
		}

		private void OnScreenPopped()
		{
			EventDelegate.Execute(onScreenPopped);
		}

		private void OnScreenPushed()
		{
			EventDelegate.Execute(onScreenPushed);
		}

		private void CreateAndRegisterScreen(UIPath _path, string _name, Transform _parent)
		{
			GameObject gameObject = NGUITools.AddChild(_parent.gameObject, Resources.Load(_path) as GameObject);
			if (gameObject != null)
			{
				UIScreen component = gameObject.GetComponent<UIScreen>();
				if (component != null)
				{
					component.Init();
					RegisterScreen(component);
				}
				gameObject.name = _name;
				UIPanel component2 = gameObject.GetComponent<UIPanel>();
				component2.SetAnchor(_parent.gameObject, 0, 0, 0, 0);
			}
		}

		private void UpdateScreenList()
		{
			m_screens.Clear();
			UIRoot[] array = UIRoot.list.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				UIScreen[] componentsInChildren = array[i].GetComponentsInChildren<UIScreen>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					RegisterScreen(componentsInChildren[j]);
				}
			}
		}

		private void FixScreenAfterMission()
		{
			LastScreenHistory.FixHistoryAfterMission();
			Singleton<StateManager>.Instance.MissionSelectionManager.FixMissionScreenOnClear();
		}
	}
}
