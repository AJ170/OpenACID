using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using Assets.Scripts.UI.ScreenManager;
using Mission;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.HUD
{
	public class UIMovement : UISkillBase
	{
		private UIMovementBehaviour m_movementBehaviour;

		private float m_delayWalkRunUpdateTimer;

		private Enum<SkillTypes> m_skillFleeFromCombat;

		private bool m_hideMovementController;

		private bool m_gpWasPressed;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Movement;
			}
		}

		public override UIBaseBehaviour Details
		{
			get
			{
				return m_movementBehaviour;
			}
		}

		public override void OnSceneUnload()
		{
			base.OnSceneUnload();
			if (m_movementBehaviour != null)
			{
				UIMovementBehaviour movementBehaviour = m_movementBehaviour;
				movementBehaviour.OnTapChangeMoveProfile = (UIMovementBehaviour.ChangeMoveProfile)Delegate.Remove(movementBehaviour.OnTapChangeMoveProfile, new UIMovementBehaviour.ChangeMoveProfile(OnTapChangeMoveProfile));
			}
			m_movementBehaviour = null;
		}

		public override void OnMissionUnloaded()
		{
			if (IsActive)
			{
				base.OnMissionUnloaded();
				SetVisible(false);
			}
		}

		public override void OnMissionOutroStarted()
		{
			base.OnMissionOutroStarted();
			if (IsActive)
			{
				SetVisible(false);
				if (m_movementBehaviour != null)
				{
					m_movementBehaviour.Exclude();
				}
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			base.OnUIAlignmentChanged(_isRightHanded);
			if (m_movementBehaviour != null)
			{
				m_movementBehaviour.OnUIAlignmentChanged(_isRightHanded);
			}
		}

		public void OnTapChangeMoveProfile()
		{
			if (m_movementBehaviour.CurrentState == UIMovementBehaviour.EProfile.Flee)
			{
				if (!UiMain.UIHideFleeOption && TriggerSkill(m_skillFleeFromCombat) && CombatController.IsActive)
				{
					m_delayWalkRunUpdateTimer = 0.2f;
				}
			}
			else if (m_movementBehaviour.CurrentState != UIMovementBehaviour.EProfile.Chased)
			{
				EMovementSpeed playerSpeed = ((UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerChosenSpeed == EMovementSpeed.Run) ? EMovementSpeed.Walk : EMovementSpeed.Run);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.SetPlayerSpeed(playerSpeed);
			}
		}

		public override void UpdateFrame(float _deltaTime)
		{
			base.UpdateFrame(_deltaTime);
			if (!(m_movementBehaviour == null) && UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.InProgress)
			{
				CheckInput();
				UpdateSkillButtonState(m_movementBehaviour.GetFleeSkillButton());
				UpdateMovementState();
				m_movementBehaviour.UpdateMovementIcon();
				if (!CombatController.IsActive && m_delayWalkRunUpdateTimer > 0f)
				{
					m_delayWalkRunUpdateTimer -= _deltaTime;
				}
			}
		}

		private void CheckInput()
		{
			if (Singleton<UIScreenManager>.Instance.CurrentModalScreen != null || Singleton<UIScreenManager>.Instance.CurrentScreen != null || OptionsScreenHelper.IsOptionsScreen())
			{
				m_movementBehaviour.ClearPressedState();
			}
			else
			{
				if (!m_movementBehaviour.MovementWidget.isVisible)
				{
					return;
				}
				GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
				if (gamepad.SwitchProfileDown)
				{
					m_gpWasPressed = true;
					m_movementBehaviour.OnPress(true, false);
				}
				if (m_gpWasPressed)
				{
					bool flag = gamepad.SwitchProfileUp;
					if (flag && UbiCore.Utils.Singleton<UiMain>.IsAvailable)
					{
						flag = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GotInput(m_movementBehaviour.gameObject, "OnPress", false);
					}
					if (flag)
					{
						m_movementBehaviour.OnPress(false, false);
						m_gpWasPressed = false;
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
			if (!(m_movementBehaviour != null))
			{
				UIBaseBehaviour widgetByType = UbiCore.Utils.Singleton<UIScreenWidgetManager>.Instance.GetWidgetByType(ScreenWidgetType);
				if (widgetByType != null)
				{
					m_movementBehaviour = widgetByType as UIMovementBehaviour;
				}
			}
		}

		protected override void InitLogic()
		{
			m_delayWalkRunUpdateTimer = 0f;
			m_hideMovementController = false;
			m_skillFleeFromCombat = ((!UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.Has("Flee_From_Combat".ToEnum<SkillTypes>())) ? Enum<SkillTypes>.Invalid : "Flee_From_Combat".ToEnum<SkillTypes>());
			if (m_movementBehaviour != null)
			{
				m_movementBehaviour.Init(m_skillFleeFromCombat);
				UIMovementBehaviour movementBehaviour = m_movementBehaviour;
				movementBehaviour.OnTapChangeMoveProfile = (UIMovementBehaviour.ChangeMoveProfile)Delegate.Combine(movementBehaviour.OnTapChangeMoveProfile, new UIMovementBehaviour.ChangeMoveProfile(OnTapChangeMoveProfile));
				UpdateMovementState();
				m_movementBehaviour.Show();
				base.IsInitialized = true;
			}
		}

		protected override void OnSkillUpdate(SkillEvent _message)
		{
			if (_message.SkillType == m_skillFleeFromCombat)
			{
				OnSkillUpdateButton(_message, m_movementBehaviour.GetFleeSkillButton());
			}
		}

		private void UpdateMovementState()
		{
			bool flag = false;
			if (m_skillFleeFromCombat.IsValid && CombatController.IsPlayerEscapeAllowed)
			{
				if (UiMain.UIHideFleeOption)
				{
					flag = true;
				}
				UISkillButtonBaseBehaviour fleeSkillButton = m_movementBehaviour.GetFleeSkillButton();
				if (fleeSkillButton.CurrentSkillStatus == UISkillButtonBaseBehaviour.ESkillStatus.ActivatedCooldown)
				{
					ILogicInterface logicInterface = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface;
					int gameTurn = logicInterface.GameTurn;
					int gameTurnDuration = logicInterface.GetCoolDownEnd(m_skillFleeFromCombat) - gameTurn;
					fleeSkillButton.SetSkillState(UISkillButtonBaseBehaviour.ESkillStatus.CoolDown, gameTurnDuration, gameTurn);
				}
				m_movementBehaviour.HandleStateChange(UIMovementBehaviour.EProfile.Flee);
			}
			else if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.IsPlayerBeingChased)
			{
				m_movementBehaviour.HandleStateChange(UIMovementBehaviour.EProfile.Chased);
			}
			else if (m_delayWalkRunUpdateTimer <= 0f && !CombatController.IsActive)
			{
				UIMovementBehaviour.EProfile newState = UIMovementBehaviour.EProfile.Run;
				switch (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerChosenSpeed)
				{
				case EMovementSpeed.WalkSlow:
				case EMovementSpeed.Walk:
				case EMovementSpeed.WalkFast:
					newState = UIMovementBehaviour.EProfile.Walk;
					break;
				}
				m_movementBehaviour.HandleStateChange(newState);
			}
			if (m_hideMovementController != flag)
			{
				m_hideMovementController = flag;
				SetVisible(!flag);
			}
		}
	}
}
