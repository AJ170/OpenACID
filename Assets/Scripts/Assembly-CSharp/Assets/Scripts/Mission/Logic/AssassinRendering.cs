using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public static class AssassinRendering
	{
		private static bool ms_mainCharacterVisible = true;

		private static int m_outlineDeactivateCount;

		private static bool m_outlineWarningRaised;

		public static void ResetStaticPrivates()
		{
			ms_mainCharacterVisible = true;
			m_outlineDeactivateCount = 0;
			m_outlineWarningRaised = false;
		}

		public static bool HasOutline()
		{
			return m_outlineDeactivateCount == 0;
		}

		public static bool IsMainCharacterVisible()
		{
			return ms_mainCharacterVisible;
		}

		public static void DisableOutline()
		{
			if (++m_outlineDeactivateCount == 1)
			{
				ToggleOutline(false);
			}
			if (!m_outlineWarningRaised && m_outlineDeactivateCount > 64)
			{
				m_outlineWarningRaised = true;
				Debug.LogError("More than 64 subsystems tried to disable the outlines. There are more calls to DisableOutline() than to EnableOutline()!");
			}
		}

		public static void EnableOutline()
		{
			if (--m_outlineDeactivateCount == 0)
			{
				ToggleOutline(true);
			}
			if (!m_outlineWarningRaised && m_outlineDeactivateCount < 0)
			{
				m_outlineWarningRaised = true;
				Debug.LogError("Too many subsystems tried to reenable the outlines. There are more calls to EnableOutline() than to DisableOutline()!");
			}
		}

		private static void ToggleOutline(bool _enable)
		{
			if (ms_mainCharacterVisible)
			{
				GameObject gameObject = PlayerHelper.Entity.UnityComponentCache.AnimationController.Agent.gameObject.FindChildByName("Outline Renderer");
				if (gameObject != null)
				{
					gameObject.SetActive(_enable);
				}
			}
		}

		public static void ToggleVisible(bool _visible)
		{
			if (ms_mainCharacterVisible != _visible)
			{
				Renderer[] componentsInChildren = PlayerHelper.Entity.GameObject.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer in componentsInChildren)
				{
					renderer.enabled = _visible;
				}
				ms_mainCharacterVisible = _visible;
				if (ms_mainCharacterVisible && m_outlineDeactivateCount > 0)
				{
					ToggleOutline(false);
				}
			}
		}
	}
}
