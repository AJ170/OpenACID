using UnityEngine;

public class ParticlesFollowPoint : MonoBehaviour
{
	private ParticleSystem.Particle[] particles;

	public Transform followTransform;

	public float Strength;

	private int numParticles;

	public int maxParticles;

	private void Start()
	{
		maxParticles = GetComponent<ParticleSystem>().maxParticles;
		particles = new ParticleSystem.Particle[maxParticles];
	}

	private void Update()
	{
		numParticles = GetComponent<ParticleSystem>().GetParticles(particles);
		float num = Strength * Mathf.Min(Time.deltaTime, 0.05f);
		for (int i = 0; i < numParticles; i++)
		{
			Vector3 vector = followTransform.position - particles[i].position;
			particles[i].velocity += vector.normalized * num;
		}
		GetComponent<ParticleSystem>().SetParticles(particles, numParticles);
	}
}
