using UnityEngine;

namespace Assets.Scripts.UI.NGUI_Extensions
{
	public class TweenCustom : UITweener
	{
		public delegate void OnCustomUpdateDelegate(float _factor);

		public OnCustomUpdateDelegate OnCustomUpdate;

		protected override void OnUpdate(float factor, bool isFinished)
		{
			if (!isFinished && OnCustomUpdate != null)
			{
				OnCustomUpdate(Mathf.Clamp01(factor));
			}
		}
	}
}
