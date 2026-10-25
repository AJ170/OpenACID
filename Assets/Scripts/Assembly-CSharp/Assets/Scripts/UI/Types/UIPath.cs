namespace Assets.Scripts.UI.Types
{
	public class UIPath : UIStringType
	{
		public override bool Valid
		{
			get
			{
				return base.Valid;
			}
		}

		public UIPath(string _id)
			: base(_id)
		{
		}

		public static implicit operator UIPath(string _value)
		{
			return new UIPath(_value);
		}
	}
}
