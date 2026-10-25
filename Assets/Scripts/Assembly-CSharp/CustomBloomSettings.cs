using UnityEngine;

public class CustomBloomSettings : BloomSettingsInterface
{
	public Vector4 Threshold { get; set; }

	public Vector4 Intensity { get; set; }

	public Vector4 Exponent { get; set; }

	public float Size { get; set; }

	public BloomSettings.Resolution BloomResolution { get; set; }

	public int Iterations { get; set; }

	public BloomSettings.DownsampleType Downsample { get; set; }

	public BloomSettings.BlurType Blur { get; set; }

	public BloomSettings.Order Ordering { get; set; }
}
