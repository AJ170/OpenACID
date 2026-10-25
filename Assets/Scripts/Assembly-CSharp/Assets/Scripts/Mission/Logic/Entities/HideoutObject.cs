namespace Assets.Scripts.Mission.Logic.Entities
{
	public abstract class HideoutObject : InteractableObject
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.HideoutObject;
			}
		}
	}
}
