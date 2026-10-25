using UnityEngine;

public class BarrierSetup : MonoBehaviour
{
	[SerializeField]
	public Vector2 m_beamDensity = new Vector2(1.1f, 0.9f);

	[SerializeField]
	public Vector2 m_padding = new Vector2(4f, 4f);

	[SerializeField]
	public Vector2 m_trapez = new Vector2(4f, 4f);

	[SerializeField]
	public Vector3 m_dimensions = new Vector3(0f, 0f, 1f);

	[SerializeField]
	public float m_depthExponent = 2f;
}
