using System.Collections;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class StalkingZoneManager : HotspotManager<StalkingZone>
	{
		public StalkingZoneManager()
			: base("Stalking Zones")
		{
		}

		protected override void CreateObject(GameObject _go)
		{
			Vector3 position = _go.transform.position;
			Quaternion rotation = _go.transform.rotation;
			EntityWrapperComponent entityWrapperComponent = _go.AddComponentIfNotExist<EntityWrapperComponent>();
			StalkingZone stalkingZone = Assets.Scripts.Mission.Logic.Helpers.CreateEntity<StalkingZone>(EntityTypes.EType.StalkingZone);
			LogicMain.EntityManager.RegisterEntity(stalkingZone);
			stalkingZone.Position = position;
			stalkingZone.Rotation = rotation;
			entityWrapperComponent.Entity = stalkingZone;
			stalkingZone.OnActivate();
		}

		protected override bool Interact(AssassinCharacter _assassin, StalkingZone _entity, NavMeshAgent _agent)
		{
			return false;
		}

		public override bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			return false;
		}

		public bool IsInsideStalkingZone(AssassinCharacter _assassin, out StalkingZone _stalkingZone)
		{
			bool flag = false;
			_stalkingZone = null;
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.StalkingZone);
			int num = 0;
			while (!flag && num < entityCount)
			{
				StalkingZone entity = LogicMain.EntityManager.GetEntity<StalkingZone>(EntityTypes.EType.StalkingZone, num);
				flag = entity.StalkingZoneComponent.IsInStalkingZone(_assassin);
				if (flag)
				{
					_stalkingZone = entity;
				}
				num++;
			}
			return flag;
		}

		public bool IsPositionInStalkingZone(Vector3 _position)
		{
			StalkingZone _stalkingZone = null;
			return IsPositionInStalkingZone(_position, out _stalkingZone);
		}

		public bool IsPositionInStalkingZone(Vector3 _position, out StalkingZone _stalkingZone)
		{
			bool flag = false;
			_stalkingZone = null;
			int entityCount = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.StalkingZone);
			int num = 0;
			while (!flag && num < entityCount)
			{
				StalkingZone entity = LogicMain.EntityManager.GetEntity<StalkingZone>(EntityTypes.EType.StalkingZone, num);
				flag = entity.StalkingZoneComponent.IsPositionInStalkingZone(_position);
				if (flag)
				{
					_stalkingZone = entity;
				}
				num++;
			}
			return flag;
		}

		public IEnumerable CollectStalkingZonesInRadius(Vector3 _center, float _radius)
		{
			float sqrRadius = _radius * _radius;
			int count = LogicMain.EntityManager.GetEntityCount(EntityTypes.EType.StalkingZone);
			for (int i = 0; i < count; i++)
			{
				StalkingZone zone = LogicMain.EntityManager.GetEntity<StalkingZone>(EntityTypes.EType.StalkingZone, i);
				float distanceSqr = zone.StalkingZoneComponent.Center.DistanceSquared(_center);
				if (distanceSqr <= sqrRadius)
				{
					yield return zone;
				}
			}
		}
	}
}
