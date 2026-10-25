using UnityEngine;

namespace Assets.Scripts.UI.NGUI_Extensions.Model
{
	public class TextureModel
	{
		public static TextureModel s_textureModel = new TextureModel();

		public Color[] Colors;

		public Texture MainTexture;

		public Texture MaskingTexture;

		public Texture ForegroundTexture;

		public Texture ThemeTexture;

		public bool HideBackground;

		public float Desaturation;

		public Vector2 OrgSize;

		public Vector2 OffsetPos;

		public Vector4? UvCoord;

		public virtual void Clear()
		{
			Colors = null;
			MainTexture = null;
			MaskingTexture = null;
			ForegroundTexture = null;
			ThemeTexture = null;
			HideBackground = false;
			Desaturation = 0f;
			OrgSize = Vector2.zero;
			OffsetPos = Vector2.zero;
			UvCoord = null;
		}
	}
}
