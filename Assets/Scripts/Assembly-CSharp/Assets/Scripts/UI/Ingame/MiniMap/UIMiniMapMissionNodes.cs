using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Mission.Interfaces;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.MiniMap
{
	public class UIMiniMapMissionNodes
	{
		private class MiniMapMissionNode
		{
			public readonly List<GameObject> GameObjects = new List<GameObject>();

			public readonly IMissionBase MissionNode;

			private readonly List<Vector3> m_targetPositions = new List<Vector3>();

			public MiniMapMissionNode(IMissionBase _node)
			{
				MissionNode = _node;
			}

			public void Destroy()
			{
				for (int i = 0; i < GameObjects.Count; i++)
				{
					Object.Destroy(GameObjects[i]);
				}
				GameObjects.Clear();
			}

			public List<Vector3> GetTargetPositions()
			{
				IObjectiveBase objectiveBase = MissionNode as IObjectiveBase;
				if (objectiveBase != null)
				{
					return objectiveBase.TargetPositions;
				}
				if (MissionNode.MapIcon == EMapIconType.Invalid_EMapIconType)
				{
					return m_targetPositions;
				}
				if (m_targetPositions.Count == 0)
				{
					m_targetPositions.Add(Vector3.zero);
				}
				MonoBehaviour monoBehaviour = MissionNode as MonoBehaviour;
				m_targetPositions[0] = monoBehaviour.transform.position;
				return m_targetPositions;
			}

			public override string ToString()
			{
				return MissionNode.MissionData.ToString();
			}
		}

		private readonly List<MiniMapMissionNode> m_missionNodes = new List<MiniMapMissionNode>();

		private readonly float m_visibleSize;

		private GameObject m_goRoot;

		private Object m_iconTemplet;

		private UIMapData m_uiMapData;

		public UIMiniMapMissionNodes(GameObject _parent, Object _iconTemplate, float _visibleSize)
		{
			m_iconTemplet = _iconTemplate;
			m_visibleSize = _visibleSize;
			m_goRoot = NGUITools.AddChild(_parent);
			m_goRoot.name = "MissionNodes";
		}

		public void OnMissionLoaded(UIMapData _uiMapData)
		{
			m_uiMapData = _uiMapData;
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.OnProgress += OnMissionProgressed;
			UpdateMissionNodes();
		}

		public void OnMissionUnload()
		{
			UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.OnProgress -= OnMissionProgressed;
			for (int i = 0; i < m_missionNodes.Count; i++)
			{
				m_missionNodes[i].Destroy();
			}
			m_missionNodes.Clear();
			m_iconTemplet = null;
			m_goRoot = null;
			m_uiMapData = null;
		}

		private void OnMissionProgressed()
		{
			UpdateMissionNodes();
		}

		private void UpdateMissionNodes()
		{
			for (int i = 0; i < m_missionNodes.Count; i++)
			{
				m_missionNodes[i].Destroy();
			}
			m_missionNodes.Clear();
			HashSet<IMissionBase> hashSet = new HashSet<IMissionBase>();
			foreach (IMissionBase missionEntry in UbiCore.Utils.Singleton<MissionManager>.Instance.Phase.MissionEntries)
			{
				hashSet.Add(missionEntry);
			}
			foreach (IMissionBase item2 in hashSet)
			{
				if (item2.MapIcon != 0)
				{
					MiniMapMissionNode item = new MiniMapMissionNode(item2);
					m_missionNodes.Add(item);
				}
			}
		}

		public void Update(ref Vector2 _mapSize, ref Vector2 _playerPosition, ref Vector2 _bgSize, float _bgRadius, float _cameraRotation)
		{
			int count = m_missionNodes.Count;
			for (int i = 0; i < count; i++)
			{
				MiniMapMissionNode miniMapMissionNode = m_missionNodes[i];
				List<Vector3> targetPositions = miniMapMissionNode.GetTargetPositions();
				int count2 = targetPositions.Count;
				if (count2 != miniMapMissionNode.GameObjects.Count)
				{
					miniMapMissionNode.Destroy();
					for (int j = 0; j < count2; j++)
					{
						GameObject gameObject = AddMapIcon(miniMapMissionNode.MissionNode.MapIcon);
						if (gameObject != null)
						{
							miniMapMissionNode.GameObjects.Add(gameObject);
						}
					}
				}
				for (int k = 0; k < count2; k++)
				{
					GameObject gameObject2 = miniMapMissionNode.GameObjects[k];
					bool flag = IsVisibleOnMap(miniMapMissionNode.MissionNode, _mapSize, _playerPosition);
					Vector2 vector = Vector2.zero;
					if (flag)
					{
						Vector2 vector2 = new Vector2((targetPositions[k].x - m_uiMapData.m_startPos.x) / _mapSize.x, (targetPositions[k].z - m_uiMapData.m_startPos.y) / _mapSize.y);
						vector2.x = (vector2.x - _playerPosition.x) / m_visibleSize;
						vector2.y = (vector2.y - _playerPosition.y) / m_visibleSize;
						vector = new Vector2(vector2.x * _bgSize.x, vector2.y * _bgSize.y);
						bool isOnMap = true;
						if (vector.magnitude > _bgRadius)
						{
							isOnMap = false;
							vector.Normalize();
							vector *= _bgRadius;
						}
						flag = IsIconVisible(miniMapMissionNode, isOnMap);
					}
					if (gameObject2.activeSelf != flag)
					{
						gameObject2.SetActive(flag);
					}
					if (flag)
					{
						gameObject2.transform.localRotation = Quaternion.Euler(0f, 0f, _cameraRotation);
						gameObject2.transform.localPosition = new Vector3(vector.x, vector.y, 0f);
					}
				}
			}
		}

		private float pointSegmentDistance(Vector2 _point, Vector2 _lineA, Vector2 _lineB)
		{
			Vector2 rhs = _point - _lineA;
			Vector2 vector = _lineB - _lineA;
			float num = Vector2.Dot(vector, rhs);
			if (num <= 0f)
			{
				return rhs.magnitude;
			}
			float num2 = Vector2.Dot(vector, vector);
			if (num2 <= num)
			{
				return (_lineB - _point).magnitude;
			}
			float num3 = num / num2;
			Vector2 vector2 = _lineA + num3 * vector;
			return (_point - vector2).magnitude;
		}

		private bool IsVisibleOnMap(IMissionBase _missionBase, Vector2 _mapSize, Vector2 _playerPosition)
		{
			if (_missionBase.MapIcon == EMapIconType.Invalid_EMapIconType)
			{
				return false;
			}
			if (!(_missionBase is MissionArea))
			{
				return true;
			}
			float num = _mapSize.x * m_visibleSize * 0.75f * 0.5f;
			MissionArea missionArea = _missionBase as MissionArea;
			if (missionArea == null)
			{
				return false;
			}
			_playerPosition = Vector2.Scale(_playerPosition, _mapSize) + m_uiMapData.m_startPos;
			if (missionArea.IsInside(new Vector3(_playerPosition.x, 0f, _playerPosition.y)))
			{
				return false;
			}
			int count = missionArea.AreaNodes.Count;
			for (int i = 0; i < count; i++)
			{
				AreaNode areaNode = missionArea.AreaNodes[i];
				AreaNode areaNode2 = missionArea.AreaNodes[(i + 1) % count];
				float num2 = pointSegmentDistance(_playerPosition, new Vector2(areaNode.transform.position.x, areaNode.transform.position.z), new Vector2(areaNode2.transform.position.x, areaNode2.transform.position.z));
				if (num2 < num)
				{
					return false;
				}
			}
			return true;
		}

		private GameObject AddMapIcon(EMapIconType _iconType)
		{
			if (_iconType == EMapIconType.Invalid_EMapIconType)
			{
				return null;
			}
			GameObject gameObject = NGUITools.AddChild(m_goRoot, (GameObject)m_iconTemplet);
			UISprite component = gameObject.GetComponent<UISprite>();
			UIMiniMapUtil.SetSpriteByMapIconType(_iconType, component, true, false);
			return gameObject;
		}

		private static bool IsIconVisible(MiniMapMissionNode _miniMapMissionNode, bool _isOnMap)
		{
			if (!_miniMapMissionNode.MissionNode.IsActive)
			{
				return false;
			}
			if (_miniMapMissionNode.MissionNode.MapIcon == EMapIconType.Invalid_EMapIconType)
			{
				return false;
			}
			bool flag = _isOnMap || _miniMapMissionNode.MissionNode.MissionData.ClampMapIconOnBorder;
			if (flag && _miniMapMissionNode.MissionNode is MissionArea && _isOnMap)
			{
				return false;
			}
			return flag;
		}
	}
}
