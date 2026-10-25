using AcierProtocol;
using Assets.Scripts.Achievements;
using Assets.Scripts.UI.Screens.NewsFeedScreen.Data;
using Assets.Scripts.UI.State.AssassinSelection;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.MailManager;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.State.ProfileManager;
using Assets.Scripts.UI.State.ThemeManager;
using Assets.Scripts.UI.Utils;

namespace Assets.Scripts.UI.State
{
	public class StateManager : Singleton<StateManager>
	{
		private AchievementsManager m_achievementsManager;

		private MissionSelectionManager m_missionSelectionManager;

		private AssassinCreationSelection m_creationSelection;

		public MysteryBoxChain CurrentChain;

		private Assets.Scripts.UI.State.ProfileManager.ProfileManager m_profileManager;

		private ItemNotificationManager m_itemNotificationManager;

		private ModuleNotificationManager m_moduleNotificationManager;

		private TemplateManager m_templateManager;

		private ServerTimeManager m_serverTimeManager;

		private Assets.Scripts.UI.State.MailManager.MailManager m_mailManager;

		private NewsFeedManager m_newsFeedManager;

		private Assets.Scripts.UI.State.EventManager.EventManager m_eventManager;

		private PersistentStateManager m_persistantStateManager;

		private CodexManager m_codexManager;

		private ThemesManager m_themesManager;

		public AchievementsManager AchievementsManager
		{
			get
			{
				return m_achievementsManager ?? (m_achievementsManager = new AchievementsManager());
			}
		}

		public MissionSelectionManager MissionSelectionManager
		{
			get
			{
				return m_missionSelectionManager ?? (m_missionSelectionManager = new MissionSelectionManager());
			}
		}

		public AssassinCreationSelection AssassinCreationSelection
		{
			get
			{
				return m_creationSelection ?? (m_creationSelection = new AssassinCreationSelection());
			}
		}

		public bool IsProfileManagerInitialized
		{
			get
			{
				return m_profileManager != null;
			}
		}

		public Assets.Scripts.UI.State.ProfileManager.ProfileManager ProfileManager
		{
			get
			{
				if (m_profileManager == null)
				{
					m_profileManager = new Assets.Scripts.UI.State.ProfileManager.ProfileManager();
					AchievementsManager.InitEvents();
				}
				return m_profileManager;
			}
		}

		public ItemNotificationManager ItemNotificationManager
		{
			get
			{
				return m_itemNotificationManager ?? (m_itemNotificationManager = new ItemNotificationManager());
			}
		}

		public ModuleNotificationManager ModuleNotificationManager
		{
			get
			{
				return m_moduleNotificationManager ?? (m_moduleNotificationManager = new ModuleNotificationManager());
			}
		}

		public TemplateManager TemplateManager
		{
			get
			{
				return m_templateManager ?? (m_templateManager = new TemplateManager());
			}
		}

		public ServerTimeManager ServerTimeManager
		{
			get
			{
				return m_serverTimeManager ?? (m_serverTimeManager = new ServerTimeManager());
			}
		}

		public Assets.Scripts.UI.State.MailManager.MailManager MailManager
		{
			get
			{
				return m_mailManager ?? (m_mailManager = new Assets.Scripts.UI.State.MailManager.MailManager());
			}
		}

		public NewsFeedManager NewsFeedManager
		{
			get
			{
				return m_newsFeedManager ?? (m_newsFeedManager = new NewsFeedManager());
			}
		}

		public Assets.Scripts.UI.State.EventManager.EventManager EventManager
		{
			get
			{
				return m_eventManager ?? (m_eventManager = new Assets.Scripts.UI.State.EventManager.EventManager());
			}
		}

		public PersistentStateManager PersistentStateManager
		{
			get
			{
				return m_persistantStateManager ?? (m_persistantStateManager = new PersistentStateManager());
			}
		}

		public CodexManager CodexManager
		{
			get
			{
				return m_codexManager ?? (m_codexManager = new CodexManager());
			}
		}

		public ThemesManager ThemesManager
		{
			get
			{
				return m_themesManager ?? (m_themesManager = new ThemesManager());
			}
		}
	}
}
