using System;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Mission;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.HUD
{
	public class UIAction : UISkillBase
	{
		private UIActionBehaviour m_actionBehaviour;

		private UISkillsWheelBehaviour m_skillsWheelBehaviour;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Action;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_actionBehaviour;
			}
		}

		public override bool InitiallyVisible
		{
			get
			{
				return true;
			}
		}

		public bool IsHoldDown { get; private set; }

		private bool SkillsWheelOpen
		{
			get
			{
				UISkillsWheelBehaviour skillsWheelBehaviour = SkillsWheelBehaviour;
				return skillsWheelBehaviour != null && skillsWheelBehaviour.IsOpen;
			}
		}

		private UISkillsWheelBehaviour SkillsWheelBehaviour
		{
			get
			{
				if (m_skillsWheelBehaviour == null)
				{
					UISkills subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UISkills>();
					m_skillsWheelBehaviour = subSystemByType.Details as UISkillsWheelBehaviour;
				}
				return m_skillsWheelBehaviour;
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			if (m_actionBehaviour != null)
			{
				UIActionBehaviour actionBehaviour = m_actionBehaviour;
				actionBehaviour.OnTriggerAction = (UIActionBehaviour.Action)Delegate.Remove(actionBehaviour.OnTriggerAction, new UIActionBehaviour.Action(OnTriggerAction));
				UIActionBehaviour actionBehaviour2 = m_actionBehaviour;
				actionBehaviour2.OnPrepareAction = (UIActionBehaviour.Action)Delegate.Remove(actionBehaviour2.OnPrepareAction, new UIActionBehaviour.Action(OnPrepareAction));
				UIActionBehaviour actionBehaviour3 = m_actionBehaviour;
				actionBehaviour3.OnCancelAction = (UIActionBehaviour.Action)Delegate.Remove(actionBehaviour3.OnCancelAction, new UIActionBehaviour.Action(OnCancelAction));
			}
			m_actionBehaviour = null;
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				SetVisible(false);
			}
		}

		public override void OnMissionEnded()
		{
			base.OnMissionEnded();
			IsHoldDown = false;
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				SetVisible(false);
				if (m_actionBehaviour != null)
				{
					m_actionBehaviour.Exclude();
				}
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			base.OnUIAlignmentChanged(_isRightHanded);
			if (m_actionBehaviour != null)
			{
				m_actionBehaviour.OnUIAlignmentChanged(_isRightHanded);
			}
		}

		public bool OnTriggerAction()
		{
			IsHoldDown = false;
			return AcierInputHandler.Instance.TriggerDefaultAction();
		}

		public bool OnPrepareAction()
		{
			IsHoldDown = false;
			if (SkillsWheelOpen)
			{
				return false;
			}
			IsHoldDown = true;
			return AcierInputHandler.Instance.PrepareDefaultAction();
		}

		public bool OnCancelAction()
		{
			IsHoldDown = false;
			if (SkillsWheelOpen)
			{
				return false;
			}
			return AcierInputHandler.Instance.CancelDefaultAction();
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			if (!(m_actionBehaviour == null) && UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.InProgress)
			{
				CheckInput();
				UpdateActionState();
				if (IsHoldDown)
				{
					OnPrepareAction();
				}
			}
		}

		private void CheckInput()
		{
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			bool actionDown = gamepad.ActionDown;
			bool flag = gamepad.ActionUp;
			UIIngamePopup subSystemByType = UbiCore.Utils.Singleton<UiMain>.Instance.GetSubSystemByType<UIIngamePopup>();
			if (actionDown && subSystemByType.IsClosedableByGamepad)
			{
				subSystemByType.OnPopupDismissed();
				m_actionBehaviour.ClearPressedState();
			}
			else if (Singleton<UIScreenManager>.Instance.CurrentModalScreen != null || Singleton<UIScreenManager>.Instance.CurrentScreen != null || OptionsScreenHelper.IsOptionsScreen())
			{
				m_actionBehaviour.ClearPressedState();
			}
			else
			{
				if (!m_actionBehaviour.gameObject.activeInHierarchy)
				{
					return;
				}
				if (actionDown)
				{
					m_actionBehaviour.OnPress(true, false);
				}
				if (m_actionBehaviour.IsButtonPressed)
				{
					if (flag && UbiCore.Utils.Singleton<UiMain>.IsAvailable)
					{
						flag = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GotInput(m_actionBehaviour.gameObject, "OnPress", false);
					}
					if (flag)
					{
						m_actionBehaviour.OnPress(false, false);
					}
				}
			}
		}

		protected override bool CanBeAborted(Enum<SkillTypes> _skill)
		{
			return false;
		}

		protected override void Init()
		{
			if (!(m_actionBehaviour != null))
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_actionBehaviour = widgetByType as UIActionBehaviour;
				}
			}
		}

		protected override void InitLogic()
		{
			if (m_actionBehaviour != null)
			{
				UIActionBehaviour actionBehaviour = m_actionBehaviour;
				actionBehaviour.OnTriggerAction = (UIActionBehaviour.Action)Delegate.Combine(actionBehaviour.OnTriggerAction, new UIActionBehaviour.Action(OnTriggerAction));
				UIActionBehaviour actionBehaviour2 = m_actionBehaviour;
				actionBehaviour2.OnPrepareAction = (UIActionBehaviour.Action)Delegate.Combine(actionBehaviour2.OnPrepareAction, new UIActionBehaviour.Action(OnPrepareAction));
				UIActionBehaviour actionBehaviour3 = m_actionBehaviour;
				actionBehaviour3.OnCancelAction = (UIActionBehaviour.Action)Delegate.Combine(actionBehaviour3.OnCancelAction, new UIActionBehaviour.Action(OnCancelAction));
				m_actionBehaviour.Show();
				UpdateActionState();
				base.IsInitialized = true;
			}
			IsHoldDown = false;
		}

		protected override void OnSkillUpdate(SkillEvent _message)
		{
		}

		private void UpdateActionState()
		{
			UISkillsWheelBehaviour skillsWheelBehaviour = SkillsWheelBehaviour;
			if (skillsWheelBehaviour != null && skillsWheelBehaviour.IsOpen)
			{
				Texture2D texture2D = null;
				bool enabled = false;
				UISkillButtonBehaviour currentSelected = skillsWheelBehaviour.SkillWheelBehaviour.CurrentSelected;
				if (currentSelected != null)
				{
					texture2D = currentSelected.GetSkillTexture();
					enabled = currentSelected.IsSkillEnabled;
				}
				else
				{
					texture2D = UbiCore.Utils.Singleton<IconManager>.Instance.GetTexture(IconMaterial.Invalid);
				}
				m_actionBehaviour.HandleSkillTextureChange(texture2D, enabled);
			}
			else
			{
				IconMaterial defaultActionIcon = AcierInputHandler.Instance.GetDefaultActionIcon();
				m_actionBehaviour.HandleIconChange(defaultActionIcon);
			}
		}
	}
}
