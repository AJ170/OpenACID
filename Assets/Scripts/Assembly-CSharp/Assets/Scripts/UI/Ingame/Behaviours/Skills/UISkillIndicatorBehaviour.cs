using AnimationOrTween;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.Skills
{
	public class UISkillIndicatorBehaviour : MonoBehaviour
	{
		public enum EStatus
		{
			Invalid = 0,
			SkillActive = 1,
			SkillCooldown = 2,
			SkillAvailable = 3,
			SkillNotAvailable = 4,
			NoSkill = 5
		}

		private static float s_maxFill = 0.16f;

		[SerializeField]
		private UISkillButtonBehaviour.EDirection m_direction;

		[SerializeField]
		private BBTweenAlpha m_alphaGlowTween;

		[SerializeField]
		private UISprite m_glow;

		[SerializeField]
		private UISprite m_cooldownIndicator;

		[SerializeField]
		private UISprite m_backgroundIndicator;

		private EStatus m_status;

		public UISkillButtonBehaviour.EDirection Direction
		{
			get
			{
				return m_direction;
			}
		}

		public bool IsVisible { get; set; }

		private void Awake()
		{
			m_backgroundIndicator.fillAmount = s_maxFill;
			m_glow.gameObject.SetActive(true);
			m_alphaGlowTween.SetTweenToBeginning();
		}

		public void UpdateStatus(UISkillButtonBehaviour _button)
		{
			EStatus status = m_status;
			if (_button.SkillType == SkillTypes.Invalid)
			{
				m_status = EStatus.NoSkill;
			}
			else if (_button.CurrentSkillStatus == UISkillButtonBaseBehaviour.ESkillStatus.ActivatedCooldown || _button.CurrentSkillStatus == UISkillButtonBaseBehaviour.ESkillStatus.Activated)
			{
				m_status = EStatus.SkillActive;
			}
			else if (_button.CurrentSkillStatus == UISkillButtonBaseBehaviour.ESkillStatus.CoolDown)
			{
				m_status = EStatus.SkillCooldown;
				m_cooldownIndicator.fillAmount = s_maxFill * _button.CooldownIndicator.fillAmount;
			}
			else if (!_button.IsSkillEnabled)
			{
				m_status = EStatus.SkillNotAvailable;
			}
			else
			{
				m_status = EStatus.SkillAvailable;
			}
			if (m_status != status)
			{
				bool active = true;
				bool active2 = false;
				float alpha = 1f;
				string spriteName = string.Empty;
				switch (m_status)
				{
				case EStatus.SkillActive:
					spriteName = "hud_skills_indicator_active";
					break;
				case EStatus.SkillAvailable:
					spriteName = "hud_btn_frame_up";
					break;
				case EStatus.SkillCooldown:
					active2 = true;
					alpha = 0f;
					break;
				case EStatus.SkillNotAvailable:
					spriteName = "hud_btn_frame_up";
					alpha = 0.3f;
					break;
				default:
					active = false;
					break;
				}
				m_cooldownIndicator.gameObject.SetActive(active2);
				m_backgroundIndicator.alpha = alpha;
				m_backgroundIndicator.spriteName = spriteName;
				base.gameObject.SetActive(active);
			}
			if (m_status == EStatus.SkillActive && status != EStatus.SkillActive && status != 0)
			{
				EventDelegate.Remove(m_alphaGlowTween.onFinished, OnFadeOUtGlow);
				m_alphaGlowTween.StartTweenPlayForward();
			}
			else if (m_status != EStatus.SkillActive && status == EStatus.SkillActive)
			{
				m_alphaGlowTween.StartTweenPlayReverse();
			}
			else if (m_status != EStatus.SkillCooldown && status == EStatus.SkillCooldown)
			{
				if (IsVisible && m_status != EStatus.NoSkill)
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_ability_replenished);
				}
				if (!m_alphaGlowTween.enabled || m_alphaGlowTween.direction != AnimationOrTween.Direction.Reverse)
				{
					EventDelegate.Add(m_alphaGlowTween.onFinished, OnFadeOUtGlow, true);
					m_alphaGlowTween.StartTweenPlayForward();
				}
			}
		}

		private void OnFadeOUtGlow()
		{
			if (m_status != EStatus.SkillActive)
			{
				m_alphaGlowTween.StartTweenPlayReverse();
			}
		}
	}
}
