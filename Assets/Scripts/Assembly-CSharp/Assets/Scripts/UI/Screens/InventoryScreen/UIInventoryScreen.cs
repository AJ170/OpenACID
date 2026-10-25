using System.Collections;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.SceenPlaceholder;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryScreen : UIScreenChild
	{
		private const string k_openScreen = "ScreenOpen";

		private const string k_detailsToForge = "DetailsToForge";

		private const string k_detailsToInventory = "DetailsToInventory";

		private const string k_forgeToDetails = "ForgeToDetails";

		private const string k_forgeToInventory = "ForgeToInventory";

		private const string k_inventoryToDetails = "InventoryToDetails";

		private const string k_inventoryToForge = "InventoryToForge";

		private const string k_openToForge = "ScreenOpenToForge";

		public static UIInventoryScreenData.EScreenState StartingState;

		[CheckAssigned]
		[SerializeField]
		private UIEquipmentHandler m_equipmentHandler;

		[SerializeField]
		[CheckAssigned]
		private UnityEngine.Animation m_mainAnimaton;

		[SerializeField]
		[CheckAssigned]
		private UIInventoryHandler m_inventoryHandler;

		[SerializeField]
		[CheckAssigned]
		private UIForgeHandler m_forgeHandler;

		[SerializeField]
		[CheckAssigned]
		private UIAssassinStats m_statsHandler;

		[CheckAssigned]
		[SerializeField]
		private UIInventoryBlocker m_blocker;

		[SerializeField]
		[CheckAssigned]
		private UIPanel[] m_hidePanels;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_filterButton;

		private UIInventoryScreenData m_data;

		private UIInventoryBlocker.BlockerHandle m_fullBlockerHandle;

		public UIInventoryHandler InventoryHandler
		{
			get
			{
				return m_inventoryHandler;
			}
		}

		public UIEquipmentHandler EquipmentHandler
		{
			get
			{
				return m_equipmentHandler;
			}
		}

		public UIForgeHandler ForgeHandler
		{
			get
			{
				return m_forgeHandler;
			}
		}

		private string ScreenOpenTrack
		{
			get
			{
				return (StartingState != UIInventoryScreenData.EScreenState.Forge) ? "ScreenOpen" : "ScreenOpenToForge";
			}
		}

		protected override void OnScreenInit()
		{
			m_fullBlockerHandle = m_blocker.GetBlocker(UIInventoryBlocker.EBlocker.Full);
			UIPlaceholder placeholder = Singleton<UIScreenManager>.Instance.SceneData.Placeholders.GetPlaceholder(UIPlaceholders.EPlaceholderType.Inventory);
			EventDelegate.Add(m_screen.onPrepareActivate, OnScreenPrepareActivate);
			EventDelegate.Add(m_screen.onEnter, OnScreenEnter);
			EventDelegate.Add(m_screen.onExit, OnScreenExit);
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Add(m_screen.onDeActivate, OnScreenDeActivate);
		}

		protected override void OnScreenDestroy()
		{
			EventDelegate.Remove(m_screen.onPrepareActivate, OnScreenPrepareActivate);
			EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			EventDelegate.Remove(m_screen.onDeActivate, OnScreenDeActivate);
			EventDelegate.Remove(m_screen.onEnter, OnScreenEnter);
			EventDelegate.Remove(m_screen.onExit, OnScreenExit);
		}

		private void OnScreenPrepareActivate()
		{
			Hide(true);
			m_mainAnimaton.SampleAnimation(0f, ScreenOpenTrack);
		}

		private void OnScreenActivate()
		{
			m_filterButton.GetComponent<BoxCollider>().size = m_filterButton.GetComponent<UIWidget>().CalculateBounds().size;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.ShowSwitchAssassins = true;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanCreateAssassins = false;
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CanSwitchRank = false;
			SetupScreen();
		}

		private void OnScreenEnter()
		{
			Hide(false);
			StartAnimation(ScreenOpenTrack);
			Singleton<UIScreenManager>.Instance.SceneData.DimmingBehaviour.SetDimming(true);
		}

		private void OnScreenExit()
		{
			m_mainAnimaton.SampleAnimation(0f, "ScreenOpen");
			Singleton<UIScreenManager>.Instance.SceneData.DimmingBehaviour.SetDimming(false);
			StartingState = UIInventoryScreenData.EScreenState.Inventory;
		}

		private void OnScreenDeActivate()
		{
			ClearScreen();
			m_mainAnimaton.Stop();
			StopAllCoroutines();
		}

		private void SetupScreen()
		{
			m_data = new UIInventoryScreenData();
			m_data.State = StartingState;
			m_equipmentHandler.Initialize(m_data);
			m_forgeHandler.Initialize(m_data);
			m_inventoryHandler.Initialize(m_data);
			m_equipmentHandler.OnInfoPopUpChange += OnEquipmentInfoPopUpChange;
			m_inventoryHandler.OnSelectedItemsChange += InventoryHandlerOnSelectedItemsChange;
			m_forgeHandler.OnSelectedItemsChange += ForgeHandlerOnSelectedItemsChange;
			m_data.OnStateChanged += OnScreenStateChange;
		}

		private void ClearScreen()
		{
			m_mainAnimaton.SampleAnimation(0f, string.Empty);
			m_mainAnimaton.SampleAnimation(0f, "ScreenOpen");
			m_fullBlockerHandle.Reset();
			m_data.OnStateChanged -= OnScreenStateChange;
			m_data = null;
			m_equipmentHandler.OnInfoPopUpChange -= OnEquipmentInfoPopUpChange;
			m_inventoryHandler.OnSelectedItemsChange -= InventoryHandlerOnSelectedItemsChange;
			m_forgeHandler.OnSelectedItemsChange -= ForgeHandlerOnSelectedItemsChange;
			m_equipmentHandler.Clear();
			m_inventoryHandler.Clear();
			m_forgeHandler.Clear();
			m_statsHandler.Clear();
		}

		private void OnScreenStateChange(UIInventoryScreenData.StateChangeData _stateChange)
		{
			StopAllCoroutines();
			switch (_stateChange.NewState)
			{
			case UIInventoryScreenData.EScreenState.Details:
				m_statsHandler.Initialize(m_data);
				break;
			}
			string clipName = GetClipName(_stateChange.OldState, _stateChange.NewState);
			if (!string.IsNullOrEmpty(clipName))
			{
				StartAnimation(clipName);
			}
		}

		private void InventoryHandlerOnSelectedItemsChange(UIInventoryItemView _item)
		{
			m_equipmentHandler.RemoveSelection();
			m_forgeHandler.RemoveSelection();
		}

		private void ForgeHandlerOnSelectedItemsChange(UIForgeItemHandler _item)
		{
			if (_item != null)
			{
				m_inventoryHandler.RemoveSelection(false);
			}
		}

		private void OnEquipmentInfoPopUpChange(bool _show)
		{
			if (_show)
			{
				m_inventoryHandler.RemoveSelection();
			}
		}

		private void StartAnimation(string _clip)
		{
			StartCoroutine(co_PlayStateChange(_clip));
		}

		private IEnumerator co_PlayStateChange(string _clip)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			yield return wait;
			m_fullBlockerHandle.On(false, false);
			m_mainAnimaton.Play(_clip);
			while (m_mainAnimaton.IsPlaying(_clip))
			{
				yield return wait;
			}
			m_fullBlockerHandle.Off(false);
			m_mainAnimaton.SampleAnimation(1f, _clip);
		}

		private void Hide(bool _hide)
		{
			for (int i = 0; i < m_hidePanels.Length; i++)
			{
				m_hidePanels[i].alpha = ((!_hide) ? 1 : 0);
			}
		}

		private static string GetClipName(UIInventoryScreenData.EScreenState _old, UIInventoryScreenData.EScreenState _new)
		{
			switch (_new)
			{
			case UIInventoryScreenData.EScreenState.Inventory:
				switch (_old)
				{
				case UIInventoryScreenData.EScreenState.Details:
					return "DetailsToInventory";
				case UIInventoryScreenData.EScreenState.Forge:
					return "ForgeToInventory";
				}
				break;
			case UIInventoryScreenData.EScreenState.Details:
				switch (_old)
				{
				case UIInventoryScreenData.EScreenState.Inventory:
					return "InventoryToDetails";
				case UIInventoryScreenData.EScreenState.Forge:
					return "ForgeToDetails";
				}
				break;
			case UIInventoryScreenData.EScreenState.Forge:
				switch (_old)
				{
				case UIInventoryScreenData.EScreenState.Inventory:
					return "InventoryToForge";
				case UIInventoryScreenData.EScreenState.Details:
					return "DetailsToForge";
				}
				break;
			}
			return string.Empty;
		}
	}
}
