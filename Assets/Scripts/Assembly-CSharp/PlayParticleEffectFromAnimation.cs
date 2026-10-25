using UnityEngine;

public class PlayParticleEffectFromAnimation : MonoBehaviour
{
	public ParticleSystem m_particleSystem;

	public void PlayParticleEffect()
	{
		if (m_particleSystem != null)
		{
			m_particleSystem.Play();
		}
	}
}
