using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions
{
	[AddComponentMenu("NGUI/Interaction/Resize Box")]
	public class UIResizeBox : MonoBehaviour
	{
		public enum EUpdateType
		{
			All = 0,
			WidthOnly = 1,
			HeightOnly = 2
		}

		[SerializeField]
		private EUpdateType m_updateType;

		[SerializeField]
		private Vector2 m_padding;

		[SerializeField]
		private Transform m_target;

		[SerializeField]
		private UIWidget[] m_updateList;

		private bool m_resize;

		public List<EventDelegate> OnResized = new List<EventDelegate>();

		public bool ResizeNow
		{
			get
			{
				return m_resize;
			}
			set
			{
				if (value)
				{
					m_resize = true;
					base.enabled = true;
				}
			}
		}

		[ContextMenu("Execute")]
		public void Resize()
		{
			if (Application.isPlaying && !NGUITools.GetActive(this))
			{
				m_resize = true;
				return;
			}
			m_resize = false;
			RepositionChildren();
			EventDelegate.Execute(OnResized);
		}

		private void Start()
		{
			m_resize = true;
		}

		private void RepositionChildren()
		{
			Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(m_target);
			for (int i = 0; i < m_updateList.Length; i++)
			{
				UpdateTagetsSize(bounds.size, m_updateList[i]);
			}
		}

		private void UpdateTagetsSize(Vector2 _size, UIWidget _target)
		{
			if (m_updateType == EUpdateType.All || m_updateType == EUpdateType.HeightOnly)
			{
				_target.height = (int)(_size.y + m_padding.y);
			}
			if (m_updateType == EUpdateType.All || m_updateType == EUpdateType.WidthOnly)
			{
				_target.width = (int)(_size.x + m_padding.x);
			}
		}

		private void LateUpdate()
		{
			if (m_resize)
			{
				Resize();
			}
			base.enabled = false;
		}
	}
}
