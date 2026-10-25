using UnityEngine;

namespace Assets.Scripts.Native
{
	// Native tools for platforms without a platform plugin (PS Vita, editor): the
	// NativeToolsBase defaults plus the values Unity can report itself.
	public class NativeToolsGeneric : NativeToolsBase
	{
		public override string GetPreferredLanguage()
		{
			switch (Application.systemLanguage)
			{
			case SystemLanguage.French:
				return "fr_FR";
			case SystemLanguage.German:
				return "de_DE";
			case SystemLanguage.Spanish:
				return "es_ES";
			case SystemLanguage.Italian:
				return "it_IT";
			case SystemLanguage.Japanese:
				return "ja_JP";
			case SystemLanguage.Korean:
				return "ko_KR";
			case SystemLanguage.Portuguese:
				return "pt_BR";
			case SystemLanguage.Russian:
				return "ru_RU";
			case SystemLanguage.ChineseSimplified:
				return "zh_CN";
			case SystemLanguage.ChineseTraditional:
				return "zh_TW";
			default:
				return "en_US";
			}
		}

		public override string BundleIdentifier()
		{
			return Application.identifier;
		}

		public override string BundleVersion()
		{
			return Application.version;
		}
	}
}
