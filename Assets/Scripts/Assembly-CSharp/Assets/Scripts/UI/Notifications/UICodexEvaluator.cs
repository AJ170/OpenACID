using System;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Database;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UICodexEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Codex;
			}
		}

		public override void Init()
		{
			base.Init();
			UbiCore.Utils.Singleton<LocalStorage>.Instance.OnCodexEntryUnlocked += OnCodexUpdate;
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			instance.OnCodexEntryRead = (LocalStorage.EntryRead)Delegate.Combine(instance.OnCodexEntryRead, new LocalStorage.EntryRead(OnCodexUpdate));
		}

		public override void Destroy()
		{
			base.Destroy();
			UbiCore.Utils.Singleton<LocalStorage>.Instance.OnCodexEntryUnlocked -= OnCodexUpdate;
			LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
			instance.OnCodexEntryRead = (LocalStorage.EntryRead)Delegate.Remove(instance.OnCodexEntryRead, new LocalStorage.EntryRead(OnCodexUpdate));
		}

		public override void Update()
		{
			OnCodexUpdate(GameID.Invalid);
		}

		private void OnCodexUpdate(GameID _id)
		{
			base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnreadEntries;
		}
	}
}
