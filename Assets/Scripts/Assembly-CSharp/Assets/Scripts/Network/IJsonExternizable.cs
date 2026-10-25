namespace Assets.Scripts.Network
{
	public interface IJsonExternizable
	{
		void ReadExternal(string _data);

		void WriteExternal(out string _data);
	}
}
