using AcierProtocol;
using Assets.Scripts.Crowd;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.CharacterPooling;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using Assets.Scripts.Mission.Selection;
using Assets.Scripts.Network.Missions;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

public static class LogicMain
{
	public const float TurnDuration = 0.1f;

	public const int TurnsPerSecond = 10;

	private static readonly GameLogic m_gameLogic = new GameLogic();

	private static TurnHandler m_turnHandler = new TurnHandler(0.1f);

	private static EntityManager m_entityManager = new EntityManager();

	private static LogicExecutor m_logicExecutor = new LogicExecutor();

	private static CombatController m_CombatController;

	private static bool m_IsLoading;

	private static GlobalTimeScale m_globalTimeScale = new GlobalTimeScale();

	public static float GlobalTimeScale
	{
		get
		{
			return m_globalTimeScale.TimeScale;
		}
		set
		{
			m_globalTimeScale.TimeScale = value;
		}
	}

	public static bool IsRunning { get; private set; }

	public static EMissionType MissionType { get; private set; }

	public static Assets.Scripts.Mission.MissionInfo MissionInfo { get; private set; }

	public static int CurrentTurn
	{
		get
		{
			return m_turnHandler.CurrentTurn;
		}
	}

	public static float LastTurnTime
	{
		get
		{
			return m_turnHandler.LastTurnTime;
		}
	}

	public static float CurrentTime
	{
		get
		{
			return UbiCore.Utils.Singleton<Timer>.Instance.time;
		}
	}

	public static float FrameTime
	{
		get
		{
			return UbiCore.Utils.Singleton<Timer>.Instance.deltaTime;
		}
	}

	public static bool IsInUpdate
	{
		get
		{
			return m_turnHandler.IsInUpdate || m_IsLoading;
		}
	}

	public static bool IsInTurnUpdate
	{
		get
		{
			return m_turnHandler.IsInTurnUpdate || m_IsLoading;
		}
	}

	public static GameObject Player { get; private set; }

	public static PlayerCharacter PlayerEntity { get; private set; }

	public static GameObject Hireling { get; private set; }

	public static EntityManager EntityManager
	{
		get
		{
			return m_entityManager;
		}
	}

	public static GameStats GameStats
	{
		get
		{
			return m_gameLogic.GameStats;
		}
	}

	public static GameData.IGameDefinition GameDefinition
	{
		get
		{
			return m_gameLogic.GameDefinition.Definition;
		}
	}

	public static Assets.Scripts.Mission.Logic.Components.MissionResult MissionResult
	{
		get
		{
			return m_gameLogic.MissionResult;
		}
	}

	public static ICharacterManager CharacterManager
	{
		get
		{
			return UbiCore.Utils.Singleton<MissionManager>.Instance.CharacterManager;
		}
	}

	public static LogicInterface LogicInterface
	{
		get
		{
			return m_gameLogic.LogicInterface;
		}
	}

	public static EntityTracking EntityTracking
	{
		get
		{
			return m_gameLogic.EntityTracking;
		}
	}

	public static LogicExecutor LogicExecutor
	{
		get
		{
			return m_logicExecutor;
		}
	}

	public static Random32 Random { get; private set; }

