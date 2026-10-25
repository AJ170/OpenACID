using SharedBaseLib.Serialization;

namespace Assets.Scripts.UI.Screens.CreditsScreen
{
	public class CreditsDefinition : DataSerializableBase
	{
		public ECreditsItemType Type { get; set; }

		public float MinTotalAdvance { get; set; }

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			Type = (ECreditsItemType)_s.SerializeEnum("Type", Type.GetType(), (int)Type);
			MinTotalAdvance = _s.SerializeFloat("TotalAdvance", MinTotalAdvance);
		}
	}
}
