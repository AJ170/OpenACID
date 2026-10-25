using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public abstract class UIInputReactor : MonoBehaviour
	{
		public enum ETrigger
		{
			DelegateClick = 0,
			StandardClick = 1
		}

		[SerializeField]
		private ETrigger m_trigger;

		public List<EventDelegate> onTrigger = new List<EventDelegate>();

		private void OnDestroy()
		{
			onTrigger.Clear();
		}

		public void OnClick()
		{
			if (m_trigger == ETrigger.StandardClick)
			{
				DoClick();
			}
		}

		public void OnDelegateClick()
		{
			if (m_trigger == ETrigger.DelegateClick)
			{
				DoClick();
			}
		}

		protected void DoClick()
		{
			EventDelegate.Execute(onTrigger);
			DoReactOnClick();
		}

		protected abstract void DoReactOnClick();
	}
}
