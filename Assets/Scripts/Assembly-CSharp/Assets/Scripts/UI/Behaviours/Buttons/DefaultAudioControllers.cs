using Assets.Scripts.Audio;
using Assets.Scripts.UI.ScreenManager;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Behaviours.Buttons
{
	public static class DefaultAudioControllers
	{
		public static GameID Transition
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_whoosh_Large;
			}
		}

		public static GameID Normal
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate;
			}
		}

		public static GameID Action
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate;
			}
		}

		public static GameID Ok
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_navigate;
			}
		}

		public static GameID Accept
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_accept;
			}
		}

		public static GameID Cancel
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel;
			}
		}

		public static GameID Close
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_backcancel;
			}
		}

		public static GameID Reset
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_decrypt_reset;
			}
		}

		public static GameID Buy
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_buy;
			}
		}

		public static GameID Equip
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_equip;
			}
		}

		public static GameID Unequip
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_unequip;
			}
		}

		public static GameID Claim
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_claim;
			}
		}

		public static GameID Decypt
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_decrypt;
			}
		}

		public static GameID Delete
		{
			get
			{
				return (!AudioManager.IsAvailable) ? GameID.Invalid : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_generic_delete;
			}
		}

		public static GameID Default(PopUpButtonConfiguration.EButtonType _buttonType)
		{
			if (!AudioManager.IsAvailable)
			{
				return GameID.Invalid;
			}
			switch (_buttonType)
			{
			case PopUpButtonConfiguration.EButtonType.eNormal:
				return Normal;
			default:
				return Normal;
			}
		}
	}
}
