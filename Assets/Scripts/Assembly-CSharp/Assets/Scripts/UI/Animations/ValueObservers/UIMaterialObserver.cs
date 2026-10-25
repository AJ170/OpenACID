using UnityEngine;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public sealed class UIMaterialObserver : IAnimationObserver
	{
		[Range(0f, 1f)]
		[SerializeField]
		private float m_r;

		[Range(0f, 1f)]
		[SerializeField]
		private float m_b;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_g;

		[SerializeField]
		[Range(0f, 1f)]
		private float m_a;

		public MeshRenderer Renderer { private get; set; }

		private void OnEnable()
		{
			if (Renderer != null)
			{
				Renderer.enabled = true;
			}
		}

		private void OnDisable()
		{
			if (Renderer != null)
			{
				Renderer.enabled = false;
			}
		}

		private void Update()
		{
			if (Renderer != null)
			{
				Renderer.material.color = new Color(m_r, m_g, m_b, m_a);
			}
		}
	}
}
