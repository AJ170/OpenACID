using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.Misc
{
	public class UICheckboxBehaviour : MonoBehaviour
	{
		public enum ECheckedState
		{
			None = 0,
			Fail = 1,
			Success = 2,
			Dot = 3
		}

		public UILabel Text;

		public UISprite Frame;

		public UISprite Checkbox;

		[SerializeField]
		private UnityEngine.Animation m_checkboxAnimation;

		private ECheckedState m_checkedState;

		public UnityEngine.Animation CheckboxAnimation
		{
			get
			{
				return m_checkboxAnimation;
			}
		}

		public ECheckedState Checked
		{
			get
			{
				return m_checkedState;
			}
			set
			{
				m_checkedState = value;
				string text = string.Empty;
				string spriteName = "hud_Checkbox_empty";
				switch (m_checkedState)
				{
				case ECheckedState.Fail:
					text = "hud_Checkbox_false";
					break;
				case ECheckedState.Success:
					text = "hud_Checkbox_true";
					break;
				case ECheckedState.Dot:
					text = string.Empty;
					spriteName = "missiontrack_modifier_dot";
					break;
				}
				Checkbox.gameObject.SetActive(!string.IsNullOrEmpty(text));
				Checkbox.spriteName = text;
				Frame.spriteName = spriteName;
			}
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(true);
		}

		public void SetData(string _text, ECheckedState _checkedState)
		{
			Text.text = _text;
			Checked = _checkedState;
		}

		private void Awake()
		{
		}
	}
}
