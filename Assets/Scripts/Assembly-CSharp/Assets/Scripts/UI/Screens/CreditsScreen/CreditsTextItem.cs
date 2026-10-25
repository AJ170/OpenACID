using SharedBaseLib.Serialization;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public class CreditsTextItem : CreditsItem
	{
		public string Text { get; set; }

		public CreditsTextItem(ECreditsItemType _type)
			: base(_type)
		{
		}

		protected override void SerializeInternal(DataSerializerBase _s)
		{
			base.SerializeInternal(_s);
			Text = _s.SerializeString("text", Text);
		}
	}
}
