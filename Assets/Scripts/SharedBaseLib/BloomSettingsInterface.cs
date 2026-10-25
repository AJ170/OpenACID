using UnityEngine;

public interface BloomSettingsInterface
{
	Vector4 Threshold { get; set; }

	Vector4 Intensity { get; set; }

	Vector4 Exponent { get; set; }

	float Size { get; set; }

	BloomSettings.Resolution BloomResolution { get; set; }

	int Iterations { get; set; }

	BloomSettings.DownsampleType Downsample { get; set; }

	BloomSettings.BlurType Blur { get; set; }

	BloomSettings.Order Ordering { get; set; }
}
