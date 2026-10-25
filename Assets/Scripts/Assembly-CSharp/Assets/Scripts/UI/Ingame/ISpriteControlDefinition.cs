using UnityEngine;

namespace Assets.Scripts.UI.Ingame
{
	public interface ISpriteControlDefinition
	{
		GameObject SpriteInstance { get; set; }

		ISpriteControlBehaviour SpriteBehaviour { get; set; }

		SpriteRenderer[] SpriteRenderers { get; set; }
	}
}
