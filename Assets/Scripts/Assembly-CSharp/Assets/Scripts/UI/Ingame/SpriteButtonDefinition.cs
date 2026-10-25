namespace Assets.Scripts.UI.Ingame
{
	public class SpriteButtonDefinition : SpriteControlDefinition, ISpriteButtonDefinition, ISpriteControlDefinition
	{
		public ISpriteButtonBehaviour ButtonSprite
		{
			get
			{
				return SpriteBehaviour as ISpriteButtonBehaviour;
			}
			set
			{
				SpriteBehaviour = value;
			}
		}
	}
}
