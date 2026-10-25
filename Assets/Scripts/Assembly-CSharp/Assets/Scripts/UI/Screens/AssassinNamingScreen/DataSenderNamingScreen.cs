using AcierProtocol;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinNamingScreen
{
	public class DataSenderNamingScreen : MonoBehaviour
	{
		[SerializeField]
		private Assassin.EClassTypeId m_type;

		public void SendData()
		{
			Singleton<StateManager>.Instance.AssassinCreationSelection.AvatarType = m_type;
		}
	}
}
