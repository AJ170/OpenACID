using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIShopItemInfoDetails : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_infoDescriptionText;

		[SerializeField]
		private UIAlignmentBox m_infoCurrencyAlignmentBox;

		[SerializeField]
		private GameObject m_bottomBar;

		[SerializeField]
		private UIWidget m_infoCurrencyIconContainer;

		[SerializeField]
		private UISprite m_infoCurrencySprite;

		[SerializeField]
		private UIExtendedLabel m_infoCurrencyValue;

		[SerializeField]
		private UIAlignmentBox m_buttonAlignmentBox;

		[SerializeField]
		private UIButtonProxy m_buyButton;

		[SerializeField]
		private GameObject m_giftButton;

		private StringMapper m_bundleStringMapper;

		private string m_stringMapperBundleId;

		private ShopItemNode ItemNodeData { get; set; }

		private ShopBundle ShopBundle { get; set; }

		private bool IsAlreadyOwned { get; set; }

		private bool IsUnlocked { get; set; }

		public GameObject BuyButton
		{
			get
			{
				return m_buyButton.gameObject;
			}
		}

		public void UpdateData(ShopItemNode _itemNodeData)
		{
			if (_itemNodeData != null)
			{
				ItemNodeData = _itemNodeData;
				ShopBundle = _itemNodeData.ShopBundle;
				IsUnlocked = ShopBundle.IsUnlocked();
				IsAlreadyOwned = ShopBundle.IsAlreadyOwned();
				UpdateItemInfo();
			}
		}

		private void Awake()
		{
		}

		private void OnDestroy()
		{
			ShopBundle = null;
			ItemNodeData = null;
		}

		private void UpdateItemInfo()
		{
			if (m_bundleStringMapper == null || m_stringMapperBundleId != ShopBundle.id)
			{
				m_stringMapperBundleId = ShopBundle.id;
				m_bundleStringMapper = ItemNodeData.ShopBundle.GetShopBundleStringMapper();
			}
			if (!string.IsNullOrEmpty(ShopBundle.houstonId))
			{
				m_bundleStringMapper["amount_credits"] = ShopBundle.GetFinalHappyHourPayout();
			}
			m_infoDescriptionText.text = ShopBundle.LocaDescription(m_bundleStringMapper);
			UpdateCurrency();
			m_infoCurrencyAlignmentBox.RepositionNow = true;
			m_bottomBar.SetActive(m_infoCurrencyAlignmentBox.gameObject.activeSelf);
			m_giftButton.SetActive(false);
			m_buyButton.gameObject.SetActive(!IsAlreadyOwned && IsUnlocked);
			m_buttonAlignmentBox.RepositionNow = true;
			Update();
		}

		private void UpdateCurrency()
		{
			bool active = false;
			if (!IsAlreadyOwned)
			{
				if (ItemNodeData.DataType == EShopDataType.DirectPurchasItem)
				{
					active = true;
					ShopDirectPurchasItemNode shopDirectPurchasItemNode = ItemNodeData as ShopDirectPurchasItemNode;
					if (shopDirectPurchasItemNode != null)
					{
						m_infoCurrencyValue.ClearFormatFlags();
						m_infoCurrencyValue.text = shopDirectPurchasItemNode.ItemCurrencyText;
						m_infoCurrencySprite.gameObject.SetActive(false);
						m_infoCurrencyIconContainer.gameObject.SetActive(false);
					}
				}
				else
				{
					active = true;
					Resource finalDiscountedResourceCost = ItemNodeData.ShopBundle.GetFinalDiscountedResourceCost();
					UIUtil.SetupCurrencyWidgets(finalDiscountedResourceCost, m_infoCurrencyValue, m_infoCurrencySprite, false, true);
				}
			}
			m_infoCurrencyAlignmentBox.gameObject.SetActive(active);
		}

		private void Update()
		{
			float disabledDelay = 0f;
			UIShopDetails.DisabledShopItems disabledShopItems = Singleton<UIScreenManager>.Instance.SceneData.ShopDetails.GetDisabledShopItems(ShopBundle.id);
			if (disabledShopItems != null)
			{
				disabledDelay = disabledShopItems.DisabledDelay;
			}
			m_buyButton.Button.DisabledDelay = disabledDelay;
		}
	}
}
