using UnityEngine;

public class ThunderstormSettings : PostprocessingSettings
{
	[SerializeField]
	public Color thunderColor;

	[SerializeField]
	public float thunderAdd = 0.75f;

	[SerializeField]
	public float thunderExp = 1.5f;
}
