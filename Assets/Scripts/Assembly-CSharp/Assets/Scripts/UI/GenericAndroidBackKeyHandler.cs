using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GenericAndroidBackKeyHandler : MonoBehaviour
	{
		[SerializeField]
		private UIButtonProxy m_buttonProxy;

		public static BBList<GenericAndroidBackKeyHandler> CloseButtonsForGenericBackButtonList = new BBList<GenericAndroidBackKeyHandler>();

		private void OnEnable()
		{
			CloseButtonsForGenericBackButtonList.Add(this);
		}

		private void OnDisable()
		{
			CloseButtonsForGenericBackButtonList.Remove(this);
		}

		public void Close()
		{
			if (m_buttonProxy != null)
			{
				m_buttonProxy.OnButtonClick();
			}
		}
	}
}
