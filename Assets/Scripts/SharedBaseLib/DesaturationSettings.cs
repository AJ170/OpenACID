using UnityEngine;

public class DesaturationSettings : PostprocessingSettings
{
	public Vector3 desaturationWeights = new Vector3(0.222f, 0.707f, 0.071f);

	[SerializeField]
	[Range(0f, 1f)]
	public float desaturation = 0.3f;
}
