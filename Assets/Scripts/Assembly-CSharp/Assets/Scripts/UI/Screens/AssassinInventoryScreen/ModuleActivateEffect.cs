using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class ModuleActivateEffect : MonoBehaviour
	{
		private ParticleSystem[] m_systems;

		public AnimateAchievementHighlight quad;

		public GameObject fan1;

		public GameObject fan2;

		public GameObject fan3;

		public GameObject fan4;

		public void Play()
		{
			m_systems = GetComponentsInChildren<ParticleSystem>(true);
			base.gameObject.SetActive(true);
			quad.gameObject.SetActive(true);
			StartCoroutine(quad.AnimateHighlight(new Color(0.8f, 0.8f, 0.8f, 1f)));
			fan1.SetActive(true);
			fan2.SetActive(true);
			fan3.SetActive(true);
			fan4.SetActive(true);
			for (int i = 0; i < m_systems.Length; i++)
			{
				Renderer component = m_systems[i].GetComponent<Renderer>();
				component.sortingLayerName = "Foreground";
				component.sortingOrder = 2;
			}
			StartCoroutine(DeactivateOnDone());
		}

		private IEnumerator DeactivateOnDone()
		{
			WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
			bool running = true;
			while (running)
			{
				running = false;
				for (int index = 0; index < m_systems.Length; index++)
				{
					if (m_systems[index].isPlaying)
					{
						running = true;
					}
				}
				yield return waitForEndOfFrame;
			}
			Cleanup();
		}

		private void Cleanup()
		{
			for (int i = 0; i < m_systems.Length; i++)
			{
				if (m_systems[i].isPlaying)
				{
					m_systems[i].Stop(true);
					m_systems[i].Clear(true);
				}
			}
			base.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			Cleanup();
		}
	}
}
