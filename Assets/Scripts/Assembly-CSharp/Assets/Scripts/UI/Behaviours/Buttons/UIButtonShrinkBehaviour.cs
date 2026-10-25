using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	[RequireComponent(typeof(UIExtendedButton))]
	public class UIButtonShrinkBehaviour : UIButtonTween
	{
		private static readonly Vector3 FROM = new Vector3(1f, 1f, 1f);

		private static readonly Vector3 TO = new Vector3(0.9f, 0.9f, 0.9f);

		protected override void DoUpdateConfig()
		{
			base.DoUpdateConfig();
			BBTweenScale tweenAs = GetTweenAs<BBTweenScale>();
			ButtonConfiguration value;
			if (tweenAs != null && UIDataMapper.ButtonConfigurations.TryGetValue((int)base.Button.Type, out value))
			{
				tweenAs.duration = value.TransitionDuration;
				tweenAs.from = FROM;
				tweenAs.to = TO;
			}
		}
	}
}
