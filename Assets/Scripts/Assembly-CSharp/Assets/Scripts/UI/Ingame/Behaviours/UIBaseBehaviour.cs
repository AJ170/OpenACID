using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIBaseBehaviour : MonoBehaviour
	{
		[SerializeField]
		private bool m_rightHandedAlignment;

		[SerializeField]
		private bool m_registerWidgetMgr = true;

		private bool m_lastRightHandedAlignment;

		protected bool m_visible = true;

		public virtual UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Invalid;
			}
		}

		public bool IsVisible
		{
			get
			{
				return m_visible;
			}
		}

		public bool IsRightHandedAlignment
		{
			get
			{
				return m_rightHandedAlignment;
			}
		}

		public bool IsLastRightHandedAlignment
		{
			get
			{
				return m_lastRightHandedAlignment;
			}
		}

		public bool ShouldRegisterWidgetWithMgr
		{
			get
			{
				return m_registerWidgetMgr;
			}
		}

		public virtual bool FullyVisible
		{
			get
			{
				UITweenHelper component = base.gameObject.GetComponent<UITweenHelper>();
				if (component != null)
				{
					return base.gameObject.activeInHierarchy && component.Tweener.value == component.Tweener.to;
				}
				return base.gameObject.activeInHierarchy;
			}
		}

		public virtual void OnUIAlignmentChanged(bool _isRightHanded)
		{
			m_rightHandedAlignment = _isRightHanded;
			m_lastRightHandedAlignment = IsRightHandedAlignment;
		}

		public virtual void SetVisibility(bool _visible)
		{
			if (m_visible != _visible)
			{
				UITweenHelper uITweenHelper = base.gameObject.AddComponentIfNotExist<UITweenHelper>();
				if (uITweenHelper != null)
				{
					uITweenHelper.SetVisibility(_visible);
				}
				m_visible = _visible;
			}
		}

		private void OnDestroy()
		{
			if (m_registerWidgetMgr && UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance != null)
			{
				UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.UnRegisterWidget(ScreenWidgetType);
			}
		}

		private void OnValidate()
		{
			OnUIAlignmentChanged(IsRightHandedAlignment);
		}
	}
}
