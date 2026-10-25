using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class HotspotService : UbiCore.Utils.Singleton<HotspotService>
	{
		private readonly RooftopGardenManager m_RooftopGardenManager;

		private readonly BenchManager m_BenchManager;

		private readonly HaystackManager m_HaystackManager;

		private readonly CodexManager m_CodexManager;

		private readonly InteractableMissionObjectManager m_InteractableMissionObjectManager;

		private readonly OffMeshLinkManager m_OffmeshLinkManager;

		private readonly List<IHotspotManager> m_objectManagers;

		private readonly StalkingZoneManager m_StalkingZoneManager;

		private readonly SpotInspectionManager m_spotInspectionManager;

		public RooftopGardenManager RooftopGardenManager
		{
			get
			{
				return m_RooftopGardenManager;
			}
		}

		public BenchManager BenchManager
		{
			get
			{
				return m_BenchManager;
			}
		}

		public HaystackManager HaystackManager
		{
			get
			{
				return m_HaystackManager;
			}
		}

		public StalkingZoneManager StalkingZoneManager
		{
			get
			{
				return m_StalkingZoneManager;
			}
		}

		public CodexManager CodexManager
		{
			get
			{
				return m_CodexManager;
			}
		}

		public InteractableMissionObjectManager InteractableMissionObjectManager
		{
			get
			{
				return m_InteractableMissionObjectManager;
			}
		}

		public OffMeshLinkManager OffMeshLinkManager
		{
			get
			{
				return m_OffmeshLinkManager;
			}
		}

		public SpotInspectionManager SpotInspectionManager
		{
			get
			{
				return m_spotInspectionManager;
			}
		}

		public HotspotService()
		{
			Profiling.Start("HotspotService::HotspotService()", float.MaxValue);
			m_RooftopGardenManager = new RooftopGardenManager();
			m_BenchManager = new BenchManager();
			m_HaystackManager = new HaystackManager();
			m_CodexManager = new CodexManager();
			m_InteractableMissionObjectManager = new InteractableMissionObjectManager();
			m_OffmeshLinkManager = new OffMeshLinkManager();
			m_StalkingZoneManager = new StalkingZoneManager();
			m_objectManagers = new List<IHotspotManager> { m_HaystackManager, m_CodexManager, m_InteractableMissionObjectManager, m_OffmeshLinkManager, m_BenchManager, m_RooftopGardenManager, m_StalkingZoneManager };
			for (int i = 0; i < m_objectManagers.Count; i++)
			{
				Profiling.Start(m_objectManagers[i].GetType().ToString(), float.MaxValue);
				m_objectManagers[i].Initialize();
				Profiling.Stop();
			}
			m_spotInspectionManager = new SpotInspectionManager();
			Profiling.Stop();
		}

		public bool Interact(AssassinCharacter _assassin, GameObject _gameObject, NavMeshAgent _navMeshAgent)
		{
			for (int i = 0; i < m_objectManagers.Count; i++)
			{
				if (m_objectManagers[i].Interact(_assassin, _gameObject, _navMeshAgent))
				{
					return true;
				}
			}
			return false;
		}

		public bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			for (int i = 0; i < m_objectManagers.Count; i++)
			{
				if (m_objectManagers[i].LeaveInteraction(_assassin, _destination))
				{
					return true;
				}
			}
			return false;
		}

		public HideoutObject GetCurrentHideout()
		{
			HideoutObject current = RooftopGardenManager.Current;
			if (current == null)
			{
				current = BenchManager.Current;
				if (current == null)
				{
					current = HaystackManager.Current;
				}
			}
			return current;
		}
	}
}
