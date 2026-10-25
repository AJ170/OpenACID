using UnityEngine;

namespace Assets.Scripts.UI.Interaction
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(UIPanel))]
	[AddComponentMenu("NGUI/BB/Interaction/Wrap Scroll View")]
	public class UIWrapScrollView : UIScrollView
	{
		public IExtendedWrapContent ExtendedWraperContent { get; set; }

		public override Bounds bounds
		{
			get
			{
				if (!mCalculatedBounds)
				{
					mCalculatedBounds = true;
					mTrans = base.transform;
					if (ExtendedWraperContent != null)
					{
						mBounds = ExtendedWraperContent.Bounds;
					}
				}
				return mBounds;
			}
		}
	}
}
