using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Mission.Interfaces;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Mission.UI
{
	public class MissionTargetIndicatorUI : IUISubSystemLayout, IUiSubSystem
	{
		internal class IconData
		{
			internal Vector3 position;

			internal IconMaterial material;

			internal float distance;
		}

		internal class MiniMapMissionNode
		{
			public readonly IMissionBase MissionNode;

			private readonly BBList<Vector3> m_lerpedTargetPositions = new BBList<Vector3>();

			private readonly BBList<Vector3> m_targetPos = new BBList<Vector3>();

			public MiniMapMissionNode(IMissionBase _node)
			{
				MissionNode = _node;
			}

			public BBList<Vector3> GetTargetPositions(Vector3 _currentPos)
			{
				IObjectiveBase objectiveBase = MissionNode as IObjectiveBase;
				if (objectiveBase != null)
				{
					return SmoothOutPositions(objectiveBase.TargetPositions, _currentPos);
				}
				if (m_targetPos.Count == 0)
				{
					m_targetPos.Add(Vector3.zero);
				}
				MonoBehaviour monoBehaviour = MissionNode as MonoBehaviour;
				m_targetPos[0] = monoBehaviour.transform.position;
				return SmoothOutPositions(m_targetPos, _currentPos);
			}

			private BBList<Vector3> SmoothOutPositions(BBList<Vector3> _newPositions, Vector3 _currentPos)
			{
				float num = 5000f;
				BBList<Vector3> bBList = _newPositions;
				if (_newPositions.Count == m_lerpedTargetPositions.Count)
				{
					bBList = new BBList<Vector3>();
					for (int i = 0; i < _newPositions.Count; i++)
					{
						float val = 1f;
						float sqrMagnitude = (_newPositions[i] - _currentPos).sqrMagnitude;
						if (sqrMagnitude > s_targetMinDistanceToSmooth)
						{
							val = Mathf.Clamp01((1f - Math.Min(sqrMagnitude, num) / num) * s_targetDeltaFactor * Time.deltaTime);
						}
						bBList.Add(Vector3.Lerp(m_lerpedTargetPositions[i], _newPositions[i], Math.Max(val, 0.05f)));
					}
				}
				m_lerpedTargetPositions.Clear();
				for (int j = 0; j < bBList.Count; j++)
				{
					m_lerpedTargetPositions.Add(bBList[j]);
				}
				return m_lerpedTargetPositions;
			}
		}

		public static float s_targetDeltaFactor = 5f;

		public static float s_targetMinDistanceToSmooth = 1000f;

		private BBList<IconData> m_data = new BBList<IconData>();

		private readonly BBList<MiniMapMissionNode> m_missionNodes = new BBList<MiniMapMissionNode>();

		private readonly BBList<MissionTargetIndicatorRenderer> m_rdrInstances = new BBList<MissionTargetIndicatorRenderer>();

		public Vector2 BorderOffset { get; set; }

		public ScreenAnchor ScreenAnchor { get; set; }

		public EUISubSystem SubSystem
		{
			get
			{
				return EUISubSystem.MissionTargetIndicator;
			}
		}

		public bool IsActive { get; private set; }

		public bool InitiallyVisible
		{
			get
			{
				return true;
			}
		}

		public MissionTargetIndicatorUI()
		{
			IsActive = false;
		}

		private void UpdateMissionNodes()
		{
			m_missionNodes.ClearIfNotEmpty();
			BBList<IMissionBase> missionEntries = UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.MissionEntries;
			for (int i = 0; i < missionEntries.Count; i++)
			{
				IMissionBase entry = missionEntries[i];
				if (entry.MapIcon != 0)
				{
					IObjectiveBase objectiveBase = entry as IObjectiveBase;
					if (objectiveBase != null)
					{
						m_missionNodes.Add(new MiniMapMissionNode(entry));
					}
				}
			}
		}

		private IconMaterial TranslateMapIcon(EMapIconType _type)
		{
			IconMaterial result = IconMaterial.Invalid;
			switch (_type)
			{
			case EMapIconType.Target_EMapIconType:
			case EMapIconType.Assassinate_EMapIconType:
			case EMapIconType.Deliver_EMapIconType:
			case EMapIconType.Find_EMapIconType:
			case EMapIconType.Gather_EMapIconType:
			case EMapIconType.Loot_EMapIconType:
			case EMapIconType.Plant_EMapIconType:
			case EMapIconType.Follow_EMapIconType:
			case EMapIconType.Reach_EMapIconType:
			case EMapIconType.UnknownTarget_EMapIconType:
				result = IconMaterial.MissionTargetIndicatorKill;
				break;
			case EMapIconType.Coins_EMapIconType:
			case EMapIconType.Escape_EMapIconType:
			case EMapIconType.Hide_EMapIconType:
				result = IconMaterial.MissionTargetIndicatorTarget;
				break;
			case EMapIconType.FreeCaptured_EMapIconType:
				result = IconMaterial.MissionTargetIndicatorDestination;
				break;
			}
			return result;
		}

		public void UpdateFrame(float _deltaTime)
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.InProgress)
			{
				for (int i = 0; i < m_rdrInstances.Count; i++)
				{
					m_rdrInstances[i].Release();
				}
				m_rdrInstances.ClearIfNotEmpty();
				return;
			}
			Profiling.Start("MissionTargetIndicatorUI.UpdateFrame", float.MaxValue);
			int num = 0;
			int count = m_missionNodes.Count;
			if (count != 0)
			{
				Vector3 position = UnityEngine.Camera.main.transform.position;
				for (int j = 0; j < count; j++)
				{
					MiniMapMissionNode miniMapMissionNode = m_missionNodes[j];
					EProgressState visualTrackerState = miniMapMissionNode.MissionNode.Cast<IObjectiveBase>().VisualTrackerState;
					if (visualTrackerState == EProgressState.Fulfilled || visualTrackerState == EProgressState.Failed)
					{
						continue;
					}
					BBList<Vector3> targetPositions = miniMapMissionNode.GetTargetPositions(position);
					int count2 = targetPositions.Count;
					IconMaterial iconMaterial = TranslateMapIcon(miniMapMissionNode.MissionNode.MapIcon);
					if (iconMaterial != 0)
					{
						for (int k = 0; k < count2; k++)
						{
							float sqrMagnitude = (targetPositions[k] - position).sqrMagnitude;
							m_data.Add(new IconData
							{
								distance = sqrMagnitude,
								material = iconMaterial,
								position = targetPositions[k]
							});
						}
					}
				}
				if (m_data.Count != 0)
				{
					m_data.Sort((IconData data1, IconData data2) => data1.distance.CompareTo(data2.distance));
					int num2 = Math.Min(5, m_data.Count);
					for (int l = 0; l < num2; l++)
					{
						IconData iconData = m_data[num];
						MissionTargetIndicatorRenderer missionTargetIndicatorRenderer = null;
						if (m_rdrInstances.Count <= num)
						{
							missionTargetIndicatorRenderer = MissionTargetIndicatorRenderer.Get(iconData.material);
							m_rdrInstances.Add(missionTargetIndicatorRenderer);
						}
						else
						{
							missionTargetIndicatorRenderer = m_rdrInstances[num];
						}
						missionTargetIndicatorRenderer.SetIcon(iconData.material);
						missionTargetIndicatorRenderer.Position3D = iconData.position;
						missionTargetIndicatorRenderer.Scale = Mathf.Clamp01(Mathf.Sin(Math.Min(iconData.distance, 1000f) / 1000f * (float)Math.PI)) * 0.3f + 0.7f;
						num++;
					}
				}
			}
			while (num < m_rdrInstances.Count)
			{
				m_rdrInstances[num].Release();
				m_rdrInstances.RemoveAt(num);
			}
			m_data.ClearIfNotEmpty();
			Profiling.Stop();
		}

		public bool Tap(TapEvent _event)
		{
			return false;
		}

		public void OnMissionLoad()
		{
		}

		public void OnMissionLoaded()
		{
			IsActive = UbiCore.Utils.Singleton<UiMain>.Instance.MissionType != EMissionType.Animus_EMissionType;
			if (IsActive)
			{
				UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.OnProgress += UpdateMissionNodes;
				UpdateMissionNodes();
			}
		}

		public void OnMissionUnload()
		{
			if (IsActive)
			{
				m_missionNodes.ClearIfNotEmpty();
				for (int i = 0; i < m_rdrInstances.Count; i++)
				{
					m_rdrInstances[i].Release();
				}
				m_rdrInstances.ClearIfNotEmpty();
				UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.OnProgress -= UpdateMissionNodes;
			}
			IsActive = false;
		}

		public void OnMissionUnloaded()
		{
		}

		public bool IsOnUi(Vector2 _mousePosition)
		{
			return false;
		}

		public void OnSceneLoaded()
		{
		}

		public void OnSceneUnload()
		{
		}

		public void OnCameraCreated()
		{
		}

		public void OnSharedUILoaded()
		{
		}

		public void OnMissionEnded()
		{
		}

		public void OnMissionOutroStarted()
		{
		}

		public void OnUIAlignmentChanged(bool _isRightHanded)
		{
		}

		public void OnUIReadyToUse()
		{
		}

		public void SetVisible(bool _value)
		{
			MissionTargetIndicatorRenderer.ForceInvisible = !_value;
		}
	}
}
