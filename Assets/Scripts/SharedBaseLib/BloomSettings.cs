using UnityEngine;

public class BloomSettings : PostprocessingSettings, BloomSettingsInterface
{
	public enum Resolution
	{
		Low = 0,
		Medium = 1,
		High = 2
	}

	public enum BlurType
	{
		Standard = 0,
		Sgx = 1
	}

	public enum DownsampleType
	{
		_4Tap = 0,
		None = 1
	}

	public enum Order
	{
		First = 0,
		Last = 1
	}

	[SerializeField]
	public Vector4 threshold = new Vector4(0.25f, 0.25f, 0.25f, 0.25f);

	[SerializeField]
	public Vector4 intensity = new Vector4(0.75f, 0.75f, 0.75f, 0.75f);

	[SerializeField]
	public Vector4 exponent = new Vector4(1f, 1f, 1f, 1f);

	[SerializeField]
	[Range(0.25f, 5.5f)]
	public float blurSize = 1f;

	[SerializeField]
	public Resolution resolution = Resolution.Medium;

	[SerializeField]
	[Range(1f, 4f)]
	public int blurIterations = 1;

	[SerializeField]
	public DownsampleType downsampleType = DownsampleType.None;

	[SerializeField]
	public BlurType blurType = BlurType.Sgx;

	[SerializeField]
	public Order order;

	public Vector4 Threshold
	{
		get
		{
			return threshold;
		}
		set
		{
			threshold = value;
		}
	}

	public Vector4 Intensity
	{
		get
		{
			return intensity;
		}
		set
		{
			intensity = value;
		}
	}

	public Vector4 Exponent
	{
		get
		{
			return exponent;
		}
		set
		{
			exponent = value;
		}
	}

	public float Size
	{
		get
		{
			return blurSize;
		}
		set
		{
			blurSize = value;
		}
	}

	public Resolution BloomResolution
	{
		get
		{
			return resolution;
		}
		set
		{
			resolution = value;
		}
	}

	public int Iterations
	{
		get
		{
			return blurIterations;
		}
		set
		{
			blurIterations = value;
		}
	}

	public DownsampleType Downsample
	{
		get
		{
			return downsampleType;
		}
		set
		{
			downsampleType = value;
		}
	}

	public BlurType Blur
	{
		get
		{
			return blurType;
		}
		set
		{
			blurType = value;
		}
	}

	public Order Ordering
	{
		get
		{
			return order;
		}
		set
		{
			order = value;
		}
	}
}
