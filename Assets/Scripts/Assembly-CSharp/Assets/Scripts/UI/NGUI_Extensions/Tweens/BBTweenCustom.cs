using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UnityEngine;

namespace Assets.Scripts.UI.NGUI_Extensions.Tweens
{
	public class BBTweenCustom : BBTargetedTween
	{
		public delegate void OnCustomUpdateDelegate(float _factor);

		public TweenCustom.OnCustomUpdateDelegate OnCustomUpdate;

		protected override void OnUpdate(float _factor, bool _isFinished)
		{
			if (!_isFinished && OnCustomUpdate != null)
			{
				OnCustomUpdate(Mathf.Clamp01(_factor));
			}
		}

		public override void SwapToAndFromValues()
		{
		}

		protected override void ClearCache()
		{
		}
	}
}
