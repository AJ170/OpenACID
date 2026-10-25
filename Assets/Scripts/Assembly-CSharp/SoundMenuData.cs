using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

public class SoundMenuData : UIScreenChild
{
	private GameObject m_root;

	protected override void OnScreenInit()
	{
		m_root = NGUITools.GetRoot(base.gameObject);
		EventDelegate.Add(m_screen.onActivate, SelfUpdate);
	}

	protected override void OnScreenDestroy()
	{
		m_root = null;
		if (m_screen != null)
		{
			EventDelegate.Remove(m_screen.onActivate, SelfUpdate);
		}
	}

	private void SelfUpdate()
	{
		if (Main.Instance.SceneLoader.InAnimus)
		{
			m_root.transform.localPosition = Vector3.zero;
		}
		else
		{
			m_root.transform.localPosition = new Vector3(-510f, 500f, 0f);
		}
	}
}
