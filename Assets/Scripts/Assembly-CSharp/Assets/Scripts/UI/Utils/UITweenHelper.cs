using AnimationOrTween;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UITweenHelper : MonoBehaviour
	{
		public BBTweenAlpha RefTweener;

		public BBTweenAlpha Tweener;

		public bool IsTweening
		{
			get
			{
				return Tweener != null && Tweener.enabled;
			}
		}

		public void SetVisibility(bool _visible)
		{
			if (_visible && (!base.gameObject.activeInHierarchy || (Tweener != null && Tweener.direction != Direction.Forward)))
			{
				base.gameObject.SetActive(true);
				if (Tweener != null)
				{
					Tweener.SetTweenToBeginning();
					Tweener.PlayForward();
				}
			}
			else if (!_visible && base.gameObject.activeInHierarchy && Tweener != null && Tweener.direction != Direction.Reverse && Tweener != null)
			{
				EventDelegate.Add(Tweener.onFinished, OnTweenFinished, true);
				Tweener.SetTweenToEnd();
				Tweener.PlayReverse();
			}
		}

		private void Awake()
		{
			Init();
		}

		private void Init()
		{
			if (RefTweener == null)
			{
				UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
				if (uIRoot != null)
				{
					RefTweener = uIRoot.GetComponent<BBTweenAlpha>();
				}
			}
			if (RefTweener != null)
			{
				BBTweenAlpha bBTweenAlpha = Tweener;
				if (bBTweenAlpha == null)
				{
					bBTweenAlpha = base.gameObject.AddComponentIfNotExist<BBTweenAlpha>();
				}
				bBTweenAlpha.from = RefTweener.from;
				bBTweenAlpha.to = RefTweener.to;
				bBTweenAlpha.animationCurve = RefTweener.animationCurve;
				bBTweenAlpha.duration = RefTweener.duration;
				bBTweenAlpha.delay = RefTweener.delay;
				bBTweenAlpha.ignoreTimeScale = RefTweener.ignoreTimeScale;
				Tweener = bBTweenAlpha;
			}
		}

		private void OnTweenFinished()
		{
			if (Tweener.direction == Direction.Reverse)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
