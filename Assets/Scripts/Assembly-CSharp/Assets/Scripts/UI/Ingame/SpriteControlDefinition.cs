using UnityEngine;

namespace Assets.Scripts.UI.Ingame
{
	public class SpriteControlDefinition : ISpriteControlDefinition
	{
		public GameObject SpriteInstance { get; set; }

		public ISpriteControlBehaviour SpriteBehaviour { get; set; }

		public SpriteRenderer[] SpriteRenderers { get; set; }
	}
}
