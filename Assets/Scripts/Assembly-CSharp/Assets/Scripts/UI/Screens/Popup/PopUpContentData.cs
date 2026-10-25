using UnityEngine;

namespace Assets.Scripts.UI.Screens.Popup
{
	public abstract class PopUpContentData
	{
		public static readonly Color RESOURCES_MISSING_BG_COLOR = new Color32(77, 1, 1, byte.MaxValue);

		public static readonly Color RESOURCES_COST_BG_COLOR = new Color32(78, 79, 82, byte.MaxValue);

		public abstract UIPopup.EContentType Type { get; }

		public virtual bool HasResourceContentDisplay
		{
			get
			{
				return false;
			}
		}
	}
}
