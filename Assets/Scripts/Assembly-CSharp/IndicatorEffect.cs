using UnityEngine;

public class IndicatorEffect : MonoBehaviour
{
	public ParticleSystem MainEmitter;

	public ParticleSystem SubEmitter;

	public void Play()
	{
		MainEmitter.emissionRate = 4f;
		SubEmitter.emissionRate = 4f;
		MainEmitter.Play();
		SubEmitter.Play();
	}

	public void Pause()
	{
		MainEmitter.emissionRate = 0f;
		SubEmitter.emissionRate = 0f;
	}
}
