using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinInventoryScreen
{
	public class CompareItemHandler : MonoBehaviour
	{
		public enum EState
		{
			Neutral = 0,
			Better = 1,
			Worse = 2
		}

		[SerializeField]
		private UISprite m_upSprite;

		[SerializeField]
		private UISprite m_downSprite;

		private EState m_state;

		public EState State
		{
			get
			{
				return m_state;
			}
			set
			{
				m_state = value;
				UpdateFromState(m_state);
			}
		}

		private void Awake()
		{
		}

		private void UpdateFromState(EState _state)
		{
			m_upSprite.gameObject.SetActive(false);
			m_downSprite.gameObject.SetActive(false);
			switch (_state)
			{
			case EState.Neutral:
				break;
			case EState.Better:
				m_upSprite.gameObject.SetActive(true);
				break;
			case EState.Worse:
				break;
			}
		}
	}
}
