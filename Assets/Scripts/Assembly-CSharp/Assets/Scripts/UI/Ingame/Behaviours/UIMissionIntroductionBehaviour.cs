using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIMissionIntroductionBehaviour : UIBaseBehaviour
	{
		public delegate void TapScreen();

		[SerializeField]
		private GameObject m_mainContainer;

		[SerializeField]
		private UITweenHelper m_tweenHelper;

		[SerializeField]
		private UIExtendedLabel m_introductionText;

		[SerializeField]
		private UIMissionIntroductionPlaceHolders[] m_placeHolders;

		[SerializeField]
		private int m_uiLocaStringID;

		[SerializeField]
		private Introduction.EUIAlignment m_uiAlignment = Introduction.EUIAlignment.BottomLeft;

		[SerializeField]
		private Vector2 m_uiFontOffset = Vector2.zero;

		[SerializeField]
		private Color m_uiFontColor = Color.white;

		[SerializeField]
		private int m_uiFontSize;

		public TapScreen OnTapScreen;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.MissionIntroduction;
			}
		}

		public void ShowIntroductionText(int _uiLocaStringID, Introduction.EUIAlignment _uiAlignment, Vector2 _uiFontOffset, Color _uiFontColor, int _uiFontSize)
		{
			if (m_uiLocaStringID >= 0)
			{
				m_uiLocaStringID = _uiLocaStringID;
			}
			m_uiAlignment = _uiAlignment;
			m_uiFontOffset = _uiFontOffset;
			m_uiFontColor = _uiFontColor;
			m_uiFontSize = _uiFontSize;
			UpdateIntroductionText();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void Init()
		{
			m_tweenHelper.gameObject.SetActive(false);
			m_mainContainer.SetActive(true);
		}

		public void SetData(string _localizedLocation)
		{
			m_introductionText.LocalizationKey = StringId.Invalid;
			m_uiLocaStringID = (int)(long)StringId.Invalid;
			m_introductionText.text = _localizedLocation;
		}

		private void OnPress(bool _isPressed)
		{
			if (!_isPressed && UICamera.hoveredObject == base.gameObject && OnTapScreen != null)
			{
				OnTapScreen();
			}
		}

		[ContextMenu("UpdateIntroductionText")]
		private void UpdateIntroductionText()
		{
			for (int i = 0; i < m_placeHolders.Length; i++)
			{
				UIMissionIntroductionPlaceHolders uIMissionIntroductionPlaceHolders = m_placeHolders[i];
				if (uIMissionIntroductionPlaceHolders.UIAlignment == m_uiAlignment)
				{
					Transform transform = m_introductionText.transform;
					transform.parent = uIMissionIntroductionPlaceHolders.ParentTransform;
					UIWidget component = uIMissionIntroductionPlaceHolders.ParentTransform.GetComponent<UIWidget>();
					m_introductionText.pivot = component.pivot;
					m_introductionText.color = m_uiFontColor;
					m_introductionText.fontSize = m_uiFontSize;
					transform.localPosition = m_uiFontOffset;
					break;
				}
			}
			if (m_uiLocaStringID > 0)
			{
				m_introductionText.LocalizationKey = m_uiLocaStringID;
			}
			m_tweenHelper.SetVisibility(true);
		}
	}
}
