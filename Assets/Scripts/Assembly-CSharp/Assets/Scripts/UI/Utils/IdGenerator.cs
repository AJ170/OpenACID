using SharedBaseLib.Serialization;

namespace Assets.Scripts.UI.Utils
{
	public sealed class IdGenerator<T>
	{
		private Id<T> m_id = new Id<T>(0L);

		public Id<T> Next()
		{
			m_id += 1;
			return m_id;
		}

		public void Serialize(DataSerializerBase _ser)
		{
			_ser.SerializeBeginNode("IdGenerator");
			m_id.Serialize(_ser);
			_ser.SerializeEndNode();
		}
	}
}
