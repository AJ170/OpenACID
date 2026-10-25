using Assets.Scripts.UI.Utils.Development;
using Assets.UI.Debriefing;
using UnityEngine;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public class UITextureAnimationObserver : IAnimationObserver
	{
		[CheckAssigned]
		[SerializeField]
		private TextureOffsetAnimation m_animation;

		private void Awake()
		{
		}

		private void OnEnable()
		{
			m_animation.gameObject.SetActive(true);
			m_animation.ActivateEffect();
		}

		private void OnDisable()
		{
			m_animation.gameObject.SetActive(false);
		}
	}
}
