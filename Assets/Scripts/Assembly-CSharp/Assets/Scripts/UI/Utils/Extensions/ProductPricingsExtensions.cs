using AcierProtocol;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class ProductPricingsExtensions
	{
		public static ProductPricing GetProductPricing(this ProductPricings _productPricings, string _id)
		{
			if (_productPricings == null)
			{
				return null;
			}
			for (int i = 0; i < _productPricings.prices.Length; i++)
			{
				ProductPricing productPricing = _productPricings.prices[i];
				if (productPricing.productId == _id)
				{
					return productPricing;
				}
			}
			return null;
		}
	}
}
