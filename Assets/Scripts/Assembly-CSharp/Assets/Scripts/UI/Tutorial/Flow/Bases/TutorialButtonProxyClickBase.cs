using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialButtonProxyClickBase : TutorialClickBase
	{
		protected UIButtonProxy m_buttonProxy;

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			foreach (UIWidget uIWidget in _widgets)
			{
				m_buttonProxy = uIWidget as UIButtonProxy;
				if (m_buttonProxy != null)
				{
					break;
				}
			}
			m_buttonProxy.gameObject.SetActive(true);
			EventDelegate.Add(m_buttonProxy.onClick, base.OnClick);
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			if (m_buttonProxy != null)
			{
				EventDelegate.Remove(m_buttonProxy.onClick, base.OnClick);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_buttonProxy != null)
			{
				EventDelegate.Remove(m_buttonProxy.onClick, base.OnClick);
			}
			m_buttonProxy = null;
		}
	}
}
