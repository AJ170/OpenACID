using UnityEngine;

public class TimeScaleIndependentParticles : MonoBehaviour
{
	private float lastTime;

	private ParticleSystem particle;

	private void Awake()
	{
		particle = GetComponent<ParticleSystem>();
	}

	private void Start()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	private void OnEnable()
	{
		lastTime = Time.realtimeSinceStartup;
	}

	private void Update()
	{
		float num = Time.realtimeSinceStartup - lastTime;
		lastTime = Time.realtimeSinceStartup;
		if (num != 0f)
		{
			particle.Simulate(num, true, false);
			if (particle.time >= particle.duration)
			{
				particle.Stop();
			}
		}
	}
}
