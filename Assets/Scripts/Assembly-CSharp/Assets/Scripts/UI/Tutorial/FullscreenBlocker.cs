using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.System.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial
{
	[RequireComponent(typeof(UIWidget))]
	public class FullscreenBlocker : MonoBehaviour
	{
		[SerializeField]
		private List<UIWidget> m_widgets = new List<UIWidget>();

		[SerializeField]
		private int m_depthIncrement = 100;

		private bool m_depthIncremented;

		private int m_defaultDepthIncrement;

		public int DepthIncrement
		{
			get
			{
				return m_depthIncrement;
			}
			set
			{
				if (!m_depthIncremented)
				{
					m_depthIncrement = value;
				}
			}
		}

		public int FinalDepthIncrement
		{
			get
			{
				return BaseDepthIncrement + DepthIncrement;
			}
		}

		private int BaseDepthIncrement
		{
			get
			{
				if (!m_depthIncremented)
				{
					UIWidget component = GetComponent<UIWidget>();
					if (component != null)
					{
						m_defaultDepthIncrement = component.depth;
					}
				}
				return m_defaultDepthIncrement;
			}
		}

		public void SetNonBlocked(UIWidget[] _widgets)
		{
			m_widgets.Clear();
			AddNonBlocked(_widgets);
		}

		public void AddNonBlocked(List<UIWidget> _widgets)
		{
			AddNonBlocked(_widgets.ToArray());
		}

		public void AddNonBlocked(UIWidget[] _widgets)
		{
			for (int i = 0; i < _widgets.Length; i++)
			{
				UIWidget uIWidget = _widgets[i];
				UIPrefabProxy uIPrefabProxy = uIWidget as UIPrefabProxy;
				if (uIPrefabProxy != null && !uIPrefabProxy.IsInitalized)
				{
					uIPrefabProxy.Init();
					List<UIWidget> list = uIPrefabProxy.GetComponentsInChildren<UIWidget>(true).ToList();
					list.Remove(uIPrefabProxy);
					AddNonBlocked(list);
				}
				m_widgets.Add(_widgets[i]);
			}
		}

		public void IncrementDepth()
		{
			if (!m_depthIncremented)
			{
				int finalDepthIncrement = FinalDepthIncrement;
				for (int i = 0; i < m_widgets.Count; i++)
				{
					m_widgets[i].depth += finalDepthIncrement;
				}
				m_depthIncremented = true;
			}
		}

		public void DecrementDepth()
		{
			if (m_depthIncremented)
			{
				int num = BaseDepthIncrement + DepthIncrement;
				for (int i = 0; i < m_widgets.Count; i++)
				{
					m_widgets[i].depth -= num;
				}
				m_depthIncremented = false;
			}
		}

		private void Awake()
		{
			if (m_widgets == null)
			{
				m_widgets = new List<UIWidget>();
			}
		}

		public void Clear()
		{
			m_widgets.Clear();
		}
	}
}
