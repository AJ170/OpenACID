namespace Assets.Scripts.UI.DataHandling
{
	public class DataManager
	{
		private static DataManager m_instance;

		public static DataManager Instance
		{
			get
			{
				if (m_instance == null)
				{
					m_instance = new DataManager();
				}
				return m_instance;
			}
		}

		private DataManager()
		{
		}
	}
}
