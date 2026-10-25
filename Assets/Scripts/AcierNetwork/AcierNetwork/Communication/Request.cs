namespace AcierNetwork.Communication
{
	public abstract class Request
	{
		public delegate void RequestCallback(RequestResult _result);

		protected Params m_param;

		public Params Param
		{
			get
			{
				return m_param;
			}
		}

		protected Request(Params _param)
		{
			m_param = _param;
		}
	}
}
