using System;
using System.Collections;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Ingame.Behaviours.MissionResult;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.MVC.UIView.Assassin;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.CodexScreen;
using Assets.Scripts.UI.Screens.InventoryScreen;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.Screens.SkillTreeScreen;
using Assets.Scripts.UI.SocialScreen;
using Assets.Scripts.UI.Utils;
using InControl;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class UIGamepadHandler : MonoBehaviour
{
	private delegate int PriorityDelegate(Component _component);

	private const float InputThresholdSq = 0.040000003f;

	private BBList<UIGamepadSelectable> BottomButtons = new BBList<UIGamepadSelectable>();

	public BBList<UIGamepadSelectable> CenterSelectables = new BBList<UIGamepadSelectable>();

	private UIGamepadSelectable BackButton;

	private UIMenubars Menubars;

	private GameObject[] ExtraInteractables;

	private BBDictionary<Type, PriorityDelegate> InteractiveTypes;

	public SelectionEffectManager m_selectionManager;

	public GamepadWrapper m_gamepad;

	public static float SelectionPeriod = 0.5f;

	public static float SelectionFastPeriod = 0.1f;

	public float ScrollSensitivity = 5f;

	public float SelectionSensitivity = 10f;

	public float SelectionMaxAngle = 80f;

	public float PadInactiveDelay = 30f;

	public float PadFastSelectionDelay = 1f;

	public float AutoSelectionDelay = 0.5f;

	private bool m_padDetected;

	private bool m_padActive;

	private bool m_buttonHeld;

	private float m_activeClock;

	private float m_inactiveClock;

	private float m_selectionClock;

	private UIGamepadSelectable m_currentSelection;

	private UIExtendedWrapContent m_currentScrollContent;

	private Vector3 m_currentScrollItemLocalPosition;

	private int m_selectableLayerMask;

	private UIScreen m_screenWaitingForSetup;

	private float m_delayAutoSelectClock;

	private bool m_dropDownActive;

	public GameObject CurrentScreen;

	public bool PadDetected
	{
		get
		{
			return m_padDetected;
		}
	}

	public bool PadActive
	{
		get
		{
			return m_padActive;
		}
	}

	public UIGamepadSelectable CurrentSelection
	{
		get
		{
			return m_currentSelection;
		}
		private set
		{
			if (m_currentSelection == value)
			{
				return;
			}
			m_currentSelection = value;
			if (m_currentSelection != null)
			{
				if (m_currentSelection.ScrollView != null)
				{
					m_currentSelection.ScrollView.InvalidateBounds();
				}
				UIWrapScrollView uIWrapScrollView = m_currentSelection.ScrollView as UIWrapScrollView;
				if (uIWrapScrollView != null)
				{
					CurrentScrollContent = uIWrapScrollView.ExtendedWraperContent as UIExtendedWrapContent;
				}
				else
				{
					CurrentScrollContent = null;
				}
				m_currentScrollItemLocalPosition = m_currentSelection.transform.localPosition;
				StartCoroutine(OnSelectInput());
				m_selectionManager.SelectElement(m_currentSelection.Widget, m_activeClock > PadFastSelectionDelay);
			}
			else
			{
				CurrentScrollContent = null;
				m_selectionManager.SelectElement(null);
			}
		}
	}

	private UIScrollView CurrentScrollView
	{
		get
		{
			return (!(CurrentSelection != null)) ? null : CurrentSelection.ScrollView;
		}
	}

	private UIExtendedWrapContent CurrentScrollContent
	{
		get
		{
			return m_currentScrollContent;
		}
		set
		{
			if (!(m_currentScrollContent == value))
			{
				if (m_currentScrollContent != null)
				{
					m_currentScrollContent.OnUpdateItem -= OnUpdateScrollItem;
				}
				m_currentScrollContent = value;
				if (m_currentScrollContent != null)
				{
					m_currentScrollContent.OnUpdateItem += OnUpdateScrollItem;
				}
			}
		}
	}

	private static PriorityDelegate PriorityFixed(int _priority)
	{
		return (Component _c) => _priority;
	}

	private static int PriorityMissionItem(Component _component)
	{
		UIMissionItemBase uIMissionItemBase = _component as UIMissionItemBase;
		return (uIMissionItemBase != null && uIMissionItemBase.IsSelected) ? 10 : 0;
	}

	private static int PriorityExtendedButton(Component _component)
	{
		UIExtendedButton uIExtendedButton = _component as UIExtendedButton;
		return (uIExtendedButton != null && uIExtendedButton.Type == EButtonTypes.Action) ? 3 : 0;
	}

	private static int PriorityGenericTab(Component _component)
	{
		GenericTab genericTab = _component as GenericTab;
		return (!(genericTab != null) || !genericTab.readMarker.gameObject.activeSelf) ? 1 : 4;
	}

	private static int PriorityInventoryItem(Component _component)
	{
		UIInventoryItemView uIInventoryItemView = _component as UIInventoryItemView;
		return (!(uIInventoryItemView != null) || !uIInventoryItemView.IsBuyInventory) ? 4 : 0;
	}

	private static int PriorityMailHeader(Component _component)
	{
		MailHeaderController mailHeaderController = _component as MailHeaderController;
		return (!(mailHeaderController != null) || !mailHeaderController.IsSelected) ? 3 : 4;
	}

	private static int PriorityFriendTab(Component _component)
	{
		FriendTab friendTab = _component as FriendTab;
		return (!(friendTab != null) || !friendTab.IsSelected) ? 5 : 6;
	}

	private static int PriorityDropDownItem(Component _component)
	{
		UIDropDownItem uIDropDownItem = _component as UIDropDownItem;
		return (uIDropDownItem != null && uIDropDownItem.Selected) ? 1 : 0;
	}

	private IEnumerator OnSelectInput()
	{
		yield return new WaitForSeconds(0.3f);
		if (CurrentSelection != null && CurrentSelection.Input != null)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (UIInput.WaitForKeyboardToHide)
			{
				yield return wait;
			}
		}
		if (!(CurrentSelection != null))
		{
			yield break;
		}
		if (CurrentSelection.Input != null)
		{
			CurrentSelection.Input.isSelected = true;
			if (InputManager.ActiveDevice != InputDevice.Null)
			{
				InputManager.ActiveDevice.ClearInputState();
			}
		}
		else if (UIInput.selection != null)
		{
			UIInput.selection.isSelected = false;
		}
	}

	private void OnUpdateScrollItem(Transform _transform, int _index)
	{
		if (!(_transform != CurrentSelection.transform) && !((m_currentScrollItemLocalPosition - _transform.localPosition).sqrMagnitude < 0.1f))
		{
			m_selectionManager.DeselectElementInstant(CurrentSelection.Widget);
			Vector3 vector = CurrentScrollContent.transform.TransformPoint(m_currentScrollItemLocalPosition);
			if (!SelectNextAtPosition(vector))
			{
				CurrentSelection = null;
			}
		}
	}

	private void OnScreenChanged()
	{
		m_selectionManager.DeselectElementsInstant();
		CurrentSelection = null;
		if (m_screenWaitingForSetup != null)
		{
			EventDelegate.Remove(m_screenWaitingForSetup.onSetup, OnScreenSetup);
		}
		UIScreenManager instance = Singleton<UIScreenManager>.Instance;
		UIScreen uIScreen = (m_screenWaitingForSetup = instance.CurrentModalScreen ?? instance.CurrentScreen);
		if (m_screenWaitingForSetup != null)
		{
			EventDelegate.Add(uIScreen.onSetup, OnScreenSetup, true);
		}
	}

	private void OnScreenSetup()
	{
		UIScreenManager instance = Singleton<UIScreenManager>.Instance;
		UIScreen uIScreen = instance.CurrentModalScreen ?? instance.CurrentScreen;
		if (uIScreen == m_screenWaitingForSetup)
		{
			m_screenWaitingForSetup = null;
			m_delayAutoSelectClock = AutoSelectionDelay;
		}
	}

	private void Start()
	{
		InteractiveTypes = new BBDictionary<Type, PriorityDelegate>
		{
			{
				typeof(UIButton),
				null
			},
			{
				typeof(UIButtonProxy),
				null
			},
			{
				typeof(UIExtendedButton),
				PriorityExtendedButton
			},
			{
				typeof(UIMissionItemCampaign),
				PriorityMissionItem
			},
			{
				typeof(UIMissionItemCampaigns),
				PriorityMissionItem
			},
			{
				typeof(UIMissionItemContract),
				PriorityMissionItem
			},
			{
				typeof(UIMissionRewardItemBehaviour),
				null
			},
			{
				typeof(UIRewardItemController),
				null
			},
			{
				typeof(UIAssassinPortraitSelectableView),
				null
			},
			{
				typeof(UITabButtonViewPresenter),
				null
			},
			{
				typeof(GenericTab),
				PriorityGenericTab
			},
			{
				typeof(ClassChooseContainer),
				null
			},
			{
				typeof(ClassTabHandler),
				null
			},
			{
				typeof(SelectClassHandler),
				null
			},
			{
				typeof(SkillWheelButton),
				null
			},
			{
				typeof(UIInventoryItemView),
				PriorityInventoryItem
			},
			{
				typeof(MailHeaderController),
				PriorityMailHeader
			},
			{
				typeof(FriendTab),
				PriorityFriendTab
			},
			{
				typeof(FriendAssassinTab),
				null
			},
			{
				typeof(UIDropDownItem),
				PriorityDropDownItem
			},
			{
				typeof(PlayerIdCard),
				null
			}
		};
		m_selectableLayerMask = (int)Layers.InteractableUiLayerMask | (int)Layers.UILayerMask | (int)Layers.UISharedLayerMask | (int)Layers.UI3DLayerMask;
		m_screenWaitingForSetup = null;
		m_delayAutoSelectClock = 0f;
		EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
		EventDelegate.Add(Singleton<UIScreenManager>.Instance.onModalScreenChanged, OnScreenChanged);
	}

	private void OnDestroy()
	{
		ClearData();
		UIScreenManager instance = Singleton<UIScreenManager>.Instance;
		if (instance != null)
		{
			EventDelegate.Remove(instance.onScreenChanged, OnScreenChanged);
			EventDelegate.Remove(instance.onModalScreenChanged, OnScreenChanged);
		}
	}

	private void OnDisable()
	{
		ClearData();
	}

	private void Update()
	{
		float unscaledDeltaTime = Time.unscaledDeltaTime;
		if (m_inactiveClock > 0f)
		{
			m_inactiveClock -= unscaledDeltaTime;
		}
		if (m_selectionClock > 0f)
		{
			m_selectionClock -= unscaledDeltaTime;
		}
		if (m_delayAutoSelectClock > 0f)
		{
			m_delayAutoSelectClock -= unscaledDeltaTime;
		}
		CurrentScreen = GetCurrentScreen();
		if (!DetectPad() || CurrentScreen == null)
		{
			if (m_padDetected)
			{
				m_selectionManager.SelectElement(null);
				ClearData();
				m_padDetected = false;
			}
		}
		else if (UpdateData())
		{
			CollectSelectables();
			CheckSelection();
			ProcessInput();
		}
	}

	private void ClearData()
	{
		CurrentSelection = null;
		if (m_screenWaitingForSetup != null)
		{
			EventDelegate.Remove(m_screenWaitingForSetup.onSetup, OnScreenSetup);
		}
		m_screenWaitingForSetup = null;
		CenterSelectables.ClearIfNotEmpty();
		BottomButtons.ClearIfNotEmpty();
		BackButton = null;
		Menubars = null;
		ExtraInteractables = null;
	}

	private bool DetectPad()
	{
		m_padActive &= m_padDetected;
		m_padActive &= m_inactiveClock > 0f;
		if (!m_gamepad.HasInput)
		{
			return m_padActive;
		}
		m_padDetected = true;
		m_padActive = true;
		m_inactiveClock = PadInactiveDelay;
		return m_padActive;
	}

	private bool UpdateData()
	{
		EventDelegate.Add(Singleton<UIScreenManager>.Instance.onScreenChanged, OnScreenChanged);
		EventDelegate.Add(Singleton<UIScreenManager>.Instance.onModalScreenChanged, OnScreenChanged);
		UISceneData sceneData = Singleton<UIScreenManager>.Instance.SceneData;
		if (sceneData == null)
		{
			Menubars = null;
			ExtraInteractables = null;
		}
		else
		{
			Menubars = sceneData.UIMenuBars;
			ExtraInteractables = sceneData.ExtraGamepadInteractables;
		}
		if (UICamera.mainCamera == null)
		{
			return false;
		}
		return true;
	}

	private void ProcessInput()
	{
		GamepadWrapper gamepad = m_gamepad;
		Vector2 vector = ((!m_buttonHeld) ? (gamepad.SelectAnimus + gamepad.Select) : Vector2.zero);
		if (vector.sqrMagnitude < 0.040000003f)
		{
			m_selectionClock = 0f;
			m_activeClock = 0f;
		}
		else
		{
			m_activeClock += Time.unscaledDeltaTime;
			if (m_selectionClock <= 0f)
			{
				SelectNextInDirection(vector.normalized, false);
				m_selectionClock = SelectionPeriod;
			}
			if (m_activeClock >= PadFastSelectionDelay && m_selectionClock > SelectionFastPeriod)
			{
				m_selectionClock = SelectionFastPeriod;
			}
		}
		if (CurrentScrollView != null)
		{
			Vector2 camera = m_gamepad.Camera;
			float num = ((!CurrentScrollView.canMoveHorizontally) ? (0f - camera.y) : camera.x);
			if (num * num > 0.040000003f)
			{
				CurrentScrollView.Scroll(num * ScrollSensitivity * Time.unscaledDeltaTime);
			}
		}
		if (gamepad.ScrollTabLeftDown)
		{
			ScrollTab(-1);
		}
		if (gamepad.ScrollTabRightDown)
		{
			ScrollTab(1);
		}
		if (gamepad.AcceptDown)
		{
			m_buttonHeld = true;
			if (CurrentSelection != null && CanHitCollider(CurrentSelection))
			{
				GameObject inventoryScreenSelection = GetInventoryScreenSelection();
				if (CurrentSelection == BackButton && inventoryScreenSelection != null)
				{
					CurrentSelection = inventoryScreenSelection.GetComponent<UIGamepadSelectable>();
					BackButton.SimulateTap();
				}
				else
				{
					CurrentSelection.SimulateTap();
					if (CurrentSelection != null && GetInventoryScreenSelection() == CurrentSelection.gameObject)
					{
						CurrentSelection = null;
						m_delayAutoSelectClock = AutoSelectionDelay;
					}
				}
			}
		}
		if (!gamepad.AcceptHeld && !gamepad.AcceptDown)
		{
			m_buttonHeld = false;
		}
		if (gamepad.CloseDown && BackButton != null && CanHitCollider(BackButton))
		{
			GameObject inventoryScreenSelection2 = GetInventoryScreenSelection();
			if (inventoryScreenSelection2 != null)
			{
				CurrentSelection = inventoryScreenSelection2.GetComponent<UIGamepadSelectable>();
			}
			BackButton.SimulateTap();
		}
	}

	private GameObject GetInventoryScreenSelection()
	{
		UIScreen currentScreen = Singleton<UIScreenManager>.Instance.CurrentScreen;
		if (currentScreen == null || currentScreen.ScreenId != EScreenID.InventoryScreen)
		{
			return null;
		}
		UIInventoryScreen componentInChildren = currentScreen.gameObject.GetComponentInChildren<UIInventoryScreen>();
		if (componentInChildren == null)
		{
			return null;
		}
		UIInventoryHandler inventoryHandler = componentInChildren.InventoryHandler;
		if (inventoryHandler != null && inventoryHandler.CurrentItemView != null)
		{
			return inventoryHandler.CurrentItemView.gameObject;
		}
		UIForgeHandler forgeHandler = componentInChildren.ForgeHandler;
		if (forgeHandler != null && forgeHandler.CurrentItem != null)
		{
			return forgeHandler.CurrentItem.gameObject;
		}
		UIEquipmentHandler equipmentHandler = componentInChildren.EquipmentHandler;
		if (equipmentHandler != null)
		{
			UIEquipmentGear uIEquipmentGear = equipmentHandler.FindSelection();
			if (uIEquipmentGear != null)
			{
				return uIEquipmentGear.IconItem.gameObject;
			}
		}
		return null;
	}

	private void CheckSelection()
	{
		if (CurrentSelection != null && !CenterSelectables.Contains(CurrentSelection))
		{
			m_selectionManager.DeselectElementInstant(CurrentSelection.Widget);
			if (m_delayAutoSelectClock > 0f || !SelectByPriority())
			{
				CurrentSelection = null;
			}
		}
		else if (CurrentSelection == null && m_screenWaitingForSetup == null && m_delayAutoSelectClock <= 0f)
		{
			SelectByPriority();
		}
	}

	private void ScrollTab(int _direction)
	{
		if (Menubars == null || BottomButtons.Count <= 0)
		{
			return;
		}
		int num = -1;
		UIButtonHeaderManager buttonHeaderManager = Menubars.ButtonHeaderManager;
		if (buttonHeaderManager.LastButton != null)
		{
			UIWidget currentButton = buttonHeaderManager.LastButton.GetComponent<UIWidget>();
			num = BottomButtons.FindIndex((UIGamepadSelectable _b) => _b.Widget == currentButton);
		}
		num = ((num >= 0) ? ((num + BottomButtons.Count + _direction) % BottomButtons.Count) : 0);
		UIGamepadSelectable uIGamepadSelectable = BottomButtons[num];
		uIGamepadSelectable.SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
	}

	private void CollectSelectables()
	{
		m_dropDownActive = false;
		CenterSelectables.ClearIfNotEmpty();
		BottomButtons.ClearIfNotEmpty();
		GameObject currentScreen = GetCurrentScreen();
		CollectSelectables(currentScreen);
		if (ExtraInteractables != null)
		{
			for (int i = 0; i < ExtraInteractables.Length; i++)
			{
				CollectSelectables(ExtraInteractables[i]);
			}
		}
		if (Menubars != null && Singleton<UIScreenManager>.Instance.CurrentModalScreen == null)
		{
			UIButton[] componentsInChildren = Menubars.TopBar.GetComponentsInChildren<UIButton>();
			foreach (UIButton component in componentsInChildren)
			{
				UIGamepadSelectable selectable = GetSelectable(component);
				if (CanHitCollider(selectable))
				{
					CenterSelectables.Add(selectable);
				}
			}
			bool flag = false;
			UIButtonHeaderManager buttonHeaderManager = Menubars.ButtonHeaderManager;
			UIButtonHeader[] allBottomBarButtons = buttonHeaderManager.AllBottomBarButtons;
			for (int k = 0; k < allBottomBarButtons.Length; k++)
			{
				UIGamepadSelectable selectable2 = GetSelectable(allBottomBarButtons[k]);
				if (allBottomBarButtons[k].IsUnlocked && (buttonHeaderManager.LastButton == allBottomBarButtons[k] || CanHitCollider(selectable2)))
				{
					flag |= buttonHeaderManager.LastButton != allBottomBarButtons[k];
					BottomButtons.Add(selectable2);
				}
			}
			if (flag)
			{
				BottomButtons.Sort((UIGamepadSelectable a, UIGamepadSelectable b) => a.GetComponent<Collider>().bounds.center.x.CompareTo(b.GetComponent<Collider>().bounds.center.x));
			}
			else
			{
				BottomButtons.ClearIfNotEmpty();
			}
		}
		if (m_dropDownActive)
		{
			CenterSelectables.RemoveAll((UIGamepadSelectable _s) => !_s.IsDropDownItem);
		}
		int num = -1;
		int num2 = -1;
		for (int l = 0; l < CenterSelectables.Count; l++)
		{
			if ((m_dropDownActive || CenterSelectables[l].IsBackButton) && CenterSelectables[l].PriorityCombined > num)
			{
				num = CenterSelectables[l].PriorityCombined;
				num2 = l;
			}
		}
		BackButton = ((num2 < 0) ? null : CenterSelectables[num2]);
	}

	private void CollectSelectables(GameObject _go)
	{
		if (!(_go != null) || !_go.activeInHierarchy)
		{
			return;
		}
		Transform transform = _go.transform;
		for (int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				CollectSelectables(child);
			}
		}
	}

	private UIGamepadSelectable GetSelectable(Component _component)
	{
		UIGamepadSelectable uIGamepadSelectable = _component.gameObject.AddComponentIfNotExist<UIGamepadSelectable>();
		if (uIGamepadSelectable.Widget == null)
		{
			uIGamepadSelectable.Widget = _component.GetComponent<UIWidget>();
		}
		return uIGamepadSelectable;
	}

	private void CollectSelectables(Transform _transform, UIScrollView _parentScrollView = null)
	{
		bool flag = false;
		for (int i = 0; i < _transform.childCount; i++)
		{
			Transform child = _transform.GetChild(i);
			if (child.gameObject.activeSelf)
			{
				UIScrollView component = child.GetComponent<UIScrollView>();
				if (!(component == null))
				{
					flag = true;
					CollectSelectables(child, component);
				}
			}
		}
		for (int j = 0; j < _transform.childCount; j++)
		{
			Transform child2 = _transform.GetChild(j);
			if (child2.gameObject.activeSelf && (!flag || !(child2.GetComponent<UIScrollView>() != null)))
			{
				CollectSelectables(child2, _parentScrollView);
			}
		}
		BoxCollider component2 = _transform.GetComponent<BoxCollider>();
		CollectSelectables(component2, _parentScrollView);
	}

	private void CollectSelectables(BoxCollider _boxCollider, UIScrollView _parentScrollView)
	{
		if (_boxCollider == null || !_boxCollider.enabled)
		{
			return;
		}
		int num = 1 << _boxCollider.gameObject.layer;
		if ((num & m_selectableLayerMask) == 0)
		{
			return;
		}
		UIWidget uIWidget = null;
		UIInput uIInput = null;
		UIGamepadSelectable uIGamepadSelectable = null;
		UIDragScrollView dragScrollView = null;
		bool flag = false;
		bool flag2 = false;
		int num2 = 0;
		int num3 = 0;
		Component[] components = _boxCollider.GetComponents<Component>();
		foreach (Component component in components)
		{
			if (component == null)
			{
				continue;
			}
			if (uIWidget == null)
			{
				uIWidget = component as UIWidget;
			}
			if (uIGamepadSelectable == null)
			{
				uIGamepadSelectable = component as UIGamepadSelectable;
			}
			if (dragScrollView == null)
			{
				dragScrollView = component as UIDragScrollView;
			}
			if (!flag2)
			{
				flag2 = component is UIDropDownItem;
			}
			if (uIInput == null)
			{
				uIInput = component as UIInput;
			}
			Type type = component.GetType();
			PriorityDelegate value;
			if (InteractiveTypes.TryGetValue(type, out value))
			{
				flag = true;
				if (value != null)
				{
					int val = value(component);
					num2 = Math.Min(num2, val);
					num3 = Math.Max(num3, val);
				}
			}
		}
		if (dragScrollView != null && dragScrollView.scrollView != null && dragScrollView.scrollView.transform.parent == dragScrollView.transform.parent)
		{
			flag = CenterSelectables.FindIndex((UIGamepadSelectable _gs) => _gs.ScrollView == dragScrollView.scrollView) < 0;
		}
		if (num2 < 0)
		{
			flag = false;
		}
		if (uIGamepadSelectable != null)
		{
			if (uIGamepadSelectable.Mode == UIGamepadSelectable.EMode.Enabled)
			{
				flag = true;
			}
			if (uIGamepadSelectable.Mode == UIGamepadSelectable.EMode.Disabled)
			{
				flag = false;
			}
		}
		if (!flag)
		{
			return;
		}
		if (uIGamepadSelectable == null)
		{
			uIGamepadSelectable = _boxCollider.gameObject.AddComponent<UIGamepadSelectable>();
		}
		uIGamepadSelectable.PriorityRuntime = num3;
		uIGamepadSelectable.IsDropDownItem = flag2;
		uIGamepadSelectable.Input = uIInput;
		if (uIGamepadSelectable.Widget == null)
		{
			uIGamepadSelectable.Widget = uIWidget;
		}
		if (uIGamepadSelectable.Widget == null || !uIGamepadSelectable.Widget.isVisible || uIGamepadSelectable.Widget.panel == null || !uIGamepadSelectable.Widget.panel.IsVisible(uIGamepadSelectable.Widget) || !CanHitCollider(uIGamepadSelectable))
		{
			return;
		}
		if (uIGamepadSelectable.ScrollView == null)
		{
			if (dragScrollView != null)
			{
				uIGamepadSelectable.ScrollView = dragScrollView.scrollView;
			}
			else
			{
				uIGamepadSelectable.ScrollView = _parentScrollView;
			}
		}
		m_dropDownActive |= flag2;
		if (!m_dropDownActive || flag2)
		{
			CenterSelectables.Add(uIGamepadSelectable);
		}
	}

	private static GameObject GetCurrentScreen()
	{
		UIScreenManager instance = Singleton<UIScreenManager>.Instance;
		UIScreen uIScreen = instance.CurrentModalScreen ?? instance.CurrentScreen;
		if (uIScreen != null)
		{
			return uIScreen.gameObject;
		}
		UIMissionResult subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIMissionResult>();
		if (subSystemByType != null && subSystemByType.Details != null && subSystemByType.Details.IsVisible)
		{
			return subSystemByType.Details.transform.parent.gameObject;
		}
		if (UIDownloadScreen.IsLoadedAndFullyVisible)
		{
			return UIDownloadScreen.Instance.Details.transform.parent.gameObject;
		}
		UIIngamePopup subSystemByType2 = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIIngamePopup>();
		if (subSystemByType2 != null && subSystemByType2.Details != null && subSystemByType2.Details.IsVisible)
		{
			return subSystemByType2.Details.transform.gameObject;
		}
		return null;
	}

	public static bool CanHitCollider(UIGamepadSelectable _selectable)
	{
		UICamera uICamera = UICamera.FindCameraForLayer(_selectable.gameObject.layer);
		if (uICamera == null)
		{
			return false;
		}
		Vector3 worldCenter = _selectable.Widget.worldCenter;
		Vector3 inPos = uICamera.cachedCamera.WorldToScreenPoint(worldCenter);
		if (!UICamera.Raycast(inPos))
		{
			return false;
		}
		return UICamera.hoveredObject == _selectable.gameObject;
	}

	public static bool CanHitCollider(GameObject _gameObject)
	{
		UICamera uICamera = UICamera.FindCameraForLayer(_gameObject.layer);
		if (uICamera == null)
		{
			return false;
		}
		Vector3 worldCenter = _gameObject.GetComponent<UIWidget>().worldCenter;
		Vector3 inPos = uICamera.cachedCamera.WorldToScreenPoint(worldCenter);
		if (!UICamera.Raycast(inPos))
		{
			return false;
		}
		return UICamera.hoveredObject == _gameObject.gameObject;
	}

	private bool SelectByPriority(bool _checkScrollView = true)
	{
		if (_checkScrollView && CurrentSelection != null && CurrentScrollView != null)
		{
			Vector3 center = CurrentSelection.GetComponent<Collider>().bounds.center;
			return SelectNextAtPosition(center);
		}
		float num = 0.001f;
		int num2 = -1;
		UIGamepadSelectable uIGamepadSelectable = null;
		Collider collider = null;
		for (int i = 0; i < CenterSelectables.Count; i++)
		{
			UIGamepadSelectable uIGamepadSelectable2 = CenterSelectables[i];
			if (uIGamepadSelectable2.PriorityCombined >= num2)
			{
				Collider component = uIGamepadSelectable2.GetComponent<Collider>();
				if (!(component == null) && component.enabled && (uIGamepadSelectable2.PriorityCombined != num2 || !(uIGamepadSelectable != null) || ((!uIGamepadSelectable2.IsBackButton || uIGamepadSelectable.IsBackButton) && !(component.bounds.center.y < collider.bounds.center.y - num) && (!(component.bounds.center.y < collider.bounds.center.y + num) || !(component.bounds.center.x > collider.bounds.center.x)))) && CanHitCollider(uIGamepadSelectable2))
				{
					num2 = uIGamepadSelectable2.PriorityCombined;
					uIGamepadSelectable = uIGamepadSelectable2;
					collider = component;
				}
			}
		}
		if (uIGamepadSelectable != null)
		{
			CurrentSelection = uIGamepadSelectable;
		}
		return uIGamepadSelectable != null;
	}

	private bool SelectNextAtPosition(Vector2 _position, bool _checkScrollView = true)
	{
		bool flag = _checkScrollView && CurrentScrollView != null;
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < CenterSelectables.Count; i++)
		{
			UIGamepadSelectable uIGamepadSelectable = CenterSelectables[i];
			if (flag && uIGamepadSelectable.ScrollView != CurrentScrollView)
			{
				continue;
			}
			Collider component = uIGamepadSelectable.GetComponent<Collider>();
			if (!(component == null) && component.enabled && CanHitCollider(uIGamepadSelectable))
			{
				float magnitude = ((Vector2)component.bounds.center - _position).magnitude;
				if (magnitude < num2)
				{
					num2 = magnitude;
					num = i;
				}
			}
		}
		if (num >= 0)
		{
			CurrentSelection = CenterSelectables[num];
			return true;
		}
		return flag && SelectByPriority(false);
	}

	private bool SelectNextInDirection(Vector2 _direction, bool _checkScrollView = true)
	{
		bool flag = _checkScrollView && CurrentScrollView != null;
		UIGamepadSelectable currentSelection = CurrentSelection;
		Collider collider = null;
		Vector2 vector = Vector2.zero;
		if (currentSelection != null)
		{
			collider = currentSelection.GetComponent<Collider>();
			vector = collider.bounds.center;
		}
		int num = -1;
		float num2 = float.MaxValue;
		for (int i = 0; i < CenterSelectables.Count; i++)
		{
			UIGamepadSelectable uIGamepadSelectable = CenterSelectables[i];
			if (currentSelection == uIGamepadSelectable || (flag && uIGamepadSelectable.ScrollView != CurrentScrollView))
			{
				continue;
			}
			Collider component = uIGamepadSelectable.GetComponent<Collider>();
			if (component == null || !component.enabled || !CanHitCollider(uIGamepadSelectable))
			{
				continue;
			}
			Vector3 vector2 = component.ClosestPointOnBounds(vector);
			Vector3 vector3 = vector;
			if (currentSelection != null)
			{
				vector3 = collider.ClosestPointOnBounds(vector2);
				Vector3 vector4 = collider.bounds.center - component.bounds.center;
				if (component.bounds.Intersects(collider.bounds))
				{
					vector3 = vector2 + vector4.normalized * 0.001f;
				}
			}
			Vector2 to = vector2 - vector3;
			float num3 = Vector2.Angle(_direction, to);
			if (!(num3 > SelectionMaxAngle))
			{
				float magnitude = to.magnitude;
				float num4 = num3 / SelectionMaxAngle + SelectionSensitivity * Mathf.Min(magnitude, 2f);
				if (num4 < num2)
				{
					num2 = num4;
					num = i;
				}
			}
		}
		if (num >= 0 && CenterSelectables[num] != currentSelection)
		{
			CurrentSelection = CenterSelectables[num];
			return true;
		}
		return flag && SelectNextInDirection(_direction, false);
	}
}
