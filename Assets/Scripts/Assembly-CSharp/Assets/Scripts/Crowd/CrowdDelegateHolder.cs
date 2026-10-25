using System;
using System.Collections.Generic;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Actions;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components.Conditionals;
using SharedBaseLib.Core;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Crowd
{
	internal class CrowdDelegateHolder : IDelegateHolder
	{
		public static class ParameterHelper
		{
			public static readonly string[] ParameterTypeNames;

			public static readonly Dictionary<int, string[]> DelegateMap;

			public static readonly Dictionary<int, string[]> ParameterMap;

			private static readonly string[] m_actionParameters;

			private static readonly string[] m_conditionalParameters;

			static ParameterHelper()
			{
				m_actionParameters = new string[Enum.GetValues(typeof(EActionParameterizedType)).Length];
				m_actionParameters[0] = EParametersType.FloatParameters.ToString();
				m_actionParameters[1] = EParametersType.RandomFloatParameters.ToString();
				m_conditionalParameters = new string[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
				m_conditionalParameters[0] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[1] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[2] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[3] = EParametersType.CompareFloatParameters.ToString();
				m_conditionalParameters[4] = EParametersType.CompareFloatParameters.ToString();
				ParameterTypeNames = Enum.GetNames(typeof(EParametersType));
				ParameterMap = new BBDictionary<int, string[]>
				{
					{ 2, m_actionParameters },
					{ 9, m_conditionalParameters }
				};
				DelegateMap = new BBDictionary<int, string[]>
				{
					{
						1,
						Enum.GetNames(typeof(EActionType))
					},
					{
						2,
						Enum.GetNames(typeof(EActionParameterizedType))
					},
					{
						8,
						Enum.GetNames(typeof(EConditionalType))
					},
					{
						9,
						Enum.GetNames(typeof(EConditionalParameterizedType))
					}
				};
			}

			public static IParameterContainer CreateParameterContainer(EParametersType parametersType)
			{
				switch (parametersType)
				{
				case EParametersType.CompareFloatParameters:
					return new CompareFloatParameters();
				case EParametersType.CompareIntParameters:
					return new CompareIntParameters();
				case EParametersType.FloatParameters:
					return new FloatParameters();
				case EParametersType.IntParameters:
					return new IntParameters();
				case EParametersType.RandomFloatParameters:
					return new RandomFloatParameters();
				case EParametersType.StringParameters:
					return new StringParameters();
				default:
					return null;
				}
			}
		}

		private BehaviorAction.DelegateType[] m_actionDelegates;

		private BehaviorAction.CancelDelegateType[] m_actionCancelDelegates;

		private BehaviorAction.AttachDelegateType[] m_actionAttachDelegates;

		private BehaviorActionParameterized.DelegateType[] m_actionParameterizedDelegates;

		private BehaviorActionParameterized.CancelDelegateType[] m_actionParameterizedCancelDelegates;

		private BehaviorActionParameterized.AttachDelegateType[] m_actionParameterizedAttachDelegates;

		private Conditional.DelegateType[] m_conditionalDelegates;

		private ConditionalParameterized.DelegateType[] m_conditionalParameterizedDelegates;

		private CrowdBehavior CrowdBehavior { get; set; }

		public CrowdDelegateHolder()
		{
			CreateActionDelegates();
			CreateActionParameterizedDelegates();
			CreateConditionalDelegates();
			CreateConditionalParameterizedDelegates();
		}

		public void Prepare(CrowdBehaviorRuntimeData _runtimeData)
		{
			CrowdBehavior = ((_runtimeData == null) ? null : _runtimeData.CrowdBehavior);
		}

		public BehaviorReturnCode Stop(BehaviorReturnCode _lastReturnCode)
		{
			CrowdBehavior.Agent.Stop();
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode FollowCrowdGraph(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				CrowdBehavior.CrowdFollowCrowdGraph.FollowCrowdGraphInit();
			}
			_lastReturnCode = BehaviorReturnCode.Running;
			if (!CrowdBehavior.CrowdFollowCrowdGraph.FollowCrowdGraphUpdate())
			{
				_lastReturnCode = BehaviorReturnCode.Failure;
			}
			if (CrowdBehavior.CrowdDetectIsStuck.IsStuck)
			{
				_lastReturnCode = BehaviorReturnCode.Failure;
			}
			return _lastReturnCode;
		}

		public BehaviorReturnCode FollowPath(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				CrowdBehavior.CrowdFollowPath.FollowPathInit();
			}
			bool flag = CrowdBehavior.Agent.remainingDistance <= CrowdBehavior.Agent.stoppingDistance;
			if (CrowdBehavior.Agent.hasPath && !flag)
			{
				_lastReturnCode = BehaviorReturnCode.Running;
				CrowdBehavior.CrowdFollowPath.FollowPathUpdate();
				if (CrowdBehavior.CrowdDetectIsStuck.IsStuck)
				{
					_lastReturnCode = BehaviorReturnCode.Failure;
				}
			}
			else
			{
				_lastReturnCode = (CrowdBehavior.Agent.pathPending ? BehaviorReturnCode.Running : ((!flag) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success));
			}
			return _lastReturnCode;
		}

		public BehaviorReturnCode ReactToHotspot(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				CrowdBehavior.CrowdReactToHotspot.PlayHotspotReactionInit();
			}
			_lastReturnCode = BehaviorReturnCode.Failure;
			if (CrowdBehavior.CrowdReactToHotspot.Hotspot != null)
			{
				_lastReturnCode = ((!CrowdBehavior.CrowdReactToHotspot.PlayHotspotReactionUpdate()) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running);
			}
			return _lastReturnCode;
		}

		public void CancelReactToHotspot()
		{
			CrowdBehavior.CrowdReactToHotspot.Reset();
		}

		public BehaviorReturnCode StopAtStation(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				CrowdBehavior.CrowdStopAtStation.StopAtStationInit();
			}
			_lastReturnCode = BehaviorReturnCode.Failure;
			if (CrowdBehavior.CrowdStopAtStation.StationData != null)
			{
				_lastReturnCode = ((!CrowdBehavior.CrowdStopAtStation.StopAtStationUpdate()) ? BehaviorReturnCode.Success : BehaviorReturnCode.Running);
			}
			return _lastReturnCode;
		}

		public void CancelStopAtStation()
		{
			CrowdBehavior.CrowdStopAtStation.LeaveStation();
		}

		public BehaviorReturnCode SetDestinationToNextNode(BehaviorReturnCode _lastReturnCode)
		{
			return (!CrowdBehavior.SetDestinationToNextNode()) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode SetDestinationToRandomPoint(BehaviorReturnCode _lastReturnCode)
		{
			NavMeshPath pathToRandomDestination = CrowdGraph.Instance.GetPathToRandomDestination(CrowdBehavior.Agent);
			if (pathToRandomDestination == null)
			{
				return BehaviorReturnCode.Failure;
			}
			CrowdBehavior.Agent.SetPath(pathToRandomDestination);
			CrowdBehavior.Agent.Resume();
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode SetDestinationToRandomStation(BehaviorReturnCode _lastReturnCode)
		{
			NavMeshPath pathToRandomCrowdStation = CrowdGraph.Instance.GetPathToRandomCrowdStation(CrowdBehavior.Agent);
			if (pathToRandomCrowdStation == null)
			{
				return BehaviorReturnCode.Failure;
			}
			CrowdBehavior.Agent.SetPath(pathToRandomCrowdStation);
			CrowdBehavior.Agent.Resume();
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode SetSpeed(IParameterContainer _parameter, BehaviorReturnCode _lastReturnCode)
		{
			FloatParameters floatParameters = _parameter as FloatParameters;
			CrowdBehavior.Agent.speed = Mathf.Max(0f, floatParameters.Value);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode SetRandomSpeed(IParameterContainer _parameter, BehaviorReturnCode _lastReturnCode)
		{
			RandomFloatParameters randomFloatParameters = _parameter as RandomFloatParameters;
			CrowdBehavior.Agent.speed = Mathf.Max(0f, randomFloatParameters.Value);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode Disable(BehaviorReturnCode _lastReturnCode)
		{
			CrowdBehavior.Disable();
			return BehaviorReturnCode.Success;
		}

		public bool HasDestination()
		{
			return CrowdBehavior.Agent.hasPath || CrowdBehavior.Agent.pathPending;
		}

		public bool DistanceToNextPathCorner(IParameterContainer _parameter)
		{
			float v = 0f;
			if (CrowdBehavior.Agent.hasPath)
			{
				v = (CrowdBehavior.Agent.steeringTarget - CrowdBehavior.Position).magnitude;
			}
			CompareFloatParameters compareFloatParameters = _parameter as CompareFloatParameters;
			return compareFloatParameters.Evaluate(v);
		}

		public bool DistanceToNavMeshEdge(IParameterContainer _parameter)
		{
			float v = ((!CrowdBehavior.HasNavMeshEdgeHit) ? float.MaxValue : CrowdBehavior.NavMeshEdgeHit.distance);
			CompareFloatParameters compareFloatParameters = _parameter as CompareFloatParameters;
			return compareFloatParameters.Evaluate(v);
		}

		public bool RandomChance(IParameterContainer _parameter)
		{
			float value = UnityEngine.Random.value;
			CompareFloatParameters compareFloatParameters = _parameter as CompareFloatParameters;
			return compareFloatParameters.Evaluate(value);
		}

		public bool DistanceToClosestNeighbor(IParameterContainer _parameter)
		{
			BBSimpleList<CrowdAvoidance.Neighbor> neighbors = CrowdBehavior.CrowdAvoidance.Neighbors;
			float num = float.MaxValue;
			for (int i = 0; i < neighbors.Count; i++)
			{
				CrowdAvoidance.Neighbor neighbor = neighbors[i];
				if (neighbor.CrowdBehavior == CrowdBehavior)
				{
					continue;
				}
				Vector3 lhs = neighbor.Position - CrowdBehavior.Position;
				float magnitude = lhs.magnitude;
				float num2 = Vector3.Dot(lhs, CrowdBehavior.Rotation * Vector3.forward) / magnitude;
				if (!(num2 <= 0f))
				{
					magnitude *= 2f - Mathf.Abs(num2);
					if (magnitude < num)
					{
						num = magnitude;
					}
				}
			}
			CompareFloatParameters compareFloatParameters = _parameter as CompareFloatParameters;
			return compareFloatParameters.Evaluate(num);
		}

		public bool DistanceToDestination(IParameterContainer _parameter)
		{
			float v = 0f;
			if (CrowdBehavior.Agent.hasPath)
			{
				v = CrowdBehavior.Agent.remainingDistance;
			}
			CompareFloatParameters compareFloatParameters = _parameter as CompareFloatParameters;
			return compareFloatParameters.Evaluate(v);
		}

		public BehaviorAction.DelegateType GetActionDelegate(string _name)
		{
			return m_actionDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.CancelDelegateType GetActionCancelDelegate(string _name)
		{
			return m_actionCancelDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorAction.AttachDelegateType GetActionAttachDelegate(string _name)
		{
			return m_actionAttachDelegates[ConvertActionStringToIndex(_name)];
		}

		public BehaviorActionParameterized.DelegateType GetActionParameterizedDelegate(string _name)
		{
			return m_actionParameterizedDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.CancelDelegateType GetActionParameterizedCancelDelegate(string _name)
		{
			return m_actionParameterizedCancelDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public BehaviorActionParameterized.AttachDelegateType GetActionParameterizedAttachDelegate(string _name)
		{
			return m_actionParameterizedAttachDelegates[ConvertActionParameterizedStringToIndex(_name)];
		}

		public Conditional.DelegateType GetConditionalDelegate(string _name)
		{
			return m_conditionalDelegates[ConvertConditionalStringToIndex(_name)];
		}

		public ConditionalParameterized.DelegateType GetConditionalParameterizedDelegate(string _name)
		{
			return m_conditionalParameterizedDelegates[ConvertConditionalParameterizedStringToIndex(_name)];
		}

		public IParameterContainer CreateActionParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[2];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateConditionalParameterContainer(int _index)
		{
			string[] array = ParameterHelper.ParameterMap[9];
			return CreateParameterContainer(array[_index]);
		}

		public IParameterContainer CreateParameterContainer(string _parameterName)
		{
			EParametersType parametersType = (EParametersType)(int)Enum.Parse(typeof(EParametersType), _parameterName);
			return ParameterHelper.CreateParameterContainer(parametersType);
		}

		public string ConvertActionIndexToString(int _index)
		{
			return ((EActionType)_index).ToString();
		}

		public string ConvertActionParameterizedIndexToString(int _index)
		{
			return ((EActionParameterizedType)_index).ToString();
		}

		public string ConvertConditionalIndexToString(int _index)
		{
			return ((EConditionalType)_index).ToString();
		}

		public string ConvertConditionalParameterizedIndexToString(int _index)
		{
			return ((EConditionalParameterizedType)_index).ToString();
		}

		public int ConvertActionStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionType), _name);
		}

		public int ConvertActionParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EActionParameterizedType), _name);
		}

		public int ConvertConditionalStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalType), _name);
		}

		public int ConvertConditionalParameterizedStringToIndex(string _name)
		{
			return (int)Enum.Parse(typeof(EConditionalParameterizedType), _name);
		}

		public string[] GetParameterTypeNames()
		{
			return ParameterHelper.ParameterTypeNames;
		}

		public string[] GetDelegateNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.DelegateMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.DelegateMap[(int)_behaviorType];
			}
			return null;
		}

		public string[] GetParameterNames(EBehaviorType _behaviorType)
		{
			if (ParameterHelper.ParameterMap.ContainsKey((int)_behaviorType))
			{
				return ParameterHelper.ParameterMap[(int)_behaviorType];
			}
			return null;
		}

		public BehaviorAction.AttachDelegateType GetDefaultAttachDelegate()
		{
			return m_actionAttachDelegates[0];
		}

		private void CreateActionDelegates()
		{
			m_actionDelegates = new BehaviorAction.DelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionCancelDelegates = new BehaviorAction.CancelDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionAttachDelegates = new BehaviorAction.AttachDelegateType[Enum.GetValues(typeof(EActionType)).Length];
			m_actionDelegates[1] = FollowPath;
			m_actionDelegates[0] = Stop;
			m_actionDelegates[3] = FollowCrowdGraph;
			m_actionDelegates[2] = Disable;
			m_actionDelegates[4] = SetDestinationToRandomPoint;
			m_actionDelegates[5] = SetDestinationToNextNode;
			m_actionDelegates[6] = SetDestinationToRandomStation;
			m_actionDelegates[7] = StopAtStation;
			m_actionDelegates[8] = ReactToHotspot;
			m_actionCancelDelegates[7] = CancelStopAtStation;
			m_actionCancelDelegates[8] = CancelReactToHotspot;
		}

		private void CreateActionParameterizedDelegates()
		{
			m_actionParameterizedDelegates = new BehaviorActionParameterized.DelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedCancelDelegates = new BehaviorActionParameterized.CancelDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedAttachDelegates = new BehaviorActionParameterized.AttachDelegateType[Enum.GetValues(typeof(EActionParameterizedType)).Length];
			m_actionParameterizedDelegates[0] = SetSpeed;
			m_actionParameterizedDelegates[1] = SetRandomSpeed;
		}

		private void CreateConditionalDelegates()
		{
			m_conditionalDelegates = new Conditional.DelegateType[Enum.GetValues(typeof(EConditionalType)).Length];
			m_conditionalDelegates[0] = HasDestination;
		}

		private void CreateConditionalParameterizedDelegates()
		{
			m_conditionalParameterizedDelegates = new ConditionalParameterized.DelegateType[Enum.GetValues(typeof(EConditionalParameterizedType)).Length];
			m_conditionalParameterizedDelegates[0] = RandomChance;
			m_conditionalParameterizedDelegates[1] = DistanceToNextPathCorner;
			m_conditionalParameterizedDelegates[2] = DistanceToNavMeshEdge;
			m_conditionalParameterizedDelegates[3] = DistanceToDestination;
			m_conditionalParameterizedDelegates[4] = DistanceToClosestNeighbor;
		}
	}
}
