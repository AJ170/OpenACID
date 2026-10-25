using AcierProtocol;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.State.ThemeManager;

namespace Assets.Scripts.UI.State.EventManager
{
	public class EventFlavorThemeManager : EventManagerBase
	{
		public override EEventType ManagerType
		{
			get
			{
				return EEventType.Event_Flavor;
			}
		}

		public override void ProcessEvent(Event _event)
		{
			EventFlavorTheme eventFlavorTheme = new EventFlavorTheme(_event);
			bool flag = HasInOldEvents(eventFlavorTheme);
			m_hasChanged |= !flag;
			m_Events.Add(eventFlavorTheme);
		}

		public override void Update()
		{
			bool flag = false;
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventFlavorTheme eventFlavorTheme = (EventFlavorTheme)m_Events[i];
				bool lastValidState = eventFlavorTheme.LastValidState;
				if (lastValidState != eventFlavorTheme.Validate())
				{
					flag = true;
				}
			}
			if (flag)
			{
				OnChanged.Trigger(this);
			}
		}

		public EventFlavorTheme GetCategoryVisibilityEvent(ThemesManager.EFlavorTheme _flavorTheme)
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventFlavorTheme eventFlavorTheme = (EventFlavorTheme)m_Events[i];
				if (eventFlavorTheme.FlavorThemeType == _flavorTheme && eventFlavorTheme.Validate())
				{
					return eventFlavorTheme;
				}
			}
			return null;
		}

		public bool IsThemeEnabled(ThemesManager.EFlavorTheme _flavorTheme)
		{
			for (int i = 0; i < m_Events.Count; i++)
			{
				EventFlavorTheme eventFlavorTheme = (EventFlavorTheme)m_Events[i];
				if (eventFlavorTheme.FlavorThemeType == _flavorTheme && eventFlavorTheme.Validate())
				{
					return true;
				}
			}
			return false;
		}
	}
}
