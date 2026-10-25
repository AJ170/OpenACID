using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIWidget))]
	public class UIButtonDepth : MonoBehaviour
	{
		private sealed class UIWidgetHandle
		{
			public UIWidget m_widget;

			public int m_baseDepth;
		}

		private UIWidget m_root;

		private int m_depth = -1;

		private readonly List<UIWidgetHandle> m_widgets = new List<UIWidgetHandle>(32);

		private void Awake()
		{
			m_root = GetComponent<UIWidget>();
		}

		private void Start()
		{
			UIWidget[] componentsInChildren = base.gameObject.GetComponentsInChildren<UIWidget>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i] != m_root)
				{
					m_widgets.Add(new UIWidgetHandle
					{
						m_widget = componentsInChildren[i],
						m_baseDepth = componentsInChildren[i].depth
					});
				}
			}
			UpdateDepth();
		}

		private void OnEnable()
		{
			UpdateDepth();
		}

		private void LateUpdate()
		{
			if (m_root.depth != m_depth)
			{
				UpdateDepth();
			}
		}

		private void UpdateDepth()
		{
			m_depth = m_root.depth;
			for (int i = 0; i < m_widgets.Count; i++)
			{
				m_widgets[i].m_widget.depth = m_depth + m_widgets[i].m_baseDepth;
			}
		}
	}
}
