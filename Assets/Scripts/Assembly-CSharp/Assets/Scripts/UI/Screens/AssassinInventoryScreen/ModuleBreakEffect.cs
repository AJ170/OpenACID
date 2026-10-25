using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class ModuleBreakEffect : MonoBehaviour
	{
		private MeshRenderer[] m_renderers;

		[SerializeField]
		private UnityEngine.Animation m_animation;

		private Material m_materialCopy;

		public void Play(UIPortraitTexture _texture)
		{
			m_renderers = GetComponentsInChildren<MeshRenderer>(true);
			base.gameObject.SetActive(true);
			m_materialCopy = new Material(_texture.material);
			MeshRenderer[] renderers = m_renderers;
			foreach (MeshRenderer meshRenderer in renderers)
			{
				meshRenderer.sharedMaterial = m_materialCopy;
				meshRenderer.sortingLayerName = "Foreground";
				meshRenderer.sortingOrder = 2;
			}
			m_animation.Play();
			StartCoroutine(DeactivateOnDone());
		}

		private IEnumerator DeactivateOnDone()
		{
			WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
			while (m_animation.isPlaying)
			{
				yield return waitForEndOfFrame;
			}
			Cleanup();
		}

		private void Cleanup()
		{
			base.gameObject.SetActive(false);
			if (m_materialCopy != null)
			{
				Object.Destroy(m_materialCopy);
				m_materialCopy = null;
			}
		}

		private void OnDisable()
		{
			StopAllCoroutines();
			Cleanup();
		}
	}
}
