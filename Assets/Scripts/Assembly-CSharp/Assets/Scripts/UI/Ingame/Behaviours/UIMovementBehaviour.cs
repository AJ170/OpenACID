using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using Assets.Scripts.UI.Ingame.Behaviours.Skills;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIMovementBehaviour : UIBaseBehaviour
	{
		public enum EProfile
		{
			None = 0,
			Walk = 1,
			Run = 2,
			Flee = 3,
			Chased = 4
		}

		public delegate void ChangeMoveProfile();

		private const string c_spriteButtonNamePrefix = "hud_btn_toggle_";

		private const string c_spriteIconNamePrefix = "hud_icon_walk_toggle_";

		private const string c_fleeSpriteIconName = "hud_icon_walk_toggle_flee_grey";

		[SerializeField]
		private UIWidget m_movementWidget;

		[SerializeField]
		private UISkillFleeButtonBehaviour m_fleeButton;

		[SerializeField]
		private UISprite m_movementButtonBackground;

		[SerializeField]
		private UISprite m_movementIcon;

		[SerializeField]
		private UISprite m_movementBackground;

		public ChangeMoveProfile OnTapChangeMoveProfile;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Movement;
			}
		}

		public EProfile CurrentState { get; private set; }

		public UIWidget MovementWidget
		{
			get
			{
				return m_movementWidget;
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			if (_isRightHanded != base.IsLastRightHandedAlignment)
			{
				UIUtil.ToggleLeftAndRightAnchorPoints(m_movementWidget);
				Transform transform = m_movementBackground.transform;
				Vector3 localPosition = transform.localPosition;
				localPosition.x *= -1f;
				transform.localPosition = localPosition;
				Quaternion localRotation = transform.localRotation;
				localRotation.z *= -1f;
				transform.localRotation = localRotation;
				m_movementWidget.ResizeCollider();
			}
			base.OnUIAlignmentChanged(_isRightHanded);
		}

		public void ClearPressedState()
		{
			UpdateButtonBackground(false);
		}

		public void HandleStateChange(EProfile _newState)
		{
			if (CurrentState != _newState && (CurrentState == EProfile.Flee || _newState != EProfile.Chased))
			{
				CurrentState = _newState;
				UpdateMovementIcon();
			}
		}

		public void Init(Enum<SkillTypes> _skillFleeFromCombat)
		{
			m_fleeButton.Init(_skillFleeFromCombat, false);
			CurrentState = EProfile.None;
			UpdateButtonBackground(false);
		}

		public UISkillButtonBaseBehaviour GetFleeSkillButton()
		{
			return m_fleeButton;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void UpdateMovementIcon()
		{
			EProfile eProfile = CurrentState;
			if (eProfile == EProfile.Chased)
			{
				eProfile = EProfile.Run;
			}
			m_fleeButton.gameObject.SetActive(eProfile == EProfile.Flee);
			string spriteName = "hud_icon_walk_toggle_" + eProfile.ToString().ToLower();
			if (eProfile == EProfile.Flee && !m_fleeButton.IsSkillEnabled)
			{
				spriteName = "hud_icon_walk_toggle_flee_grey";
			}
			m_movementIcon.spriteName = spriteName;
		}

		private void OnValidate()
		{
			OnUIAlignmentChanged(base.IsRightHandedAlignment);
		}

		public void OnPress(bool _isPressed)
		{
			OnPress(_isPressed, true);
		}

		public void OnPress(bool _isPressed, bool _checkHover)
		{
			if (CurrentState != EProfile.Flee || m_fleeButton.IsSkillEnabled)
			{
				if (!_isPressed && (UICamera.hoveredObject == base.gameObject || !_checkHover) && OnTapChangeMoveProfile != null)
				{
					OnTapChangeMoveProfile();
				}
				UpdateButtonBackground(_isPressed);
			}
			else if (!_isPressed)
			{
				UpdateButtonBackground(false);
			}
		}

		private void OnDrag()
		{
			UpdateButtonBackground(UICamera.hoveredObject == base.gameObject);
		}

		private void UpdateButtonBackground(bool _pressed)
		{
			m_movementButtonBackground.spriteName = "hud_btn_toggle_" + ((!_pressed) ? "up" : "down");
		}
	}
}
