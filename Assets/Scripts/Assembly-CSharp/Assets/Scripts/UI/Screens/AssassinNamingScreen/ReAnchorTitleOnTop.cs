using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinNamingScreen
{
	[RequireComponent(typeof(UIWidget))]
	public class ReAnchorTitleOnTop : MonoBehaviour
	{
		private UIWidget m_widget;

		private UIRect.AnchorPoint m_topAnchor;

		private UIRect.AnchorPoint m_bottomAnchor;

		private void OnEnable()
		{
			if (m_widget == null)
			{
				m_widget = GetComponent<UIWidget>();
			}
			if (Singleton<StateManager>.Instance.ProfileManager.AssassinCount == 0)
			{
				m_topAnchor = new UIRect.AnchorPoint();
				m_topAnchor.Set(m_widget.topAnchor.relative, m_widget.topAnchor.absolute);
				m_bottomAnchor = new UIRect.AnchorPoint();
				m_bottomAnchor.Set(m_widget.bottomAnchor.relative, m_widget.bottomAnchor.absolute);
				m_widget.topAnchor.Set(1f, 0f);
				m_widget.bottomAnchor.Set(1f, -100f);
				m_widget.ResetAnchors();
				m_widget.UpdateAnchors();
				m_widget.ForceUpdate(true);
			}
		}

		private void OnDisable()
		{
			if (m_topAnchor != null && m_bottomAnchor != null && m_widget != null)
			{
				m_widget.topAnchor.Set(m_topAnchor.relative, m_topAnchor.absolute);
				m_widget.bottomAnchor.Set(m_bottomAnchor.relative, m_bottomAnchor.absolute);
				m_topAnchor = null;
				m_bottomAnchor = null;
			}
		}
	}
}
