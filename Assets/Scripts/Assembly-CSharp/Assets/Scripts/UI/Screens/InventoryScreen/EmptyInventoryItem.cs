using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class EmptyInventoryItem : InventoryItemBase
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
				return 1;
			}
		}

		public EmptyInventoryItem(UIInventoryScreenData _data)
			: base(_data)
		{
		}

		public override void Render(UIInventoryItemView _view)
		{
			_view.EmptyIcon();
			_view.Clickable = Clickable;
		}

		public override void Clicked(Transform _transform)
		{
		}
	}
}
