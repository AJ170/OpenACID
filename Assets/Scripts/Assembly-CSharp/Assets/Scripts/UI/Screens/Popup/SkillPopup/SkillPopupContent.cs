using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.SkillPopup
{
	public class SkillPopupContent : PopUpContentView<SkillPopupContent.SkillPopupData>
	{
		public class SkillPopupData : PopUpContentData
		{
			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public SkillData[] SkillData;

			public string Description;

			public bool ResourceSymbolAdditive;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.Skills;
				}
			}

			public override bool HasResourceContentDisplay
			{
				get
				{
					return true;
				}
			}
		}

		[SerializeField]
		private UIAlignmentBox m_skillBox;

		[SerializeField]
		private UIAlignmentBox m_vbox;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UISkillPopupSkillWidget[] m_skillWidgets;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		private void Awake()
		{
		}

		protected override void Render(SkillPopupData _data)
		{
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
			ConfigureItem(_data);
			m_description.text = _data.Description;
		}

		public override void UpdateLayout()
		{
			EventDelegate.Add(m_skillBox.OnReposition, OnSkillsAligned, true);
			m_skillBox.RepositionNow = true;
		}

		public override void Purge()
		{
			base.Purge();
			if (m_vbox != null)
			{
				m_vbox.transform.ClearChildren();
			}
		}

		private void OnSkillsAligned()
		{
			if (m_vbox.gameObject.activeSelf)
			{
				EventDelegate.Add(m_vbox.OnReposition, OnAlignmentUpdate, true);
				m_vbox.RepositionNow = true;
			}
			else
			{
				OnAlignmentUpdate();
			}
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}

		private void ConfigureItem(SkillPopupData _config)
		{
			int i;
			for (i = 0; i < _config.SkillData.Length; i++)
			{
				m_skillWidgets[i].gameObject.SetActive(true);
				m_skillWidgets[i].Init(_config.SkillData[i], _config.SkillData.Length == 1);
			}
			for (; i < m_skillWidgets.Length; i++)
			{
				m_skillWidgets[i].gameObject.SetActive(false);
			}
		}
	}
}
