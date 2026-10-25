using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.SkillTreeScreen
{
	public class IconHandler : MonoBehaviour
	{
		public List<EventDelegate> onSelected = new List<EventDelegate>();

		public List<EventDelegate> onDeSelected = new List<EventDelegate>();

		[SerializeField]
		private UISprite m_BackgroundNormal;

		[SerializeField]
		private UISprite m_BackgroundFull;

		[SerializeField]
		private UISprite m_Highlight;

		[SerializeField]
		private UITexture m_icon;

		[SerializeField]
		private UISprite m_Glow;

		[SerializeField]
		private UIToggle m_Toggle;

		[SerializeField]
		private UIWidget m_colliderWidget;

		private SkillTreeData m_data;

		private int m_tierIndex;

		private AssassinSkillInTraining.SkillType m_type;

		private Assets.Scripts.UI.Utils.Extensions.SkillData m_skillData;

		private bool m_updateTexturesOnUpdate;

		public bool Visible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
			set
			{
				base.gameObject.SetActive(value);
			}
		}

		public bool Highlighted
		{
			get
			{
				return m_Glow.gameObject.activeSelf;
			}
			set
			{
				m_Glow.gameObject.SetActive(value);
			}
		}

		public void Init(SkillTreeData _data, int _tierIndex, AssassinSkillInTraining.SkillType _type)
		{
			m_data = _data;
			m_tierIndex = _tierIndex;
			m_type = _type;
			m_colliderWidget.ResizeCollider();
		}

		public void UpdateHandler(Assets.Scripts.UI.Utils.Extensions.SkillData _skillData = null)
		{
			DisableAll();
			m_skillData = _skillData;
			if (m_skillData == null)
			{
				m_skillData = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
			}
			if (m_skillData != null)
			{
				m_updateTexturesOnUpdate = true;
				m_Highlight.gameObject.SetActive(!m_skillData.Locked && m_skillData.Level < 5);
				m_BackgroundFull.gameObject.SetActive(m_skillData.Level == 5);
				m_BackgroundNormal.gameObject.SetActive(m_skillData.Level < 5 && m_skillData.Locked);
				m_icon.gameObject.SetActive(false);
				m_Glow.gameObject.SetActive(m_data.SelectedSkill == m_skillData.SkillId);
			}
		}

		private void Awake()
		{
			DisableAll();
		}

		private void OnDestroy()
		{
			m_data = null;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_Toggle.onChange, OnToggleChange);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_Toggle.onChange, OnToggleChange);
			DisableAll();
		}

		private void DisableAll()
		{
			if (m_Toggle != null)
			{
				m_Toggle.optionCanBeNone = true;
				m_Toggle.value = false;
				m_Toggle.optionCanBeNone = false;
			}
			if (m_BackgroundNormal != null)
			{
				m_BackgroundNormal.gameObject.SetActive(false);
			}
			if (m_BackgroundFull != null)
			{
				m_BackgroundFull.gameObject.SetActive(false);
			}
			if (m_Highlight != null)
			{
				m_Highlight.gameObject.SetActive(false);
			}
			if (m_icon != null)
			{
				m_icon.gameObject.SetActive(false);
			}
			if (m_Glow != null)
			{
				m_Glow.gameObject.SetActive(false);
			}
		}

		private void Update()
		{
			if (m_updateTexturesOnUpdate)
			{
				m_updateTexturesOnUpdate = false;
				GameDB gameDB = Main.Instance.GameDB;
				Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition item = gameDB.GetItem<Assets.Scripts.Mission.Definitions.SkillData.SkillDefinition>(gameDB.FindGameIdFromGuid(m_skillData.SkillId));
				m_icon.mainTexture = item.IconUp.AccessObject<Texture2D>();
				m_icon.gameObject.SetActive(true);
			}
		}

		private void OnToggleChange()
		{
			Highlighted = m_Toggle.value;
			EventDelegate.Execute((!m_Toggle.value) ? onDeSelected : onSelected);
			if (m_Toggle.value)
			{
				Assets.Scripts.UI.Utils.Extensions.SkillData skill = m_data.GetSkill(m_data.PrimarySelected, m_tierIndex, m_type);
				m_data.SelectedSkill = skill.SkillId;
			}
		}
	}
}
