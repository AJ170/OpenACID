namespace Assets.Scripts.UI.Types
{
	public class UIMissionToken : UIStringType
	{
		public UIMissionToken(string _token)
			: base(_token)
		{
		}

		public static implicit operator UIMissionToken(string _value)
		{
			return new UIMissionToken(_value);
		}
	}
}
