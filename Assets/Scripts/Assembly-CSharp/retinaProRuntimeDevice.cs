using System.Collections.Generic;

public class retinaProRuntimeDevice
{
	public string name;

	public float pixelSize;

	public bool rootAuto;

	public int rootHeight;

	public int rootWidth;

	public bool rootUseBothPortLand;

	public List<retinaProRuntimeScreen> screens;

	public retinaProRuntimeDevice()
	{
		screens = new List<retinaProRuntimeScreen>();
	}
}
