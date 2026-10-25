using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class UICameraViewportToScreenElement : MonoBehaviour
	{
		public GameObject sprite;

		private Rect m_rect;

		private Camera m_mainCamera;

		private void Start()
		{
			if (m_mainCamera == null)
			{
				m_mainCamera = Singleton<UIScreenManager>.Instance.SceneData.MainUICamera;
			}
			m_rect = new Rect(m_mainCamera.WorldToScreenPoint(sprite.transform.position).x, m_mainCamera.WorldToScreenPoint(sprite.transform.position).y, m_mainCamera.WorldToScreenPoint(new Vector3(sprite.GetComponent<UIWidget>().width, sprite.GetComponent<UIWidget>().height, 0f)).x / 768f, m_mainCamera.WorldToScreenPoint(new Vector3(sprite.GetComponent<UIWidget>().width, sprite.GetComponent<UIWidget>().height, 0f)).y / 768f);
			GetComponent<Camera>().pixelRect = m_rect;
		}

		private void Update()
		{
			m_rect = new Rect(m_mainCamera.WorldToScreenPoint(sprite.transform.position).x, m_mainCamera.WorldToScreenPoint(sprite.transform.position).y, m_mainCamera.WorldToScreenPoint(new Vector3(sprite.GetComponent<UIWidget>().width, sprite.GetComponent<UIWidget>().height, 0f)).x / 768f, m_mainCamera.WorldToScreenPoint(new Vector3(sprite.GetComponent<UIWidget>().width, sprite.GetComponent<UIWidget>().height, 0f)).y / 768f);
			GetComponent<Camera>().pixelRect = m_rect;
		}
	}
}
