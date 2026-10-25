using System.Collections.Generic;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.ChooseSecondClass
{
	public class ChooseSecondClassContent : PopUpContentView<ChooseSecondClassContent.ChooseSecondClassData>
	{
		public class ChooseSecondClassData : PopUpContentData
		{
			public List<UIPopupResourcesContent.ResourceConfiguration> ResourcesCost;

			public string Description;

			public bool ResourceSymbolAdditive;

			public ClassDescription ClassDescription;

			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.ChooseSecondClass;
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
		private UIAlignmentBox m_vbox;

		[SerializeField]
		private UIExtendedLabel m_description;

		[SerializeField]
		private UIExtendedLabel m_headingLabel;

		[SerializeField]
		private UIPopupResourcesContent m_uiPopupResources;

		[SerializeField]
		private UISprite m_classSprite;

		private void Awake()
		{
		}

		protected override void Render(ChooseSecondClassData _data)
		{
			m_uiPopupResources.ConfigurateItemsCost(_data.ResourcesCost, _data.ResourceSymbolAdditive);
			m_headingLabel.text = LocalizationManager.Instance.Get(_data.ClassDescription.NameId);
			m_description.text = _data.Description;
			m_classSprite.spriteName = _data.ClassDescription.AvatarCreation;
			if (string.IsNullOrEmpty(m_headingLabel.text))
			{
				m_headingLabel.gameObject.SetActive(false);
			}
			if (string.IsNullOrEmpty(m_description.text))
			{
				m_description.gameObject.SetActive(false);
			}
		}

		public override void UpdateLayout()
		{
			OnIconsAligned();
		}

		private void OnIconsAligned()
		{
			EventDelegate.Add(m_vbox.OnReposition, OnAlignmentUpdate, true);
			m_vbox.RepositionNow = true;
		}

		private void OnAlignmentUpdate()
		{
			EventDelegate.Execute(onLayoutFinished);
		}
	}
}
