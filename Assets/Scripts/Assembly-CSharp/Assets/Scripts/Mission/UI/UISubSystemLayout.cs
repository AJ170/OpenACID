using System;
using Assets.Scripts.Mission.HUD;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	[Serializable]
	public class UISubSystemLayout : IUISubSystemLayout
	{
		[SerializeField]
		private Vector2 m_borderOffset;

		[SerializeField]
		private ScreenAnchor m_screenAnchor;

		[SerializeField]
		private EUISubSystem m_subSystem;

		public Vector2 BorderOffset
		{
			get
			{
				return m_borderOffset;
			}
			set
			{
				m_borderOffset = value;
			}
		}

		public ScreenAnchor ScreenAnchor
		{
			get
			{
				return m_screenAnchor;
			}
			set
			{
				m_screenAnchor = value;
			}
		}

		public virtual EUISubSystem SubSystem
		{
			get
			{
				return m_subSystem;
			}
		}
	}
}
