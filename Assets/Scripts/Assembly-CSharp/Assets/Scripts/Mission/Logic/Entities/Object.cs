namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class Object : GameEntity
	{
		public override bool HighlightedSelection
		{
			get
			{
				return false;
			}
		}
	}
}
