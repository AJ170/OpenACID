using System;
using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class BuyBluePrintSlot : InventoryItemBase
	{
		private readonly Action m_onPopUpOpenedCallback;

		public override bool Selectable
		{
			get
			{
				return false;
			}
		}

		public override bool Clickable
		{
			get
			{
				return true;
			}
		}

		public override InventoryElement Element
		{
			get
			{
				return null;
			}
		}

		public override int Order
		{
			get
			{
				return -1;
			}
		}

		public BuyBluePrintSlot(UIInventoryScreenData _data, Action _onPopUpOpenedCallback)
			: base(_data)
		{
			m_onPopUpOpenedCallback = _onPopUpOpenedCallback;
		}

		public override void Render(UIInventoryItemView _view)
		{
			_view.UpdateView(UIInventoryItemView.ELockType.BuyBluePrint);
			_view.Clickable = Clickable;
		}

		public override void Clicked(Transform _transform)
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility).OnChanged += new SimpleDelegate.CallbackFunction(OnShopItemEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility).OnChanged += new SimpleDelegate.CallbackFunction(OnShopCategoryEventChanged);
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewCloseCallback += RemoveBlueprintsEventsDelegates;
			bool flag = !base.ScreenData.IsForgeEmpty;
			UIBuyHelper.OpenBuyBluePrintPopup(!flag);
			Action onPopUpOpenedCallback = m_onPopUpOpenedCallback;
			if (onPopUpOpenedCallback != null)
			{
				onPopUpOpenedCallback();
			}
		}

		private void OnShopItemEventChanged(params object[] _params)
		{
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnClosePopup();
			Clicked(null);
		}

		private void OnShopCategoryEventChanged(params object[] _params)
		{
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnClosePopup();
			Clicked(null);
		}

		private void RemoveBlueprintsEventsDelegates()
		{
			EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
			eventManager.GetEventManger(EEventType.Event_Shop_Item_Visibility).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopItemEventChanged);
			eventManager.GetEventManger(EEventType.Event_Shop_Category_Visibility).OnChanged -= new SimpleDelegate.CallbackFunction(OnShopCategoryEventChanged);
			Singleton<UIScreenManager>.Instance.SceneData.ItemBundlePreview.OnPreviewCloseCallback -= RemoveBlueprintsEventsDelegates;
		}
	}
}
