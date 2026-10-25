using AcierProtocol;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIRecivedItem : UIRecivedBase
	{
		[SerializeField]
		private UIPortraitTexture m_itemTexture;

		[SerializeField]
		private UITexture m_simpleTexture;

		protected override void OnAwake()
		{
		}

		public void SetTextureData(ShopBundle _shopBundle)
		{
			Reset();
			m_itemTexture.gameObject.SetActive(true);
			UIItemUtil.UpdateShopItemReward(_shopBundle, false, m_itemTexture, UIItemUtil.ItemUVCoordType.ENone, 0f);
		}

		public void SetTextureData(EResourceType _resType)
		{
			Reset();
			m_itemTexture.gameObject.SetActive(true);
			UIItemUtil.UpdateResourceItem(_resType, false, m_itemTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
		}

		public void SetTextureData(LootTemplate _template)
		{
			Reset();
			m_itemTexture.gameObject.SetActive(true);
			UIItemUtil.UpdateItemByLootTemplate(_template, false, m_itemTexture, 0f);
		}

		public void SetTextureData(UIPortraitTexture _texture)
		{
			Reset();
			m_itemTexture.gameObject.SetActive(true);
			m_itemTexture.SetBasicMaterialData(_texture);
		}

		public void SetTextureData(UITexture _texture)
		{
			Reset();
			m_simpleTexture.gameObject.SetActive(true);
			m_simpleTexture.mainTexture = _texture.mainTexture;
		}

		private void Reset()
		{
			m_itemTexture.gameObject.SetActive(false);
			m_simpleTexture.gameObject.SetActive(false);
		}
	}
}
