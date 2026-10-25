using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame.Implementation;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIMailsEvaluator : UINewsEvaluatorBase
	{
		private const string c_envelopeIconName = "aUI_icon_newMail";

		public override string DisplayIcon
		{
			get
			{
				return "aUI_icon_newMail";
			}
		}

		public override ENewsType Type
		{
			get
			{
				return ENewsType.Mail;
			}
		}

		public override void Init()
		{
			base.Init();
			Singleton<StateManager>.Instance.MailManager.OnMailsChanged += OnMailsUpdated;
		}

		public override void Destroy()
		{
			base.Destroy();
			Singleton<StateManager>.Instance.MailManager.OnMailsChanged -= OnMailsUpdated;
		}

		public override void Update()
		{
			if (Singleton<StateManager>.Instance.IsProfileManagerInitialized && Singleton<StateManager>.Instance.ProfileManager.Profile != null)
			{
				UITutorialManager tutorialManager = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager;
				if (!tutorialManager.IsBasicTutorialFinished && tutorialManager.CurrentCategory < tutorialManager.GetCategory(ETutorialCategory.TutCatGuideShop))
				{
					base.Value = false;
					return;
				}
				bool flag = Singleton<StateManager>.Instance.ProfileManager.Profile.dirtyFlags.IsBitSet(Convert.ToInt32(EDirtyFlag.DirtyMails));
				flag |= Singleton<StateManager>.Instance.MailManager.UnreadMails > 0;
				base.Value = flag;
			}
		}

		private void OnMailsUpdated(List<MailHeader> _headers)
		{
			Update();
		}
	}
}
