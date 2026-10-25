using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Color")]
	public class BBTweenColor : BBTargetedTween
	{
		public Color from = Color.white;

		public Color to = Color.white;

		private bool mCached;

		private UIWidget mWidget;

		private Material mMat;

		private Light mLight;

		private SpriteRenderer mSr;

		public Color value
		{
			get
			{
				if (!mCached)
				{
					Cache();
				}
				if (mWidget != null)
				{
					return mWidget.color;
				}
				if (mMat != null)
				{
					return mMat.color;
				}
				if (mSr != null)
				{
					return mSr.color;
				}
				if (mLight != null)
				{
					return mLight.color;
				}
				return Color.black;
			}
			set
			{
				if (!mCached)
				{
					Cache();
				}
				if (mWidget != null)
				{
					mWidget.color = value;
				}
				else if (mMat != null)
				{
					mMat.color = value;
				}
				else if (mSr != null)
				{
					mSr.color = value;
				}
				else if (mLight != null)
				{
					mLight.color = value;
					mLight.enabled = value.r + value.g + value.b > 0.01f;
				}
			}
		}

		private void Cache()
		{
			mCached = true;
			mWidget = base.Target.GetComponent<UIWidget>();
			if (mWidget != null)
			{
				return;
			}
			mSr = base.Target.GetComponent<SpriteRenderer>();
			if (mSr != null)
			{
				return;
			}
			Renderer component = base.Target.GetComponent<Renderer>();
			if (component != null)
			{
				mMat = component.material;
				return;
			}
			mLight = base.Target.GetComponent<Light>();
			if (mLight == null)
			{
				mWidget = base.Target.GetComponentInChildren<UIWidget>();
			}
		}

		public override void SwapToAndFromValues()
		{
			Color color = from;
			from = to;
			to = color;
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			value = Color.Lerp(from, to, factor);
		}

		protected override void ClearCache()
		{
			mCached = false;
			mWidget = null;
			mMat = null;
			mLight = null;
			mSr = null;
		}

		public static BBTweenColor Begin(GameObject go, float duration, Color color)
		{
			BBTweenColor bBTweenColor = UITweener.Begin<BBTweenColor>(go, duration);
			bBTweenColor.from = bBTweenColor.value;
			bBTweenColor.to = color;
			if (duration <= 0f)
			{
				bBTweenColor.Sample(1f, true);
				bBTweenColor.enabled = false;
			}
			return bBTweenColor;
		}

		[ContextMenu("Set 'From' to current value")]
		public override void SetStartToCurrentValue()
		{
			from = value;
		}

		[ContextMenu("Set 'To' to current value")]
		public override void SetEndToCurrentValue()
		{
			to = value;
		}

		[ContextMenu("Assume value of 'From'")]
		private void SetCurrentValueToStart()
		{
			value = from;
		}

		[ContextMenu("Assume value of 'To'")]
		private void SetCurrentValueToEnd()
		{
			value = to;
		}
	}
}
