using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class FakeCategoryNode : ShopBaseNode
	{
		public int FakeSortID;

		public StringId NameKey = 7226;

		public string ItemIconName = "aUI_img_shop_category_adcurrency";

		public override EShopDataType DataType
		{
			get
			{
				return EShopDataType.AdColony;
			}
		}

		public FakeCategoryNode()
		{
			FakeSortID = Singleton<StateManager>.Instance.TemplateManager.AdColonySortId;
			SortID = FakeSortID;
		}
	}
}
