using System;
using System.Collections.Generic;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Helpers;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class OffMeshLinkManager : HotspotManager<OffMeshLinkEntity>
	{
		public class OffMeshLinkOctreeEntry : IOctreeDynamicEntity
		{
			public GameObject Link { get; private set; }

			public OffMeshLink OffMeshLink { get; private set; }

			public Vector3 Center { get; private set; }

			public float Radius { get; private set; }

			public OctreeDynamic CurrentCell { get; set; }

			public OffMeshLinkOctreeEntry(GameObject _link, OffMeshLink _offMeshLink, Bounds _bounds)
			{
				Link = _link;
				OffMeshLink = _offMeshLink;
				Center = _bounds.center;
				Radius = Math.Max(_bounds.extents.magnitude, 0.1f);
			}
		}

		public class OffMeshLinkBlockedEntry
		{
			public float MinUseTime;

			public bool IsReversed;

			public int BlockCount;
		}

		private float m_maxJumpHeight;

		private readonly BBDictionary<GameObject, OffMeshLinkEntity> m_EntityCache = new BBDictionary<GameObject, OffMeshLinkEntity>();

		private OctreeDynamic m_OffMeshLinkIndex;

		private readonly BBDictionary<OffMeshLink, OffMeshLinkBlockedEntry> m_blockedOffMeshLinks = new BBDictionary<OffMeshLink, OffMeshLinkBlockedEntry>();

		private readonly BBList<IOctreeDynamicEntity> m_queryBuffer = new BBList<IOctreeDynamicEntity>();

		private readonly BBList<OffMeshLinkOctreeEntry> m_linkQueryBuffer = new BBList<OffMeshLinkOctreeEntry>();

		public OffMeshLinkManager()
			: base(string.Empty)
		{
		}

		public override void Initialize()
		{
			Profiling.Start("Constructing Index", float.MaxValue);
			Bounds sceneCollidersBounds = SceneTools.GetSceneCollidersBounds();
			m_OffMeshLinkIndex = new OctreeDynamic(sceneCollidersBounds.size.magnitude + 5f, 1f, 5, sceneCollidersBounds.center);
			Profiling.Stop();
			Profiling.Start("Index OffMeshLinks", float.MaxValue);
			OffMeshLink[] array = (OffMeshLink[])UnityEngine.Object.FindObjectsOfType(typeof(OffMeshLink));
			for (int i = 0; i < array.Length; i++)
			{
				CreateObject(array[i].gameObject);
			}
			Profiling.Stop();
			Profiling.Start("Fixate", float.MaxValue);
			m_OffMeshLinkIndex.Fixate();
			Profiling.Stop();
			SetMaxJumpDownHeight(ParcourHelper.Acrobatics.JumpMaxFallHeightNoDamage);
			DeactivateLinksAroundDynamicObstacles();
		}

		public void Reset()
		{
			foreach (KeyValuePair<GameObject, OffMeshLinkEntity> item in m_EntityCache)
			{
				LogicMain.EntityManager.UnregisterEntity(item.Value);
			}
			m_EntityCache.Clear();
			Initialize();
		}

		public void SetMaxJumpDownHeight(float maxHeight)
		{
			m_maxJumpHeight = maxHeight;
			BBList<IOctreeDynamicEntity> bBList = new BBList<IOctreeDynamicEntity>();
			m_OffMeshLinkIndex.GetObjects(bBList);
			foreach (IOctreeDynamicEntity item in bBList)
			{
				OffMeshLinkOctreeEntry offMeshLinkOctreeEntry = item as OffMeshLinkOctreeEntry;
				OffMeshLink offMeshLink = offMeshLinkOctreeEntry.OffMeshLink;
				offMeshLink.activated = ShouldBeActivated(offMeshLink);
			}
		}

		private bool ShouldBeActivated(OffMeshLink _link)
		{
			OffMeshLinkMeta component = _link.GetComponent<OffMeshLinkMeta>();
			eLinkType type = component.Type;
			if (type == eLinkType.JumpDown || type == eLinkType.RunPassOverDown)
			{
				float num = _link.startTransform.position.y - _link.endTransform.position.y;
				return num <= m_maxJumpHeight;
			}
			return true;
		}

		private void DeactivateLinksAroundDynamicObstacles()
		{
			EntityManager entityManager = LogicMain.EntityManager;
			entityManager.DoHouseKeeping();
			int[] array = new int[2] { 25, 22 };
			for (int i = 0; i < array.Length; i++)
			{
				EntityTypes.EType type = (EntityTypes.EType)array[i];
				int entityCount = entityManager.GetEntityCount(type);
				for (int j = 0; j < entityCount; j++)
				{
					GameEntity entity = entityManager.GetEntity<GameEntity>(type, j);
					DeactivateLinksAroundDynamicObstacle(entity);
				}
			}
		}

		public void DeactivateLinksAroundDynamicObstacle(GameEntity _entity)
		{
			float num = 1f;
			float num2 = 1f;
			if (_entity.Type == EntityTypes.EType.Pigeonry)
			{
				NavMeshObstacle component = _entity.GameObject.GetComponent<NavMeshObstacle>();
				num = component.radius + 0.5f;
				num2 = component.height;
			}
			else
			{
				if (_entity.Type != EntityTypes.EType.InteractibleMissionObject)
				{
					return;
				}
				num = 0.7f;
				num2 = 1f;
			}
			m_linkQueryBuffer.ClearIfNotEmpty();
			float radius = Mathf.Max(Mathf.Max(num, num2), 2f);
			FindOffMeshLinks(_entity.Position, radius, m_linkQueryBuffer);
			m_linkQueryBuffer.Sort((OffMeshLinkOctreeEntry _a, OffMeshLinkOctreeEntry _b) => _a.OffMeshLink.GetHashCode().CompareTo(_b.OffMeshLink.GetHashCode()));
			OffMeshLink offMeshLink = null;
			for (int i = 0; i < m_linkQueryBuffer.Count; i++)
			{
				OffMeshLink offMeshLink2 = m_linkQueryBuffer[i].OffMeshLink;
				if (!(offMeshLink == offMeshLink2))
				{
					if (IsInside(offMeshLink2.startTransform.position, _entity.Position, num, num2) || IsInside(offMeshLink2.endTransform.position, _entity.Position, num, num2))
					{
						BlockOffMeshLink(offMeshLink2);
					}
					offMeshLink = offMeshLink2;
				}
			}
		}

		private static bool IsInside(Vector3 _position, Vector3 _object, float _radius, float _height)
		{
			if (_object.y < _position.y - 2f || _position.y + _height < _object.y)
			{
				return false;
			}
			return _position.DistanceXZSquared(_object) <= _radius * _radius;
		}

		public override bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			return false;
		}

		public override OffMeshLinkEntity GetEntity(GameObject _gameObject)
		{
			OffMeshLinkEntity value;
			if (!m_EntityCache.TryGetValue(_gameObject, out value))
			{
				OffMeshLink component = _gameObject.GetComponent<OffMeshLink>();
				if (component == null)
				{
					return null;
				}
				value = (m_EntityCache[_gameObject] = new OffMeshLinkEntity(component));
				LogicMain.EntityManager.RegisterEntity(value);
				value.OnActivate();
			}
			return value;
		}

		protected override void CreateObject(GameObject _go)
		{
			Profiling.Start("CreateObject", float.MaxValue);
			OffMeshLink component = _go.GetComponent<OffMeshLink>();
			if (Assets.Scripts.Engine.Helpers.Verify(component != null && component.startTransform != null, "GameObject {0} at {1} marked as offmesh link, but no component assigned - renamed to \"MeshLink BROKEN\"", _go, _go.transform.position))
			{
				Bounds bounds = new Bounds(component.startTransform.position, Vector3.zero);
				m_OffMeshLinkIndex.Add(new OffMeshLinkOctreeEntry(_go, component, bounds));
				if (component.biDirectional)
				{
					bounds = new Bounds(component.endTransform.position, Vector3.zero);
					m_OffMeshLinkIndex.Add(new OffMeshLinkOctreeEntry(_go, component, bounds));
				}
				component.activated = true;
				component.UpdatePositions();
			}
			else
			{
				_go.name = "MeshLink BROKEN";
			}
			Profiling.Stop();
		}

		protected override bool Interact(AssassinCharacter _assassin, OffMeshLinkEntity _entity, NavMeshAgent _agent)
		{
			NavMeshAgent navMeshAgent = _assassin.UnityComponentCache.NavMeshAgent;
			NavMeshPath navMeshPath = new NavMeshPath();
			if (NavMesh.CalculatePath(navMeshAgent.transform.position, _entity.StartPosition, navMeshAgent.areaMask, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
			{
				return _assassin.AssassinCharacterStateMachines.AssassinApproachingTarget.SetTarget(_entity.Id, EInteractionReason.OffMeshLink, _entity.StartLocator);
			}
			return false;
		}

		public void FindOffMeshLinks(Vector3 _position, float _radius, BBList<OffMeshLinkOctreeEntry> _links)
		{
			Profiling.Start("OffMeshLinkManager::FindOffMeshLinks", float.MaxValue);
			m_queryBuffer.ClearIfNotEmpty();
			m_OffMeshLinkIndex.GetObjects(m_queryBuffer, _position, _radius);
			for (int i = 0; i < m_queryBuffer.Count; i++)
			{
				IOctreeDynamicEntity octreeDynamicEntity = m_queryBuffer[i];
				_links.Add((OffMeshLinkOctreeEntry)octreeDynamicEntity);
			}
			Profiling.Stop();
		}

		public void BlockOffMeshLink(OffMeshLink _link, bool _isReversed = false, float _duration = float.MaxValue)
		{
			_link.activated = false;
			OffMeshLinkBlockedEntry value;
			m_blockedOffMeshLinks.TryGetValue(_link, out value);
			if (value == null)
			{
				value = new OffMeshLinkBlockedEntry();
				m_blockedOffMeshLinks.Add(_link, value);
			}
			value.BlockCount++;
			float num = LogicMain.CurrentTime + _duration;
			if (value.MinUseTime < num)
			{
				value.MinUseTime = num;
				value.IsReversed = _isReversed;
			}
		}

		public bool UnblockOffMeshLink(OffMeshLink _link)
		{
			OffMeshLinkBlockedEntry value;
			m_blockedOffMeshLinks.TryGetValue(_link, out value);
			if (value == null || value.BlockCount == 0)
			{
				_link.activated = ShouldBeActivated(_link);
				return true;
			}
			value.BlockCount--;
			if (value.BlockCount == 0)
			{
				m_blockedOffMeshLinks.Remove(_link);
				_link.activated = ShouldBeActivated(_link);
				return true;
			}
			return false;
		}

		public bool IsOffMeshLinkBlocked(OffMeshLink _link, bool _isReversed = false)
		{
			OffMeshLinkBlockedEntry value;
			m_blockedOffMeshLinks.TryGetValue(_link, out value);
			if (value == null)
			{
				return false;
			}
			if (value.IsReversed != _isReversed)
			{
				return true;
			}
			if (LogicMain.CurrentTime >= value.MinUseTime)
			{
				return false;
			}
			return true;
		}
	}
}
