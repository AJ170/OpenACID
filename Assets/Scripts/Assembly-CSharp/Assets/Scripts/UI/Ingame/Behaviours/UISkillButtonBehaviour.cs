using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using Assets.Scripts.UI.Localization;
using Database;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UISkillButtonBehaviour : UISkillButtonBaseBehaviour
	{
		public enum EDirection
		{
			TopMiddle = 0,
			TopLeft = 1,
			BottomLeft = 2,
			BottomMiddle = 3,
			BottomRight = 4,
			TopRight = 5
		}

		public EDirection Direction;

		[SerializeField]
		private GameObject m_iconContainer;

		[SerializeField]
		private UITexture m_skillIconTexture;

		[SerializeField]
		private UISprite m_skillBackground;

		[SerializeField]
		private UISprite m_selectedDecal;

		[SerializeField]
		private TweenAlpha m_selectionDecalTween;

		[SerializeField]
		private UIExtendedLabel m_skillName;

		[SerializeField]
		private UISkillsWheelBehaviour m_skillsWheel;

		private Texture2D m_skillPressed;

		private Texture2D m_skillReleased;

		private SkillData.ISkillDefinition m_skillDescription;

		public SkillData.ISkillDefinition SkillDefinition
		{
			get
			{
				return m_skillDescription;
			}
		}

		public override Enum<SkillTypes> SkillType
		{
			get
			{
				return m_skillType;
			}
			protected set
			{
				base.SkillType = value;
				if (value == SkillTypes.Invalid)
				{
					m_iconContainer.SetActive(false);
					m_skillPressed = null;
					m_skillReleased = null;
					m_skillDescription = null;
					return;
				}
				m_skillDescription = null;
				GameID gameID = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetGameID(m_skillType);
				if (GameID.IsValid(gameID))
				{
					m_skillDescription = m_skillType.GetDefinition();
					m_skillPressed = m_skillDescription.IconDown.GetObject<Texture2D>();
					m_skillReleased = m_skillDescription.IconUp.GetObject<Texture2D>();
				}
				m_iconContainer.SetActive(m_skillType != SkillTypes.Invalid);
			}
		}

		public Texture2D GetSkillTexture()
		{
			return m_skillReleased;
		}

		public override void OnPressed()
		{
			if (!m_skillsWheel.IsSwipping)
			{
				Select();
			}
		}

		public override void OnReleased()
		{
			if (!m_skillsWheel.IsSwipping)
			{
				Deselect();
				if (UICamera.hoveredObject == m_skillIconTexture.gameObject)
				{
					TriggerSkillButton();
				}
			}
			m_skillsWheel.CloseWheel();
		}

		public override void OnDrag()
		{
			if (!m_skillsWheel.IsSwipping)
			{
				if (UICamera.hoveredObject == m_skillIconTexture.gameObject)
				{
					Select();
				}
				else
				{
					Deselect();
				}
			}
		}

		protected override void OnDestroy()
		{
			m_skillPressed = null;
			m_skillReleased = null;
		}

		protected override void OnSelected()
		{
			m_selectionDecalTween.ResetToBeginning();
			m_selectionDecalTween.PlayForward();
		}

		protected override void OnUpdateSkill()
		{
			bool active = false;
			if (base.IsSelected && SkillDefinition != null)
			{
				if (SkillDefinition.NameLocaKey.Key == -1)
				{
					m_skillName.text = "No valid name found";
				}
				else
				{
					m_skillName.text = LocalizationManager.Instance.Get(SkillDefinition.NameLocaKey.Key);
				}
				active = true;
			}
			m_skillName.gameObject.SetActive(active);
		}

		protected override void OnCoolDownCompleted()
		{
			if (base.gameObject.activeInHierarchy)
			{
				TweenScale component = m_skillIconTexture.GetComponent<TweenScale>();
				component.ResetToBeginning();
				component.PlayForward();
			}
		}

		protected override void UpdateSkillLayout()
		{
			base.UpdateSkillLayout();
			bool flag = !base.IsSelected;
			string text = "active";
			string text2 = string.Empty;
			int depth = 0;
			if (base.IsSelected)
			{
				if (!base.IsSkillEnabled)
				{
					depth = 10;
					text2 = "_negative";
				}
				else
				{
					depth = 8;
					text = "down";
				}
			}
			switch (base.CurrentSkillStatus)
			{
			case ESkillStatus.Activated:
				text = "down";
				break;
			case ESkillStatus.CoolDown:
				flag = true;
				text = "active";
				break;
			case ESkillStatus.ActivatedCooldown:
				flag = false;
				text = "down";
				break;
			}
			m_skillIconTexture.mainTexture = ((!flag) ? m_skillPressed : m_skillReleased);
			m_skillBackground.spriteName = "skillwheel_btn_" + text;
			float skillStatusAlpha = GetSkillStatusAlpha();
			m_skillIconTexture.alpha = skillStatusAlpha;
			m_skillBackground.alpha = skillStatusAlpha;
			m_selectedDecal.depth = depth;
			m_selectedDecal.spriteName = "btn_glow_skillbtn" + text2;
			m_selectedDecal.gameObject.SetActive(base.IsSelected);
		}
	}
}
