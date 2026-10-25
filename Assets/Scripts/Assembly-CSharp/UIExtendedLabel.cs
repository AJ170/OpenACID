using System.Collections;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.Layout;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

[AddComponentMenu("NGUI/BB/UI/Extended Label")]
[ExecuteInEditMode]
public class UIExtendedLabel : UILabel
{
	[SerializeField]
	private string m_tagNameId = string.Empty;

	[SerializeField]
	private bool m_useLocalizedText;

	[SerializeField]
	private int m_localizationKey = -1;

	private int m_originalFontSize;

	private StringMapper m_localizationVariables;

	private bool m_checkIfTextFits;

	private long m_unformattedNumber;

	public string TagNameId
	{
		get
		{
			return m_tagNameId;
		}
		set
		{
			if (m_tagNameId == value)
			{
				return;
			}
			if (mPlayMode)
			{
				if (m_tagNameId == string.Empty)
				{
					m_originalFontSize = base.fontSize;
				}
				else if (value == string.Empty)
				{
					base.fontSize = m_originalFontSize;
				}
			}
			m_tagNameId = value;
			UpdateDeviceLayout();
		}
	}

	public bool UseLocalizedText
	{
		get
		{
			return m_useLocalizedText;
		}
		set
		{
			m_useLocalizedText = value;
			if (base.gameObject.activeInHierarchy && m_useLocalizedText)
			{
				OnLanguageChange();
			}
		}
	}

	public StringId LocalizationKey
	{
		get
		{
			return m_localizationKey;
		}
		set
		{
			m_localizationKey = (int)(long)value;
			if (m_localizationKey != -1)
			{
				m_useLocalizedText = true;
			}
			m_localizationVariables = null;
			OnLanguageChange();
		}
	}

	public UILayoutManager LayoutManager
	{
		get
		{
			if (!Application.isPlaying)
			{
				return null;
			}
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				return UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager;
			}
			return null;
		}
	}

	public string LocalizedText
	{
		get
		{
			return base.text;
		}
		set
		{
			m_useLocalizedText = false;
			base.text = value;
		}
	}

	public void ClearFormatFlags()
	{
		m_unformattedNumber = 0L;
		m_useLocalizedText = false;
		m_checkIfTextFits = false;
	}

	public void SetNumber(long _number)
	{
		m_unformattedNumber = _number;
		m_useLocalizedText = false;
		base.text = LocalizationManager.Instance.GetInteger(_number);
	}

	public void SetAbbreviatedNumberIfNotFit(long _number)
	{
		m_unformattedNumber = _number;
		m_useLocalizedText = false;
		m_checkIfTextFits = true;
		base.text = LocalizationManager.Instance.GetInteger(_number);
	}

	public void SetLocalizationKey(StringId _key, StringMapper _variables = null)
	{
		if (!(_key == null))
		{
			m_localizationKey = (int)(long)_key;
			if (m_localizationKey != -1)
			{
				m_useLocalizedText = true;
			}
			m_localizationVariables = _variables;
			OnLanguageChange();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		m_originalFontSize = base.fontSize;
	}

	protected override void OnStart()
	{
		UpdateDeviceLayout();
		base.OnStart();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (m_useLocalizedText)
		{
			OnLanguageChange();
		}
		if (LayoutManager != null)
		{
			OnDeviceLayoutChanged();
		}
		else
		{
			StartCoroutine(WaitForValidLayoutManager());
		}
		ConfigFont();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		StopAllCoroutines();
	}

	private IEnumerator WaitForValidLayoutManager()
	{
		while (LayoutManager == null)
		{
			yield return new WaitForEndOfFrame();
		}
		OnDeviceLayoutChanged();
	}

	private void OnLanguageChange()
	{
		ConfigFont();
		if (m_useLocalizedText)
		{
			base.text = LocalizationManager.Instance.Get(m_localizationKey, m_localizationVariables);
		}
	}

	private void ConfigFont()
	{
		FontConfig value;
		if (UbiCore.Utils.Singleton<UIDataMapper>.IsAvailable && UbiCore.Utils.Singleton<UIDataMapper>.Instance.FontConfigs.TryGetValue((int)LocalizationManager.Instance.CurrentLanguage, out value))
		{
			base.GlyphOffsetY = value.GlyphOffsetY;
			base.spacingY += value.LineSpacing;
			base.MixedOffsetY = value.MixedOffsetY;
		}
	}

	private void OnDeviceLayoutChanged()
	{
		UpdateDeviceLayout();
	}

	private void UpdateDeviceLayout()
	{
		if (mPlayMode && !string.IsNullOrEmpty(m_tagNameId) && LayoutManager != null)
		{
			FamilyLayoutData familyLayoutData = LayoutManager.GetFamilyLayoutData(m_tagNameId);
			if (familyLayoutData != null)
			{
				base.fontSize = familyLayoutData.FontData.FontSize;
			}
			else
			{
				base.fontSize = m_originalFontSize;
			}
		}
	}

	protected override void OnProcessTextFinished(bool _textFits)
	{
		base.OnProcessTextFinished(_textFits);
		if (m_checkIfTextFits)
		{
			m_checkIfTextFits = false;
			base.text = LocalizationHelper.AbbreviateNumbers(m_unformattedNumber);
		}
	}
}
