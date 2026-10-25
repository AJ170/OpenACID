using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIBackgroundDimmingBehaviour : MonoBehaviour
	{
		private const string k_animationOn = "BackgroundDimmingOn";

		private const string k_animationOff = "BackgroundDimmingOff";

		[SerializeField]
		private UnityEngine.Animation m_animation;

		[SerializeField]
		private float AnimatedAlphaValue;

		private MeshRenderer m_renderer;

		public void SetDimming(bool _active)
		{
			if (m_renderer != null)
			{
				m_renderer.enabled = true;
				m_renderer.gameObject.SetActive(true);
			}
			string animation = ((!_active) ? "BackgroundDimmingOff" : "BackgroundDimmingOn");
			m_animation.Play(animation);
		}

		private void Update()
		{
			if (m_renderer == null)
			{
				m_renderer = BehaviourSingleton<EnvironmentBridgeMain>.Instance.DimmingBackground.GetComponent<MeshRenderer>();
				AnimatedAlphaValue = 1f;
			}
			if (m_renderer != null)
			{
				m_renderer.gameObject.SetActive(AnimatedAlphaValue < 0.95f);
			}
			if (m_animation.isPlaying && m_renderer != null)
			{
				m_renderer.material.SetFloat("_Darken", AnimatedAlphaValue);
			}
		}
	}
}
