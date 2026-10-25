using System.Collections;
using AcierProtocol;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.ShopScreen
{
	public class UIHeroicRankUnlock : MonoBehaviour
	{
		[SerializeField]
		private UIPortraitTexture m_heroicRankTexture;

		[SerializeField]
		private UIExtendedLabel m_headerText;

		[SerializeField]
		private UIExtendedLabel m_messageText;

		private UnityEngine.Animation m_animation;

		public void Awake()
		{
			m_animation = GetComponent<UnityEngine.Animation>();
		}

		public void OnEnable()
		{
			StartCoroutine(RunAnimation());
		}

		public void Init(int _rank, ShopBundle _shopBundle)
		{
			HeroicRankDescription value;
			UbiCore.Utils.Singleton<UIDataMapper>.Instance.HeroicRankDescription.TryGetValue(_rank, out value);
			string text = LocalizationManager.Instance.Get(value.NameId);
			m_headerText.text = LocalizationManager.Instance.Get(7799, new StringMapper { { "heroic_rank", text } });
			UIItemUtil.UpdateShopItemReward(_shopBundle, true, m_heroicRankTexture, UIItemUtil.ItemUVCoordType.ENone, 0f);
			m_messageText.text = text;
		}

		private IEnumerator RunAnimation()
		{
			UINotificationPopup popup = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UINotificationPopup>();
			popup.IsShowingNotificationPaused = true;
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.IsAnimationPlaying = true;
			m_animation.Play();
			while (m_animation.isPlaying)
			{
				yield return 0;
			}
			popup.IsShowingNotificationPaused = false;
			UbiCore.Utils.Singleton<UiMain>.Instance.FlowHandler.IsAnimationPlaying = false;
			base.gameObject.SetActive(false);
		}
	}
}
