using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	[AddComponentMenu("NGUI/Extras/Image Localizer")]
	[RequireComponent(typeof(UISprite))]
	public class UIImageLocaliser : UILocalization
	{
		private UISprite m_targetSprite;

		private void Awake()
		{
			m_targetSprite = GetComponent<UISprite>();
		}

		protected override void DoLocalize(StringId _key)
		{
			m_targetSprite.spriteName = LocalizationManager.Instance.Get(_key);
			m_targetSprite.MakePixelPerfect();
		}
	}
}
