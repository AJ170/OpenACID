using System;
using AcierProtocol;
using Assets.Scripts.Mission.AI.System;
using Assets.Scripts.Mission.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.Network.Missions.Mission.Serialization;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.System;
using UbiCore.Debug;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission
{
	public class MissionLoader
	{
		public delegate void MissionChangedDelegate(MissionLoader _sender);

		public MissionRoot MissionMain { get; private set; }

		public MissionPhase State { get; private set; }

		public GameObject Root { get; private set; }

		public event MissionChangedDelegate OnMissionLoaded;

		public void OnSceneLoaderStep(SceneLoader.ELoadingStep _step)
		{
			bool flag = false;
			Profiling.Start("MissionLoader::OnSceneLoaderStep", float.MaxValue);
			switch (_step)
			{
			case SceneLoader.ELoadingStep.CameraCreated:
				if (!DeserializeCurrentMission())
				{
					flag = true;
				}
				else
				{
					InitializeMission();
				}
				break;
			case SceneLoader.ELoadingStep.AllDone:
				if (!Main.Instance.BuildDefinition.DontStartLogic)
				{
					InvokeOnMissionLoaded();
				}
				if (Main.Instance.Startup.Quality == EQualityAssetBundle.Low)
				{
					GameObject gameObject = GameObject.Find("TurbulentLogo");
					if (gameObject != null)
					{
						UnityEngine.Object.Destroy(gameObject);
					}
				}
				break;
			}
			Profiling.Stop();
			if (flag)
			{
				UIUtil.QuitApplication();
			}
		}

		public void AddTappedMesh(GameObject _root, GameObject _start, GameObject _end)
		{
			_root.AddComponentIfNotExist<BoxCollider>();
			TappedMesh tappedMesh = _root.AddComponentIfNotExist<TappedMesh>();
			tappedMesh.Type = TappedMesh.eTriggerType.MoveToBench;
			tappedMesh.Data.mStart = _start.transform;
			tappedMesh.Data.mEnd = _end.transform;
		}

		private bool DeserializeCurrentMission()
		{
			MissionLoaderProvider.ResultData loadedMissionData = Main.Instance.SceneLoader.LoadedMissionData;
			if (loadedMissionData == null)
			{
				return false;
			}
			Profiling.Start("MissionLoader::DeserializeCurrentMission", float.MaxValue);
			if (Root == null)
			{
				Root = new GameObject("MissionRoot");
			}
			SetupEnvironment();
			if (Main.Instance.Startup.QualitySettings.downScaling && !Main.Instance.VrEnabled)
			{
				UnityEngine.Camera main = UnityEngine.Camera.main;
				RenderingUpscaler renderingUpscaler = main.gameObject.AddComponent<RenderingUpscaler>();
				renderingUpscaler.SetupLowResRenderTarget(main.pixelWidth, main.pixelHeight, 0.5f, 24, RenderTextureFormat.Default);
			}
			Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode missionNode = loadedMissionData.MissionNode;
			bool isEliteMission = loadedMissionData.IsEliteMission;
			GameObject gameObject = BuildTreeInternal(missionNode, Root, isEliteMission);
			if (loadedMissionData.MissionGoals != null)
			{
				MissionMetaDataManager.Instance.SetMissionGoals(loadedMissionData.MissionGoals, loadedMissionData.IsEliteMission);
			}
			BuildTreeReadCompleted(gameObject);
			MissionMonoBehaviour[] componentsInChildren = gameObject.GetComponentsInChildren<MissionMonoBehaviour>();
			UbiCore.Utils.Singleton<BehaviorTreeService>.Initialize();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!componentsInChildren[i].OnDeserialized())
				{
					Profiling.Stop();
					Profiling.DumpLastFrame();
					return false;
				}
			}
			MissionMain = gameObject.GetComponent<MissionRoot>();
			if (MissionMain.gameObject.GetComponent<ObjectiveStayAlive>() == null)
			{
				MissionMain.gameObject.AddComponent<ObjectiveStayAlive>();
			}
			Profiling.Stop();
			Profiling.DumpLastFrame();
			return true;
		}

		public static void BuildTree(Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode _serializedNode, GameObject _root, bool _isElite)
		{
			GameObject result = BuildTreeInternal(_serializedNode, _root, _isElite);
			BuildTreeReadCompleted(result);
		}

		private static void BuildTreeReadCompleted(GameObject result)
		{
			Profiling.Start("MissionLoader::BuildTreeReadCompleted", float.MaxValue);
			MissionMonoBehaviour[] componentsInChildren = result.GetComponentsInChildren<MissionMonoBehaviour>();
			foreach (MissionMonoBehaviour missionMonoBehaviour in componentsInChildren)
			{
				missionMonoBehaviour.OnReadCompleted();
			}
			Profiling.Stop();
		}

		private static GameObject BuildTreeInternal(Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode _serializedNode, GameObject _root, bool _isElite)
		{
			Profiling.Start("MissionLoader::BuildTreeInternal", float.MaxValue);
			Profiling.Start("Creating GO", float.MaxValue);
			GameObject gameObject = new GameObject(_serializedNode.Name);
			gameObject.transform.position = new Vector3(_serializedNode.PositionX, _serializedNode.PositionY, _serializedNode.PositionZ);
			gameObject.transform.eulerAngles = new Vector3(_serializedNode.EulerAnglesX, _serializedNode.EulerAnglesY, _serializedNode.EulerAnglesZ);
			gameObject.transform.parent = _root.transform;
			Profiling.Stop();
			int count = _serializedNode.Components.Count;
			for (int i = 0; i < count; i++)
			{
				Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNodeComponent serializedNodeComponent = _serializedNode.Components[i];
				ObjectiveBaseData objectiveBaseData = serializedNodeComponent.NodeData as ObjectiveBaseData;
				if (objectiveBaseData == null || !objectiveBaseData.IsEliteObjective || _isElite)
				{
					Profiling.Start("Add Cmp: {0}", float.MaxValue, serializedNodeComponent.ComponentType);
					IMissionBase missionBase = (IMissionBase)gameObject.AddComponent(Type.GetType(serializedNodeComponent.ComponentType));
					Profiling.Stop();
					Profiling.Start("PostRead: {0}", float.MaxValue, serializedNodeComponent.ComponentType);
					missionBase.OnPostRead(serializedNodeComponent.NodeData);
					Profiling.Stop();
				}
			}
			Profiling.Stop();
			count = _serializedNode.Children.Count;
			for (int j = 0; j < count; j++)
			{
				Assets.Scripts.Network.Missions.Mission.Serialization.SerializedNode serializedNode = _serializedNode.Children[j];
				BuildTreeInternal(serializedNode, gameObject, _isElite);
			}
			return gameObject;
		}

		private void InitializeMission()
		{
			Profiling.Start("m_missionLoader::InitializeMission()", float.MaxValue);
			State = new MissionPhase(MissionMain);
			Profiling.Stop();
		}

		private void InvokeOnMissionLoaded()
		{
			Profiling.Start("MissionLoader::InvokeOnMissionLoaded", float.MaxValue);
			MissionChangedDelegate onMissionLoaded = this.OnMissionLoaded;
			if (onMissionLoaded != null)
			{
				onMissionLoaded(this);
			}
			Profiling.Stop();
		}

		private void SetupBench(GameObject _bench)
		{
			if (!(_bench == null))
			{
				GameObject gameObject = _bench.FindChildByName("Icon");
				GameObject start = _bench.FindChildByName("Direction");
				GameObject end = _bench.FindChildByName("SitPlace B");
				GameObject gameObject2 = Helpers.Instantiate("Bench");
				AddTappedMesh(_bench, start, end);
				gameObject2.transform.parent = _bench.transform;
				gameObject2.transform.position = gameObject.transform.position;
				AddTappedMesh(gameObject2, start, end);
				gameObject2.layer = Layers.InteractorsLayer;
				gameObject2.tag = "Interactable";
			}
		}

		private void SetupEnvironment()
		{
			SetupInteractors();
		}

		private void SetupInteractors()
		{
		}

		public void Destroy()
		{
			UbiCore.Utils.Singleton<BehaviorTreeService>.Shutdown();
			State.Destroy();
			State = null;
			UnityEngine.Object.Destroy(Root);
			MissionMain = null;
			Root = null;
		}
	}
}
