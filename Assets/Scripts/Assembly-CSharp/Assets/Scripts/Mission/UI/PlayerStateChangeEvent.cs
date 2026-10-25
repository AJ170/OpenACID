using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.UI
{
	public class PlayerStateChangeEvent : LogicUiEvent
	{
		public EPlayerState PlayerState { get; private set; }

		public PlayerStateChangeEvent(EPlayerState _playerState)
		{
			PlayerState = _playerState;
		}
	}
}
