using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UISkillsWheelBehaviour : UIBaseBehaviour
	{
		public enum EOperationMode
		{
			None = 0,
			Touch = 1,
			Gamepad = 2
		}

		public UISkillWheelControllerBehaviour WheelControllerBehaviour;

		public UISkillWheelBehaviour SkillWheelBehaviour;

		private bool m_closeOnTap;

		private float m_lastPressedTime;

		private Vector3 m_lastPos;

		private bool m_hadPadInput;

		private bool m_wasHeld;

		private float m_cancelTime;

		private EOperationMode m_operationMode;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.SkillWheel;
			}
		}

		public bool IsSwipping { get; private set; }

		public bool IsOpen
		{
			get
			{
				return SkillWheelBehaviour.IsWheelOpen;
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			if (_isRightHanded != base.IsLastRightHandedAlignment)
			{
				WheelControllerBehaviour.OnUIAlignmentChanged(_isRightHanded);
			}
			base.OnUIAlignmentChanged(_isRightHanded);
		}

		public override void SetVisibility(bool _visible)
		{
			base.SetVisibility(_visible);
			WheelControllerBehaviour.IsVisible = _visible;
		}

		public void Init()
		{
			m_closeOnTap = false;
			m_lastPressedTime = 0f;
			m_lastPos = Vector3.zero;
			WheelControllerBehaviour.Init();
			SkillWheelBehaviour.Init();
			SkillWheelBehaviour.Exclude();
			Show();
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public List<UISkillButtonBehaviour> GetAllSkillButtons()
		{
			return SkillWheelBehaviour.Children;
		}

		public UISkillButtonBehaviour GetSkillButton(Enum<SkillTypes> _skillType)
		{
			return SkillWheelBehaviour.GetSkillButton(_skillType);
		}

		public void CloseWheel()
		{
			IsSwipping = false;
			SkillWheelBehaviour.DeselectAllSkills();
			SkillWheelBehaviour.Exclude();
			WheelControllerBehaviour.UpdateOpenButton(false);
		}

		public void OpenWheel()
		{
			IsSwipping = false;
			SkillWheelBehaviour.Show();
			WheelControllerBehaviour.UpdateOpenButton(true);
		}

		public void TriggerSkill()
		{
			if (SkillWheelBehaviour.CurrentSelected != null)
			{
				if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GotInput("Input/SkillWheel/Triggered" + SkillWheelBehaviour.CurrentSelected.Direction);
				}
				SkillWheelBehaviour.CurrentSelected.TriggerSkillButton();
				SkillWheelBehaviour.CurrentSelected.Deselect();
				SkillWheelBehaviour.CurrentSelected = null;
			}
		}

		public void OnWheelAreaPressed()
		{
			OnPress(true);
		}

		public void OnWheelAreaReleased()
		{
			OnPress(false);
		}

		public void OnWheelAreaDrag()
		{
			if (IsSwipping)
			{
				Vector3 vector = UICamera.currentTouch.pos;
				Vector3 vector2 = m_lastPos - vector;
				float angle = Mathf.Atan2(vector2.y, vector2.x) * 180f / (float)Math.PI;
				bool deselectAll = false;
				if (Vector3.Distance(m_lastPos, vector) < 50f)
				{
					SkillWheelBehaviour.CurrentSelected = null;
					deselectAll = true;
				}
				UpdateSelectedSkill(angle, deselectAll);
			}
		}

		public void UpdateFrame(float _deltaTime)
		{
			CheckInput();
			WheelControllerBehaviour.UpdateIndicators();
		}

		private void CheckInput()
		{
			if (Singleton<UIScreenManager>.Instance.CurrentModalScreen != null || Singleton<UIScreenManager>.Instance.CurrentScreen != null || OptionsScreenHelper.IsOptionsScreen() || !WheelControllerBehaviour.gameObject.activeInHierarchy)
			{
				return;
			}
			GamepadWrapper gamepad = AcierInputHandler.Instance.Gamepad;
			bool skillWheelHeld = gamepad.SkillWheelHeld;
			if (m_operationMode == EOperationMode.Touch && skillWheelHeld && IsOpen)
			{
				SkillWheelBehaviour.Exclude();
				m_operationMode = EOperationMode.None;
			}
			if (skillWheelHeld && !IsOpen)
			{
				OpenWheel();
				m_operationMode = EOperationMode.Gamepad;
				m_wasHeld = true;
			}
			if (!skillWheelHeld && IsOpen && m_wasHeld)
			{
				TriggerSkill();
				CloseWheel();
				m_operationMode = EOperationMode.None;
				m_wasHeld = false;
			}
			if (IsOpen)
			{
				Vector2 skillWheel = gamepad.SkillWheel;
				bool flag = skillWheel.sqrMagnitude > 0.0361f;
				if (m_operationMode == EOperationMode.Touch && flag)
				{
					SkillWheelBehaviour.Exclude();
					m_operationMode = EOperationMode.None;
				}
				if (flag)
				{
					m_cancelTime = LogicMain.CurrentTime + 0.5f;
					float angle = Mathf.Atan2(skillWheel.y, 0f - skillWheel.x) * 180f / (float)Math.PI;
					UpdateSelectedSkill(angle, false);
					m_hadPadInput = true;
				}
				if (!flag && LogicMain.CurrentTime > m_cancelTime && m_hadPadInput)
				{
					UpdateSelectedSkill(0f, true);
					m_hadPadInput = false;
				}
			}
		}

		public void OnPress(bool _isPressed)
		{
			if (SkillWheelBehaviour.IsWheelOpen && _isPressed)
			{
				m_closeOnTap = true;
			}
			UICamera.MouseOrTouch currentTouch = UICamera.currentTouch;
			if (_isPressed)
			{
				WheelControllerBehaviour.UpdateOpenButton(true);
				SkillWheelBehaviour.Show();
				m_lastPos = ((currentTouch == null) ? Vector2.zero : currentTouch.pos);
				m_lastPressedTime = RealTime.time;
				m_operationMode = EOperationMode.Touch;
			}
			else if (IsSwipping)
			{
				bool flag = false;
				if (SkillWheelBehaviour.CurrentSelected != null)
				{
					TriggerSkill();
				}
				else if (!m_closeOnTap && currentTouch != null && !currentTouch.dragStarted && m_lastPressedTime + 0.75f > RealTime.time)
				{
					flag = true;
				}
				m_closeOnTap = false;
				WheelControllerBehaviour.UpdateOpenButton(flag);
				if (flag)
				{
					SkillWheelBehaviour.Show();
					m_operationMode = EOperationMode.Touch;
				}
				else
				{
					SkillWheelBehaviour.Exclude();
					m_operationMode = EOperationMode.None;
				}
			}
			IsSwipping = _isPressed;
		}

		private void UpdateSelectedSkill(float _angle, bool _deselectAll)
		{
			bool flag = _angle < 0f;
			_angle = Mathf.Abs(_angle);
			UISkillButtonBehaviour.EDirection direction = UISkillButtonBehaviour.EDirection.TopMiddle;
			if (_angle > 0f && _angle < 60f)
			{
				direction = (flag ? UISkillButtonBehaviour.EDirection.TopLeft : UISkillButtonBehaviour.EDirection.BottomLeft);
			}
			else if (_angle > 120f && _angle < 180f)
			{
				direction = ((!flag) ? UISkillButtonBehaviour.EDirection.BottomRight : UISkillButtonBehaviour.EDirection.TopRight);
			}
			else if (!flag)
			{
				direction = UISkillButtonBehaviour.EDirection.BottomMiddle;
			}
			SkillWheelBehaviour.UpdateSkillSelection(direction, _deselectAll);
		}
	}
}
