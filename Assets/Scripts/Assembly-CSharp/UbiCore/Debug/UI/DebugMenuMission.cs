using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.CharacterPooling;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Network;
using Assets.Scripts.Network.Missions;
using Assets.Scripts.Network.Missions.Mission.Serialization;
using Assets.Scripts.UI.Network;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.MissionManager;
using Mission;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Helpers;
using UbiCore.Utils;
using UnityEngine;

namespace UbiCore.Debug.UI
{
	public class DebugMenuMission : DebugMenuItem
	{
		private bool m_ShowPoolingStats;

		private bool m_ShowMissionInfo;

		private BuildDefinition m_BuildDefinition;

		private Vector2 m_missionScrollPos;

		private Vector2 m_avatarScrollPos;

		private Vector2 m_hirelingScrollPos;

		private BBList<MissionDescExt> m_missionDescs;

		private string m_playerFirstName = string.Empty;

		private string m_playerLastName = string.Empty;

		private string[] m_playerFirstNames = new string[107]
		{
			"Adamo", "Adriano", "Agnolo", "Agostino", "Alessio", "Ambrogio", "Amerigo", "Andrea", "Antonello", "Antonio",
			"Arnolfo", "Bartolo", "Bartolomeo", "Benvenuto", "Bernardo", "Bertoldo", "Borso", "Carlo", "Cesare", "Chiero",
			"Clemente", "Cosimo", "Cristofo", "Daniele", "Demetrio", "Domenico", "Donato", "Duccio", "Durante", "Emanuele",
			"Emilo", "Ercole", "Essandro", "Eugenio", "Federigo", "Felice", "Filippo", "Fiorentino", "Flavio", "Fonso",
			"Francesco", "Galeazzo", "Gasparo", "Gentile", "Gherardo", "Giacomo", "Giorgio", "Giotto", "Giovanni", "Giovenale",
			"Girardus", "Girolamo", "Giuliano", "Giulio", "Giuseppe", "Gregorio", "Guarino", "Guido", "Guilio", "Ivo",
			"Jacopo", "Leonardo", "Leone", "Leonello", "Lionello", "Lorenzo", "Luca", "Ludovico", "Luigi", "Marco",
			"Marsilio", "Martino", "Masolino", "Matteo", "Michele", "Niccolo", "Nichola", "Nicodemo", "Nicola", "Orcagna",
			"Ottavio", "Paganinus", "Pantaleo", "Paolo", "Petrus", "Pierandrea", "Piero", "Pietro", "Prospero", "Qurino",
			"Raffaele", "Riccardo", "Rido", "Santino", "Sigismondo", "Simone", "Sirus", "Stefano", "Taddeo", "Tomaso",
			"Turone", "Ugo", "Ugolino", "Ulisse", "Vitale", "Vitore", "Vittorio"
		};

		private string[] m_playerLastNames = new string[107]
		{
			"Acerbi", "Acqua", "Airo", "Aldobrandi", "Arcimboldo", "Arongheri", "Baldovetti", "Barbarigo", "Barberini", "Bellini",
			"Berardi", "Borghese", "Borromeo", "Bronzino", "Caito", "Campo", "Carafa", "Cavante", "Cellini", "Chigi",
			"Cino", "Coiro", "Contarini", "d' Este", "da Montefe", "da Vigo", "Damiani", "De Felice", "De Fiore", "De Luca",
			"de' Barbari", "de' Medici", "de' Rossi", "di Fini", "di Mercio", "Donati", "Donato", "Drago", "Ermacora", "Falco",
			"Farina", "Farnese", "Farro", "Fausti", "Fava", "Favero", "Fernese", "Ferrara", "Ferro", "Fontana",
			"Fonte", "Foscari", "Gaspari", "Ghiberti", "Gismondi", "Gonzaga", "Gritti", "Lagorio", "Landi", "Landolfi",
			"Lazzari", "Loredan", "Lupo", "Maestri", "Malatesta", "Mantovani", "Marchetti", "Mattei", "Mazza", "Milani",
			"Monigo", "Montagna", "Montana", "Monte", "Nazario", "Notario", "Orsini", "Orsini", "Padovan", "Palladino",
			"Pamphili", "Parrino", "Ravenna", "Rinaldi", "Rosa", "Rosso", "Sacco", "Santini", "Sarno", "Savoy",
			"Scarpa", "Sforza", "Silvestri", "Taranto", "Todaro", "Uberti", "Valenti", "Varano", "Vasari", "Veneziano",
			"Ventura", "Vico", "Vigo", "Visconti", "Vitali", "Zangari", "Zino"
		};

