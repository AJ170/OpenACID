using UnityEngine;

public class ParticleFront : MonoBehaviour
{
	private void Start()
	{
		ParticleSystem component = GetComponent<ParticleSystem>();
		Renderer component2 = component.GetComponent<Renderer>();
		component2.sortingLayerName = "Foreground";
		component2.sortingOrder = 2;
	}
}
public class particleFront : MonoBehaviour
{
	public int SortingOrder = 2;

	private void Start()
	{
		ParticleSystem component = GetComponent<ParticleSystem>();
		Renderer component2 = component.GetComponent<Renderer>();
		component2.sortingLayerName = "Foreground";
		component2.sortingOrder = SortingOrder;
	}
}
