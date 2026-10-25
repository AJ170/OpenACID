using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIMysteryBoxEvaluator : UINewsEvaluatorBase
	{
		private const string c_mysteryBoxIconName = "aUI_icon_mysteryChest";

		public override ENewsType Type
		{
			get
			{
				return ENewsType.MysteryBox;
			}
		}

		public override string DisplayIcon
		{
			get
			{
				return "aUI_icon_mysteryChest";
			}
		}

		public override void Update()
		{
			OnLocalStorageUpdate();
		}

		private void OnLocalStorageUpdate()
		{
			base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && Singleton<StateManager>.Instance.ProfileManager.Profile.IsMysteryBoxOpen();
		}
	}
}
