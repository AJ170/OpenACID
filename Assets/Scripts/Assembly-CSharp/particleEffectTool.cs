using UnityEngine;

public class particleEffectTool : MonoBehaviour
{
	public ParticleSystem particleSys;

	public int numParticlesX;

	public int numParticlesY;

	public float minLifeTime;

	private int m_maxParticles;

	public bool isAlwaysOnTop;

	private void Start()
	{
		if (isAlwaysOnTop)
		{
			particleSys.GetComponent<Renderer>().sortingLayerName = "Foreground";
			particleSys.GetComponent<Renderer>().sortingOrder = 2;
		}
	}
}