	public static void OnMissionLoad(EMissionType _missionType)
	{
		if (Assets.Scripts.Engine.Helpers.Verify(!IsRunning, "Calling OnMissionLoad although logic is already running! Tell the mission logic departement!"))
		{
			Profiling.Start("LogicMain::OnMissionLoad", float.MaxValue);
			Singleton<EffectManager>.Instance.BeginUsingEffects();
			Profiling.Start("AvatarData", float.MaxValue);
			UbiCore.Utils.Singleton<AvatarData>.Instance.PrepareMission();
			Profiling.Stop();
			MissionType = _missionType;
			MissionInfo = new Assets.Scripts.Mission.MissionInfo();
			Random = new Random32((uint)UbiCore.Utils.Singleton<Timer>.Instance.realTime);
			EntityManager.OnMissionLoad();
			m_gameLogic.GameDefinition.Definition = UbiCore.Utils.Singleton<GameData>.Instance.Definition;
			EntityManager.RegisterEntity(m_gameLogic);
			Profiling.Start("CombatController", float.MaxValue);
			CombatController.Initialize();
			m_CombatController = CombatController.Instance;
			Profiling.Stop();
			Profiling.Start("SelectionManager", float.MaxValue);
			UbiCore.Utils.Singleton<SelectionManager>.Initialize();
			Profiling.Stop();
			Profiling.Start("Hireling", float.MaxValue);
			AvatarData.IAvatarDefinition hirelingDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetHirelingDefinition();
			if (hirelingDefinition != null)
			{
				Hireling = SpawnHelper.CreateHireling(hirelingDefinition);
			}
			Profiling.Stop();
			m_IsLoading = true;
			Profiling.Start("Crowd", float.MaxValue);
			Crowd.Instance.ClearCrowd();
			MissionDescExt currentMissionDescription = Main.Instance.SceneLoader.CurrentMissionDescription;
			if (currentMissionDescription.CrowdDensity > 0)
			{
				Crowd.Instance.CrowdDensity = currentMissionDescription.CrowdDensity;
				Crowd.Instance.MaxCrowdEntities = currentMissionDescription.MaxCrowdEntities;
				Crowd.Instance.SetQuality(Crowd.CrowdQuality);
				Crowd.Instance.Initialize();
				Crowd.Instance.SpawnCrowd(Crowd.Instance.DesiredSpawnCount);
			}
			Profiling.Stop();
			Profiling.Stop();
		}
	}

	public static void OnMissionLoaded()
	{
		if (Assets.Scripts.Engine.Helpers.Verify(!IsRunning, "Calling OnMissionLoaded although logic is already running! Tell the mission logic departement!"))
		{
			Profiling.Start("LogicMain::OnMissionLoaded", float.MaxValue);
			m_CombatController.LoadMission();
			if (MissionType == EMissionType.Mission_EMissionType)
			{
				PlayerHelper.UpdateVision();
				m_gameLogic.OnActivate();
			}
			UbiCore.Utils.Singleton<HotspotService>.Initialize();
			m_IsLoading = false;
			IsRunning = true;
			EntityManager.DoHouseKeeping();
			TimeScaleManager.RegisterMultiplier(m_globalTimeScale);
			Profiling.Stop();
		}
	}

	public static void OnMissionUnloaded()
	{
		if (IsRunning)
		{
			TimeScaleManager.UnregisterMultiplier(m_globalTimeScale);
			IsRunning = false;
			m_CombatController.UnloadMission();
			m_CombatController.Release();
			m_CombatController = null;
			MissionType = EMissionType.None_EMissionType;
			EntityManager.UnregisterEntities();
			EntityManager.DoHouseKeeping();
			UbiCore.Utils.Singleton<HotspotService>.Shutdown();
			UbiCore.Utils.Singleton<SelectionManager>.Shutdown();
			Player = null;
			PlayerEntity = null;
			Random = null;
			m_turnHandler = new TurnHandler(0.1f);
			m_entityManager = new EntityManager();
			m_logicExecutor = new LogicExecutor();
			if (Crowd.Instance != null)
			{
				Crowd.Instance.Release();
			}
		}
	}

	public static void InitPlayer(GameObject _go)
	{
		Player = _go;
		PlayerEntity = Player.GetComponent<EntityWrapperComponent>().GetEntity<PlayerCharacter>();
	}

	public static void UpdateFrame(float _timeDelta)
	{
		if (IsRunning)
		{
			Profiling.Start("LogicMain::UpdateFrame()", float.MaxValue);
			m_turnHandler.Execute();
			EntityManager.DoHouseKeeping();
			m_CombatController.FrameUpdate();
			Profiling.Stop();
		}
	}

	public static void Initialize()
	{
	}

	public static void AddJob(ILogicExecutorJob _job)
	{
		m_logicExecutor.AddJob(_job);
	}
}
