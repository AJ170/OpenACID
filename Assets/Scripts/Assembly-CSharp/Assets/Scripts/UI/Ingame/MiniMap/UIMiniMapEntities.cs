using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.WithoutAgent;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.MiniMap
{
	public class UIMiniMapEntities
	{
		private class MiniMapEntity
		{
			public EntityId ID;

			public GameEntity GameEntity;

			public Entity Entity;

			public GameObject Go;

			public Transform Transform;

			public bool UpdateRotation;

			public readonly List<EntityId> GroupedEntityIds = new List<EntityId>();

			public MiniMapEntity(EntityId _id, Entity _entity, GameEntity _gameEntity, GameObject _go, Transform _transform, bool _updateRotation)
			{
				ID = _id;
				GameEntity = _gameEntity;
				Entity = _entity;
				Go = _go;
				Transform = _transform;
				UpdateRotation = _updateRotation;
				if (Go.activeSelf)
				{
					DemandFrameUpdate(true);
				}
			}

			public void SetActive(bool _active)
			{
				if (Go.activeSelf != _active)
				{
					Go.SetActive(_active);
					DemandFrameUpdate(_active);
				}
			}

			private void DemandFrameUpdate(bool _demand)
			{
				Helper.DemandNavAgentSurrogateFrameUpdate(Entity, _demand);
			}

			public override string ToString()
			{
				return GameEntity.ToString();
			}
		}

		private readonly List<MiniMapEntity> m_entities = new List<MiniMapEntity>();

		private readonly float m_visibleSize;

		private GameObject m_goRoot;

		private UIMapData m_uiMapData;

		private UnityEngine.Object m_iconTemplet;

		public UIMiniMapEntities(GameObject _parent, UnityEngine.Object _iconTemplet, float _visibleSize)
		{
			m_iconTemplet = _iconTemplet;
			m_visibleSize = _visibleSize;
			m_goRoot = NGUITools.AddChild(_parent);
			m_goRoot.name = "Entities";
		}

		public void OnMissionLoaded(UIMapData _uiMapData)
		{
			m_uiMapData = _uiMapData;
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<EntityRegisteredEvent>(OnEntityRegistered);
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<EntityMapIconChangedEvent>(OnEntityMapIconChanged);
		}

		public void OnMissionUnload()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<EntityRegisteredEvent>(OnEntityRegistered);
			UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<EntityMapIconChangedEvent>(OnEntityMapIconChanged);
			m_iconTemplet = null;
			m_goRoot = null;
			m_uiMapData = null;
			for (int i = 0; i < m_entities.Count; i++)
			{
				UnityEngine.Object.Destroy(m_entities[i].Go);
			}
			m_entities.Clear();
		}

		public void Update(ref Vector2 _mapSize, ref Vector2 _playerPosition, ref Vector2 _bgSize, float _bgRadius, float _cameraRotation)
		{
			int count = m_entities.Count;
			for (int i = 0; i < count; i++)
			{
				MiniMapEntity miniMapEntity = m_entities[i];
				GameEntity gameEntity = miniMapEntity.GameEntity;
				if (!gameEntity.IsValid)
				{
					continue;
				}
				Character character = gameEntity as Character;
				if (character != null && (character.Id == EntityId.InvalidId || character.GetIsDead()))
				{
					miniMapEntity.SetActive(false);
					continue;
				}
				Vector3 vector = ((gameEntity is Patrol) ? gameEntity.As<Patrol>().PatrolComponent.Formation.CalcCurrentMemberCenter() : ((!(gameEntity is StalkingZone)) ? gameEntity.Position : gameEntity.As<StalkingZone>().StalkingZoneComponent.Center));
				Vector2 vector2 = new Vector2((vector.x - m_uiMapData.m_startPos.x) / _mapSize.x, (vector.z - m_uiMapData.m_startPos.y) / _mapSize.y);
				vector2.x = (vector2.x - _playerPosition.x) / m_visibleSize;
				vector2.y = (vector2.y - _playerPosition.y) / m_visibleSize;
				if (IsEntityClamped(miniMapEntity) && vector2.sqrMagnitude >= 0.24f)
				{
					vector2.Normalize();
					vector2 *= 0.48f;
				}
				bool flag = vector2.sqrMagnitude <= 0.26f && IsVisibleByCondition(miniMapEntity);
				miniMapEntity.SetActive(flag);
				if (flag)
				{
					if (miniMapEntity.UpdateRotation)
					{
						miniMapEntity.Transform.localRotation = Quaternion.Euler(0f, 0f, 0f - gameEntity.Rotation.eulerAngles.y);
					}
					else
					{
						miniMapEntity.Transform.localRotation = Quaternion.Euler(0f, 0f, _cameraRotation);
					}
					miniMapEntity.Transform.localPosition = new Vector3(vector2.x * _bgSize.x, vector2.y * _bgSize.y, 0f);
				}
			}
		}

		private static bool IsEntityClamped(MiniMapEntity _miniMapEntity)
		{
			return (_miniMapEntity.Entity is HideoutObject || _miniMapEntity.Entity is StalkingZone) && UbiCore.Utils.Singleton<MissionManager>.Instance.ShowHidingSpots;
		}

		private static bool IsVisibleByCondition(MiniMapEntity _miniMapEntity)
		{
			bool result = true;
			if (_miniMapEntity.Entity is HideoutObject || _miniMapEntity.Entity is StalkingZone)
			{
				result = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel >= EAlertLevel.Alerted || UbiCore.Utils.Singleton<MissionManager>.Instance.ShowHidingSpots;
			}
			else if (_miniMapEntity.GameEntity is Codex)
			{
				Codex codex = (Codex)_miniMapEntity.GameEntity;
				result = codex.CanInteract();
			}
			else if (_miniMapEntity.GameEntity is Patrol)
			{
				Patrol patrol = (Patrol)_miniMapEntity.GameEntity;
				result = patrol.CanInteract();
			}
			else if (_miniMapEntity.GameEntity is StaticObject)
			{
				result = false;
			}
			else if (_miniMapEntity.GameEntity is EnemyCharacter)
			{
				EnemyCharacter entity = (EnemyCharacter)_miniMapEntity.GameEntity;
				result = !entity.GetIsInDisguise();
			}
			else if (_miniMapEntity.GameEntity is InteractionCharacter)
			{
				InteractionCharacter entity2 = (InteractionCharacter)_miniMapEntity.GameEntity;
				result = entity2.CanInteract();
			}
			else if (_miniMapEntity.GameEntity is InteractableMissionObject)
			{
				InteractableMissionObject interactableMissionObject = (InteractableMissionObject)_miniMapEntity.GameEntity;
				result = interactableMissionObject.CanInteract();
			}
			return result;
		}

		private void OnEntityMapIconChanged(EntityMapIconChangedEvent _event)
		{
			if (GetMapEntityByEntityId(_event.Entity.Id) != null && (!RemoveEntityFromList(_event.Entity.Id) || _event.MapIconType != 0))
			{
				MiniMapEntity miniMapEntity = CreateMiniMapEntity(_event.Entity.Id);
				if (miniMapEntity != null)
				{
					m_entities.Add(miniMapEntity);
				}
			}
		}

		private void OnEntityRegistered(EntityRegisteredEvent _event)
		{
			if (_event.Registered)
			{
				if (_event.EntityId == EntityId.InvalidId || _event.EntityId.GameEntity == null)
				{
					return;
				}
				MiniMapEntity mapEntityByEntityId = GetMapEntityByEntityId(_event.EntityId);
				if (mapEntityByEntityId == null)
				{
					MiniMapEntity miniMapEntity = CreateMiniMapEntity(_event.EntityId);
					if (miniMapEntity != null)
					{
						m_entities.Add(miniMapEntity);
					}
				}
			}
			else
			{
				RemoveEntityFromList(_event.EntityId);
			}
		}

		private GameEntity GetGameEntityFromEntity(Entity _entity)
		{
			GameEntity gameEntity = null;
			NpcCharacter npcCharacter = _entity as NpcCharacter;
			if (npcCharacter != null && npcCharacter.Patrol != null && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetMapIconType(npcCharacter.Patrol.Id) != 0)
			{
				gameEntity = npcCharacter.Patrol;
			}
			return (gameEntity != null) ? gameEntity : _entity.Id.GameEntity;
		}

		private EMapIconType GetMapIconTypeFromEntity(IEntity _entity)
		{
			EMapIconType eMapIconType = EMapIconType.Invalid_EMapIconType;
			EntityId id = _entity.Id;
			if (_entity is NpcCharacter)
			{
				NpcCharacter npcCharacter = _entity as NpcCharacter;
				if (npcCharacter.Patrol != null)
				{
					eMapIconType = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetMapIconType(npcCharacter.Patrol.Id);
				}
			}
			if (eMapIconType == EMapIconType.Invalid_EMapIconType)
			{
				eMapIconType = ((_entity.Type != EntityTypes.EType.StalkingZone) ? UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.GetMapIconType(id) : EMapIconType.Hide_EMapIconType);
			}
			return eMapIconType;
		}

		private MiniMapEntity CreateMiniMapEntity(EntityId _entityId)
		{
			Entity entity = _entityId.Entity;
			EMapIconType mapIconTypeFromEntity = GetMapIconTypeFromEntity(entity);
			if (mapIconTypeFromEntity == EMapIconType.Invalid_EMapIconType)
			{
				return null;
			}
			GameEntity gameEntityFromEntity = GetGameEntityFromEntity(entity);
			MiniMapEntity miniMapEntity = null;
			for (int i = 0; i < m_entities.Count; i++)
			{
				if (gameEntityFromEntity.Id == m_entities[i].ID)
				{
					miniMapEntity = m_entities[i];
					break;
				}
			}
			if (miniMapEntity == null)
			{
				GameObject gameObject = NGUITools.AddChild(m_goRoot, (GameObject)m_iconTemplet);
				UISprite component = gameObject.GetComponent<UISprite>();
				bool isPlayerCharacter = entity is PlayerCharacter;
				UIMiniMapUtil.SetSpriteByMapIconType(mapIconTypeFromEntity, component, false, isPlayerCharacter);
				bool updateRotation = mapIconTypeFromEntity == EMapIconType.Enemy_EMapIconType || mapIconTypeFromEntity == EMapIconType.Player_EMapIconType;
				miniMapEntity = new MiniMapEntity(gameEntityFromEntity.Id, entity, gameEntityFromEntity, gameObject, component.transform, updateRotation);
			}
			miniMapEntity.GroupedEntityIds.Add(_entityId);
			return miniMapEntity;
		}

		private MiniMapEntity GetMapEntityByEntityId(EntityId _entityId)
		{
			for (int i = 0; i < m_entities.Count; i++)
			{
				MiniMapEntity miniMapEntity = m_entities[i];
				for (int j = 0; j < miniMapEntity.GroupedEntityIds.Count; j++)
				{
					if (_entityId == miniMapEntity.GroupedEntityIds[j])
					{
						return miniMapEntity;
					}
				}
			}
			return null;
		}

		private bool RemoveEntityFromList(EntityId _entityId)
		{
			MiniMapEntity mapEntityByEntityId = GetMapEntityByEntityId(_entityId);
			if (mapEntityByEntityId != null)
			{
				bool result = false;
				if (mapEntityByEntityId.GroupedEntityIds.Count != 0)
				{
					for (int i = 0; i < mapEntityByEntityId.GroupedEntityIds.Count; i++)
					{
						if (_entityId == mapEntityByEntityId.GroupedEntityIds[i])
						{
							mapEntityByEntityId.GroupedEntityIds.Remove(_entityId);
							result = true;
							break;
						}
					}
				}
				if (mapEntityByEntityId.GroupedEntityIds.Count == 0)
				{
					UnityEngine.Object.Destroy(mapEntityByEntityId.Go);
					m_entities.Remove(mapEntityByEntityId);
					result = true;
				}
				else if (mapEntityByEntityId.Entity.Id == _entityId)
				{
					mapEntityByEntityId.Entity = mapEntityByEntityId.GroupedEntityIds[0].Entity;
				}
				return result;
			}
			return false;
		}
	}
}
