using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Mission.Interfaces;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.MiniMap
{
	public class UIMiniMapMovingAreas
	{
		private class MovingArea
		{
			public UISprite Sprite;

			public GameObject AreaGameObject;

			public MissionAreaMoving MissionArea;
		}

		private readonly List<MovingArea> m_movingAreas = new List<MovingArea>();

		private readonly float m_visibleSize;

		private GameObject m_goRoot;

		private UnityEngine.Object m_iconTemplet;

		private UIMapData m_uiMapData;

		public UIMiniMapMovingAreas(GameObject _parent, UnityEngine.Object _iconTemplet, float _visibleSize)
		{
			m_iconTemplet = _iconTemplet;
			m_visibleSize = _visibleSize;
			m_goRoot = NGUITools.AddChild(_parent);
			m_goRoot.name = "MovingAreas";
		}

		public void OnMissionLoaded(UIMapData _uiMapData)
		{
			m_uiMapData = _uiMapData;
			UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet += OnAreasChanged;
			CreateAreasInternal();
		}

		private void CreateAreasInternal()
		{
			List<IMissionAreaBase> areas = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.GetAreas();
			for (int i = 0; i < areas.Count; i++)
			{
				MissionAreaMoving missionAreaMoving = areas[i] as MissionAreaMoving;
				if (!(missionAreaMoving == null) && !missionAreaMoving.Data.DoNotRenderOnMiniMap)
				{
					MovingArea movingArea = new MovingArea();
					movingArea.MissionArea = missionAreaMoving;
					movingArea.AreaGameObject = NGUITools.AddChild(m_goRoot, (GameObject)m_iconTemplet);
					MovingArea movingArea2 = movingArea;
					movingArea2.Sprite = movingArea2.AreaGameObject.GetComponent<UISprite>();
					string spriteName = GetSpriteName(missionAreaMoving.AreaData.Type);
					UISpriteData sprite = movingArea2.Sprite.atlas.GetSprite(spriteName);
					if (sprite != null)
					{
						movingArea2.Sprite.spriteName = spriteName;
					}
					movingArea2.Sprite.depth = 0;
					movingArea2.Sprite.MakePixelPerfect();
					m_movingAreas.Add(movingArea2);
				}
			}
		}

		private string GetSpriteName(EMissionAreaType _type)
		{
			if (_type == EMissionAreaType.Restricted)
			{
				return "icon_minimap_moving_area_restricted";
			}
			return "icon_minimap_moving_area_escort";
		}

		public void OnMissionUnload()
		{
			m_iconTemplet = null;
			m_goRoot = null;
			m_uiMapData = null;
			UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.OnDirtyFlagSet -= OnAreasChanged;
			ClearAreas();
		}

		private void ClearAreas()
		{
			for (int i = 0; i < m_movingAreas.Count; i++)
			{
				UnityEngine.Object.Destroy(m_movingAreas[i].AreaGameObject);
			}
			m_movingAreas.Clear();
		}

		public void Update(ref Vector2 _mapSize, ref Vector2 _playerPosition, ref Vector2 _bgSize, float _bgRadius)
		{
			for (int i = 0; i < m_movingAreas.Count; i++)
			{
				MovingArea movingArea = m_movingAreas[i];
				bool flag = UpdateMovingArea(movingArea, ref _mapSize, ref _bgSize, ref _playerPosition, _bgRadius);
				if (movingArea.AreaGameObject.activeSelf != flag)
				{
					movingArea.AreaGameObject.SetActive(flag);
				}
			}
		}

		private void OnAreasChanged()
		{
			ClearAreas();
			CreateAreasInternal();
		}

		private bool UpdateMovingArea(MovingArea _movingArea, ref Vector2 _mapSize, ref Vector2 _bgSize, ref Vector2 _playerPosition, float _bgRadius)
		{
			if (_movingArea.MissionArea.Data.DoNotRenderOnMiniMap)
			{
				return false;
			}
			EntityId entityID = _movingArea.MissionArea.TargetEntity.EntityID;
			if (entityID == EntityId.InvalidId)
			{
				return false;
			}
			GameEntity gameEntity = entityID.GameEntity;
			if (gameEntity == null)
			{
				return false;
			}
			Character character = gameEntity as Character;
			if (character != null && character.GetIsDead())
			{
				return false;
			}
			float num = _movingArea.MissionArea.Data.Radius / _mapSize.x / m_visibleSize * _bgSize.x;
			_movingArea.Sprite.width = (int)(num * 2f);
			_movingArea.Sprite.height = (int)(num * 2f);
			Vector2 vector = new Vector2((gameEntity.Position.x - m_uiMapData.m_startPos.x) / _mapSize.x, (gameEntity.Position.z - m_uiMapData.m_startPos.y) / _mapSize.y);
			vector.x = (vector.x - _playerPosition.x) / m_visibleSize;
			vector.y = (vector.y - _playerPosition.y) / m_visibleSize;
			_movingArea.AreaGameObject.transform.localPosition = new Vector3(vector.x * _bgSize.x, vector.y * _bgSize.y, 0f);
			return _bgRadius >= _movingArea.AreaGameObject.transform.localPosition.magnitude - num;
		}
	}
}
