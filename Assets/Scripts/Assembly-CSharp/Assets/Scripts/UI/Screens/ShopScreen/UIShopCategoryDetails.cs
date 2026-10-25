using AcierProtocol;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopCategoryDetails : UIShopBaseItem
	{
		private const string m_hcHeaderSpriteName = "aUI_fra_adcolony_bandolier";

		private const string m_heroicHeaderSpriteName = "aUI_fra_heroic_bandolier";

		[SerializeField]
		private GameObject m_onSelectedTweensGameObject;

		[SerializeField]
		private GameObject m_onFadeOutTweensGameObject;

		[SerializeField]
		private BBTargetedTween m_categoryPressedTween;

		[SerializeField]
		private UIExtendedLabel m_categoryName;

		[SerializeField]
		private UIExtendedLabel m_specialCategoryName;

		[SerializeField]
		private UISprite m_categoryBackground;

		[SerializeField]
		private GameObject m_standardHeader;

		[SerializeField]
		private GameObject m_specialHeader;

		[SerializeField]
		private GameObject m_saleBanner;

		[SerializeField]
		private UISprite m_saleBackgroundSprite;

		private static string s_normalSaleBackground = "aUI_fra_label_sale";

		private static string s_heroicSalebackground = "aUI_fra_label_heroic_shop";

		private EShopDataType m_dataType;

		public BBTargetedTween[] SelectItemTweens { get; private set; }

		public BBTargetedTween[] FadeOutItemTweens { get; private set; }

		public override EShopNodeType ShopDataNodeType
		{
			get
			{
				return (m_dataType == EShopDataType.AdColony) ? EShopNodeType.AdColony : EShopNodeType.Category;
			}
		}

		public override void UpdateData(ShopBaseNode _baseNode)
		{
			base.UpdateData(_baseNode);
			m_standardHeader.SetActive(false);
			m_specialHeader.SetActive(false);
			m_saleBanner.SetActive(false);
			m_saleBackgroundSprite.spriteName = s_normalSaleBackground;
			ShopCategoryNode shopCategoryNode = _baseNode as ShopCategoryNode;
			if (shopCategoryNode != null && shopCategoryNode.HasDiscount())
			{
				m_saleBanner.SetActive(true);
			}
			if (_baseNode.DataType == EShopDataType.AdColony)
			{
				UpdateSubCategoryData(_baseNode as FakeCategoryNode);
				m_standardHeader.SetActive(true);
			}
			else
			{
				UpdateSubCategoryData(shopCategoryNode);
				if (shopCategoryNode.Category == EShopCategory.Shopcat_HcPackage)
				{
					bool active = ShopBundleExtension.IsPayoutIncreaseActForCategory(EShopCategory.Shopcat_HcPackage);
					m_saleBanner.SetActive(active);
					m_specialHeader.GetComponent<UISprite>().spriteName = "aUI_fra_adcolony_bandolier";
					m_specialHeader.SetActive(true);
				}
				else if (shopCategoryNode.Category == EShopCategory.Shopcat_Heroic)
				{
					bool flag = ShopBundleExtension.IsPayoutIncreaseActForCategory(EShopCategory.Shopcat_Heroic);
					m_saleBanner.SetActive(m_saleBanner.activeSelf || flag);
					m_saleBackgroundSprite.spriteName = s_heroicSalebackground;
					m_standardHeader.SetActive(true);
				}
				else
				{
					m_standardHeader.SetActive(true);
				}
			}
			m_dataType = _baseNode.DataType;
		}

		protected override void OnAwake()
		{
			base.OnAwake();
			SelectItemTweens = m_onSelectedTweensGameObject.GetComponents<BBTargetedTween>();
			FadeOutItemTweens = m_onFadeOutTweensGameObject.GetComponents<BBTargetedTween>();
		}

		protected override void OnDestroyItem()
		{
			FadeOutItemTweens = null;
			SelectItemTweens = null;
		}

		protected override void OnItemPress(bool _pressed)
		{
			if (_pressed)
			{
				m_categoryPressedTween.StartTweenPlayForward();
			}
			else
			{
				m_categoryPressedTween.SetTweenToBeginning();
			}
		}

		protected override void OnItemDragStart()
		{
			if (base.IsSelected)
			{
				m_categoryPressedTween.StartTweenPlayReverse();
			}
		}

		protected override void OnItemClicked()
		{
			if (base.IsSelected)
			{
				EventDelegate.Execute(OnSelected);
			}
		}

		private void UpdateSubCategoryData(ShopCategoryNode _categoryNode)
		{
			if (_categoryNode.Category == EShopCategory.Shopcat_HcPackage)
			{
				m_specialCategoryName.LocalizationKey = _categoryNode.NameKey;
			}
			else
			{
				m_categoryName.LocalizationKey = _categoryNode.NameKey;
				if (_categoryNode.Category == EShopCategory.Shopcat_Heroic)
				{
					m_categoryName.color = UIDataMapper.UITextColors.HeroicShopCategoryNameColor;
				}
				else
				{
					m_categoryName.color = UIDataMapper.UITextColors.ShopCategoryNameColor;
				}
			}
			m_categoryBackground.spriteName = _categoryNode.ItemIconName;
		}

		private void UpdateSubCategoryData(FakeCategoryNode _categoryNode)
		{
			m_categoryName.LocalizationKey = _categoryNode.NameKey;
			m_categoryBackground.spriteName = _categoryNode.ItemIconName;
		}
	}
}
