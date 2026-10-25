using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIMissionActionButtonBar : MonoBehaviour
	{
		public enum EButtonType
		{
			Primary = 0,
			Secondary = 1
		}

		[SerializeField]
		private UIExtendedButton m_primaryButton;

		[SerializeField]
		private UIExtendedButton m_secondaryButton;

		private bool m_isUpdating = true;

		public bool IsUpdating
		{
			get
			{
				return m_isUpdating;
			}
			set
			{
				m_isUpdating = value;
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.IsUpdating = value;
			}
		}

		private void Awake()
		{
		}

		public void SetupButton(EButtonType _type, StringId _locaId, bool _isVisible, EResourceType? _eResourceType = null)
		{
			UIExtendedButton buttonByType = GetButtonByType(_type);
			if (buttonByType == null)
			{
				return;
			}
			UIButtonLabelHandler component = buttonByType.gameObject.GetComponent<UIButtonLabelHandler>();
			if (component != null)
			{
				component.ShowResource = _eResourceType.HasValue;
				if (_eResourceType.HasValue)
				{
					component.ResourceType = _eResourceType.Value;
				}
			}
			buttonByType.SetLabel(_locaId);
			buttonByType.gameObject.SetActive(_isVisible);
		}

		public void ChangeButtonVisibility(EButtonType _type, bool _isVisible)
		{
			UIExtendedButton buttonByType = GetButtonByType(_type);
			if (!(buttonByType == null))
			{
				buttonByType.gameObject.SetActive(_isVisible);
			}
		}

		public UIExtendedButton GetButtonByType(EButtonType _type)
		{
			switch (_type)
			{
			case EButtonType.Primary:
				return m_primaryButton;
			case EButtonType.Secondary:
				return m_secondaryButton;
			default:
				return null;
			}
		}
	}
}
