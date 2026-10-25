using AcierProtocol;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIRecivedFromMysteryBox : UIRecivedBase
	{
		[SerializeField]
		private UIPortraitTexture m_itemTexture;

		[SerializeField]
		private MysteryBoxEffects m_effects;

		protected override void OnAwake()
		{
		}

		public void PlayMysteryBoxEffects(Quality.EQuality _quality)
		{
			if (m_effects != null)
			{
				m_effects.PlayEffect(_quality);
			}
		}

		public void SetTextureData(ShopBundle _shopBundle)
		{
			UIItemUtil.UpdateShopItemReward(_shopBundle, false, m_itemTexture, UIItemUtil.ItemUVCoordType.ENone, 0f);
		}

		public void SetTextureData(EResourceType _resType)
		{
			UIItemUtil.UpdateResourceItem(_resType, false, m_itemTexture, false, UIItemUtil.ItemUVCoordType.ENone, string.Empty, 0f);
		}

		public void SetTextureData(LootTemplate _template)
		{
			UIItemUtil.UpdateItemByLootTemplate(_template, false, m_itemTexture, 0f);
		}

		public void SetTextureData(UIPortraitTexture _texture)
		{
			m_itemTexture.SetBasicMaterialData(_texture);
		}
	}
}
