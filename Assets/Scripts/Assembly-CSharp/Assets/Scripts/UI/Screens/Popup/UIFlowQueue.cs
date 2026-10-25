using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Screens.Popup
{
	public class UIFlowQueue
	{
		public delegate void OnFlowQueueDoneDelegate();

		private readonly BBList<UIFlowQueueItem> m_queue = new BBList<UIFlowQueueItem>();

		public static event OnFlowQueueDoneDelegate OnFlowQueueFinished;

		public bool IsDone()
		{
			return m_queue.Count == 0;
		}

		public void Clear()
		{
			m_queue.ClearIfNotEmpty();
		}

		public void Add(UIFlowQueueItem _delegate)
		{
			m_queue.Add(_delegate);
		}

		public void Proceed()
		{
			if (m_queue.Count == 0)
			{
				if (UIFlowQueue.OnFlowQueueFinished != null)
				{
					UIFlowQueue.OnFlowQueueFinished();
				}
			}
			else
			{
				UIFlowQueueItem uIFlowQueueItem = m_queue[0];
				m_queue.RemoveAt(0);
				uIFlowQueueItem();
			}
		}
	}
}
