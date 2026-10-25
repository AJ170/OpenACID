using Assets.Scripts.Audio;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.MVC.Controllers.Banner
{
	public abstract class UIBannerBaseController : UIControllerBase
	{
		public virtual EBannerType LayoutType { get; set; }

		public virtual bool IsAllowedToBeTapped { get; set; }

		public abstract void UpdateBannerData(EventBase _event);

		public virtual void InstantShowItem()
		{
		}

		public virtual void OnNavigateToScreen()
		{
			AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_accept);
		}
	}
}
