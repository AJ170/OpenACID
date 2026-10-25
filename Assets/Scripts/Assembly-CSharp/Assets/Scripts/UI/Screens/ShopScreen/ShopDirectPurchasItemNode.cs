namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class ShopDirectPurchasItemNode : ShopItemNode
	{
		public string ItemCurrencyText = string.Empty;

		public override EShopDataType DataType
		{
			get
			{
				return EShopDataType.DirectPurchasItem;
			}
		}
	}
}
