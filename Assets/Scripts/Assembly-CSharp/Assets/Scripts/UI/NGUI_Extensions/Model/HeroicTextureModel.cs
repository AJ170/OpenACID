using UnityEngine;

namespace Assets.Scripts.UI.NGUI_Extensions.Model
{
	public class HeroicTextureModel : TextureModel
	{
		public static HeroicTextureModel s_heroicTextureModel = new HeroicTextureModel();

		public Texture HeroicFrameTexture;

		public Texture HeroicFrameStencilTexture;

		public override void Clear()
		{
			base.Clear();
			HeroicFrameTexture = null;
			HeroicFrameStencilTexture = null;
		}
	}
}
