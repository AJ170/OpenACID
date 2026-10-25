using System.Collections.Generic;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.Skills
{
	public class UISkillWheelControllerBehaviour : MonoBehaviour
	{
		private const string c_spriteButtonNamePrefix = "hud_btn_toggle_";

		public UIWidget ControllerWidget;

		public UISprite ControllerIcon;

		public UISprite ControllerBackground;

		public GameObject SkillIndicatorsContainer;

		public UISkillWheelBehaviour SkillWheelBehaviour;

		private readonly List<UISkillIndicatorBehaviour> m_children = new List<UISkillIndicatorBehaviour>();

		private bool m_isVisible;

		public List<UISkillIndicatorBehaviour> Children
		{
			get
			{
				if (m_children.Count == 0)
				{
					Transform transform = SkillIndicatorsContainer.transform;
					for (int i = 0; i < transform.childCount; i++)
					{
						UISkillIndicatorBehaviour component = SkillIndicatorsContainer.transform.GetChild(i).GetComponent<UISkillIndicatorBehaviour>();
						m_children.Add(component);
					}
				}
				return m_children;
			}
		}

		public bool IsVisible
		{
			get
			{
				return m_isVisible;
			}
			set
			{
				m_isVisible = value;
				for (int i = 0; i < m_children.Count; i++)
				{
					m_children[i].IsVisible = value;
				}
			}
		}

		public void OnUIAlignmentChanged(bool _isRightHanded)
		{
			UIUtil.ToggleLeftAndRightAnchorPoints(ControllerWidget);
			Transform transform = ControllerBackground.transform;
			Vector3 localPosition = transform.localPosition;
			localPosition.x *= -1f;
			transform.localPosition = localPosition;
			Quaternion localRotation = transform.localRotation;
			localRotation.z *= -1f;
			transform.localRotation = localRotation;
			ControllerWidget.ResizeCollider();
		}

		public void Init()
		{
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

		public void UpdateIndicators()
		{
			for (int i = 0; i < Children.Count; i++)
			{
				UISkillIndicatorBehaviour uISkillIndicatorBehaviour = Children[i];
				UISkillButtonBehaviour skillButtonByDirection = SkillWheelBehaviour.GetSkillButtonByDirection(uISkillIndicatorBehaviour.Direction);
				if (skillButtonByDirection != null)
				{
					uISkillIndicatorBehaviour.UpdateStatus(skillButtonByDirection);
				}
			}
		}

		public void UpdateOpenButton(bool _pressed)
		{
			string text = ((!_pressed) ? "up" : "down");
			ControllerIcon.spriteName = "hud_btn_toggle_" + text;
		}
	}
}
