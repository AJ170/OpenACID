namespace Assets.Scripts.UI.Types
{
	public class UIAssasinId : UIStringType
	{
		public static readonly UIAssasinId Invalid = new UIAssasinId(string.Empty);

		public UIAssasinId(string _id)
			: base(_id)
		{
		}

		public static implicit operator UIAssasinId(string _value)
		{
			return new UIAssasinId(_value);
		}
	}
}
