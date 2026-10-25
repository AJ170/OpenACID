using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UIScreenChanger : UIInputReactor
	{
		[SerializeField]
		private UIScreen m_screen;

		[SerializeField]
		private EScreenID m_screenID = EScreenID.Invalid;

		[SerializeField]
		private UITweener m_in;

		[SerializeField]
		private UITweener m_out;

		private UIButton m_button;

		public EScreenID ScreenID
		{
			get
			{
				return m_screenID;
			}
			set
			{
				m_screenID = value;
			}
		}

		public UIScreen Screen
		{
			get
			{
				return Singleton<UIScreenManager>.Instance.GetScreen(m_screenID);
			}
			set
			{
				m_screen = value;
			}
		}

		public UITweener InTween
		{
			get
			{
				return m_in;
			}
			set
			{
				m_in = value;
			}
		}

		public UITweener OutTween
		{
			get
			{
				return m_out;
			}
			set
			{
				m_out = value;
			}
		}

		public bool IsCurrentScreentActive
		{
			get
			{
				return Singleton<UIScreenManager>.Instance.CurrentScreenId == m_screenID;
			}
		}

		public void OnValidate()
		{
			if (m_screen != null)
			{
				m_screenID = m_screen.ScreenId;
				m_screen = null;
			}
		}

		private void Awake()
		{
			if ((bool)InTween)
			{
				InTween.enabled = false;
			}
			if ((bool)OutTween)
			{
				OutTween.enabled = false;
			}
			m_button = GetComponent<UIButton>();
		}

		protected override void DoReactOnClick()
		{
			if (m_screenID != EScreenID.Invalid)
			{
				EScreenID screenID = UIScreenChangerHelper.IsScreenChangeValid(m_screenID);
				if (m_button == null || m_button.isEnabled)
				{
					Singleton<UIScreenManager>.Instance.ChangeScreen(screenID, new UITweenHandle(InTween, OutTween));
				}
			}
		}
	}
}
