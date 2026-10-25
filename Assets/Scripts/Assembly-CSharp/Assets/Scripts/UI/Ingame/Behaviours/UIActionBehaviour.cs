using Assets.Scripts.Mission.Logic;
using Assets.Scripts.UI.NGUIExtentions;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIActionBehaviour : UIBaseBehaviour
	{
		public delegate bool Action();

		private const string c_spriteButtonNamePrefix = "hud_btn_toggle_";

		[SerializeField]
		private UIWidget m_actionWidget;

		[SerializeField]
		private UISprite m_actionButtonBackground;

		[SerializeField]
		private UITexture m_skillTexture;

		[SerializeField]
		private UISprite m_actionRim;

		[SerializeField]
		private UISprite m_actionBackground;

		[SerializeField]
		private UIPlayAudio m_playAudio;

		public Action OnTriggerAction;

		public Action OnPrepareAction;

		public Action OnCancelAction;

		private bool m_enabled;

		public bool IsButtonPressed { get; private set; }

		public IconMaterial CurrentActionIcon { get; private set; }

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Action;
			}
		}

		public override void OnUIAlignmentChanged(bool _isRightHanded)
		{
			if (_isRightHanded != base.IsLastRightHandedAlignment)
			{
				UIUtil.ToggleLeftAndRightAnchorPoints(m_actionWidget);
				Transform transform = m_actionBackground.transform;
				Vector3 localPosition = transform.localPosition;
				localPosition.x *= -1f;
				transform.localPosition = localPosition;
				Quaternion localRotation = transform.localRotation;
				localRotation.z *= -1f;
				transform.localRotation = localRotation;
				m_actionWidget.ResizeCollider();
			}
			base.OnUIAlignmentChanged(_isRightHanded);
		}

		public void HandleSkillTextureChange(Texture2D texture, bool enabled)
		{
			m_enabled = enabled;
			CurrentActionIcon = IconMaterial.Invalid;
			m_skillTexture.mainTexture = texture;
			m_skillTexture.enabled = texture != null;
			m_skillTexture.alpha = ((!m_enabled) ? 0.5f : 1f);
			m_actionButtonBackground.alpha = ((!m_enabled) ? 0f : 1f);
			m_actionRim.enabled = m_enabled;
		}

		public void HandleIconChange(IconMaterial _newIcon)
		{
			CurrentActionIcon = _newIcon;
			m_enabled = CurrentActionIcon != IconMaterial.Invalid;
			Texture2D texture = UbiCore.Utils.Singleton<IconManager>.Instance.GetTexture(CurrentActionIcon);
			HandleSkillTextureChange(texture, m_enabled);
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		private void OnValidate()
		{
			OnUIAlignmentChanged(base.IsRightHandedAlignment);
		}

		public void OnPress(bool _isPressed)
		{
			OnPress(_isPressed, false);
		}

		public void ClearPressedState()
		{
			IsButtonPressed = false;
			UpdateButtonBackground(false);
		}

		public void OnPress(bool _isPressed, bool _checkHover)
		{
			IsButtonPressed = _isPressed;
			m_playAudio.Volume = ((!m_enabled || LogicMain.PlayerEntity.GetIsInCombat()) ? 0f : 1f);
			if (_isPressed && OnPrepareAction != null)
			{
				OnPrepareAction();
			}
			if (!m_enabled)
			{
				UpdateButtonBackground(false);
				return;
			}
			if (!_isPressed)
			{
				if (UICamera.hoveredObject == base.gameObject || !_checkHover)
				{
					if (OnTriggerAction != null)
					{
						OnTriggerAction();
					}
				}
				else if (OnCancelAction != null)
				{
					OnCancelAction();
				}
			}
			UpdateButtonBackground(_isPressed);
		}

		private void OnDrag()
		{
			IsButtonPressed = UICamera.hoveredObject == base.gameObject;
			UpdateButtonBackground(IsButtonPressed);
		}

		private void UpdateButtonBackground(bool _pressed)
		{
			m_actionButtonBackground.spriteName = "hud_btn_toggle_" + ((!_pressed) ? "up" : "down");
		}
	}
}
