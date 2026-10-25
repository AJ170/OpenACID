using UnityEngine;

namespace Assets.Scripts.UI.NGUIExtensions.Tweens
{
	[AddComponentMenu("NGUI/BB/Tween/Targeted Tween Transform")]
	public class BBTweenTransform : BBTargetedTween
	{
		public Transform from;

		public Transform to;

		public bool parentWhenFinished;

		private Transform mTrans;

		private Vector3 mPos;

		private Quaternion mRot;

		private Vector3 mScale;

		public override void SwapToAndFromValues()
		{
			Transform transform = from;
			from = to;
			to = transform;
		}

		protected override void OnUpdate(float factor, bool isFinished)
		{
			if (to != null)
			{
				if (mTrans == null)
				{
					mTrans = base.Target.transform;
					mPos = mTrans.position;
					mRot = mTrans.rotation;
					mScale = mTrans.localScale;
				}
				if (from != null)
				{
					mTrans.position = from.position * (1f - factor) + to.position * factor;
					mTrans.localScale = from.localScale * (1f - factor) + to.localScale * factor;
					mTrans.rotation = Quaternion.Slerp(from.rotation, to.rotation, factor);
				}
				else
				{
					mTrans.position = mPos * (1f - factor) + to.position * factor;
					mTrans.localScale = mScale * (1f - factor) + to.localScale * factor;
					mTrans.rotation = Quaternion.Slerp(mRot, to.rotation, factor);
				}
				if (parentWhenFinished && isFinished)
				{
					mTrans.parent = to;
				}
			}
		}

		public static BBTweenTransform Begin(GameObject go, float duration, Transform to)
		{
			return Begin(go, duration, null, to);
		}

		public static BBTweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
		{
			BBTweenTransform bBTweenTransform = UITweener.Begin<BBTweenTransform>(go, duration);
			bBTweenTransform.from = from;
			bBTweenTransform.to = to;
			if (duration <= 0f)
			{
				bBTweenTransform.Sample(1f, true);
				bBTweenTransform.enabled = false;
			}
			return bBTweenTransform;
		}

		protected override void ClearCache()
		{
			mTrans = null;
		}
	}
}
