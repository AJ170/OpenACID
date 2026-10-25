namespace SharedBaseLib.Serialization
{
	public abstract class DataSerializableBase : IDataSerializable
	{
		public void Serialize(string _name, DataSerializerBase _s, object _customData = null)
		{
			_s.SerializeBeginNode(_name);
			SerializeInternal(_s, _customData);
			_s.SerializeEndNode();
		}

		protected abstract void SerializeInternal(DataSerializerBase _s, object _customData = null);
	}
}
