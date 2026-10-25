using UnityEngine;

[ExecuteInEditMode]
public class GlobalTextures : MonoBehaviour
{
	public Texture HighlightTexture;

	public Texture NoiseTexture;

	public Texture EagleVisionTexture;

	public Texture RasterTexture;

	private void Update()
	{
		if (HighlightTexture != null)
		{
			Shader.SetGlobalTexture("_GridTex", HighlightTexture);
		}
		if (NoiseTexture != null)
		{
			Shader.SetGlobalTexture("_NoiseTexture", NoiseTexture);
		}
		if (EagleVisionTexture != null)
		{
			Shader.SetGlobalTexture("_EagleVisionTexture", EagleVisionTexture);
		}
		if (RasterTexture != null)
		{
			Shader.SetGlobalTexture("_RasterTex", RasterTexture);
		}
	}
}
