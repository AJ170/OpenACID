namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class ShopBaseNode
	{
		public ShopCategoryNode ParentNode;

		public int SortID;

		public virtual EShopDataType DataType
		{
			get
			{
				return EShopDataType.Base;
			}
		}
	}
}
