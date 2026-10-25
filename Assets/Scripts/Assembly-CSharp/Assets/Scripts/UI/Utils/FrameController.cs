using System;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class FrameController : MonoBehaviour
	{
		[Flags]
		public enum EDisplayedSide
		{
			None = 1,
			Left = 2,
			Right = 4,
			Both = 6
		}

		[SerializeField]
		private UISprite m_left;

		[SerializeField]
		private UISprite m_right;

		[SerializeField]
		private EDisplayedSide m_sides = EDisplayedSide.None;

		private static bool CheckFlag(EDisplayedSide _sides, EDisplayedSide _checked)
		{
			return (_sides & _checked) == _checked;
		}

		private void Start()
		{
			if ((bool)m_left)
			{
				m_left.enabled = CheckFlag(m_sides, EDisplayedSide.Left);
			}
			if ((bool)m_right)
			{
				m_right.enabled = CheckFlag(m_sides, EDisplayedSide.Right);
			}
		}
	}
}
