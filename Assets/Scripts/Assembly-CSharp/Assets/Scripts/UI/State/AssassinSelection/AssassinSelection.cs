using System.Collections.Generic;

namespace Assets.Scripts.UI.State.AssassinSelection
{
	public class AssassinSelection
	{
		public List<EventDelegate> onSelectionChange = new List<EventDelegate>();

		private string m_id;

		public string AssassinId
		{
			get
			{
				return m_id;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					string id = m_id;
					m_id = value;
					if (id != m_id)
					{
						EventDelegate.Execute(onSelectionChange);
					}
				}
			}
		}

		public bool Valid
		{
			get
			{
				return !string.IsNullOrEmpty(m_id);
			}
		}

		public void Clear()
		{
			m_id = string.Empty;
		}
	}
}
