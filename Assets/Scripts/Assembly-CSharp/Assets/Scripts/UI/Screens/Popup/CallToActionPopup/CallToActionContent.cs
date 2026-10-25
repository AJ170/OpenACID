using System.Collections;
using Assets.Scripts.Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup.CallToActionPopup
{
	public class CallToActionContent : PopUpContentView<CallToActionContent.CallToActionData>
	{
		public class CallToActionData : PopUpContentData
		{
			public override UIPopup.EContentType Type
			{
				get
				{
					return UIPopup.EContentType.CallToAction;
				}
			}

			public CTADefinitionBase Definition { get; set; }

			public IEnumerator BackgroundHandle { get; set; }
		}

		[SerializeField]
		private UIExtendedLabel m_questionLabel;

		[SerializeField]
		private UITexture m_background;

		protected override void Render(CallToActionData _data)
		{
			CTADefinitionBase definition = _data.Definition;
			string resourcePath = definition.Image.GetResourcePath();
			bool flag = !string.IsNullOrEmpty(resourcePath);
			m_background.gameObject.SetActive(flag);
			m_background.mainTexture = ((!flag) ? null : definition.Image.AccessObject<Texture2D>());
			m_background.color = Color.white;
			m_questionLabel.SetLocalizationKey(definition.BodyLocaKey.Key, definition.GetBodyMapper());
		}

		private void Awake()
		{
		}
	}
}
