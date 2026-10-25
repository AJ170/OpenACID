using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Models.Assassin
{
	public class UIAssassinBaseModel : UIModelBase
	{
		public int SortId;

		public virtual AcierProtocol.Assassin Assassin { get; set; }

		public virtual string Title { get; set; }

		public virtual Color SelectedTextColor { get; set; }

		public virtual Color DeselectedTextColor { get; set; }

		public virtual string ClassName
		{
			get
			{
				if (Assassin != null)
				{
					return Assassin.GetAssassinClassDisplayNames();
				}
				return string.Empty;
			}
		}

		public virtual string AssassinName
		{
			get
			{
				if (Assassin != null)
				{
					return Assassin.GetAssassinName();
				}
				return string.Empty;
			}
		}

		public virtual AssassinSummary Summery
		{
			get
			{
				if (Assassin != null)
				{
					return Assassin.GetSummary();
				}
				return null;
			}
		}

		public virtual void GetAssasssinPortraitData(ref GameID _headId, ref GameID _humanoidId, ref GameID _colorId, ref GameID _itemId)
		{
			if (Assassin != null)
			{
				bool _transparentAssassin = false;
				Assassin.GetAssasssinPortraitData(out _headId, out _humanoidId, out _colorId, out _itemId, out _transparentAssassin);
			}
		}
	}
}
