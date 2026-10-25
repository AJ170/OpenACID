using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.ScreenManager
{
	public class LoadingOverlayConfiguration
	{
		private static LoadingOverlayConfiguration m_default;

		public string Heading;

		public string SubHeading;

		public static LoadingOverlayConfiguration Default
		{
			get
			{
				LoadingOverlayConfiguration loadingOverlayConfiguration = m_default;
				if (loadingOverlayConfiguration == null)
				{
					LoadingOverlayConfiguration loadingOverlayConfiguration2 = new LoadingOverlayConfiguration();
					loadingOverlayConfiguration2.Heading = LocalizationManager.Instance.Get(2274);
					loadingOverlayConfiguration2.SubHeading = LocalizationManager.Instance.Get(2275);
					loadingOverlayConfiguration = loadingOverlayConfiguration2;
				}
				m_default = loadingOverlayConfiguration;
				return loadingOverlayConfiguration;
			}
		}
	}
}
