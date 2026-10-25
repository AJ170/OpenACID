using System;
using Assets.Scripts.UI.MVC.Models.Misc;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public abstract class UITabButtonHeaderController : UIControllerBase
	{
		public Action OnItemClicked { get; set; }

		public abstract void Initialize(UITabButtonModel _model);

		public abstract void UpdateController(UITabButtonModel _model);

		public abstract void DeselectRewardItem();
	}
}
