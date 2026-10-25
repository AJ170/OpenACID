using UnityEngine;

public class RenderingUpscaleCopy : MonoBehaviour
{
	public RenderTexture DownscaledSourceTexture;

	public void OnRenderImage(RenderTexture _src, RenderTexture _dest)
	{
		Graphics.Blit(DownscaledSourceTexture, _dest);
	}
}
