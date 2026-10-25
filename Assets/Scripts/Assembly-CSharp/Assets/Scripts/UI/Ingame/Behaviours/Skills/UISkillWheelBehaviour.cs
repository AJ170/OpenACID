using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours.Skills
{
	public class UISkillWheelBehaviour : MonoBehaviour
	{
		public GameObject SkillWheelChildContainer;

		private UISkillButtonBehaviour m_currentSelected;

		private readonly List<UISkillButtonBehaviour> m_children = new List<UISkillButtonBehaviour>();

		public List<UISkillButtonBehaviour> Children
		{
			get
			{
				if (m_children.Count == 0)
				{
					Transform transform = SkillWheelChildContainer.transform;
					for (int i = 0; i < transform.childCount; i++)
					{
						UISkillButtonBehaviour component = SkillWheelChildContainer.transform.GetChild(i).GetComponent<UISkillButtonBehaviour>();
						m_children.Add(component);
					}
					m_children.Sort(SortByDirection);
				}
				return m_children;
			}
		}

		public UISkillButtonBehaviour CurrentSelected
		{
			get
			{
				return m_currentSelected;
			}
			set
			{
				if (!(m_currentSelected == value))
				{
					if (m_currentSelected != null)
					{
						m_currentSelected.Deselect();
					}
					m_currentSelected = value;
					if (m_currentSelected != null)
					{
						m_currentSelected.Select();
					}
				}
			}
		}

		public bool IsWheelOpen
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		protected static int SortByDirection(UISkillButtonBehaviour _a, UISkillButtonBehaviour _b)
		{
			return _a.Direction.CompareTo(_b.Direction);
		}

		public void Init()
		{
			if (CurrentSelected != null)
			{
				CurrentSelected.Deselect();
			}
			CurrentSelected = null;
		}

		public void Show()
		{
			if (!base.gameObject.activeSelf && UILoadingScreen.Instance.IsHidden)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
			}
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			if (base.gameObject.activeSelf && UILoadingScreen.Instance.IsHidden)
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate);
			}
			base.gameObject.SetActive(false);
		}

		public UISkillButtonBehaviour GetSkillButton(Enum<SkillTypes> _skillType)
		{
			UISkillButtonBehaviour result = null;
			for (int i = 0; i < Children.Count; i++)
			{
				UISkillButtonBehaviour uISkillButtonBehaviour = Children[i];
				if (_skillType == uISkillButtonBehaviour.SkillType)
				{
					result = uISkillButtonBehaviour;
					break;
				}
			}
			return result;
		}

		public UISkillButtonBehaviour GetSkillButtonByDirection(UISkillButtonBehaviour.EDirection _direction)
		{
			UISkillButtonBehaviour result = null;
			for (int i = 0; i < Children.Count; i++)
			{
				UISkillButtonBehaviour uISkillButtonBehaviour = Children[i];
				if (uISkillButtonBehaviour.Direction == _direction)
				{
					result = uISkillButtonBehaviour;
					break;
				}
			}
			return result;
		}

		public void UpdateSkillSelection(UISkillButtonBehaviour.EDirection _direction, bool _deselectAll)
		{
			if (_deselectAll)
			{
				CurrentSelected = null;
			}
			for (int i = 0; i < Children.Count; i++)
			{
				UISkillButtonBehaviour uISkillButtonBehaviour = Children[i];
				if (uISkillButtonBehaviour.Direction == _direction && !_deselectAll)
				{
					CurrentSelected = uISkillButtonBehaviour;
				}
				else
				{
					uISkillButtonBehaviour.Deselect();
				}
			}
		}

		public void DeselectAllSkills()
		{
			CurrentSelected = null;
			for (int i = 0; i < Children.Count; i++)
			{
				UISkillButtonBehaviour uISkillButtonBehaviour = Children[i];
				uISkillButtonBehaviour.Deselect();
			}
		}
	}
}
