using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Screens.Popup;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialPopupBase : TutorialFlowBase, IPopupTexts
	{
		[SerializeField]
		protected UISlantedPopup m_textPopup;

		public abstract int GetHeaderText();

		public abstract int GetDescriptionText();

		public virtual string GetDescription()
		{
			return null;
		}

		protected virtual int GetButtonText()
		{
			return -1;
		}

		protected override void OnSetupInternal()
		{
			base.OnSetupInternal();
			if (m_textPopup != null)
			{
				m_textPopup.gameObject.SetActive(true);
				m_textPopup.SetHeaderTextLocalized(GetHeaderText());
				int descriptionText = GetDescriptionText();
				if (descriptionText == StringId.Invalid)
				{
					string description = GetDescription();
					m_textPopup.SetDescriptionText(description);
				}
				else
				{
					m_textPopup.SetDescriptionTextLocalized(descriptionText);
				}
				int buttonText = GetButtonText();
				if (buttonText > 0)
				{
					m_textPopup.SetButtonText(LocalizationManager.Instance.Get(buttonText));
				}
				m_textPopup.MoveIn();
			}
			ActivateBackButton(false);
		}

		public override void Deactivate()
		{
			if (m_textPopup != null)
			{
				m_textPopup.MoveOutInstant();
			}
			base.Deactivate();
		}
	}
}
