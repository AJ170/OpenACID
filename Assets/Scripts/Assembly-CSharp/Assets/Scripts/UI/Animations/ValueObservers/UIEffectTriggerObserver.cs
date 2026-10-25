using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public class UIEffectTriggerObserver : IAnimationObserver
	{
		[SerializeField]
		[CheckAssigned]
		private ParticleSystem m_particleSystem;

		[SerializeField]
		[CheckAssigned]
		private bool m_includeChildren;

		[SerializeField]
		[CheckAssigned(NullAllowed = true)]
		private GameObject m_positionMarker;

		private void Awake()
		{
			m_particleSystem.Stop(m_includeChildren);
		}

		private void OnEnable()
		{
			if (m_positionMarker != null)
			{
				m_particleSystem.transform.position = m_positionMarker.transform.position;
			}
			m_particleSystem.gameObject.SetActive(false);
			m_particleSystem.gameObject.SetActive(true);
			m_particleSystem.Play(m_includeChildren);
		}

		private void OnDisable()
		{
			base.enabled = false;
			m_particleSystem.gameObject.SetActive(false);
		}
	}
}
