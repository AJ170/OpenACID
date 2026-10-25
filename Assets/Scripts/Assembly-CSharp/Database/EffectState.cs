using SharedBaseLib.Serialization;

namespace Database
{
	public class EffectState : DataSerializableBase
	{
		public int Layer;

		public string State;

		protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
		{
			Layer = _s.SerializeInt("Layer", Layer);
			State = _s.SerializeString("State", State);
		}
	}
}
