using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UIRewardItemViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIPortraitTexture m_rewardItemTexture;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private GameObject m_highlight;

		[SerializeField]
		[CheckAssigned]
		private CompareItemHandler m_compareItemHandler;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_amountIcon;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_amountValue;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_amountContainer;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_amountAlignmentBox;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_checkMarkSprite;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private UISprite m_backgroundSprite;

		[SerializeField]
		[CheckAssigned]
		private UIRewardItemController m_controller;

		public UIPortraitTexture RewardItemTexture
		{
			get
			{
				return m_rewardItemTexture;
			}
		}

		public UISprite BackgroundSprite
		{
			get
			{
				return m_backgroundSprite;
			}
		}

		public bool IsHighlighted
		{
			get
			{
				return m_highlight != null && m_highlight.activeSelf;
			}
			set
			{
				if (m_highlight != null)
				{
					m_highlight.SetActive(value);
				}
			}
		}

		public CompareItemHandler.EState CompareState
		{
			set
			{
				m_compareItemHandler.State = value;
			}
		}

		public bool IsAmountContainerVisible
		{
			get
			{
				return m_amountContainer.gameObject.activeSelf;
			}
			set
			{
				m_amountContainer.gameObject.SetActive(value);
			}
		}

		public bool IsCheckMarkVisible
		{
			get
			{
				return m_checkMarkSprite.gameObject.activeSelf;
			}
			set
			{
				m_checkMarkSprite.gameObject.SetActive(value);
			}
		}

		public void SetupAmount(long _count, string _spriteName)
		{
			m_amountValue.SetNumber(_count);
			bool flag = !string.IsNullOrEmpty(_spriteName);
			if (flag)
			{
				m_amountIcon.spriteName = _spriteName;
			}
			m_amountIcon.gameObject.SetActive(flag);
			m_amountAlignmentBox.RepositionNow = true;
		}

		private void OnClick()
		{
			if (m_controller.IsInitialized)
			{
				m_controller.OnClickRewardItem();
			}
		}
	}
}
