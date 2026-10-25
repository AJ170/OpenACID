using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class LockedInventorySlot : InventoryItemBase
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
				return false;
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
				return 99;
			}
		}

		public LockedInventorySlot(UIInventoryScreenData _data)
			: base(_data)
		{
		}

		public override void Render(UIInventoryItemView _view)
		{
			_view.UpdateView(UIInventoryItemView.ELockType.Locked);
			_view.Clickable = Clickable;
		}

		public override void Clicked(Transform _transform)
		{
		}
	}
}
