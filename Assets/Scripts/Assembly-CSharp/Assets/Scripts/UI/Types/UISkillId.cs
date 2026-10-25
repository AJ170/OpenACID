namespace Assets.Scripts.UI.Types
{
	public class UISkillId : UIStringType
	{
		public UISkillId(string _id)
			: base(_id)
		{
		}

		public static implicit operator UISkillId(string _value)
		{
			return new UISkillId(_value);
		}
	}
}
