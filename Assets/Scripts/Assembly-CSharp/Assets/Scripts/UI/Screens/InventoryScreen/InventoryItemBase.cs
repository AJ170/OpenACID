using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public abstract class InventoryItemBase
	{
		public abstract bool Selectable { get; }

		public abstract bool Clickable { get; }

		public abstract InventoryElement Element { get; }

		public abstract int Order { get; }

		protected UIInventoryScreenData ScreenData { get; private set; }

		protected InventoryItemBase(UIInventoryScreenData _data)
		{
			ScreenData = _data;
		}

		public abstract void Render(UIInventoryItemView _view);

		public abstract void Clicked(Transform _transform);
	}
}
