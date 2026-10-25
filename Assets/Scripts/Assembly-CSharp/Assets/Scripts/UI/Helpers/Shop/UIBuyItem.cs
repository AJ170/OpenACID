using AcierProtocol;
using Assets.Scripts.UI.Screens.ShopScreen;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers.Shop
{
	public class UIBuyItem : MonoBehaviour
	{
		[SerializeField]
		private EShopCategory m_category;

		private void Awake()
		{
			UIButton component = GetComponent<UIButton>();
			EventDelegate.Add(component.onClick, OnBuyItem);
		}

		public void OnBuyItem()
		{
			UIShopDetails.OpenAndSetFocusOnItem(m_category);
		}
	}
}
