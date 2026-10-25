using AcierProtocol;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIEquipmentEmpty : UIEquipmentGear
	{
		public override bool Empty
		{
			get
			{
				return true;
			}
		}

		public override bool Selectable
		{
			get
			{
				return false;
			}
		}

		public override InventoryElement InventoryElement
		{
			get
			{
				return null;
			}
		}

		protected override void DoInitialize(UIInventoryScreenData _data)
		{
			base.DoInitialize(_data);
			base.IconItem.ReservedSlotIcon();
			base.IconItem.Clickable = false;
		}
	}
}
