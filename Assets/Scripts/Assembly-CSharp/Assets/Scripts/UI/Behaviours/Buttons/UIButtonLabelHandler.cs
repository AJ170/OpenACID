using System;
using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIExtendedButton))]
	public class UIButtonLabelHandler : MonoBehaviour
	{
		private const int LABEL_FONT_SIZE = 48;

		private const int LABEL_MIN_SIZE = 40;

		private const string ICON_NAME = "#1 Icon";

		private const string ICON_SECOND_POS_NAME = "#2 Icon";

		private const string RESOURCE_NAME = "#3 Resource";

		private const string LABEL_NAME = "#2 Label";

		private const string LABEL_SECOND_POS_NAME = "#1 Label";

		[SerializeField]
		private UIExtendedLabel m_label;

		[SerializeField]
		private UISprite m_iconSprite;

		[SerializeField]
		private bool m_showResource;

		[SerializeField]
		private UIAlignmentBox m_vBox;

		[SerializeField]
		private EResourceType m_resourceType;

		[SerializeField]
		private UIAtlas m_atlas;

		[SerializeField]
		private string m_spriteName;

		[SerializeField]
		private bool m_showLabel = true;

		[SerializeField]
		private bool m_showIconOnSecondPosition;

		[SerializeField]
		private UIWidget m_contentContainer;

		private bool m_iconVisible;

		public EResourceType ResourceType
		{
			set
			{
				if (m_resourceType != value)
				{
					m_resourceType = value;
					UpdateVisuals();
				}
			}
		}

		public bool ShowResource
		{
			get
			{
				return m_showResource;
			}
			set
			{
				if (m_showResource != value)
				{
					m_showResource = value;
					UpdateVisuals();
				}
			}
		}

		public bool ShowIcon
		{
			set
			{
				if (m_iconVisible != value)
				{
					m_iconVisible = value && m_atlas != null && !string.IsNullOrEmpty(m_spriteName);
					UpdateVisuals();
				}
			}
		}

		public bool ShowLabel
		{
			get
			{
				return m_showLabel;
			}
			set
			{
				if (m_showLabel != value)
				{
					m_showLabel = value;
					UpdateVisuals();
				}
			}
		}

		public UIExtendedLabel Label
		{
			get
			{
				return m_label;
			}
		}

		public UISprite Sprite
		{
			get
			{
				return m_iconSprite;
			}
		}

		private void Awake()
		{
			if (m_label != null)
			{
				m_label.gameObject.name = ((!m_showIconOnSecondPosition) ? "#2 Label" : "#1 Label");
				m_label.fontSize = 48;
				m_label.color = Color.white;
				m_label.applyGradient = false;
				m_label.supportEncoding = false;
				m_label.maxLineCount = 2;
				m_label.overflowMethod = UILabel.Overflow.DynamicScale;
				m_label.MinimumFontSize = 40;
				m_vBox.MinPadding.x = 5f;
			}
			m_vBox.AlignToPivot = true;
			m_vBox.HideInactive = true;
			m_vBox.Sorted = true;
			m_vBox.StrechWidget = false;
			m_iconVisible = m_atlas != null && !string.IsNullOrEmpty(m_spriteName);
			m_iconSprite.gameObject.name = ((!m_showIconOnSecondPosition) ? "#1 Icon" : "#2 Icon");
		}

		private void OnEnable()
		{
			UISprite iconSprite = m_iconSprite;
			iconSprite.onResize = (UIWidget.OnDimensionsChanged)Delegate.Combine(iconSprite.onResize, new UIWidget.OnDimensionsChanged(OnSpriteResize));
			UpdateVisuals();
		}

		private void OnDisable()
		{
			UISprite iconSprite = m_iconSprite;
			iconSprite.onResize = (UIWidget.OnDimensionsChanged)Delegate.Remove(iconSprite.onResize, new UIWidget.OnDimensionsChanged(OnSpriteResize));
		}

		private void OnSpriteResize()
		{
			m_vBox.RepositionNow = true;
		}

		public void SetIcon(UIAtlas _atlas, string _spriteName)
		{
			m_iconVisible = true;
			SetIconImpl(_atlas, _spriteName);
			UpdateVisuals();
			m_vBox.RepositionNow = true;
		}

		public void SetText(StringId _id, StringMapper _mapper = null)
		{
			if (m_label != null)
			{
				m_label.UseLocalizedText = true;
				m_label.SetLocalizationKey(_id, _mapper);
				m_vBox.RepositionNow = true;
				ShowLabel = true;
				UpdateVisuals();
			}
		}

		public void SetText(string _text)
		{
			if (m_label != null)
			{
				m_label.UseLocalizedText = false;
				m_label.text = _text;
				m_vBox.RepositionNow = true;
				ShowLabel = true;
				UpdateVisuals();
			}
		}

		private void UpdateVisuals()
		{
			if (m_showResource)
			{
				UpdateResource();
			}
			else if (m_iconVisible)
			{
				UpdateIcon();
			}
			else
			{
				m_iconSprite.gameObject.SetActive(false);
			}
			if (m_label != null)
			{
				m_label.gameObject.SetActive(ShouldShowLabel());
				UpdateLabel();
			}
			m_vBox.RepositionNow = true;
		}

		private bool ShouldShowLabel()
		{
			if (m_label.UseLocalizedText && !m_label.LocalizationKey.Valid)
			{
				return false;
			}
			return m_showLabel && !string.IsNullOrEmpty(m_label.text);
		}

		private void UpdateLabel()
		{
			if (!(m_label == null))
			{
				if (m_iconSprite.gameObject.activeSelf)
				{
					float num = (float)m_iconSprite.width + m_vBox.MinPadding.x;
					m_contentContainer.Update();
					m_label.MaxBoxContainer = null;
					m_label.MaxBoxHeight = m_contentContainer.height;
					m_label.MaxBoxWidth = (int)((float)m_contentContainer.width - num);
					m_label.ProcessText();
				}
				else
				{
					m_label.MaxBoxContainer = m_contentContainer;
					m_label.ProcessText();
				}
			}
		}

		private void UpdateIcon()
		{
			SetIconImpl(m_atlas, m_spriteName);
			m_iconSprite.gameObject.name = ((!m_showIconOnSecondPosition) ? "#1 Icon" : "#2 Icon");
		}

		private void SetIconImpl(UIAtlas _atlas, string _spriteName)
		{
			m_atlas = _atlas;
			m_spriteName = _spriteName;
			m_iconSprite.atlas = _atlas;
			m_iconSprite.spriteName = _spriteName;
			m_iconSprite.MakePixelPerfect();
			bool active = !(_atlas == null) && !string.IsNullOrEmpty(_spriteName) && (m_iconVisible || m_showResource);
			m_iconSprite.gameObject.SetActive(active);
		}

		private void UpdateResource()
		{
			CurrencyDescription value;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.CurrencyDescriptions.TryGetValue((int)m_resourceType, out value))
			{
				UIAtlas atlasByType = Singleton<UIScreenManager>.Instance.GetAtlasByType(UIScreenManager.EAtlasType.Shared);
				SetIconImpl(atlasByType, value.SmallIconName);
				m_iconSprite.gameObject.name = "#3 Resource";
			}
			else
			{
				m_iconSprite.gameObject.SetActive(false);
			}
		}
	}
}
