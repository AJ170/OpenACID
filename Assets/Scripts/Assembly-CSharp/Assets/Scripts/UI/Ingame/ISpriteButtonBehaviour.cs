namespace Assets.Scripts.UI.Ingame
{
	public interface ISpriteButtonBehaviour : ISpriteControlBehaviour
	{
		void AnimateActivate();

		void AnimateActivityStart();

		void AnimateBlocked();

		void AnimateDeactivate();

		void AnimateInstantActivation();

		void AnimateLoad();

		void AnimatePrepare();

		void AnimateReset();

		void AnimateUnload();

		void AnimateUnprepare(bool _isEnabled);

		void StartCooldown(float _duration);

		void StartActivity(float _duration);
	}
}
