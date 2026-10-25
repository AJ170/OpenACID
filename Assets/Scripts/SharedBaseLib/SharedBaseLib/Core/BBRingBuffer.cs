namespace SharedBaseLib.Core
{
	public class BBRingBuffer<T> : RingBuffer<T>
	{
		public BBRingBuffer(int _size)
			: base(_size)
		{
		}
	}
}
