using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Shop
{
	public class UIShopProductLabelView : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_label;

		[CheckAssigned]
		[SerializeField]
		private UISprite m_icon;

		[SerializeField]
		[CheckAssigned]
		private UISprite m_background;

		[SerializeField]
		[CheckAssigned]
		private UIAlignmentBox m_alignmentBox;

		public StringId ProductLocaKey
		{
			set
			{
				m_label.SetLocalizationKey(value);
				m_alignmentBox.RepositionNow = true;
			}
		}

		public string Icon
		{
			set
			{
				m_icon.spriteName = value;
				m_icon.MakePixelPerfect();
				m_alignmentBox.RepositionNow = true;
			}
		}

		public string Background
		{
			set
			{
				m_background.spriteName = value;
			}
		}

		public override void Show()
		{
			base.Show();
			base.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			base.gameObject.SetActive(false);
		}
	}
}
