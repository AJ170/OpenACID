using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions
{
	[AddComponentMenu("NGUI/Interaction/Trigger Particals")]
	public class UIParticaleTrigger : MonoBehaviour
	{
		public enum Trigger
		{
			OnClick = 0,
			OnMouseOver = 1,
			OnMouseOut = 2,
			OnPress = 3,
			OnRelease = 4
		}

		public enum Stop
		{
			OnClick = 0,
			OnMouseOver = 1,
			OnMouseOut = 2,
			OnPress = 3,
			OnRelease = 4,
			Never = 5
		}

		[SerializeField]
		private ParticleSystem mSystem;

		[SerializeField]
		private Trigger mTrigger;

		[SerializeField]
		private Stop mStop = Stop.OnRelease;

		private void Awake()
		{
			if ((bool)mSystem)
			{
				mSystem.Stop();
			}
		}

		private void OnHover(bool isOver)
		{
			if (base.enabled && (bool)mSystem)
			{
				if (mSystem.isPlaying && isOver && mStop == Stop.OnMouseOver)
				{
					mSystem.Stop();
				}
				else if (mSystem.isPlaying && !isOver && mStop == Stop.OnMouseOut)
				{
					mSystem.Stop();
				}
				else if (!mSystem.isPlaying && isOver && mTrigger == Trigger.OnMouseOver)
				{
					mSystem.Play();
				}
				else if (!mSystem.isPlaying && !isOver && mTrigger == Trigger.OnMouseOut)
				{
					mSystem.Play();
				}
			}
		}

		private void OnPress(bool isPressed)
		{
			if (base.enabled && (bool)mSystem)
			{
				if (mSystem.isPlaying && isPressed && mStop == Stop.OnPress)
				{
					mSystem.Stop();
				}
				else if (mSystem.isPlaying && !isPressed && mStop == Stop.OnRelease)
				{
					mSystem.Stop();
				}
				else if (!mSystem.isPlaying && isPressed && mTrigger == Trigger.OnPress)
				{
					mSystem.Play();
				}
				else if (!mSystem.isPlaying && !isPressed && mTrigger == Trigger.OnRelease)
				{
					mSystem.Play();
				}
			}
		}

		private void OnClick()
		{
			if (base.enabled && (bool)mSystem)
			{
				if (mSystem.isPlaying && mStop == Stop.OnClick)
				{
					mSystem.Stop();
				}
				else if (!mSystem.isPlaying && mTrigger == Trigger.OnClick)
				{
					mSystem.Play();
				}
			}
		}
	}
}
