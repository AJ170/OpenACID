namespace Assets.Scripts.UI.Types
{
	public class UIModifierId : UIStringType
	{
		public UIModifierId(string _id)
			: base(_id)
		{
		}

		public static implicit operator UIModifierId(string _value)
		{
			return new UIModifierId(_value);
		}
	}
}
