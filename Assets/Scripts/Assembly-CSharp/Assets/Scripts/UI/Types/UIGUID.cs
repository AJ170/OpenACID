namespace Assets.Scripts.UI.Types
{
	public class UIGUID : UIStringType
	{
		private const int GUID_LENGTH = 36;

		public override bool Valid
		{
			get
			{
				return base.Valid && base.Value.Length == 36;
			}
		}

		public UIGUID(string _id)
			: base(_id)
		{
		}

		public static implicit operator UIGUID(string _value)
		{
			return new UIGUID(_value);
		}
	}
}
