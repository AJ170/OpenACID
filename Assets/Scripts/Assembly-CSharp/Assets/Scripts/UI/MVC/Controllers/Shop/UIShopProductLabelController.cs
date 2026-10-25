using Assets.Scripts.UI.MVC.Models.Shop;
using Assets.Scripts.UI.MVC.UIView.Shop;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Shop
{
	public class UIShopProductLabelController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIShopProductLabelView m_shopProductLabel;

		private UIShopProductLabelModel m_model;

		public override void Show()
		{
			base.Show();
			m_shopProductLabel.Show();
		}

		public override void Hide()
		{
			base.Hide();
			m_shopProductLabel.Hide();
		}

		public void UpdateByModel(UIShopProductLabelModel _model)
		{
			m_model = _model;
			m_shopProductLabel.ProductLocaKey = _model.LocaKey;
			m_shopProductLabel.Background = _model.Background;
			m_shopProductLabel.Icon = _model.Icon;
		}
	}
}
