using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.Skills
{
	public class UISkillButtonBaseBehaviour : MonoBehaviour
	{
		public enum ESkillStatus
		{
			None = 0,
			Activated = 1,
			CoolDown = 2,
			ActivatedCooldown = 3,
			Removed = 4
		}

		public delegate bool TriggerSkill(UISkillButtonBaseBehaviour _selectedSkill);

		public UISprite CooldownIndicator;

		public UISprite CooldownDecal;

		public UISprite CanBeAbortedIcon;

		public TriggerSkill OnTriggerSkill;

		protected Enum<SkillTypes> m_skillType = SkillTypes.Invalid;

		protected ESkillStatus m_currentSkillStatus;

		protected int m_cooldownGameTurnDuration;

		protected int m_cooldownStartGameTurn;

		private bool m_isSelected;

		private bool m_isSkillEnabled;

		public ESkillEventType LastLogicSubType { get; set; }

		public ESkillStatus CurrentSkillStatus
		{
			get
			{
				return m_currentSkillStatus;
			}
		}

		public bool CachedSkillAvailable { get; set; }

		public virtual Enum<SkillTypes> SkillType
		{
			get
			{
				return m_skillType;
			}
			protected set
			{
				if (value == SkillTypes.Invalid)
				{
					m_skillType = SkillTypes.Invalid;
				}
				else
				{
					m_skillType = value;
				}
			}
		}

		public bool IsSelected
		{
			get
			{
				return m_isSelected;
			}
			private set
			{
				if (m_isSelected != value)
				{
					m_isSelected = value;
					UpdateSkillLayout();
					OnUpdateSkill();
				}
			}
		}

		public bool IsSkillEnabled
		{
			get
			{
				return m_isSkillEnabled;
			}
			set
			{
				if (m_isSkillEnabled != value)
				{
					m_isSkillEnabled = value;
					UpdateSkillLayout();
				}
			}
		}

		public void Init(Enum<SkillTypes> _skillType, bool _isEnabled)
		{
			SkillType = _skillType;
			IsSkillEnabled = _isEnabled;
			IsSelected = false;
			SetSkillState(ESkillStatus.None, 0, 0);
			CooldownIndicator.gameObject.SetActive(false);
			OnUpdateSkill();
		}

		public virtual void Select()
		{
			if (!(m_skillType == SkillTypes.Invalid) && !IsSelected)
			{
				IsSelected = true;
				OnSelected();
			}
		}

		public virtual void Deselect()
		{
			IsSelected = false;
		}

		public virtual void OnPressed()
		{
			Select();
		}

		public virtual void OnReleased()
		{
			Deselect();
		}

		public virtual void OnDrag()
		{
			Deselect();
		}

		public void TriggerSkillButton()
		{
			if (OnTriggerSkill != null)
			{
				if (OnTriggerSkill(this))
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_subaccept);
				}
				else
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny);
				}
			}
		}

		public virtual void SetSkillState(ESkillStatus _status, int _gameTurnDuration, int _startGameTurn)
		{
			if (m_currentSkillStatus != _status)
			{
				switch (m_currentSkillStatus)
				{
				case ESkillStatus.ActivatedCooldown:
					m_cooldownStartGameTurn = 0;
					m_cooldownGameTurnDuration = 0;
					CooldownIndicator.gameObject.SetActive(false);
					break;
				case ESkillStatus.CoolDown:
					m_cooldownStartGameTurn = 0;
					m_cooldownGameTurnDuration = 0;
					CooldownIndicator.gameObject.SetActive(false);
					OnCoolDownCompleted();
					break;
				}
			}
			switch (_status)
			{
			case ESkillStatus.CoolDown:
			case ESkillStatus.ActivatedCooldown:
				m_cooldownStartGameTurn = _startGameTurn;
				m_cooldownGameTurnDuration = _gameTurnDuration;
				CooldownIndicator.gameObject.SetActive(true);
				CooldownIndicator.fillAmount = 0f;
				break;
			case ESkillStatus.Removed:
				Init(SkillTypes.Invalid, false);
				break;
			}
			m_currentSkillStatus = _status;
			UpdateSkillLayout();
		}

		public void UpdateButtonState(int _gameTurn, float _gameTurnProgress, bool _canBeAborted)
		{
			switch (m_currentSkillStatus)
			{
			case ESkillStatus.CoolDown:
			case ESkillStatus.ActivatedCooldown:
			{
				float fillAmount = CooldownIndicator.fillAmount;
				float num = Mathf.Clamp01((float)(_gameTurn - m_cooldownStartGameTurn) / (float)m_cooldownGameTurnDuration);
				fillAmount += (num - fillAmount) * _gameTurnProgress;
				CooldownIndicator.fillAmount = fillAmount;
				break;
			}
			}
			CanBeAbortedIcon.gameObject.SetActive(_canBeAborted);
		}

		protected virtual void OnSelected()
		{
		}

		protected virtual void OnUpdateSkill()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void OnCoolDownCompleted()
		{
		}

		protected virtual void UpdateSkillLayout()
		{
			bool active = false;
			ESkillStatus currentSkillStatus = m_currentSkillStatus;
			if (currentSkillStatus == ESkillStatus.ActivatedCooldown)
			{
				active = !IsSelected;
			}
			CooldownDecal.gameObject.SetActive(active);
		}

		public float GetSkillStatusAlpha()
		{
			float result = 1f;
			if (!m_isSkillEnabled)
			{
				result = 0.5f;
			}
			switch (m_currentSkillStatus)
			{
			case ESkillStatus.ActivatedCooldown:
				result = 1f;
				break;
			case ESkillStatus.CoolDown:
				result = 0.5f;
				break;
			}
			return result;
		}
	}
}
