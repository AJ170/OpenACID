using UnityEngine;

namespace Assets.Scripts.UI.Localization
{
	[ExecuteInEditMode]
	public class UITryOpenUrlLocalizer : UIOpenUrlLocalizer
	{
		public UIOpenUrlLocalizer Fallback;

		public override void OnOpenUrl()
		{
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			string url = LocalizationManager.Instance.Get(base.Key);
			Application.OpenURL(url);
			if (!(Time.realtimeSinceStartup - realtimeSinceStartup > 0.5f) && Fallback != null)
			{
				Fallback.OnOpenUrl();
			}
		}
	}
}
