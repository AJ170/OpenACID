using UnityEngine;

public class ColorCorrectionSettings : PostprocessingSettings
{
	public enum LutSize
	{
		_2 = 2,
		_4 = 4,
		_8 = 8,
		_16 = 0x10,
		_32 = 0x20
	}

	[SerializeField]
	public AnimationCurve red = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	public AnimationCurve green = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	public AnimationCurve blue = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	public AnimationCurve global = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	[SerializeField]
	public LutSize lutSize = LutSize._16;
}
