using AcierProtocol;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class ShopItemNode : ShopBaseNode
	{
		public ShopBundle ShopBundle;

		public override EShopDataType DataType
		{
			get
			{
				return EShopDataType.Item;
			}
		}
	}
}
