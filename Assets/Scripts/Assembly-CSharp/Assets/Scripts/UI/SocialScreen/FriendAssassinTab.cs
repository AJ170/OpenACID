using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.SocialScreen
{
	public class FriendAssassinTab : MonoBehaviour
	{
		[SerializeField]
		private UIWidget m_widget;

		[SerializeField]
		private UIPortraitTexture m_portrait;

		[SerializeField]
		private UISprite m_highlight;

		[SerializeField]
		private UIEventTrigger m_uiEventTrigger;

		public UIWidget Widget
		{
			get
			{
				return m_widget;
			}
		}

		public UIPortraitTexture Portrait
		{
			get
			{
				return m_portrait;
			}
		}

		public UISprite Highlight
		{
			get
			{
				return m_highlight;
			}
		}

		public UIEventTrigger UIEventTrigger
		{
			get
			{
				return m_uiEventTrigger;
			}
		}

		public FriendAssassin Assassin { get; set; }

		public float Clock { get; set; }
	}
}
