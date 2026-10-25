using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	public class LocalizationManager
	{
		private const string s_LanguagePref = "LOC_LANGUAGE";

		private const string s_charsetFile = "Localization/charset";

		public const ELanguages FallbackLanguage = ELanguages.en_US;

		private BitArray m_charset;

		private readonly Regex m_variableRegex = new Regex("(?<start>{)(?<value>[0-9a-zA-Z_-]{1,})(?::(?<format>([A-Z]|\\d)\\d{0,}?))?(?<end>})", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		private readonly Regex m_clearNGUIFormatRegex = new Regex("\\[([0-9a-fA-F]{6}|[buiBUI]|\\\\[ibuIBU]|-)\\]", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

		private static readonly StringMapper s_defaultMapper = new StringMapper { 
		{
			"EMPTY",
			string.Empty
		} };

		private ELanguages m_language;

		public BBDictionary<StringId, string> m_cachedLocaStrings = new BBDictionary<StringId, string>();

		private BBDictionary<StringId, string> m_languageData = new BBDictionary<StringId, string>();

		private BBDictionary<StringId, string> m_currentLoadingDict;

		private static readonly BBTextFormatter s_textFormatter = new BBTextFormatter();

		private readonly List<object> m_mappedValues = new List<object>();

		private bool m_tagTranslatedText;

		private bool m_showStringId;

		public static List<EventDelegate> onLanguageChange = new List<EventDelegate>();

		private static LocalizationManager m_instance;

		public ELanguages CurrentLanguage
		{
			get
			{
				return m_language;
			}
		}

		public bool IsFinalLanguageInitilized { get; private set; }

		public bool TagTranslatedText
		{
			get
			{
				return m_tagTranslatedText;
			}
			set
			{
				if (m_tagTranslatedText != value)
				{
					m_tagTranslatedText = value;
					EventDelegate.Execute(onLanguageChange);
				}
			}
		}

		public bool ShowStringId
		{
			get
			{
				return m_showStringId;
			}
			set
			{
				if (m_showStringId != value)
				{
					m_showStringId = value;
					EventDelegate.Execute(onLanguageChange);
				}
			}
		}

		public static LocalizationManager Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new LocalizationManager();
					m_instance.Setup();
				}
				return m_instance;
			}
		}

		public static bool LanguageLoaded
		{
			get
			{
				return m_instance != null && m_instance.m_languageData != null;
			}
		}

		private LocalizationManager()
		{
		}

		public static LocalizationManager Initialize()
		{
			return Instance;
		}

		public void PurgeManager()
		{
			PlayerPrefs.SetString("LOC_LANGUAGE", m_language.ToString());
		}

		public void ResetManager()
		{
		}

		private void Setup()
		{
			ELanguages eLanguages = Main.Instance.BuildDefinition.PreferredLanguage;
			if (!UIDataMapper.AvailableLanguages.ContainsKey((int)eLanguages))
			{
				eLanguages = ELanguages.en_US;
			}
			TextAsset language = UnityEngine.Resources.Load<TextAsset>(UIDataMapper.AvailableLanguages[(int)eLanguages]);
			LoadLanguage(language);
			m_language = eLanguages;
			EventDelegate.Execute(onLanguageChange);
			TextAsset textAsset = UnityEngine.Resources.Load<TextAsset>("Localization/charset");
			m_charset = new BitArray(textAsset.bytes);
			m_cachedLocaStrings.Add(9770, GetRaw(9770));
			m_cachedLocaStrings.Add(9771, GetRaw(9771));
			m_cachedLocaStrings.Add(9772, GetRaw(9772));
			m_cachedLocaStrings.Add(9773, GetRaw(9773));
			m_cachedLocaStrings.Add(9774, GetRaw(9774));
		}

		private string Translate(StringId _key)
		{
			if (_key == null || !_key.Valid)
			{
				return string.Empty;
			}
			string value;
			if (!m_languageData.TryGetValue(_key, out value) && !m_cachedLocaStrings.TryGetValue(_key, out value))
			{
				return string.Empty;
			}
			return value;
		}

		public List<string> GetParametersInString(StringId _stringId)
		{
			string raw = GetRaw(_stringId);
			List<string> parameters = new List<string>();
			m_variableRegex.Replace(raw, delegate(Match m)
			{
				string value = m.Groups["value"].Captures[0].Value;
				parameters.Add(value);
				return null;
			});
			return parameters;
		}

		public string Get(StringId _key, StringMapper _mapper = null)
		{
			string raw = GetRaw(_key);
			return Get(raw, _mapper);
		}

		public string GetRaw(StringId _key)
		{
			int locaReplacementThemeKey = Singleton<StateManager>.Instance.ThemesManager.GetLocaReplacementThemeKey((int)(long)_key);
			return Translate(locaReplacementThemeKey);
		}

		public string Get(string _baseString, StringMapper _mapper)
		{
			string empty = string.Empty;
			_mapper = _mapper ?? s_defaultMapper;
			m_mappedValues.Clear();
			empty = m_variableRegex.Replace(_baseString, delegate(Match m)
			{
				string value = m.Groups["start"].Captures[0].Value;
				string value2 = m.Groups["end"].Captures[0].Value;
				string text = ((m.Groups["format"].Captures.Count <= 0) ? string.Empty : (":" + m.Groups["format"].Captures[0].Value));
				string value3 = m.Groups["value"].Captures[0].Value;
				string key = value3.ToLowerInvariant();
				object value4;
				if (_mapper.TryGetValue(key, out value4))
				{
					m_mappedValues.Add(value4);
					return value + (m_mappedValues.Count - 1) + text + value2;
				}
				return string.Empty;
			});
			try
			{
				if (_mapper != null)
				{
					return string.Format(s_textFormatter, empty, m_mappedValues.ToArray());
				}
				return _baseString;
			}
			catch (FormatException)
			{
				return string.Empty;
			}
		}

		public string StripNGUIAndNewLineFormat(StringId _locaKey, StringMapper _mapper = null)
		{
			return StripNGUIAndNewLineFormat(Get(_locaKey, _mapper), _mapper);
		}

		public string StripNGUIAndNewLineFormat(string _locaText, StringMapper _mapper = null)
		{
			string removeFormat = _locaText.Replace("\n", " ");
			return ClearNGUIFormat(removeFormat);
		}

		public string ClearNGUIFormat(string _removeFormat)
		{
			return m_clearNGUIFormatRegex.Replace(_removeFormat, string.Empty);
		}

		public string GetInteger(long _value, string _format = null)
		{
			return FormatNumber(_value, _format);
		}

		public string GetDecimal(double _value, string _format = null)
		{
			return FormatNumber(_value, _format);
		}

		private static string FormatNumber(object _number, string _format = null)
		{
			if (_format == null)
			{
				return string.Format(s_textFormatter, "{0}", _number);
			}
			string format = "{0:" + _format + "}";
			return string.Format(s_textFormatter, format, _number);
		}

		public bool HasKey(StringId _key)
		{
			return m_languageData.ContainsKey(_key);
		}

		public IEnumerator ChangeLanguageAsync(TranslationsDef _translationsDef)
		{
			if (!IsFinalLanguageInitilized)
			{
				IsFinalLanguageInitilized = true;
				WaitForEndOfFrame wait = new WaitForEndOfFrame();
				TextAsset languageAsset = _translationsDef.TranslationResource.GetObject<TextAsset>();
				m_language = _translationsDef.Language;
				IEnumerator ite = LoadLanguageAsync(languageAsset);
				while (ite.MoveNext())
				{
					yield return wait;
				}
				m_languageData = m_currentLoadingDict;
				m_currentLoadingDict = null;
				EventDelegate.Execute(onLanguageChange);
				_translationsDef.TranslationResource.ClearCache();
			}
		}

		private void LoadLanguage(TextAsset _language)
		{
			IEnumerator enumerator = LoadLanguageAsync(_language);
			while (enumerator.MoveNext())
			{
			}
			m_languageData = m_currentLoadingDict;
			m_currentLoadingDict = null;
		}

		private IEnumerator LoadLanguageAsync(TextAsset _language)
		{
			IEnumerator ite = LoadLanguageFileAsync(_language);
			while (ite.MoveNext())
			{
				yield return new WaitForEndOfFrame();
			}
		}

		private IEnumerator LoadLanguageFileAsync(TextAsset _language)
		{
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			long mem2 = Profiler.GetMonoUsedSize();
			Exception error = null;
			bool done = false;
			byte[] bytes = _language.bytes;
			ThreadPool.QueueUserWorkItem(delegate
			{
				try
				{
					using (MemoryStream stream = new MemoryStream(bytes))
					{
						using (DataBinarySerializer dataBinarySerializer = new DataBinarySerializer(stream, EMode.EMode_Read))
						{
							int num = dataBinarySerializer.SerializeInt32("total");
							m_currentLoadingDict = new BBDictionary<StringId, string>(num);
							for (int i = 0; i < num; i++)
							{
								long num2 = dataBinarySerializer.SerializeInt64("id", 0L);
								string value = dataBinarySerializer.SerializeString("c", string.Empty);
								m_currentLoadingDict.Add(num2, value);
							}
						}
					}
				}
				catch (Exception ex)
				{
					error = ex;
				}
				done = true;
			});
			while (!done)
			{
				yield return wait;
			}
			mem2 = Profiler.GetMonoUsedSize() - mem2;
			if (error != null)
			{
				m_currentLoadingDict = null;
			}
		}

		public bool CanDisplayGlyph(int _glyphId)
		{
			if (m_charset == null)
			{
				return false;
			}
			if (_glyphId < 0 || _glyphId >= m_charset.Count)
			{
				return false;
			}
			return m_charset[_glyphId];
		}
	}
}
