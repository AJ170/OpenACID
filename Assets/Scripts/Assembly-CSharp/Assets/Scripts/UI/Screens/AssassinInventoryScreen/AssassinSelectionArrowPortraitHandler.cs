using AcierProtocol;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class AssassinSelectionArrowPortraitHandler : MonoBehaviour
	{
		[SerializeField]
		private UIPortraitTexture m_portraitTexture;

		[SerializeField]
		private UISprite m_arrowSprite;

		[SerializeField]
		private GameObject m_buyButton;

		[SerializeField]
		private bool m_allowBuySlot;

		public bool Active
		{
			set
			{
				m_portraitTexture.gameObject.SetActive(value);
				m_arrowSprite.enabled = value;
				if (m_buyButton != null)
				{
					if (m_allowBuySlot)
					{
						m_buyButton.gameObject.SetActive(!value);
					}
					else
					{
						m_buyButton.gameObject.SetActive(false);
					}
				}
			}
		}

		public void Init(Assassin _assassin, bool _placeholder)
		{
			if (_assassin != null)
			{
				UIPortraitUtil.UpdatePortraitByColorDef(_assassin, m_portraitTexture);
			}
			else
			{
				Active = false;
			}
		}
	}
}
