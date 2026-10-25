using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class BuyInventorySlot : InventoryItemBase
	{
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
				return Singleton<StateManager>.Instance.TemplateManager.BuyInventorySlotsSortId;
			}
		}

		public BuyInventorySlot(UIInventoryScreenData _data)
			: base(_data)
		{
		}

		public override void Render(UIInventoryItemView _view)
		{
			_view.UpdateView(UIInventoryItemView.ELockType.BuyInventory);
			_view.Clickable = Clickable;
		}

		public override void Clicked(Transform _transform)
		{
			UIBuyHelper.OpenBuyInventoryPopup();
		}
	}
}
