using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	[ExecuteInEditMode]
	public class UIOpenUrlLocalizer : UILocalization
	{
		protected override void DoLocalize(StringId _key)
		{
		}

		public virtual void OnOpenUrl()
		{
			string url = LocalizationManager.Instance.Get(base.Key);
			Application.OpenURL(url);
		}
	}
}
