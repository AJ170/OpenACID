using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.CharacterPooling;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Utilities;
using Database;
using Humanoids;
using Mission;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class Crowd : MonoBehaviour
	{
		public class CrowdNodeData
		{
			public int NodeId;

			public CrowdGraph.CrowdNode Node;

			public BBList<CrowdBehavior> Behaviors = new BBList<CrowdBehavior>();

			public bool IsInRestrictedArea;

			public bool IsInCombat;

			public bool IsOnNavMesh = true;

			public bool Flee;

			public float EstimatedPopulation;

			public CrowdHotspotData Hotspot;
		}

		public class CrowdStationData
		{
			public int StationId;

			public CrowdStation Station;

			public CrowdNodeData NodeData;

			public BBList<CrowdBehavior> CrowdOnStation = new BBList<CrowdBehavior>();

			public void Add(CrowdBehavior _behavior)
			{
				CrowdOnStation.Add(_behavior);
				Station.CurrentCrowdOnStation = CrowdOnStation.Count;
			}

			public void Remove(CrowdBehavior _behavior)
			{
				CrowdOnStation.SwapRemove(_behavior);
				Station.CurrentCrowdOnStation = CrowdOnStation.Count;
			}
		}

		public struct CrowdNodeRelevance
		{
			public double Relevance;

			public CrowdNodeData NodeData;

			public double Weight;
		}

		public class CrowdStatistics
		{
			private int m_unitsPanicking;

			private int m_unitsPanickingHeightDiff;

			private int m_smokeBombed;

			public float SmokeBombedAccumulator;

			public float UnitsPanickingHeightDiffAccumulator;

			public float UnitsPanickingAccumulator;

			public int UnitsPanicking
			{
				get
				{
					return m_unitsPanicking;
				}
				set
				{
					if (m_unitsPanicking != value)
					{
						m_unitsPanicking = value;
						OnChanged();
					}
				}
			}

			public int UnitsPanickingHeightDiff
			{
				get
				{
					return m_unitsPanickingHeightDiff;
				}
				set
				{
					if (m_unitsPanickingHeightDiff != value)
					{
						m_unitsPanickingHeightDiff = value;
						OnChanged();
					}
				}
			}

			public int SmokeBombed
			{
				get
				{
					return m_smokeBombed;
				}
				set
				{
					if (m_smokeBombed != value)
					{
						m_smokeBombed = value;
						OnChanged();
					}
				}
			}

			public event Action<CrowdStatistics> Changed;

			private void OnChanged()
			{
				Action<CrowdStatistics> changed = this.Changed;
				if (changed != null)
				{
					changed(this);
				}
			}
		}

		public static double NodeRelevanceMaxDistance = 50.0;

		public static double NodeRelevanceFrustumFactor = 0.5;

		public static double NodeRelevanceLOSFactor = 0.10000000149011612;

		public static double NodeRelevanceCutoff = 0.10000000149011612;

		public static double BehaviorRelevanceCutoff = 0.20000000298023224;

		public static float NodeRaycastHeight = 1.5f;

		public static float AvoidCombatRadius = 40f;

		public static float FleeCombatRadius = 20f;

		public static float AvoidChaseRadius = 10f;

		public static float AvoidAssassinationRadius = 20f;

		public static float FleeAssassinationRadius = 10f;

		public static float DynamicWeightDamping = 0.5f;

		public static float DynamicWeightBleed = 0.4f;

		public static float DynamicWeightUpdatePeriod = 1f;

		public static float RelevantNodeUpdatePeriod = 0.25f;

		public static float CrowdStationUpdatePeriod = 0.5f;

		public static float CrowdStationWeightCutoff = 0.9f;

		public static float EstimatedPopulationDamping = 2f;

		public int CrowdDensity = 250;

		public int MaxCrowdEntities = 50;

		public int DesiredSpawnCount = 10;

		public double MissingCrowd;

		public double DesiredCrowd;

		public double CurrentCrowd;

		public float MaxStationRadius;

		public bool DisableUnusedCrowd = true;

		public bool DoNotSpawnInLOS = true;

		public Vector3 LastCameraPosition;

		public Plane[] LastCameraFrustum;

		public bool IsPlayerNoticeable;

		public List<CrowdGraph.CrowdNode> RefCrowdNodes = new List<CrowdGraph.CrowdNode>();

		public BBList<CrowdNodeData> CrowdNodes = new BBList<CrowdNodeData>();

		public Octree<CrowdNodeData> CrowdNodeIndex;

		public BBList<CrowdStationData> CrowdStations = new BBList<CrowdStationData>();

		public Octree<CrowdStationData> CrowdStationIndex;

		public BBList<CrowdHotspot> CrowdHotspots = new BBList<CrowdHotspot>();

		public BBList<CrowdHotspotData> CrowdHotspotDatas = new BBList<CrowdHotspotData>();

		public BBList<CrowdBehavior> CrowdBehaviors = new BBList<CrowdBehavior>();

		public BBSimpleList<CrowdNodeRelevance> RelevantNodes = new BBSimpleList<CrowdNodeRelevance>();

		public List<GameEntity> AvoidanceAgents = new List<GameEntity>();

		private static float m_crowdQuality;

		private int m_lastDynamicWeightUpdateIndex = -1;

		private int m_lastRelevantNodeUpdateIndex = -1;

		private int m_lastCrowdStationUpdateIndex = -1;

		private BBSimpleList<CrowdNodeRelevance> m_newRelevantNodes = new BBSimpleList<CrowdNodeRelevance>();

		private List<CrowdBehavior> m_availableCrowd = new List<CrowdBehavior>();

		private CrowdStatistics m_statistics = new CrowdStatistics();

		private static Crowd m_instance;

		public static float CrowdQuality
		{
			get
			{
				return m_crowdQuality;
			}
			set
			{
				m_crowdQuality = value;
				if (m_instance != null)
				{
					m_instance.SetQuality(value);
				}
			}
		}

		public CrowdStatistics Statistics
		{
			get
			{
				return m_statistics;
			}
		}

		public static Crowd Instance
		{
			get
			{
				if (m_instance == null)
				{
					CrowdGraph instance = CrowdGraph.Instance;
					m_instance = instance.gameObject.GetComponent<Crowd>();
					if (m_instance == null)
					{
						m_instance = instance.gameObject.AddComponent<Crowd>();
						m_instance.InitCrowdNodes();
						m_instance.SetQuality(CrowdQuality);
					}
				}
				return m_instance;
			}
		}

		public void Release()
		{
			CrowdGraph.Instance.Release();
			CrowdStaticCache.FlowWeights = null;
			CrowdStaticCache.Stations = null;
			CrowdStaticCache.NodeQueryList = null;
			CrowdStaticCache.NodeQuerySet = null;
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager != null)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet -= m_instance.UpdateRestrictedAreas;
			}
			foreach (CrowdBehavior crowdBehavior in CrowdBehaviors)
			{
				UnityEngine.Object.Destroy(crowdBehavior.gameObject);
			}
			m_instance = null;
			UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged -= OnMissionChanged;
		}

		private void OnMissionChanged(EMissionState _state)
		{
			if (_state == EMissionState.InProgress)
			{
				DisableCrowd();
			}
		}

		public void SetQuality(float crowdQuality)
		{
			DesiredSpawnCount = Mathf.Min(MaxCrowdEntities, Mathf.FloorToInt(QualityRange(crowdQuality, 1.75f, (crowdQuality > 0f) ? 3 : 0, MaxCrowdEntities)));
			float quality = 1f - crowdQuality;
			CrowdSettings.UpdatePeriod = QualityRange(quality, 1.5f, 1f / 30f, 0.2f, 30);
			CrowdSettings.CorrectPositionPeriod = QualityRange(quality, 1f, 1f / 30f, 1f / 15f, 30);
			DynamicWeightUpdatePeriod = QualityRange(quality, 1f, 1f / 15f, 1f, 30);
			RelevantNodeUpdatePeriod = QualityRange(quality, 1f, 1f / 30f, 0.5f, 30);
			CrowdStationUpdatePeriod = QualityRange(quality, 1f, 1f / 15f, 1f, 30);
		}

		private float QualityRange(float quality, float pow, float minValue, float maxValue, int steps = 0)
		{
			float num = Mathf.Pow(Mathf.Clamp01(quality), pow);
			if (steps <= 0)
			{
				return Mathf.Lerp(minValue, maxValue, num);
			}
			return Mathf.Lerp(minValue, maxValue, Mathf.Round(num * (float)steps) / (float)steps);
		}

		public void Start()
		{
			Vector3 position = base.transform.position;
			base.transform.position = Vector3.zero;
			GameObject gameObject = base.gameObject.FindChildByName("CrowdStations");
			if (gameObject != null)
			{
				gameObject.transform.position += position;
			}
		}

		public void Initialize()
		{
			CrowdStaticCache.FlowWeights = new BBSimpleList<double>();
			CrowdStaticCache.Stations = new BBList<CrowdStationData>();
			CrowdStaticCache.NodeQueryList = new BBList<CrowdNodeData>();
			CrowdStaticCache.NodeQuerySet = new HashSet<CrowdNodeData>();
			InitCrowdNodes();
			RebuildNodeIndex();
			RebuildStationIndex();
			RegisterHotspots();
			DisableCrowd();
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager != null)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet += UpdateRestrictedAreas;
			}
			UbiCore.Utils.Singleton<MissionManager>.Instance.OnMissionStateChanged += OnMissionChanged;
			UpdateRestrictedAreas();
			CrowdHotspotDatas.Clear();
		}

		private void UpdateRestrictedAreas()
		{
			AreaManager areaManager = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager;
			if (areaManager != null)
			{
				for (int i = 0; i < CrowdNodes.Count; i++)
				{
					CrowdNodeData crowdNodeData = CrowdNodes[i];
					Vector3 centerPosition = crowdNodeData.Node.CenterPosition;
					crowdNodeData.IsInRestrictedArea = areaManager.IsInArea(centerPosition) || areaManager.IsInArea(centerPosition, EMissionAreaType.BlockCrowd);
				}
			}
		}

		public void AddHotspot(CrowdHotspotData hotspot)
		{
			if (CrowdNodes.Count > 0 && CrowdBehaviors.Count > 0)
			{
				CrowdHotspotDatas.Add(hotspot);
			}
		}

		public void AddCrowdBehavior(CrowdBehavior crowdBehavior)
		{
			CrowdBehaviors.Add(crowdBehavior);
		}

		public void RemoveCrowdBehavior(CrowdBehavior crowdBehavior)
		{
			CrowdBehaviors.Remove(crowdBehavior);
		}

		public void DisableCrowd()
		{
			CrowdBehavior[] componentsInChildren = base.gameObject.GetComponentsInChildren<CrowdBehavior>(true);
			foreach (CrowdBehavior crowdBehavior in componentsInChildren)
			{
				crowdBehavior.Disable();
			}
		}

		public void RepositionCrowd()
		{
			CrowdBehavior[] componentsInChildren = base.gameObject.GetComponentsInChildren<CrowdBehavior>(true);
			foreach (CrowdBehavior crowdBehavior in componentsInChildren)
			{
				bool flag = false;
				while (!flag)
				{
					double num = UnityEngine.Random.value;
					double num2 = 0.0;
					if (CrowdNodes.Count == 0)
					{
						crowdBehavior.Warp(null, null, crowdBehavior.transform.position);
						break;
					}
					for (int j = 0; j < CrowdNodes.Count; j++)
					{
						CrowdNodeData crowdNodeData = CrowdNodes[j];
						CrowdGraph.CrowdNode node = crowdNodeData.Node;
						num2 += node.NormalizedDensity * node.NormalizedArea;
						if (num < num2)
						{
							flag = RepositionOnNode(crowdBehavior, crowdNodeData);
							break;
						}
					}
				}
			}
		}

		private bool RepositionOnNode(CrowdBehavior crowdBehavior, CrowdNodeData nodeData)
		{
			CrowdGraph.CrowdNode node = nodeData.Node;
			Vector3 centerPosition = node.CenterPosition;
			Vector3 vector = centerPosition + Quaternion.Euler(0f, UnityEngine.Random.value * 360f, 0f) * Vector3.forward * UnityEngine.Random.value * node.Radius;
			NavMeshHit _hit;
			if (NavigationHelper.SafeRaycast(centerPosition, vector, out _hit, NavMeshLayers.GroundSurfaceLayerMask))
			{
				vector = _hit.position;
				vector.y = centerPosition.y;
			}
			if (NavigationHelper.CascadingSamplePosition(vector, out _hit, NavMeshLayers.GroundSurfaceLayerMask))
			{
				vector = _hit.position;
				Vector3 vector2 = vector + Vector3.up * 1.5f;
				Vector3 lastCameraPosition = LastCameraPosition;
				Vector3 vector3 = vector2 - lastCameraPosition;
				bool flag = GeometryUtility.TestPlanesAABB(LastCameraFrustum, new Bounds(vector2, Vector3.one)) && !Physics.Raycast(lastCameraPosition, vector3.normalized, vector3.magnitude, Layers.StaticCollidersLayersMaskPreset);
				if (DoNotSpawnInLOS && flag)
				{
					return false;
				}
				double num = UnityEngine.Random.value;
				double num2 = 0.0;
				CrowdGraph.CrowdNode.Flow flow = null;
				for (int i = 0; i < node.Flows.Count; i++)
				{
					CrowdGraph.CrowdNode.Flow flow2 = node.Flows[i];
					num2 += flow2.NormalizedWeight;
					if (num < num2)
					{
						flow = flow2;
						break;
					}
				}
				crowdBehavior.Warp(nodeData, flow, vector);
				return true;
			}
			return false;
		}

		public void RebuildNodeIndex()
		{
			List<CrowdGraph.CrowdNode> refCrowdNodes = RefCrowdNodes;
			CrowdNodes.Clear();
			for (int i = 0; i < refCrowdNodes.Count; i++)
			{
				CrowdGraph.CrowdNode crowdNode = refCrowdNodes[i];
				crowdNode.DynamicWeight = 0f;
				CrowdNodeData crowdNodeData = new CrowdNodeData();
				crowdNodeData.NodeId = i;
				crowdNodeData.Node = crowdNode;
				NavMeshHit hit;
				crowdNodeData.IsOnNavMesh = NavMesh.SamplePosition(crowdNode.CenterPosition, out hit, crowdNode.Radius, NavMeshLayers.GroundSurfaceLayerMask);
				CrowdNodeData item = crowdNodeData;
				CrowdNodes.Add(item);
			}
			CrowdNodeIndex = null;
			if (CrowdNodes.Count > 0)
			{
				CrowdNodeIndex = new Octree<CrowdNodeData>(CrowdGraph.Instance.SceneBounds.size.magnitude, 1f, 4, CrowdGraph.Instance.SceneBounds.center);
				for (int j = 0; j < CrowdNodes.Count; j++)
				{
					CrowdNodeData crowdNodeData2 = CrowdNodes[j];
					CrowdGraph.CrowdNode node = crowdNodeData2.Node;
					CrowdNodeIndex.Add(crowdNodeData2, node.CenterPosition, node.Radius);
				}
				CrowdNodeIndex.Fixate();
			}
			for (int k = 0; k < CrowdBehaviors.Count; k++)
			{
				CrowdBehaviors[k].CrowdFollowCrowdGraph.Reset();
			}
		}

		public void RebuildStationIndex()
		{
			CrowdStationIndex = null;
			CrowdStations.Clear();
			for (int i = 0; i < CrowdBehaviors.Count; i++)
			{
				CrowdBehaviors[i].CrowdStopAtStation.LeaveStation();
			}
			CrowdStation[] array = UnityEngine.Object.FindObjectsOfType<CrowdStation>();
			MaxStationRadius = 0f;
			if (array.Length > 0 && CrowdNodeIndex != null)
			{
				CrowdStationIndex = new Octree<CrowdStationData>(CrowdGraph.Instance.SceneBounds.size.magnitude, 1f, 4, CrowdGraph.Instance.SceneBounds.center);
				for (int j = 0; j < array.Length; j++)
				{
					CrowdStation crowdStation = array[j];
					CrowdNodeData closestNode = GetClosestNode(crowdStation.CachedPosition, true);
					CrowdStationData crowdStationData = new CrowdStationData();
					crowdStationData.Station = crowdStation;
					crowdStationData.StationId = j;
					crowdStationData.NodeData = closestNode;
					CrowdStationData crowdStationData2 = crowdStationData;
					CrowdStations.Add(crowdStationData2);
					CrowdStationIndex.Add(crowdStationData2, crowdStation.CachedPosition, crowdStation.AttractRadius);
					MaxStationRadius = Mathf.Max(MaxStationRadius, crowdStation.AttractRadius);
				}
				CrowdStationIndex.Fixate();
			}
		}

		public void RegisterHotspots()
		{
			for (int i = 0; i < CrowdHotspots.Count; i++)
			{
				CrowdHotspot crowdHotspot = CrowdHotspots[i];
				crowdHotspot.OnTrigger = (CrowdHotspot.TriggerDelegate)Delegate.Remove(crowdHotspot.OnTrigger, new CrowdHotspot.TriggerDelegate(AddHotspot));
			}
			CrowdHotspots.Clear();
			CrowdHotspot[] array = UnityEngine.Object.FindObjectsOfType<CrowdHotspot>();
			foreach (CrowdHotspot crowdHotspot2 in array)
			{
				crowdHotspot2.OnTrigger = (CrowdHotspot.TriggerDelegate)Delegate.Combine(crowdHotspot2.OnTrigger, new CrowdHotspot.TriggerDelegate(AddHotspot));
				CrowdHotspots.Add(crowdHotspot2);
			}
		}

		public CrowdNodeData GetClosestNode(Vector3 position, bool expandSearchRadius = false)
		{
			if (CrowdNodeIndex == null)
			{
				return null;
			}
			CrowdStaticCache.NodeQueryList.Clear();
			CrowdNodeIndex.GetObjects(CrowdStaticCache.NodeQueryList, position, 0f);
			if (expandSearchRadius)
			{
				float num = 2f;
				while (CrowdStaticCache.NodeQueryList.Count <= 0 && num < CrowdGraph.Instance.SceneBounds.size.magnitude)
				{
					CrowdNodeIndex.GetObjects(CrowdStaticCache.NodeQueryList, position, num);
					num *= 2f;
				}
			}
			if (CrowdStaticCache.NodeQueryList.Count == 1)
			{
				return CrowdStaticCache.NodeQueryList[0];
			}
			CrowdNodeData result = null;
			float num2 = float.MaxValue;
			for (int i = 0; i < CrowdStaticCache.NodeQueryList.Count; i++)
			{
				CrowdNodeData crowdNodeData = CrowdStaticCache.NodeQueryList[i];
				float num3 = Vector3.Distance(position, crowdNodeData.Node.CenterPosition);
				if (num3 < num2)
				{
					num2 = num3;
					result = crowdNodeData;
				}
			}
			return result;
		}

		private void Update()
		{
			if (!LogicMain.IsRunning)
			{
				return;
			}
			if (AcierCameraController.Instance != null)
			{
				LastCameraFrustum = AcierCameraController.Instance.LastCameraFrustum;
				LastCameraPosition = AcierCameraController.Instance.LastCameraPosition;
			}
			else
			{
				Camera main = Camera.main;
				if (main != null)
				{
					LastCameraPosition = main.transform.position;
					LastCameraFrustum = GeometryUtility.CalculateFrustumPlanes(main);
				}
			}
			if (RefCrowdNodes != CrowdGraph.Instance.CrowdNodes)
			{
				Initialize();
			}
			if (CrowdNodes.Count > 0 && CrowdBehaviors.Count > 0)
			{
				UpdatePlayerStatus();
				ProcessHotspots();
				UpdateCrowdGraphWeights();
				EnableCrowdStations();
				ListRelevantNodes();
				PlaceCrowd();
				if (CrowdAvoidance.EnableCrowdAvoidance)
				{
					UpdateAvoidanceAgents();
				}
			}
		}

		private void UpdateAvoidanceAgents()
		{
			AvoidanceAgents.Clear();
			DefaultCharacterManager defaultCharacterManager = ((UbiCore.Utils.Singleton<MissionManager>.Instance == null) ? null : (UbiCore.Utils.Singleton<MissionManager>.Instance.CharacterManager as DefaultCharacterManager));
			if (defaultCharacterManager == null)
			{
				return;
			}
			ReadOnlyCollection<DefaultCharacterManager.CacheEntry> activeCacheEntries = defaultCharacterManager.ActiveCacheEntries;
			for (int i = 0; i < activeCacheEntries.Count; i++)
			{
				DefaultCharacterManager.CacheEntry cacheEntry = activeCacheEntries[i];
				if (!(cacheEntry.AnimationController == null))
				{
					Character character = activeCacheEntries[i].AnimationController.Character;
					if (character != null && character.IsAttached && character.IsActive && character.UnityComponentCache.NavMeshAgent.enabled)
					{
						AvoidanceAgents.Add(character);
					}
				}
			}
		}

		private void UpdatePlayerStatus()
		{
			if (LogicMain.PlayerEntity == null)
			{
				IsPlayerNoticeable = false;
				return;
			}
			bool flag = PlayerHelper.PlayerProfile.CurrentStateType >= EStateType.PlayerProfile_High;
			IsPlayerNoticeable = !PlayerHelper.IsHiding && !PlayerHelper.IsInDisguise && (PlayerHelper.IsInCombat || PlayerHelper.IsInParcour || PlayerHelper.IsBeingChased || PlayerHelper.IsExposed || (PlayerHelper.IsRoaming && flag));
		}

		private void InitCrowdNodes()
		{
			RefCrowdNodes = CrowdGraph.Instance.CrowdNodes;
			m_lastDynamicWeightUpdateIndex = -1;
			m_lastRelevantNodeUpdateIndex = -1;
			m_newRelevantNodes.Clear();
		}

		private void EnableCrowdStations()
		{
			float num = Mathf.Max(CrowdStationUpdatePeriod, LogicMain.FrameTime);
			if (num < float.Epsilon)
			{
				return;
			}
			int num2 = Mathf.Min(Mathf.CeilToInt((float)CrowdStations.Count / num * LogicMain.FrameTime), CrowdStations.Count);
			while (num2 > 0)
			{
				m_lastCrowdStationUpdateIndex = (m_lastCrowdStationUpdateIndex + 1) % CrowdStations.Count;
				num2--;
				CrowdStationData crowdStationData = CrowdStations[m_lastCrowdStationUpdateIndex];
				if (crowdStationData.NodeData != null)
				{
					crowdStationData.Station.enabled = crowdStationData.NodeData.Node.DynamicWeight > CrowdStationWeightCutoff;
				}
			}
		}

		private void ProcessHotspots()
		{
			for (int i = 0; i < CrowdHotspotDatas.Count; i++)
			{
				CrowdHotspotData crowdHotspotData = CrowdHotspotDatas[i];
				if (LogicMain.CurrentTime > crowdHotspotData.EndTime)
				{
					CrowdHotspotDatas.RemoveAt(i);
					i--;
				}
			}
		}

		private void ListRelevantNodes()
		{
			float num = Mathf.Max(RelevantNodeUpdatePeriod, LogicMain.FrameTime);
			if (num < float.Epsilon)
			{
				return;
			}
			int num2 = Mathf.Min(Mathf.CeilToInt((float)CrowdNodes.Count / num * LogicMain.FrameTime), CrowdNodes.Count - m_lastRelevantNodeUpdateIndex - 1);
			BBSimpleList<CrowdNodeRelevance> newRelevantNodes = m_newRelevantNodes;
			while (num2 > 0)
			{
				m_lastRelevantNodeUpdateIndex++;
				num2--;
				CrowdNodeData crowdNodeData = CrowdNodes[m_lastRelevantNodeUpdateIndex];
				CrowdGraph.CrowdNode node = crowdNodeData.Node;
				Vector3 vector = node.CenterPosition + Vector3.up * NodeRaycastHeight;
				Vector3 lastCameraPosition = LastCameraPosition;
				Vector3 vector2 = vector - lastCameraPosition;
				double num3 = Math.Max(0f, vector2.magnitude - node.Radius);
				bool flag = GeometryUtility.TestPlanesAABB(bounds: new Bounds(vector, Vector3.one * node.Radius), planes: LastCameraFrustum);
				bool flag2 = flag && !Physics.Raycast(lastCameraPosition, vector2.normalized, vector2.magnitude, Layers.StaticCollidersLayersMaskPreset);
				double num4 = Math.Max(0.0, 1.0 - num3 / NodeRelevanceMaxDistance);
				if (!flag)
				{
					num4 *= NodeRelevanceFrustumFactor;
				}
				if (flag2)
				{
					num4 *= NodeRelevanceLOSFactor;
				}
				if (crowdNodeData.IsInRestrictedArea || crowdNodeData.IsInCombat || !crowdNodeData.IsOnNavMesh)
				{
					num4 = 0.0;
				}
				if (num4 > NodeRelevanceCutoff)
				{
					newRelevantNodes.Add(new CrowdNodeRelevance
					{
						Relevance = num4,
						NodeData = crowdNodeData,
						Weight = 0.0
					});
				}
			}
			if (m_lastRelevantNodeUpdateIndex >= CrowdNodes.Count - 1)
			{
				BBSimpleList<CrowdNodeRelevance> relevantNodes = RelevantNodes;
				RelevantNodes = m_newRelevantNodes;
				m_newRelevantNodes = relevantNodes;
				m_newRelevantNodes.Clear();
				m_lastRelevantNodeUpdateIndex = -1;
			}
		}

		private void UpdateCrowdGraphWeights()
		{
			float num = Mathf.Max(DynamicWeightUpdatePeriod, LogicMain.FrameTime);
			if (num < float.Epsilon)
			{
				return;
			}
			bool flag = LogicMain.PlayerEntity != null && PlayerHelper.IsInCombat;
			bool flag2 = LogicMain.PlayerEntity != null && PlayerHelper.IsBeingChased;
			bool flag3 = LogicMain.PlayerEntity != null && (PlayerHelper.IsPerformingAirAssassination || PlayerHelper.IsPerformingLeapAssassination || PlayerHelper.IsPerformingStrikingAssassination);
			Vector3 vector = ((LogicMain.PlayerEntity == null) ? Vector3.zero : LogicMain.PlayerEntity.Position);
			float t = num / (num + DynamicWeightDamping);
			int num2 = Mathf.Min(Mathf.CeilToInt((float)CrowdNodes.Count / num * LogicMain.FrameTime), CrowdNodes.Count);
			while (num2 > 0)
			{
				bool flag4 = false;
				m_lastDynamicWeightUpdateIndex = (m_lastDynamicWeightUpdateIndex + 1) % CrowdNodes.Count;
				num2--;
				CrowdNodeData crowdNodeData = CrowdNodes[m_lastDynamicWeightUpdateIndex];
				Vector3 centerPosition = crowdNodeData.Node.CenterPosition;
				float magnitude = (centerPosition - vector).magnitude;
				float num3 = Math.Abs(centerPosition.y - vector.y);
				float num4 = 1f;
				crowdNodeData.Flee = false;
				if (crowdNodeData.IsInRestrictedArea)
				{
					num4 = 0f;
				}
				if (flag)
				{
					float num5 = Mathf.Pow(Mathf.Clamp01(magnitude / AvoidCombatRadius), 2f);
					num4 *= num5;
					crowdNodeData.IsInCombat = magnitude < FleeCombatRadius;
					crowdNodeData.Flee |= crowdNodeData.IsInCombat;
					if (crowdNodeData.IsInCombat)
					{
						flag4 = true;
					}
				}
				if (flag2)
				{
					float num6 = Mathf.Pow(Mathf.Clamp01(magnitude / AvoidChaseRadius), 2f);
					num4 *= num6;
				}
				if (flag3)
				{
					float num7 = Mathf.Pow(Mathf.Clamp01(magnitude / AvoidAssassinationRadius), 2f);
					num4 *= num7;
					crowdNodeData.Flee |= magnitude < FleeAssassinationRadius;
					if (crowdNodeData.Flee)
					{
						flag4 = true;
					}
				}
				if (crowdNodeData.Node.Neighbors.Count == 0)
				{
					num4 = 0f;
				}
				CrowdHotspotData hotspot = null;
				float num8 = float.MaxValue;
				for (int i = 0; i < CrowdHotspotDatas.Count; i++)
				{
					CrowdHotspotData crowdHotspotData = CrowdHotspotDatas[i];
					float magnitude2 = (centerPosition - crowdHotspotData.Position).magnitude;
					if (magnitude2 < crowdHotspotData.Radius)
					{
						num4 = Mathf.Lerp(num4, crowdHotspotData.DynamicWeight, 1f - Mathf.Pow(Mathf.Clamp01(magnitude2 / crowdHotspotData.Radius), 2f));
						if (magnitude2 < num8)
						{
							hotspot = crowdHotspotData;
							num8 = magnitude2;
						}
					}
				}
				crowdNodeData.Hotspot = hotspot;
				crowdNodeData.Flee |= crowdNodeData.Hotspot != null && crowdNodeData.Hotspot.Flee;
				float b = 0f;
				for (int j = 0; j < crowdNodeData.Node.Neighbors.Count; j++)
				{
					int index = crowdNodeData.Node.Neighbors[j];
					b = Mathf.Max(CrowdNodes[index].Node.DynamicWeight, b);
				}
				num4 = Mathf.Lerp(num4, b, DynamicWeightBleed);
				crowdNodeData.Node.DynamicWeight = Mathf.Lerp(crowdNodeData.Node.DynamicWeight, num4, t);
				crowdNodeData.EstimatedPopulation = Damping.Damp(crowdNodeData.EstimatedPopulation, crowdNodeData.Behaviors.Count, EstimatedPopulationDamping, 1f, num);
				if (crowdNodeData.Hotspot != null && crowdNodeData.Hotspot.TriggerReactionType == 14)
				{
					Statistics.SmokeBombedAccumulator += crowdNodeData.EstimatedPopulation;
				}
				if (flag4)
				{
					if (num3 > 3f)
					{
						Statistics.UnitsPanickingHeightDiffAccumulator += crowdNodeData.EstimatedPopulation;
					}
					else
					{
						Statistics.UnitsPanickingAccumulator += crowdNodeData.EstimatedPopulation;
					}
				}
				if (m_lastDynamicWeightUpdateIndex == 0)
				{
					Statistics.SmokeBombed = Mathf.RoundToInt(Statistics.SmokeBombedAccumulator);
					Statistics.UnitsPanicking = Mathf.RoundToInt(Statistics.UnitsPanickingAccumulator);
					Statistics.UnitsPanickingHeightDiff = Mathf.RoundToInt(Statistics.UnitsPanickingHeightDiffAccumulator);
					Statistics.SmokeBombedAccumulator = 0f;
					Statistics.UnitsPanickingAccumulator = 0f;
					Statistics.UnitsPanickingHeightDiffAccumulator = 0f;
				}
			}
		}

		private void PlaceCrowd()
		{
			m_availableCrowd.Clear();
			for (int i = 0; i < CrowdBehaviors.Count; i++)
			{
				CrowdBehavior crowdBehavior = CrowdBehaviors[i];
				if ((double)crowdBehavior.CrowdRelevance.Relevance < BehaviorRelevanceCutoff || !crowdBehavior.enabled)
				{
					m_availableCrowd.Add(crowdBehavior);
				}
			}
			m_availableCrowd.Sort((CrowdBehavior a, CrowdBehavior b) => a.CrowdRelevance.Relevance.CompareTo(b.CrowdRelevance.Relevance));
			BBSimpleList<CrowdNodeRelevance> relevantNodes = RelevantNodes;
			double num = 0.0;
			double num2 = 0.0;
			double num3 = 0.0;
			double val = 0.0;
			for (int j = 0; j < relevantNodes.Count; j++)
			{
				CrowdNodeData nodeData = relevantNodes[j].NodeData;
				CrowdGraph.CrowdNode node = nodeData.Node;
				double num4 = nodeData.Behaviors.Count;
				double num5 = node.NormalizedDensity * node.NormalizedArea * (double)node.DynamicWeight;
				double num6 = (double)CrowdDensity * num5;
				double num7 = Math.Max(0.0, num6 - num4);
				if (num6 > double.Epsilon)
				{
					num2 += num6;
					num += num4;
					CrowdNodeRelevance crowdNodeRelevance = relevantNodes[j];
					double num8 = crowdNodeRelevance.Relevance * num5 * (num7 / num6);
					relevantNodes[j] = new CrowdNodeRelevance
					{
						NodeData = crowdNodeRelevance.NodeData,
						Relevance = crowdNodeRelevance.Relevance,
						Weight = num8
					};
					num3 += num8;
					val = Math.Max(val, num8);
				}
			}
			for (int k = 0; k < relevantNodes.Count; k++)
			{
				CrowdNodeRelevance crowdNodeRelevance2 = relevantNodes[k];
				relevantNodes[k] = new CrowdNodeRelevance
				{
					NodeData = crowdNodeRelevance2.NodeData,
					Relevance = crowdNodeRelevance2.Relevance,
					Weight = crowdNodeRelevance2.Weight / num3
				};
			}
			int l = 0;
			while (num < num2 && l < m_availableCrowd.Count)
			{
				CrowdBehavior crowdBehavior2 = m_availableCrowd[l];
				if (!crowdBehavior2.enabled)
				{
					crowdBehavior2.Enable();
				}
				bool flag = false;
				int num9 = 10;
				while (!flag && num9 > 0)
				{
					num9--;
					double num10 = UnityEngine.Random.value;
					double num11 = 0.0;
					for (int m = 0; m < relevantNodes.Count; m++)
					{
						num11 += relevantNodes[m].Weight;
						if (num10 < num11)
						{
							flag = RepositionOnNode(crowdBehavior2, relevantNodes[m].NodeData);
							if (flag)
							{
								num += 1.0;
							}
							break;
						}
					}
				}
				l++;
			}
			if (DisableUnusedCrowd)
			{
				for (; l < m_availableCrowd.Count; l++)
				{
					CrowdBehavior crowdBehavior3 = m_availableCrowd[l];
					crowdBehavior3.Disable();
				}
			}
			MissingCrowd = Math.Max(0.0, num2 - num);
			CurrentCrowd = num;
			DesiredCrowd = num2;
		}

		public void PreloadCrowdPrefab(GameID _id)
		{
			if (GameID.IsValid(_id))
			{
				AnimatorDef item = Main.SafeGameDb.GetItem<AnimatorDef>(_id);
				GameObject _go = null;
				if (!HumanoidFactoryHelpers.TryGetCachedGo(item.GameID, out _go))
				{
					_go = item.AnimatorPrefab.GetObject<GameObject>();
					HumanoidFactoryHelpers.AddGoToCache(item.GameID, _go);
				}
			}
		}

		public void PreloadCrowd()
		{
			UbiCore.Utils.Singleton<GlobalGameIDs>.Initialize();
			UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.InitializeIDs(Main.SafeGameDb);
			GameDB safeGameDb = Main.SafeGameDb;
			HumanoidFactoryHelpers.PreloadEditorData();
			string[] guidList = CrowdSettings.GuidList;
			foreach (string guid in guidList)
			{
				GameID humandoidGameID = safeGameDb.FindGameIdFromGuid(guid);
				Singleton<HumanoidFactory>.Instance.PreCacheHumanoidFromDB(humandoidGameID, null);
			}
		}

		public List<GameObject> SpawnCrowd(int count)
		{
			GameDB safeGameDb = Main.SafeGameDb;
			PreloadCrowdPrefab(safeGameDb.FindGameIdFromGuid(CrowdSettings.FemalePrefabGuid));
			PreloadCrowdPrefab(safeGameDb.FindGameIdFromGuid(CrowdSettings.MalePrefabGuid));
			List<GameObject> list = new List<GameObject>();
			for (int i = 0; i < count; i++)
			{
				list.Add(SpawnCrowdCharacter());
			}
			return list;
		}

		private GameObject SpawnCrowdCharacter()
		{
			int num = UnityEngine.Random.Range(0, Mathf.FloorToInt((float)CrowdSettings.GuidList.Length * CrowdQuality + 0.9f));
			string guid = CrowdSettings.GuidList[num];
			GameDB safeGameDb = Main.SafeGameDb;
			GameID gameID = safeGameDb.FindGameIdFromGuid(guid);
			CharacterAppearance appearance = new CharacterAppearance();
			HumanoidDef item = safeGameDb.GetItem<HumanoidDef>(gameID);
			GameID overridePrefab = ((item.CharType != HumanoidDef.ECharType.Female) ? safeGameDb.FindGameIdFromGuid(CrowdSettings.MalePrefabGuid) : safeGameDb.FindGameIdFromGuid(CrowdSettings.FemalePrefabGuid));
			Singleton<HumanoidFactory>.Instance.PreCacheHumanoidFromDB(gameID, overridePrefab);
			Singleton<HumanoidFactory>.Instance.RandomizeHumanoid(item, appearance, false);
			GameObject gameObject = Singleton<HumanoidFactory>.Instance.CreateHumanoidFromDefinition(item, false, appearance, null, overridePrefab);
			CrowdBehavior crowdBehavior = gameObject.AddComponent<CrowdBehavior>();
			crowdBehavior.GameID = gameID;
			crowdBehavior.BehaviorTreeXMLPath = "AI/BehaviorTrees/Crowd/CrowdBehavior";
			gameObject.transform.parent = base.transform;
			return gameObject;
		}

		public void ClearCrowd()
		{
			CrowdBehavior[] componentsInChildren = base.gameObject.GetComponentsInChildren<CrowdBehavior>(true);
			CrowdBehavior[] array = componentsInChildren;
			foreach (CrowdBehavior crowdBehavior in array)
			{
				Singleton<HumanoidFactory>.Instance.DestroyHumanoid(crowdBehavior.gameObject);
			}
		}
	}
}
