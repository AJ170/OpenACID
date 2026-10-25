using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Helpers
{
	public class UIImageLocalizationHelper
	{
		public static Texture GetLogoForCurrentLanguage()
		{
			ELanguages preferredLanguage = Main.Instance.BuildDefinition.PreferredLanguage;
			string path = "UI/Logos/Texture/intromission_logo_01_UK";
			switch (preferredLanguage)
			{
			case ELanguages.ja_JP:
				path = "UI/Logos/Texture/intromission_logo_01_JAP";
				break;
			case ELanguages.ru_RU:
				path = "UI/Logos/Texture/intromission_logo_01_RU";
				break;
			case ELanguages.zh_CHS:
				path = "UI/Logos/Texture/intromission_logo_01_S_CH";
				break;
			case ELanguages.zh_CHT:
				path = "UI/Logos/Texture/intromission_logo_01_T_CH";
				break;
			}
			return UnityEngine.Resources.Load<Texture>(path);
		}

		public static Texture GetSplashScreenLogoForCurrentLanguage()
		{
			ELanguages preferredLanguage = Main.Instance.BuildDefinition.PreferredLanguage;
			string path = "UI/LoadingScreen/ACI_splash_screen_UK";
			switch (preferredLanguage)
			{
			case ELanguages.ja_JP:
				path = "UI/LoadingScreen/ACI_splash_screen_JAP";
				break;
			case ELanguages.ru_RU:
				path = "UI/LoadingScreen/ACI_splash_screen_RU";
				break;
			case ELanguages.zh_CHS:
				path = "UI/LoadingScreen/ACI_splash_screen_S_CH";
				break;
			case ELanguages.zh_CHT:
				path = "UI/LoadingScreen/ACI_splash_screen_T_CH";
				break;
			}
			return UnityEngine.Resources.Load<Texture>(path);
		}
	}
}