		private string m_requestMissionText = "G_It_Mo_Da_PaRa_D0_R0_S8008_B0030";

		public static Regex s_MissionCodeRegex = new Regex("G_(?<Era>(?:[A-Z]+[a-z]*)+)_(?<Region>(?:[A-Z]+[a-z,0-9]*)+)_(?<Mood>(?:[A-Z][a-z]+)+)_(?<Type>(?:[A-Z][a-z]+)+)_(?<Difficulty>D[0-9])_(?<Rank>R[0-9])_(?<Seed>S[0-9]+)_(?<Budget>B[0-9]+)", RegexOptions.CultureInvariant);

		private bool m_requestMissionTextValid;

		private bool m_localMissionListInitialized;

		private bool m_isElite;

		public override string GetName()
		{
			return "Mission";
		}

		public override void OnMenu()
		{
			InitLocalMissionList();
			GUILayout.BeginVertical();
			OnMenuPlayerStatus();
			OnMenuMission();
			GUILayout.EndVertical();
		}

		private void OnMenuPlayerStatus()
		{
			GUILayout.BeginHorizontal();
			PlayerCharacter playerEntity = LogicMain.PlayerEntity;
			if (playerEntity != null)
			{
				GUILayout.BeginVertical("box");
				GUILayout.Label("Notoriety:" + UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerNotoriety);
				if (GUILayout.Button("Anonymous"))
				{
					playerEntity.EntityStats.Set_float(StatTypes.EStatType.Character_Conspicuous_Notoriety, 0f);
				}
				if (GUILayout.Button("Alerted"))
				{
					playerEntity.EntityStats.Set_float(StatTypes.EStatType.Character_Conspicuous_Notoriety, 0.5f);
				}
				if (GUILayout.Button("Exposed"))
				{
					playerEntity.EntityStats.Set_float(StatTypes.EStatType.Character_Conspicuous_Notoriety, 1f);
				}
				PlayerAlertLevel.s_cheatNoNotoriety = GUILayout.Toggle(PlayerAlertLevel.s_cheatNoNotoriety, "No Range Notoriety");
				PlayerAlertLevel.s_cheatNoDecreaseNotorietyIfUnseen = GUILayout.Toggle(PlayerAlertLevel.s_cheatNoDecreaseNotorietyIfUnseen, "No decrease notoriety if unseen");
				GUILayout.EndVertical();
			}
			GUILayout.BeginVertical("box");
			Main.ShowConfidentialWarning = GUILayout.Toggle(Main.ShowConfidentialWarning, "Display confidentiality warning");
			Main.OverwriteAnimusMissionLock = GUILayout.Toggle(Main.OverwriteAnimusMissionLock, "Overwrite Animus Mission Lock");
			OnMenuToggleMissionInfoGUI();
			OnMenuToggleShowPoolingStats();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		public override void OnOpen()
		{
			m_BuildDefinition = Main.Instance.BuildDefinition;
		}

		private void InitLocalMissionList()
		{
			if (!m_localMissionListInitialized)
			{
				string path = Main.Instance.BaseURL.Replace("file://", string.Empty);
				path = Path.Combine(path, "Missions");
				path = GameFolders.FixSlashes(path, false);
				RuntimePlatform platform = Application.platform;
				if (platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.IPhonePlayer || platform == RuntimePlatform.Android)
				{
					ReadMissionsServer(path);
				}
				else
				{
					ReadMissionsLocal(path);
				}
				m_localMissionListInitialized = true;
			}
		}

		private void ReadMissionsServer(string _uri)
		{
			m_missionDescs = MissionDescExt.CreateMissionListUrl(_uri);
			if (m_missionDescs == null)
			{
				m_missionDescs = new BBList<MissionDescExt>();
			}
		}

		private void ReadMissionsLocal(string _path)
		{
			try
			{
				if (Directory.Exists(_path))
				{
					string[] files = Directory.GetFiles(_path, "*.bin", SearchOption.AllDirectories);
					List<string> list = new List<string>();
					list.AddRange(files);
					m_missionDescs = MissionDescExt.CreateMissionListLocal(null, list);
				}
			}
			catch (DirectoryNotFoundException)
			{
			}
			catch
			{
			}
			finally
			{
				if (m_missionDescs == null)
				{
					m_missionDescs = new BBList<MissionDescExt>();
				}
			}
		}

		public override void OnApply()
		{
		}

		private InteractableObject GetFirstChallengeLootChest()
		{
			BBList<IMissionBase> missionEntries = UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.MissionEntries;
			for (int i = 0; i < missionEntries.Count; i++)
			{
				ObjectiveLoot objectiveLoot = missionEntries[i] as ObjectiveLoot;
				if (!(objectiveLoot == null))
				{
					MissionGoalLoot _goalLoot = null;
					InteractableObject challengeLootChest = objectiveLoot.GetChallengeLootChest(ref _goalLoot);
					if (challengeLootChest != null)
					{
						return challengeLootChest;
					}
				}
			}
			return null;
		}

		private void OnMenuMission()
		{
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical("box");
			GUILayout.Label(string.Format("Avatar: {0}", m_BuildDefinition.Avatar));
			m_avatarScrollPos = GUILayout.BeginScrollView(m_avatarScrollPos);
			foreach (AvatarData.AvatarType playerType in UbiCore.Utils.Singleton<AvatarData>.Instance.PlayerTypes)
			{
				string text = playerType.Definition.Name;
				if (GUILayout.Button(text, GUILayout.MinHeight(20f)))
				{
					m_BuildDefinition.AvatarFixed = false;
					m_BuildDefinition.Avatar = text;
					m_BuildDefinition.AvatarFixed = true;
					m_BuildDefinition.HasUsedDebugAvatarSelection = true;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			GUILayout.Label(string.Format("Hireling: {0}", m_BuildDefinition.Hireling));
			m_hirelingScrollPos = GUILayout.BeginScrollView(m_hirelingScrollPos);
			foreach (AvatarData.AvatarType hirelingType in UbiCore.Utils.Singleton<AvatarData>.Instance.HirelingTypes)
			{
				string text2 = hirelingType.Definition.Name;
				if (GUILayout.Button(text2, GUILayout.MinHeight(20f)))
				{
					m_BuildDefinition.AvatarFixed = false;
					m_BuildDefinition.Hireling = text2;
					m_BuildDefinition.AvatarFixed = true;
					m_BuildDefinition.HasUsedDebugAvatarSelection = true;
				}
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginVertical("box");
			if (LogicMain.PlayerEntity != null)
			{
				InteractableObject firstChallengeLootChest = GetFirstChallengeLootChest();
				if (firstChallengeLootChest != null && GUILayout.Button("Move Player to chest"))
				{
					NavMeshPath _path;
					Vector3 vector = SpawnHelper.ComputeSpawnPos(firstChallengeLootChest.Position, Quaternion.LookRotation(firstChallengeLootChest.Position), 1f, 0f, NavMeshLayers.WalkableLayerMaskPreset, false, out _path);
					LogicMain.PlayerEntity.UnityComponentCache.AnimationController.Agent.nextPosition = vector;
					LogicMain.PlayerEntity.UnityComponentCache.AnimationController.Agent.Warp(vector);
					m_debugMenu.CloseDebugMenu();
				}
				if (!PlayerHelper.IsDead && GUILayout.Button("Kill player"))
				{
					PlayerHelper.Die();
					m_debugMenu.CloseDebugMenu();
				}
				if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.Success && GUILayout.Button("Instant complete mission"))
				{
					UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState = EMissionState.Success;
					m_debugMenu.CloseDebugMenu();
				}
			}
			if (GUILayout.Button("Restart mission"))
			{
				Singleton<UIScreenManager>.Instance.ClearStack();
				Main.Instance.SwitchScene(Main.Instance.SceneLoader.CurrentMissionDescription.MissionUrl, true);
				m_debugMenu.CloseDebugMenu();
			}
			if (!Main.Instance.SceneLoader.InAnimus && GUILayout.Button("Save mission locally"))
			{
				string text3 = Path.Combine(Main.LoadSavedDataPath(), "Documents");
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
				text3 = Path.Combine(text3, "Missions");
				if (!Directory.Exists(text3))
				{
					Directory.CreateDirectory(text3);
				}
				text3 = Path.Combine(text3, UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.name + ".bin");
				MissionSerializer.WriteBinaryFile(text3, Main.Instance.SceneLoader.LoadedMissionData.MissionNode);
			}
			if (GUILayout.Button("Go to animus"))
			{
				Singleton<UIScreenManager>.Instance.PurgeManager();
				Singleton<StateManager>.Instance.ProfileManager.EnableUpdates = true;
				Main.Instance.LoadMission(BuildDefinition.AnimusMission, null, false);
				m_debugMenu.CloseDebugMenu();
			}
			GUILayout.BeginHorizontal();
			GUILayout.Label("Request mission:", GUILayout.MaxWidth(120f));
			m_requestMissionText = GUILayout.TextField(m_requestMissionText);
			MakeMissionNameSane();
			m_isElite = GUILayout.Toggle(m_isElite, "elite?", GUILayout.Width(60f));
			GUI.enabled = m_requestMissionTextValid;
			if (GUILayout.Button("Request", GUILayout.MaxWidth(70f)))
			{
				SwitchSceneDebug(m_requestMissionText, false, m_isElite);
				m_debugMenu.CloseDebugMenu();
			}
			GUI.enabled = true;
			GUI.enabled = UbiCore.Utils.Singleton<MissionManager>.IsAvailable && UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain != null;
			if (GUILayout.Button("Fetch", GUILayout.MaxWidth(70f)))
			{
				m_requestMissionText = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionMain.name;
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			GUILayout.Label((!m_requestMissionTextValid) ? "<i>The entered mission text is <b>NOT valid</b>!!!</i>" : "<i>Entered mission name is valid.</i>");
			GUILayout.BeginHorizontal();
			Main.Instance.BuildDefinition.ShowBaseName = GUILayout.Toggle(Main.Instance.BuildDefinition.ShowBaseName, "Show Base Name", GUILayout.MaxWidth(400f));
			GUILayout.EndHorizontal();
			GUILayout.Label("Select mission:");
			m_missionScrollPos = GUILayout.BeginScrollView(m_missionScrollPos);
			foreach (MissionDescExt missionDesc in m_missionDescs)
			{
				GUILayout.BeginHorizontal("Button");
				bool flag = GUILayout.Button(missionDesc.Name, "Label", GUILayout.Width(300f));
				flag = GUILayout.Button(missionDesc.MissionUrl, "Label", GUILayout.Width(300f)) || flag;
				if (GUILayout.Button(string.Empty, "Label") || flag)
				{
					Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
					MissionMetaDataManager.Instance.Clear(true);
					Singleton<UIScreenManager>.Instance.PurgeManager();
					Main.Instance.SwitchScene(missionDesc.MissionUrl, false);
					m_debugMenu.CloseDebugMenu();
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		private void MakeMissionNameSane()
		{
			Match match = s_MissionCodeRegex.Match(m_requestMissionText);
			m_requestMissionTextValid = match.Success;
			if (m_requestMissionTextValid)
			{
				m_requestMissionText = string.Format("G_{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}", match.Groups["Era"].Value, match.Groups["Region"].Value, match.Groups["Mood"].Value, match.Groups["Type"].Value, match.Groups["Difficulty"].Value, match.Groups["Rank"].Value, match.Groups["Seed"].Value, match.Groups["Budget"].Value);
			}
		}

		public void SwitchSceneDebug(string _requestMissionText, bool _isQuickRestart, bool _isElite)
		{
			Singleton<StateManager>.Instance.ProfileManager.GetMissionByName(_requestMissionText, _isElite, LoadMissionFromServerDebug);
		}

		private static void LoadMissionFromServerDebug(IAsyncResult _result)
		{
			IAsyncDataResult<CheatResponse> asyncDataResult = _result as IAsyncDataResult<CheatResponse>;
			CheatResponse cheatResponse = asyncDataResult.Result();
			string missionId = cheatResponse.missionToken.missionId;
			AcierProtocol.Mission mission = new AcierProtocol.Mission();
			mission.id = missionId;
			AcierProtocol.Mission contractMission = mission;
			Singleton<StateManager>.Instance.MissionSelectionManager.SubMenuType = MissionSelectionManager.EMissionMenu.ContractMissions;
			Singleton<StateManager>.Instance.MissionSelectionManager.ContractMission = contractMission;
			string hirelingId = cheatResponse.missionToken.hirelingId;
			Singleton<StateManager>.Instance.ProfileManager.LastSelectedHirelingId = hirelingId;
			string assassinId = cheatResponse.missionToken.assassinId;
			Singleton<StateManager>.Instance.ProfileManager.SelectedAssassin = assassinId;
			Singleton<StateManager>.Instance.ProfileManager.MissionTicket = cheatResponse.param2;
			Singleton<StateManager>.Instance.ProfileManager.MissionToken = cheatResponse.missionToken;
			int rank = cheatResponse.missionToken.rank;
			string campaignId = string.Empty;
			UINetworkAdaptor.NetworkRequest(delegate(AsyncCallback _asyncResult)
			{
				Singleton<StateManager>.Instance.ProfileManager.GetMissionInfo(missionId, assassinId, hirelingId, campaignId, rank, _asyncResult);
			}, null, delegate(IAsyncResult _ar)
			{
				Singleton<StateManager>.Instance.ProfileManager.SaveLastProfileDateBeforeMission();
				Singleton<UIScreenManager>.Instance.PurgeManager();
				GetMissionInfoResponse getMissionInfoResponse = ((IAsyncDataResult<GetMissionInfoResponse>)_ar).Result();
				Main.Instance.LoadMission(cheatResponse.missionFile, getMissionInfoResponse.missionGoals, cheatResponse.missionToken.eliteMission);
			});
		}

		private void OnMenuToggleMissionInfoGUI()
		{
			GUI.enabled = true;
			bool value = m_ShowMissionInfo;
			Toggle(ref value, "Show Mission Info");
			m_ShowMissionInfo = value;
		}

		private void OnMenuToggleShowPoolingStats()
		{
			GUI.enabled = true;
			bool value = m_ShowPoolingStats;
			Toggle(ref value, "Show Pooling Stats");
			m_ShowPoolingStats = value;
		}

		private string FormatStatsSection(string _name, CharacterStatsSection _section)
		{
			return string.Format(" Section [{0}]:\n  Total    : {1}\n  Local    : {2}\n  Forced   : {3}\n  InFrustum: {4}\n  Drawn    : {5}\n\n", _name, _section.Total, _section.Local, _section.Forced, _section.InFrustum, _section.Drawn);
		}

		public void OnGUI()
		{
		}
	}
}
