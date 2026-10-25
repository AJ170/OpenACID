using System;
using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions.Tweens;

namespace Assets.Scripts.UI.Loading
{
	[Serializable]
	public class LoadAnimationDefinition
	{
		public class DescendingOrder : IComparer<LoadAnimationDefinition>
		{
			public int Compare(LoadAnimationDefinition _a, LoadAnimationDefinition _b)
			{
				return (_a.ObjectMaxSize < _b.ObjectMaxSize) ? 1 : ((_a.ObjectMaxSize > _b.ObjectMaxSize) ? (-1) : 0);
			}
		}

		public UIWidget LoadAnimPrefab;

		public float ObjectMaxSize;

		public LoadAnimationDefinition()
		{
		}

		public LoadAnimationDefinition(UIWidget _object, float _size)
		{
			LoadAnimPrefab = _object;
			ObjectMaxSize = _size;
		}

		public void Show()
		{
			LoadAnimPrefab.GetComponent<BBTweenAlpha>().StartTweenPlayForward();
		}

		public void Hide()
		{
			if (!(LoadAnimPrefab == null) && !(LoadAnimPrefab.gameObject == null) && LoadAnimPrefab.gameObject.activeSelf)
			{
				BBTweenAlpha component = LoadAnimPrefab.GetComponent<BBTweenAlpha>();
				EventDelegate.Add(component.onFinished, OnHideFinish, true);
				component.StartTweenPlayReverse();
			}
		}

		private void OnHideFinish()
		{
			if (LoadAnimPrefab != null)
			{
				LoadAnimPrefab.gameObject.SetActive(false);
			}
		}
	}
}
