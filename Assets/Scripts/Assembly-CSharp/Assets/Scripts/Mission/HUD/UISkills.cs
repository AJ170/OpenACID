using System.Collections.Generic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UISkills : UISkillBase
	{
		private UISkillsWheelBehaviour m_skillWheelBehaviour;

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_skillWheelBehaviour;
			}
		}

		public List<UISkillButtonBehaviour> GetAllSkillButtons()
		{
			return m_skillWheelBehaviour.GetAllSkillButtons();
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			if (m_skillWheelBehaviour != null)
			{
				List<UISkillButtonBehaviour> allSkillButtons = m_skillWheelBehaviour.GetAllSkillButtons();
				for (int i = 0; i < allSkillButtons.Count; i++)
				{
					allSkillButtons[i].OnTriggerSkill = null;
				}
			}
			m_skillWheelBehaviour = null;
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			SetVisible(false);
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			if (m_skillWheelBehaviour != null)
			{
				m_skillWheelBehaviour.OnUIAlignmentChanged(_isRightHanded);
			}
		}

		public bool OnTriggerSelectedSkill(UISkillButtonBaseBehaviour _skillButton)
		{
			return TriggerSkill(_skillButton.SkillType);
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (!(m_skillWheelBehaviour == null))
			{
				base.UpdateFrame(_deltaTime);
				List<UISkillButtonBehaviour> allSkillButtons = m_skillWheelBehaviour.GetAllSkillButtons();
				for (int i = 0; i < allSkillButtons.Count; i++)
				{
					UpdateSkillButtonState(allSkillButtons[i]);
				}
				m_skillWheelBehaviour.UpdateFrame(_deltaTime);
			}
		}

		protected override void OnSkillUpdate(SkillEvent _message)
		{
			if (!(m_skillWheelBehaviour == null))
			{
				UISkillButtonBehaviour skillButton = m_skillWheelBehaviour.GetSkillButton(_message.SkillType);
				OnSkillUpdateButton(_message, skillButton);
			}
		}

		protected override void Init()
		{
			if (m_skillWheelBehaviour != null)
			{
				return;
			}
			UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
			if (widgetByType != null)
			{
				m_skillWheelBehaviour = widgetByType as UISkillsWheelBehaviour;
				if (m_skillWheelBehaviour != null)
				{
					m_skillWheelBehaviour.Init();
				}
			}
			base.IsInitialized = true;
		}

		protected override void InitLogic()
		{
			if (!base.IsInitialized || !(m_skillWheelBehaviour != null))
			{
				return;
			}
			base.IsInitialized = false;
			Enum<SkillTypes>[] skillWheel = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.SkillWheel;
			if (skillWheel == null)
			{
				return;
			}
			List<UISkillButtonBehaviour> allSkillButtons = m_skillWheelBehaviour.GetAllSkillButtons();
			for (int i = 0; i < allSkillButtons.Count; i++)
			{
				UISkillButtonBehaviour uISkillButtonBehaviour = allSkillButtons[i];
				uISkillButtonBehaviour.OnTriggerSkill = OnTriggerSelectedSkill;
				Enum<SkillTypes> @enum = SkillTypes.Invalid;
				if (i < skillWheel.Length)
				{
					@enum = skillWheel[i];
				}
				ESkillAvailabilityFlags _availabilityFlags;
				bool isEnabled = @enum != SkillTypes.Invalid && IsSkillReady(@enum, out _availabilityFlags);
				uISkillButtonBehaviour.Init(@enum, isEnabled);
			}
			base.IsInitialized = true;
		}
	}
}
