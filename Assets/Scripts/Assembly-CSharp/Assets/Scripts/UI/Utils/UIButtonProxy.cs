using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Notifications;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UIButtonProxy : UIPrefabProxy
	{
		public List<EventDelegate> onClick = new List<EventDelegate>();

		public List<EventDelegate> onStateChange = new List<EventDelegate>();

		[SerializeField]
		private bool m_changeTranslation;

		[SerializeField]
		private bool m_changeIcon;

		[SerializeField]
		private bool m_changeResource;

		[SerializeField]
		private bool m_changeNewsDisplay;

		[SerializeField]
		private bool m_showLabel = true;

		[SerializeField]
		private ENewsType m_newsType;

		[SerializeField]
		private int m_translation;

		[SerializeField]
		private UIAtlas m_atlas;

		[SerializeField]
		private string m_spriteName;

		[SerializeField]
		private EResourceType m_resourceType;

		[SerializeField]
		private EButtonTypes m_type;

		[SerializeField]
		private bool m_allowDisabledClick;

		[SerializeField]
		private bool m_changeSound;

		[SerializeField]
		private EButtonSoundFamily m_soundFamily;

		[SerializeField]
		private EButtonIntent m_buttonIntent;

		[SerializeField]
		private UIGamepadSelectable.EMode m_gamepadSelectableMode;

		[SerializeField]
		private int m_gamepadSelectablePriority;

		[SerializeField]
		private UIWidget m_gamepadSelectableWidget;

		private UIExtendedButton m_proxyButton;

		public bool ShowLabel
		{
			get
			{
				return Button.GetComponent<UIButtonLabelHandler>().ShowLabel;
			}
			set
			{
				Button.GetComponent<UIButtonLabelHandler>().ShowLabel = value;
			}
		}

		public bool AllowDisabledClick
		{
			get
			{
				return Button.AllowDisabledClick;
			}
			set
			{
				Button.AllowDisabledClick = value;
			}
		}

		public bool isEnabled
		{
			get
			{
				return Button.isEnabled;
			}
			set
			{
				Button.isEnabled = value;
			}
		}

		public bool Isinstanciated
		{
			get
			{
				return m_proxyButton != null;
			}
		}

		public UIExtendedButton Button
		{
			get
			{
				if (m_proxyButton == null)
				{
					Init();
				}
				return m_proxyButton;
			}
		}

		public UIButtonColor.State ButtonState
		{
			get
			{
				return (m_proxyButton != null) ? m_proxyButton.state : UIButtonColor.State.Normal;
			}
		}

		public int TranslationKey
		{
			get
			{
				return m_translation;
			}
			set
			{
				m_translation = value;
				m_changeTranslation = true;
				Button.SetLabel(m_translation);
			}
		}

		public bool ShowIcon
		{
			set
			{
				m_changeIcon = true;
				UIButtonLabelHandler component = Button.GetComponent<UIButtonLabelHandler>();
				if (component != null)
				{
					component.ShowIcon = value;
				}
			}
		}

		public EResourceType ShowResourceType
		{
			set
			{
				m_changeResource = true;
				UIButtonLabelHandler component = Button.GetComponent<UIButtonLabelHandler>();
				if (component != null)
				{
					component.ShowResource = true;
					component.ShowIcon = false;
					component.ResourceType = value;
				}
			}
		}

		public bool ShowResource
		{
			set
			{
				UIButtonLabelHandler component = Button.GetComponent<UIButtonLabelHandler>();
				if (component != null)
				{
					component.ShowResource = value;
				}
			}
		}

		private void OnDestroy()
		{
			if (m_proxyButton != null)
			{
				EventDelegate.Remove(m_proxyButton.onClick, OnButtonClick);
				EventDelegate.Remove(m_proxyButton.onStateChange, OnStateChange);
			}
			onClick.Clear();
			m_atlas = null;
		}

		public override void Init()
		{
			if (!base.IsInitalized)
			{
				base.Init();
				m_proxyButton = m_instance.GetComponent<UIExtendedButton>();
				SetupGamepadSelectable();
				SetupTextAndIcon();
				if (Application.isPlaying)
				{
					SetupButton();
					SetupSound();
					SetupNews();
				}
				EventDelegate.Add(m_proxyButton.onClick, OnButtonClick);
				EventDelegate.Add(m_proxyButton.onStateChange, OnStateChange);
			}
		}

		public void OnButtonClick()
		{
			EventDelegate.Execute(onClick);
		}

		private void SetupGamepadSelectable()
		{
			if (m_gamepadSelectablePriority != 0 || m_gamepadSelectableMode != 0 || m_gamepadSelectableWidget != null)
			{
				UIGamepadSelectable uIGamepadSelectable = m_instance.AddComponentIfNotExist<UIGamepadSelectable>();
				uIGamepadSelectable.Mode = m_gamepadSelectableMode;
				uIGamepadSelectable.Priority = m_gamepadSelectablePriority;
				uIGamepadSelectable.Widget = m_gamepadSelectableWidget;
			}
		}

		private void SetupNews()
		{
			if (m_changeNewsDisplay)
			{
				UINewsDisplay component = m_instance.GetComponent<UINewsDisplay>();
				if (component != null)
				{
					component.Type = m_newsType;
				}
			}
		}

		private void SetupSound()
		{
			if (m_changeSound)
			{
				UIButtonSounds component = m_proxyButton.gameObject.GetComponent<UIButtonSounds>();
				if (component != null)
				{
					component.SoundFamily = m_soundFamily;
					component.ButtonIntent = m_buttonIntent;
				}
			}
		}

		private void SetupTextAndIcon()
		{
			ShowLabel = m_showLabel;
			if (m_changeTranslation)
			{
				m_proxyButton.SetLabel(m_translation);
			}
			if (m_changeIcon)
			{
				UIButtonLabelHandler component = m_proxyButton.GetComponent<UIButtonLabelHandler>();
				if (component != null)
				{
					component.SetIcon(m_atlas, m_spriteName);
				}
			}
			else if (m_changeResource)
			{
				UIButtonLabelHandler component2 = m_proxyButton.GetComponent<UIButtonLabelHandler>();
				if (component2 != null)
				{
					component2.ResourceType = m_resourceType;
					component2.ShowResource = true;
				}
			}
		}

		private void SetupButton()
		{
			m_proxyButton.Type = m_type;
			m_proxyButton.AllowDisabledClick = m_allowDisabledClick;
		}

		private void OnStateChange()
		{
			EventDelegate.Execute(onStateChange);
		}
	}
}
