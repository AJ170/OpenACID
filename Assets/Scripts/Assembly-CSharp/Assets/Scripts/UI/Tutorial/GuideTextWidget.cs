using System.Collections.Generic;
using Assets.Scripts.UI.Localization;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial
{
	public class GuideTextWidget : MonoBehaviour
	{
		public struct Data
		{
			public class Defaults
			{
				private static string m_buttonText;

				public static string ButtonText
				{
					get
					{
						string obj = m_buttonText ?? LocalizationManager.Instance.Get(1780);
						m_buttonText = obj;
						return obj;
					}
				}
			}

			public string Title;

			public string Text;

			public string ButtonText;

			private static Defaults m_default;

			public static Defaults Default
			{
				get
				{
					Defaults obj = m_default ?? new Defaults();
					m_default = obj;
					return obj;
				}
			}
		}

		[SerializeField]
		private UISprite m_icon;

		[SerializeField]
		private UIExtendedLabel m_title;

		[SerializeField]
		private UIExtendedLabel m_text;

		[SerializeField]
		private GameObject m_button;

		[SerializeField]
		private UIExtendedLabel m_buttonLabel;

		private readonly List<EventDelegate> m_eventDelegates = new List<EventDelegate>();

		private void Awake()
		{
		}

		private void OnButtonClick()
		{
			EventDelegate.Execute(m_eventDelegates);
		}

		public void Set(Data _data, EventDelegate.Callback _callback)
		{
			m_title.text = _data.Title ?? m_title.text;
			m_text.text = _data.Text ?? m_text.text;
			m_buttonLabel.text = _data.Text ?? m_buttonLabel.text;
			if (string.IsNullOrEmpty(m_buttonLabel.text))
			{
				m_button.SetActive(false);
			}
			else
			{
				m_button.SetActive(true);
			}
			m_eventDelegates.Clear();
			EventDelegate.Add(m_eventDelegates, _callback, true);
		}
	}
}
