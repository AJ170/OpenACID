using System.Collections.Generic;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic.CharacterPooling
{
	public static class LogicAdapter
	{
		public class EntityData
		{
			public readonly GameEntity Entity;

			public float Priority;

			public bool Valid
			{
				get
				{
					return Entity != null && Entity.Id != EntityId.InvalidId;
				}
			}

			public bool CanBePooled
			{
				get
				{
					return Entity.CanBePooled;
				}
			}

			public Vector3 Position
			{
				get
				{
					return Entity.Position;
				}
			}

			public EntityTypes.EType EntityType
			{
				get
				{
					return Entity.Type;
				}
			}

			public NpcData.NpcType NpcType
			{
				get
				{
					return ((NpcCharacter)Entity).NpcDefinition.Definition.Type;
				}
			}

			public EntityData(GameEntity _entity)
			{
				Entity = _entity;
				Priority = float.MaxValue;
			}

			public float DistanceToSq(Vector3 _location)
			{
				return (_location - Position).sqrMagnitude;
			}

			public bool InsideFrustum(Plane[] _frustumPlanes)
			{
				if (_frustumPlanes == null)
				{
					return false;
				}
				bool flag = true;
				int num = _frustumPlanes.Length;
				for (int i = 0; num > i; i++)
				{
					flag &= _frustumPlanes[i].GetDistanceToPoint(Position) > -3f;
				}
				return flag;
			}

			public override int GetHashCode()
			{
				return Entity.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				if (obj is EntityData)
				{
					EntityData entityData = (EntityData)obj;
					return Entity.Id == entityData.Entity.Id;
				}
				return base.Equals(obj);
			}

			public int CompareTo(EntityData other)
			{
				return Entity.Id.CompareTo(other.Entity.Id);
			}

			public override string ToString()
			{
				return Entity.Id.ToString();
			}

			public static bool operator ==(EntityData _left, EntityData _right)
			{
				return _left.Entity.Id == _right.Entity.Id;
			}

			public static bool operator !=(EntityData _left, EntityData _right)
			{
				return !(_left == _right);
			}
		}

		public class OrderByPriorityComparator : IComparer<EntityData>
		{
			public int Compare(EntityData _left, EntityData _right)
			{
				int num = _left.Priority.CompareTo(_right.Priority);
				if (num == 0)
				{
					num = _left.Entity.Id.CompareTo(_right.Entity.Id);
				}
				return num;
			}
		}

		public class CharacterCollection
		{
			public int Count
			{
				get
				{
					return LogicMain.EntityManager.NpcCharacterCount;
				}
			}

			public EntityData this[int _index]
			{
				get
				{
					return new EntityData(LogicMain.EntityManager.GetNpcCharacter(_index));
				}
			}
		}

		public static readonly CharacterCollection Characters = new CharacterCollection();

		public static bool Active
		{
			get
			{
				return LogicMain.PlayerEntity != null;
			}
		}

		public static int CharacterCount
		{
			get
			{
				return LogicMain.EntityManager.NpcCharacterCount;
			}
		}

		public static Vector3 PlayerPosition
		{
			get
			{
				return LogicMain.PlayerEntity.Position;
			}
		}

		public static void OnSetActive(GameObject _go, bool _activate)
		{
			NavMeshAgent component = _go.GetComponent<NavMeshAgent>();
			OnSetActive(_go, component, _activate);
		}

		public static void OnSetActive(GameObject _go, NavMeshAgent _agent, bool _activate)
		{
			if ((bool)_agent)
			{
				_agent.enabled = _activate;
				if (_activate)
				{
					_agent.obstacleAvoidanceType = ObstacleAvoidanceType.GoodQualityObstacleAvoidance;
				}
			}
			ShowCharacter(_go, _activate);
		}

		public static void ShowCharacter(GameObject _go, bool _show)
		{
			LODGroup component = _go.GetComponent<LODGroup>();
			if (component != null)
			{
				component.enabled = _show;
			}
			Renderer[] componentsInChildren = _go.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				renderer.enabled = _show;
			}
			Transform transform = _go.transform.Find("Shadows");
			if ((bool)transform)
			{
				transform.gameObject.SetActive(_show);
			}
			WeaponHandler component2 = _go.GetComponent<WeaponHandler>();
			if (component2 != null)
			{
				component2.ShowWeapon(_show);
			}
		}

		public static void OnMissionUnload()
		{
		}

		public static void OnMissionLoad()
		{
		}
	}
}
