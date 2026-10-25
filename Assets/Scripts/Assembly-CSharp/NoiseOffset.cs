using UnityEngine;

public class NoiseOffset : MonoBehaviour
{
	[SerializeField]
	private Vector2 SpeedA;

	[SerializeField]
	private Vector2 SpeedB;

	private Vector2 ProgressA;

	private Vector2 ProgressB;

	private static int PropertyID;

	private Material ShaderMat;

	private Vector4 Offsets;

	private void Start()
	{
		PropertyID = Shader.PropertyToID("_Offsets");
		ShaderMat = base.gameObject.GetComponent<Renderer>().material;
		Offsets = default(Vector4);
	}

	private void Update()
	{
		Offsets.x += SpeedA.x * Time.deltaTime;
		Offsets.z += SpeedB.x * Time.deltaTime;
		Offsets.y += SpeedA.y * Time.deltaTime;
		Offsets.w += SpeedB.y * Time.deltaTime;
		ShaderMat.SetVector(PropertyID, Offsets);
	}
}
