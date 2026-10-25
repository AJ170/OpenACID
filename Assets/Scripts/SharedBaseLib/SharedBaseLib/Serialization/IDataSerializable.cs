namespace SharedBaseLib.Serialization
{
	public interface IDataSerializable
	{
		void Serialize(string _name, DataSerializerBase _s, object _customData = null);
	}
}
