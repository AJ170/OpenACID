using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Notifications;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using SharedBaseLib.Core;
using UbiCore.Utils;

public class UIShopEvaluator : UINewsEvaluatorBase
{
	public override ENewsType Type
	{
		get
		{
			return ENewsType.Shop;
		}
	}

	public override void Init()
	{
		base.Init();
		Singleton<StateManager>.Instance.EventManager.OnChanged += new SimpleDelegate.CallbackFunction(OnEventManagerChanged);
	}

	private void OnEventManagerChanged(object[] _params)
	{
		EventManagerBase eventManagerBase = (EventManagerBase)_params[0];
		LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
		EEventType managerType = eventManagerBase.ManagerType;
		ReadOnlyCollection<EventBase> events = eventManagerBase.Events;
		for (int i = 0; i < events.Count; i++)
		{
			EventBase eventBase = events[i];
			if (eventBase.LastValidState && !instance.SHOPGetValue(managerType, eventBase.CRC32))
			{
				base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished;
				break;
			}
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		Singleton<StateManager>.Instance.EventManager.OnChanged -= new SimpleDelegate.CallbackFunction(OnEventManagerChanged);
	}

	public override void Update()
	{
		LocalStorage instance = UbiCore.Utils.Singleton<LocalStorage>.Instance;
		Array values = Enum.GetValues(typeof(EEventType));
		EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
		bool flag = false;
		for (int i = 0; i < values.Length; i++)
		{
			if (flag)
			{
				break;
			}
			EEventType eEventType = (EEventType)(int)values.GetValue(i);
			if (!EventManager.IsShopEventType(eEventType))
			{
				continue;
			}
			EventManagerBase eventManger = eventManager.GetEventManger(eEventType);
			if (eventManger == null)
			{
				continue;
			}
			ReadOnlyCollection<EventBase> events = eventManger.Events;
			for (int j = 0; j < events.Count; j++)
			{
				EventBase eventBase = events[j];
				if (eventBase.LastValidState && !instance.SHOPGetValue(eEventType, eventBase.CRC32))
				{
					flag = true;
					break;
				}
			}
		}
		base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && flag;
	}
}
