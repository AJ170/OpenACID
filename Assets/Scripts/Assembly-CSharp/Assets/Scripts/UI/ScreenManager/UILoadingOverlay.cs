using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public class UILoadingOverlay : UIScreen
	{
		private static readonly string s_loadInAnimation = "LoadingOverlayIn";

		[SerializeField]
		private UIExtendedLabel m_heading;

		[SerializeField]
		private UIExtendedLabel m_subHeading;

		[SerializeField]
		private BBTweenAlpha m_mainTweenAlpha;

		[SerializeField]
		private UnityEngine.Animation m_animator;

		public override bool Active
		{
			get
			{
				return base.Active;
			}
			set
			{
				m_mainTweenAlpha.SetTweenToBeginning();
				base.Active = value;
			}
		}

		public override void Init()
		{
			base.Init();
			base.OnScreenAwoken();
			base.Modal = true;
			m_animator.SampleAnimation(0f, string.Empty);
			m_mainTweenAlpha.SetTweenToBeginning();
		}

		[ContextMenu("Execute")]
		public void Restart()
		{
			base.gameObject.SetActive(true);
			DoEnter();
		}

		private bool IsAnimationStillPlaying()
		{
			foreach (AnimationState item in m_animator)
			{
				if (item.normalizedTime > 0f && item.normalizedTime < 1f)
				{
					return true;
				}
			}
			return false;
		}

		protected override void DoActivate()
		{
			base.DoActivate();
			m_mainTweenAlpha.SetTweenToBeginning();
			m_animator.SampleAnimation(0f, string.Empty);
		}

		protected override void DoEnter()
		{
			base.DoEnter();
			m_mainTweenAlpha.SetTweenToEnd();
			m_animator.Play(s_loadInAnimation);
		}

		private void Update()
		{
			m_animator.UpdateAnimationSpeed();
		}

		public void Configure(LoadingOverlayConfiguration _config)
		{
			m_heading.text = _config.Heading;
			m_subHeading.text = _config.SubHeading;
		}
	}
}
